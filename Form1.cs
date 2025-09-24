using LibVLCSharp.Shared;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace ffmpegGui_SimpleCut;

public partial class Form1 : Form
{
    private readonly FormLoading FormLoading = new FormLoading();
    private readonly ListSplits ListSplits = new ListSplits();
    private readonly Render Render = new Render();

    private string FileName = "";
    private System.Timers.Timer Timer;
    private float TotalDuration;
    private bool PlayerStopped = true;

    private LibVLC LibVLC;
    private MediaPlayer MediaPlayer;

    private SynchronizationContext _ui;

    public Form1()
    {
        InitializeComponent();
        Core.Initialize();
        _ui = SynchronizationContext.Current ?? new WindowsFormsSynchronizationContext();
    }

    public void SetFileName(string filename)
    {
        FileName = filename;
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        // Display loading
        FormLoading.Text = Text;
        FormLoading.Show();

        // Initialize somes composants
        label_Version.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

        ListSplits.Add(0, 0);
        UpdatePresetOptions(Preset.Medium);
        UpdateComponents();
        SetStatePlayer(false);

        if(GraphicUtil.Detect() == "")
            checkBox_useGC.Enabled = false;

        // Initialize LibVLC
        LibVLC = new LibVLC();
        MediaPlayer = new MediaPlayer(LibVLC)
        {
            Hwnd = panelPlayerVideo.Handle
        };

        MediaPlayer.Playing += (_, __) => _ui.Post(_ =>
        {
            MediaPlayer_Playing();
            MediaPlayer_PositionChanged(true);
        }, null);
        MediaPlayer.Paused += (_, __) => _ui.Post(_ => MediaPlayer_Paused(), null);
        MediaPlayer.Stopped += (_, __) => _ui.Post(_ => MediaPlayer_Stopped(), null);
        MediaPlayer.PositionChanged += (_, __) => _ui.Post(_ => MediaPlayer_PositionChanged(true), null);
        MediaPlayer.LengthChanged += (_, __) => _ui.Post(_ => MediaPlayer_LengthChanged(), null);
        MediaPlayer.VolumeChanged += (_, __) => _ui.Post(_ => MediaPlayer_VolumeChanged(), null);

        // Initialize in argument
        if(!string.IsNullOrEmpty(FileName))
        {
            LoadVideo();
        }

        FormLoading.Close();

        // Initialize codecs list
        codecsToolStrip = new List<ToolStripMenuItem>();

        statusBar_Information.Text = "Loading codecs...";
        List<Codec> codecs = await MediaInfo.GetCodecsList();

        foreach(Codec codec in codecs)
        {
            ToolStripItem item = codecToolStripMenuItem.DropDownItems.Add(codec.Name);
            item.Name = codec.Value;
            item.ToolTipText = codec.Value;
            item.Click += codecToolStripMenuItem_Click;

            codecsToolStrip.Add((ToolStripMenuItem)item);
        }

        if(codecs.Count > 0)
        {
            Render.Codec = "h264";
            ToolStripMenuItem? item = codecsToolStrip.FirstOrDefault(a => a.Name == Render.Codec);

            if(item != null)
                item.Checked = true;
        }
        else
        {
            codecToolStripMenuItem.Enabled = false;
        }

        statusBar_Information.Text = "Ready!";
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        if(Render.StateRender == StateRender.Running)
        {
            e.Cancel = true;
            MessageBox.Show("A render is running. You must cancel this before to quit.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MediaPlayer?.Dispose();
            LibVLC?.Dispose();
        }
    }

    private void LoadVideo()
    {
        MediaPlayer.Play(new Media(LibVLC, FileName));
        MediaPlayer.SetPause(true);
        MediaPlayer.Position = 0;

        SetStatePlayer(true);
        DisplayInfo($"File loaded: {FileName}");

        Render.SetData(FileName, new List<Split>());
        Render.DetectAndSetValue();

        ListSplits.SetOutputDirectory(string.Empty);
        toolStripMenuItem_Render.Enabled = true;
        editbitrateToolStripMenuItem.Enabled = true;
    }

    private void DisplayInfo(string str)
    {
        statusBar_Information.Text = str;
    }

    private async void OpenFileDialog_FileOk(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        float timeFrom = ParseTime.Parse(textBox_from.Text);
        float duration = await MediaInfo.GetDuration(openFileDialog.FileName);

        TotalDuration = duration;
        SetFileName(openFileDialog.FileName);

        ListSplits.Update(timeFrom, duration);
        UpdateComponents();
        LoadVideo();
    }

    private async void btn_Start_Click(object sender, EventArgs e)
    {
        // To cancel render
        if(Render.StateRender == StateRender.Running)
        {
            Render.Stop();
            SetStatePlayer(true);
            return;
        }

        if(String.IsNullOrEmpty(FileName))
        {
            MessageBox.Show("Select a video file to start", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if(!File.Exists(FileName))
        {
            MessageBox.Show("File doesn't exist, please select a correct file video", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if(!ListSplits.CheckOutputDirectory())
        {
            MessageBox.Show("Directory output doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if(checkBox_durationMode.Checked)
        {
            float start;
            float duration;

            try
            {
                start = ParseTime.Parse(textBox_from.Text);
                duration = float.Parse(textBox_duration.Text, CultureInfo.InvariantCulture);
            }
            catch(FormatException)
            {
                MessageBox.Show("Syntax error on \"start\" or \"duration\" time", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        else
        {
            float start;
            float from;

            try
            {
                start = ParseTime.Parse(textBox_from.Text);
                from = ParseTime.Parse(textBox_to.Text);
            }
            catch(FormatException)
            {
                MessageBox.Show("Syntax error on \"start\" or \"from\" time", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        bool getArgsOnly = ModifierKeys == Keys.Shift;

        ListSplits.InitializeNames(FileName);
        Render.SetData(FileName, ListSplits.ToList());
        Render.UseGraphicCard = checkBox_useGC.Checked;
        TotalDuration = Render.GetTotalDuration();

        if(getArgsOnly)
        {
            Clipboard.SetText(Render.FFmpeg + " " + Render.GetArguments());
            MessageBox.Show("ffmpeg command copied in clipboard");
            return;
        }

        string oldText = btn_Start.Text;

        Timer = new System.Timers.Timer();
        Timer.Interval = 100;
        Timer.Elapsed += (_, __) => _ui.Post(_ => UpdateTextRender(), null);
        Timer.Enabled = true;

        btn_Start.Text = "Cancel render";
        TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Indeterminate);
        DisplayInfo("Rendering...");

        SetStatePlayer(false);
        statusBar_ProgressBar.Visible = true;

        await Render.Execute();

        Timer.Enabled = false;
        btn_Start.Text = oldText;

        SetStatePlayer(true);
        statusBar_ProgressBar.Visible = false;

        if(Render.StateRender == StateRender.Cancelled)
        {
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
            DisplayInfo("Render cancelled!");

            foreach(Split split in Render.GetSplits())
            {
                await FileUtils.DeleteFile(split.OutputFile);
            }
        }
        else if(Render.StateRender == StateRender.Error)
        {
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
            DisplayInfo("Error on render!");

            MessageBox.Show($"An ffmpeg's error was excepted!\n{Render.LastErrorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
        }
        else
        {
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
            DisplayInfo("Render done!");
        }
    }

    private void checkBox_durationMode_CheckedChanged(object sender, EventArgs e)
    {
        if(checkBox_durationMode.Checked)
        {
            label2.Visible = false;
            textBox_to.Visible = false;
            label3.Visible = true;
            label4.Visible = true;
            textBox_duration.Visible = true;
        }
        else
        {
            label2.Visible = true;
            textBox_to.Visible = true;
            label3.Visible = false;
            label4.Visible = false;
            textBox_duration.Visible = false;
        }
    }

    private void statusBar_Copyright_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://github.com/GuiguiBlocCraft") { UseShellExecute = true });
    }

    private async void Form1_DragDrop(object sender, DragEventArgs e)
    {
        if(e.Data == null)
            return;

        if(!e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            Invalidate();
            return;
        }

        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

        float timeFrom = ParseTime.Parse(textBox_from.Text);
        float duration = await MediaInfo.GetDuration(files[0]);

        SetFileName(files[0]);

        ListSplits.Update(timeFrom, duration);
        UpdateComponents();
        LoadVideo();
    }

    private void Form1_DragEnter(object sender, DragEventArgs e)
    {
        if(e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            e.Effect = DragDropEffects.Move;
        }
        else
        {
            e.Effect = DragDropEffects.None;
        }
    }

    #region TextBot for time insertion

    private void textBox_from_Validated(object sender, EventArgs e)
    {
        UpdateTextTime();
    }

    private void textBox_to_Validated(object sender, EventArgs e)
    {
        UpdateTextTime();
    }

    private void textBox_duration_Validated(object sender, EventArgs e)
    {
        try
        {
            float timeFrom = ParseTime.Parse(textBox_from.Text);
            float duration = float.Parse(textBox_duration.Text);

            ListSplits.Update(timeFrom, duration);
            UpdateComponents();
        }
        catch(FormatException)
        {
            UpdateComponents();
        }
    }

    #endregion

    #region Buttons for pagination

    private void btnPagePrev_Click(object sender, EventArgs e)
    {
        ListSplits.PreviousPage();
        UpdateComponents();
    }

    private void btnPageNext_Click(object sender, EventArgs e)
    {
        ListSplits.NextPage();
        UpdateComponents();
    }

    private void btnAddList_Click(object sender, EventArgs e)
    {
        ListSplits.Add(0, TotalDuration);
        UpdateComponents();
    }

    private void btnRemoveList_Click(object sender, EventArgs e)
    {
        ListSplits.Remove();
        UpdateComponents();
    }

    #endregion

    private void UpdateComponents()
    {
        lblPagination.Text = $"Split {ListSplits.Page} / {ListSplits.MaxPage}";
        btnPagePrev.Enabled = ListSplits.Page > 1;
        btnPageNext.Enabled = ListSplits.Page < ListSplits.MaxPage;
        btnRemoveList.Enabled = ListSplits.MaxPage > 1;

        Split split = ListSplits.GetData();
        textBox_from.Text = ParseTime.Stringify(split.StartPos);
        textBox_to.Text = ParseTime.Stringify(split.StartPos + split.Duration);
        textBox_duration.Text = split.Duration.ToString();
    }

    #region MediaPlayer - Events

    private void MediaPlayer_Playing()
    {
        btn_VideoPlay.Image = Properties.Resources.Pause;
        PlayerStopped = false;
    }

    private void MediaPlayer_Paused()
    {
        btn_VideoPlay.Image = Properties.Resources.Play;
        PlayerStopped = false;
    }

    private void MediaPlayer_Stopped()
    {
        btn_VideoPlay.Image = Properties.Resources.Play;
        PlayerStopped = true;
    }

    private void MediaPlayer_PositionChanged(bool updateTrackBar)
    {
        float length = MediaPlayer.Length / 1000;
        float position = MediaPlayer.Position * length;

        label_Position.Text = $"{ParseTime.Stringify(position, false)} / {ParseTime.Stringify(length, false)}";
        if(updateTrackBar)
            trackBar_Player.Value = (int)(MediaPlayer.Position * trackBar_Player.Maximum);
    }

    private void MediaPlayer_LengthChanged()
    {
        trackBar_Player.Maximum = (int)MediaPlayer.Length / 1000;
    }

    private void MediaPlayer_VolumeChanged()
    {
        DisplayInfo($"Volume set to {MediaPlayer.Volume}%");
    }

    #endregion

    private void UpdateTextRender()
    {
        if(Render.Progress != null && Render.StateRender == StateRender.Running)
        {
            var time = Render.Progress.ProcessedDuration;
            string strTime = time.Hours + "h"
                + (time.Minutes < 10 ? "0" : "") + time.Minutes + ":"
                + (time.Seconds < 10 ? "0" : "") + time.Seconds;

            DisplayInfo($"{Render.Progress.Fps} fps - {strTime} ({Math.Floor(time.TotalSeconds / TotalDuration * 100)}%)");

            statusBar_ProgressBar.Maximum = (int)TotalDuration;
            statusBar_ProgressBar.Value = (int)time.TotalSeconds;

            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
            TaskbarManager.Instance.SetProgressValue((int)time.TotalSeconds, (int)TotalDuration);
        }
    }

    private void UpdatePresetOptions(Preset preset)
    {
        Render.Preset = preset;

        ultrafastToolStripMenuItem.Checked = preset == Preset.UltraFast;
        superfastToolStripMenuItem.Checked = preset == Preset.SuperFast;
        veryFastToolStripMenuItem.Checked = preset == Preset.VeryFast;
        fasterToolStripMenuItem.Checked = preset == Preset.Faster;
        fastToolStripMenuItem.Checked = preset == Preset.Fast;
        mediumToolStripMenuItem.Checked = preset == Preset.Medium;
        slowToolStripMenuItem.Checked = preset == Preset.Slow;
        slowerToolStripMenuItem.Checked = preset == Preset.Slower;
    }

    private void UpdateTextTime()
    {
        try
        {
            float timeTo = ParseTime.Parse(textBox_to.Text);
            float timeFrom = ParseTime.Parse(textBox_from.Text);

            ListSplits.Update(timeFrom, timeTo - timeFrom);
            UpdateComponents();
        }
        catch(FormatException)
        {
            UpdateComponents();
        }
    }

    #region Menu items - Preset's option

    private void ultrafastToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UpdatePresetOptions(Preset.UltraFast);
    }

    private void superfastToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UpdatePresetOptions(Preset.SuperFast);
    }

    private void veryFastToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UpdatePresetOptions(Preset.VeryFast);
    }

    private void fasterToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UpdatePresetOptions(Preset.Faster);
    }

    private void fastToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UpdatePresetOptions(Preset.Fast);
    }

    private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UpdatePresetOptions(Preset.Medium);
    }

    private void slowToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UpdatePresetOptions(Preset.Slow);
    }

    private void slowerToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UpdatePresetOptions(Preset.Slower);
    }

    private void codecToolStripMenuItem_Click(object sender, EventArgs e)
    {
        foreach(ToolStripMenuItem codec in codecsToolStrip)
        {
            codec.Checked = codec.Pressed;
        }

        ToolStripMenuItem codecSelected = codecsToolStrip.First(a => a.Pressed);
        Render.Codec = codecSelected.Name;
    }

    private void editbitrateToolStripMenuItem_Click(object sender, EventArgs e)
    {
        FormEditBitrate formEditBitrate = new FormEditBitrate();
        formEditBitrate.SetValue(Render.BitRateVideo, Render.BitRateAudio);
        DialogResult dialogResult = formEditBitrate.ShowDialog();

        if(dialogResult == DialogResult.OK)
        {
            Render.BitRateVideo = formEditBitrate.BitrateVideo;
            Render.BitRateAudio = formEditBitrate.BitrateAudio;
        }
    }

    private void toolStripMenuItem_Open_Click(object sender, EventArgs e)
    {
        openFileDialog.FileName = "";
        openFileDialog.Filter = "All Videos Files |*.wmv; *.avi; *.flv; *.mkv; *.mov; *.mp4; *.mpeg; *.webm";
        openFileDialog.RestoreDirectory = true;
        openFileDialog.ShowDialog();
    }

    private void toolStripMenuItem_RenderAs_Click(object sender, EventArgs e)
    {
        saveFilesDialog.InitialDirectory = FileName;
        DialogResult result = saveFilesDialog.ShowDialog();

        if(result == DialogResult.Cancel)
            return;

        ListSplits.SetOutputDirectory(saveFilesDialog.SelectedPath);

        DisplayInfo($"Folder output selected: {saveFilesDialog.SelectedPath}");
    }

    private void toolStripMenuItem_Quit_Click(object sender, EventArgs e)
    {
        Close();
    }

    #endregion

    #region Video player - Buttons

    private void btn_VideoPlay_Click(object sender, EventArgs e)
    {
        if(MediaPlayer.IsPlaying)
        {
            MediaPlayer.Pause();
        }
        else if(PlayerStopped)
        {
            MediaPlayer.Position = 0;
            MediaPlayer.Play(MediaPlayer.Media);
        }
        else
        {
            MediaPlayer.Play();
        }
    }

    private void btn_TakePositionStart_Click(object sender, EventArgs e)
    {
        float time = (MediaPlayer.Length / 1000) * MediaPlayer.Position;

        textBox_from.Text = ParseTime.Stringify(time);
        DisplayInfo($"Take start position: {ParseTime.Stringify(time, false)}");
        UpdateTextTime();
    }

    private void btn_TakePositionEnd_Click(object sender, EventArgs e)
    {
        float time = (MediaPlayer.Length / 1000) * MediaPlayer.Position;

        textBox_to.Text = ParseTime.Stringify(time);
        DisplayInfo($"Take end position: {ParseTime.Stringify(time, false)}");
        UpdateTextTime();
    }

    private void trackBar_Player_Scroll(object sender, EventArgs e)
    {
        MediaPlayer.Position = (float)trackBar_Player.Value / trackBar_Player.Maximum;
        MediaPlayer_PositionChanged(false);
    }

    private void panelPlayerButtons_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
        if(e.KeyCode == Keys.Space)
            MediaPlayer.Pause();
        else if(e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
        {
            if(MediaPlayer.Position == -1)
                return;

            if(e.KeyCode == Keys.Left)
                MediaPlayer.Position = (MediaPlayer.Position * (MediaPlayer.Length / 1000) - 10) / MediaPlayer.Length * 1000;
            else if(e.KeyCode == Keys.Right)
                MediaPlayer.Position = (MediaPlayer.Position * (MediaPlayer.Length / 1000) + 10) / MediaPlayer.Length * 1000;

            if(MediaPlayer.Position > 1)
                MediaPlayer.Position = 1;
            else if(MediaPlayer.Position < 0)
                MediaPlayer.Position = 0;

            MediaPlayer_PositionChanged(true);
        }
        else if(e.KeyCode == Keys.Up)
            MediaPlayer.Volume += 5;
        else if(e.KeyCode == Keys.Down)
            MediaPlayer.Volume -= 5;
    }

    private void SetStatePlayer(bool state)
    {
        trackBar_Player.Enabled = state;
        btn_VideoPlay.Enabled = state;
        btn_TakePositionStart.Enabled = state;
        btn_TakePositionEnd.Enabled = state;
    }

    #endregion
}