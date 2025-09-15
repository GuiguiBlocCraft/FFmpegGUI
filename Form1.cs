using LibVLCSharp.Shared;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Diagnostics;
using System.Globalization;

namespace ffmpegGui_SimpleCut
{
    public partial class Form1 : Form
    {
        private FormLoading FormLoading = new FormLoading();
        private ListSplits ListSplits = new ListSplits();

        private string FileName = "";
        private System.Timers.Timer Timer;
        private Render render;
        private float TotalDuration;
        private bool PlayerStopped = true;
        private Preset Preset;

        private LibVLC _libVLC;
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

        private void Form1_Load(object sender, EventArgs e)
        {
            // Display loading
            FormLoading.Text = Text;
            FormLoading.Show();

            // Initialize somes composants
            openFileDialog.FileOk += OpenFileDialog_FileOk;

            ListSplits.Add(0, 0);
            UpdatePresetOptions(Preset.Medium);
            UpdateComponents();
            SetStatePlayer(false);

            if(GraphicUtil.Detect() == "")
                checkBox_useGC.Enabled = false;

            // Initialize LibVLC
            _libVLC = new LibVLC();
            MediaPlayer = new MediaPlayer(_libVLC)
            {
                Hwnd = panelPlayerVideo.Handle
            };

            MediaPlayer.Playing += (_, __) => _ui.Post(_ => MediaPlayer_Playing(), null);
            MediaPlayer.Paused += (_, __) => _ui.Post(_ => MediaPlayer_Paused(), null);
            MediaPlayer.Stopped += (_, __) => _ui.Post(_ => MediaPlayer_Stopped(), null);
            MediaPlayer.PositionChanged += (_, __) => _ui.Post(_ => MediaPlayer_PositionChanged(true), null);
            MediaPlayer.LengthChanged += (_, __) => _ui.Post(_ => MediaPlayer_LengthChanged(), null);

            MediaPlayer_PositionChanged(false);

            // Initialize in argument
            if(!string.IsNullOrEmpty(FileName))
            {
                LoadVideo();
            }

            FormLoading.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(render?.StateRender == StateRender.Running)
            {
                e.Cancel = true;
                MessageBox.Show("A render is running. You must cancel this before to quit.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MediaPlayer?.Dispose();
                _libVLC?.Dispose();
            }
        }

        private void LoadVideo()
        {
            if(_libVLC != null)
            {
                using var media = new Media(_libVLC, FileName);
                MediaPlayer.Play(media);
                MediaPlayer.SetPause(true);
                MediaPlayer.Position = 0;

                MediaPlayer_PositionChanged(true);
                SetStatePlayer(true);
                DisplayInfo($"File loaded: {FileName}");

                ListSplits.SetOutputDirectory(string.Empty);
                toolStripMenuItem_Render.Enabled = true;
            }
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
            if(render != null && render.StateRender == StateRender.Running)
            {
                render.Stop();
                SetStatePlayer(true);
                return;
            }

            render = new Render();

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
            render.SetData(FileName, ListSplits.ToList());
            render.UseGraphicCard = checkBox_useGC.Checked;
            render.Preset = Preset;
            TotalDuration = render.GetTotalDuration();
            await render.DetectAndSetValue();

            if(getArgsOnly)
            {
                Clipboard.SetText(Render.FFmpeg + render.GetArguments());
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

            await render.Execute();

            Timer.Enabled = false;
            btn_Start.Text = oldText;

            SetStatePlayer(true);
            statusBar_ProgressBar.Visible = false;

            if(render.StateRender == StateRender.Cancelled)
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                DisplayInfo("Render cancelled!");

                foreach(Split split in render.GetSplits())
                {
                    await FileUtils.DeleteFile(split.OutputFile);
                }
            }
            else if(render.StateRender == StateRender.Error)
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
                DisplayInfo("Error on render!");

                MessageBox.Show($"FFmpeg was killed! ({render.LastErrorMessage})", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if(!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                this.Invalidate();
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

        private void UpdateTextRender()
        {
            if(render?.Progress != null && render.StateRender == StateRender.Running)
            {
                var time = render.Progress.ProcessedDuration;
                string strTime = time.Hours + "h"
                    + (time.Minutes < 10 ? "0" : "") + time.Minutes + ":"
                    + (time.Seconds < 10 ? "0" : "") + time.Seconds;

                DisplayInfo($"{render.Progress.Fps} fps - {strTime} ({Math.Floor(time.TotalSeconds / TotalDuration * 100)}%)");

                statusBar_ProgressBar.Maximum = (int)TotalDuration;
                statusBar_ProgressBar.Value = (int)time.TotalSeconds;

                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                TaskbarManager.Instance.SetProgressValue((int)time.TotalSeconds, (int)TotalDuration);
            }
        }

        private void UpdatePresetOptions(Preset preset)
        {
            Preset = preset;

            ultrafastToolStripMenuItem.Checked = Preset == Preset.UltraFast;
            superfastToolStripMenuItem.Checked = Preset == Preset.SuperFast;
            veryFastToolStripMenuItem.Checked = Preset == Preset.VeryFast;
            fasterToolStripMenuItem.Checked = Preset == Preset.Faster;
            fastToolStripMenuItem.Checked = Preset == Preset.Fast;
            mediumToolStripMenuItem.Checked = Preset == Preset.Medium;
            slowToolStripMenuItem.Checked = Preset == Preset.Slow;
            slowerToolStripMenuItem.Checked = Preset == Preset.Slower;
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

        public void SetTitleVersion(Version version)
        {
            Text += $" ({version.Major}.{version.Minor}.{version.Build})";
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

        #endregion

        private void SetStatePlayer(bool state)
        {
            trackBar_Player.Enabled = state;
            btn_VideoPlay.Enabled = state;
            btn_TakePositionStart.Enabled = state;
            btn_TakePositionEnd.Enabled = state;
        }
    }
}