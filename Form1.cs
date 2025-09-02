using LibVLCSharp.Shared;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Diagnostics;
using System.Globalization;

namespace ffmpegGui_SimpleCut
{
    public partial class Form1 : Form
    {
        private ListSplits ListSplits = new ListSplits();
        private System.Timers.Timer Timer;
        private Render render;
        private int TotalDuration = 0;
        private Preset Preset;

        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;

        private SynchronizationContext _ui;

        public Form1(string inputFile = null)
        {
            InitializeComponent();
            Core.Initialize();
            _ui = SynchronizationContext.Current ?? new WindowsFormsSynchronizationContext();

            if(inputFile != null)
            {
                textBox_file.Text = Path.GetFullPath(inputFile);
            }

            openFileDialog.FileOk += OpenFileDialog_FileOk;

            // Check ffmpeg and ffprobe
            if(!FileUtils.IsFileExistsInPath(Render.FFmpeg) || !FileUtils.IsFileExistsInPath(Render.FFprobe))
            {
                MessageBox.Show("FFmpeg was not found in your PATH. Please install it before launch this app.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            ListSplits.Add(0, 0);
            UpdatePresetOptions(Preset.Medium);
            UpdateComponents();
            LoadVideo();
            SetStatePlayer(false);

            if(GraphicUtil.Detect() == "")
                checkBox_useGC.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize LibVLC
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC)
            {
                Hwnd = panelPlayerVideo.Handle
            };

            _mediaPlayer.Playing += (_, __) => _ui.Post(_ => _mediaPlayer_Playing(), null);
            _mediaPlayer.Paused += (_, __) => _ui.Post(_ => _mediaPlayer_Paused(), null);
            _mediaPlayer.Stopped += (_, __) => _ui.Post(_ => _mediaPlayer_Stopped(), null);
            _mediaPlayer.PositionChanged += (_, __) => _ui.Post(_ => _mediaPlayer_PositionChanged(true), null);
            _mediaPlayer.LengthChanged += (_, __) => _ui.Post(_ => _mediaPlayer_LengthChanged(), null);

            _mediaPlayer_PositionChanged(false);
        }

        private void LoadVideo()
        {
            if(_libVLC != null)
            {
                using var media = new Media(_libVLC, textBox_file.Text);
                _mediaPlayer.Play(media);
                _mediaPlayer.SetPause(true);
                _mediaPlayer.Position = 0;

                _mediaPlayer_PositionChanged(true);
                SetStatePlayer(true);
            }
        }

        private void btn_openFile_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "";
            openFileDialog.Filter = "All Videos Files |*.wmv; *.avi; *.flv; *.mkv; *.mov; *.mp4; *.mpeg; *.webm";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();
        }

        private async void OpenFileDialog_FileOk(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            float timeFrom = ParseTime.Parse(textBox_from.Text);
            float duration = await MediaInfo.GetDuration(openFileDialog.FileName);

            textBox_file.Text = openFileDialog.FileName;
            textBox_to.Text = ParseTime.Stringify(duration);

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
                return;
            }

            render = new Render();

