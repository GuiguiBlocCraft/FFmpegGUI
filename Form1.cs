using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace ffmpegGui_SimpleCut
{
    public partial class Form1 : Form
    {
        private ListSplits ListSplits = new ListSplits();

        public Form1(string inputFile = null)
        {
            InitializeComponent();

            if(inputFile != null)
            {
                textBox_file.Text = inputFile;
            }

            openFileDialog.FileOk += OpenFileDialog_FileOk;

            // Check ffmpeg and ffprobe
            if(!FileUtils.IsFileExistsInPath("ffmpeg.exe") && !FileUtils.IsFileExistsInPath("ffprobe.exe"))
            {
                MessageBox.Show("FFmpeg was not found in your PATH. Please install it before launch this app.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            ListSplits.Add(0, 0);
            UpdateComponents();
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
            float duration = await Render.GetDuration(openFileDialog.FileName);

            textBox_file.Text = openFileDialog.FileName;
            textBox_to.Text = ParseTime.Stringify(duration);

            ListSplits.Update(timeFrom, duration);
            UpdateComponents();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            Render render = new Render();

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

            ListSplits.InitializeNames(textBox_file.Text);
            render.SetSplits(textBox_file.Text, ListSplits.ToList());
            render.UseGraphicCard = checkBox_useGC.Checked;
            render.SetBitrate();
            render.Execute();
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
            float duration = await Render.GetDuration(files[0]);

            textBox_file.Text = files[0];

            ListSplits.Update(timeFrom, duration);
            UpdateComponents();
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

        private void textBox_to_Validated(object sender, EventArgs e)
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

        private void textBox_duration_Validated(object sender, EventArgs e)
        {
            try
            {
                float timeFrom = ParseTime.Parse(textBox_from.Text);
                float timeTo = float.Parse(textBox_duration.Text);
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

        public void SetTitleVersion(Version version)
        {
            Text += $" ({version.Major}.{version.Minor}.{version.Build})";
        }
    }
}