            if(String.IsNullOrEmpty(textBox_file.Text))
            {
                MessageBox.Show("Select a video file to start", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(!File.Exists(textBox_file.Text))
            {
                MessageBox.Show("File doesn't exist, please select a correct file video", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            ListSplits.InitializeNames(textBox_file.Text);
            render.SetData(textBox_file.Text, ListSplits.ToList());
            render.UseGraphicCard = checkBox_useGC.Checked;
            render.Preset = Preset;
            TotalDuration = (int)render.GetTotalDuration();
            await render.DetectAndSetValue();

            if(getArgsOnly)
            {
                Clipboard.SetText(Render.FFmpeg + render.GetArguments());
                MessageBox.Show("ffmpeg command copied in clipboard");
                return;
            }

            string oldText = btn_Start.Text;

            Timer = new System.Timers.Timer();
            Timer.Interval = 50;
            Timer.Elapsed += UpdateButtonStart;
            Timer.Enabled = true;

            btn_Start.Text = "Cancel render";
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Indeterminate);
            lblInfo.Text = "Rendering...";

            await render.Execute();

            Timer.Enabled = false;
            btn_Start.Text = oldText;

            if(render.StateRender == StateRender.Cancelled)
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                lblInfo.Text = "Render cancelled!";

                foreach(Split split in render.GetSplits())
                {
                    await FileUtils.DeleteFile(split.OutputFile);
                }
            }
            else if(render.StateRender == StateRender.Error)
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
                lblInfo.Text = "Error on render!";

                MessageBox.Show($"FFmpeg was killed! ({render.LastErrorMessage})", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                lblInfo.Text = "Render done!";
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

        private void label_Author_Click(object sender, EventArgs e)
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

            textBox_file.Text = files[0];

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
            ListSplits.Add(0, 0);
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

        private void _mediaPlayer_Playing()
        {
            btn_VideoPlay.Text = "Pause";
        }

        private void _mediaPlayer_Paused()
        {
            btn_VideoPlay.Text = "Play";
        }

        private void _mediaPlayer_Stopped()
        {
            btn_VideoPlay.Text = "Play";
            _mediaPlayer.Position = 0;
        }

        private void _mediaPlayer_PositionChanged(bool updateTrackBar)
        {
            float length = _mediaPlayer.Length / 1000;
            float position = _mediaPlayer.Position * length;

            label_Position.Text = $"Position: {ParseTime.Stringify(position, false)} / {ParseTime.Stringify(length, false)}";
            if(updateTrackBar)
                trackBar_Player.Value = (int)(_mediaPlayer.Position * trackBar_Player.Maximum);
        }

        private void _mediaPlayer_LengthChanged()
        {
            trackBar_Player.Maximum = (int)_mediaPlayer.Length / 1000;
        }

        private void UpdateButtonStart(object source, System.Timers.ElapsedEventArgs e)
        {
            if(render?.Progress != null)
            {
                try
                {
                    var time = render.Progress.ProcessedDuration;
                    string strTime = time.Hours + "h"
                        + (time.Minutes < 10 ? "0" : "") + time.Minutes + ":"
                        + (time.Seconds < 10 ? "0" : "") + time.Seconds;

                    lblInfo.Text = $"{render.Progress.Fps} fps - {strTime} ({Math.Floor(time.TotalSeconds / TotalDuration * 100)}%)";

                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                    TaskbarManager.Instance.SetProgressValue((int)time.TotalSeconds, TotalDuration);
                }
                catch(Exception) { }
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

        // Menu items - Preset's option
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

        // Video player - Buttons
        private void btn_VideoPlay_Click(object sender, EventArgs e)
        {
            if(_mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Pause();
            }
            else if(_mediaPlayer.Position < 1)
            {
                _mediaPlayer.Play();
            }
            else
            {
                _mediaPlayer.Play(_mediaPlayer.Media);
            }
        }

        private void btn_TakePositionStart_Click(object sender, EventArgs e)
        {
            textBox_from.Text = ParseTime.Stringify((_mediaPlayer.Length / 1000) * _mediaPlayer.Position);
            UpdateTextTime();
        }

        private void btn_TakePositionEnd_Click(object sender, EventArgs e)
        {
            textBox_to.Text = ParseTime.Stringify((_mediaPlayer.Length / 1000) * _mediaPlayer.Position);
            UpdateTextTime();
        }

        private void trackBar_Player_Scroll(object sender, EventArgs e)
        {
            _mediaPlayer.Position = (float)trackBar_Player.Value / trackBar_Player.Maximum;
            _mediaPlayer_PositionChanged(false);
        }

        private void SetStatePlayer(bool state)
        {
            trackBar_Player.Enabled = state;
            btn_VideoPlay.Enabled = state;
            btn_TakePositionStart.Enabled = state;
            btn_TakePositionEnd.Enabled = state;
        }
    }
}