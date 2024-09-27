using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace ffmpegGui_SimpleCut
{
    public partial class Form1 : Form
    {
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
                btn_Start.Enabled = false;
                MessageBox.Show("FFmpeg was not found in your PATH. Please install it before launch this app.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
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
            textBox_file.Text = openFileDialog.FileName;
            textBox_from.Text = ParseTime.Stringify(await Render.GetDuration(openFileDialog.FileName));
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
                    start = ParseTime.Parse(textBox_start.Text);
                    duration = float.Parse(textBox_duration.Text, CultureInfo.InvariantCulture);
                }
                catch(FormatException)
                {
                    MessageBox.Show("Syntax error on \"start\" or \"duration\" time", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                render.SetStartDuration(start, duration);
            }
            else
            {
                float start;
                float from;

                try
                {
                    start = ParseTime.Parse(textBox_start.Text);
                    from = ParseTime.Parse(textBox_from.Text);
                }
                catch(FormatException)
                {
                    MessageBox.Show("Syntax error on \"start\" or \"from\" time", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                render.SetStartToFrom(start, from);
            }

            render.SetFiles(textBox_file.Text, FileUtils.MakeFileOutput(textBox_file.Text));
            render.UseGraphicCard = checkBox_useGC.Checked;
            render.SetBitrate();
            render.Execute();
        }

        private void checkBox_durationMode_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox_durationMode.Checked)
            {
                label2.Visible = false;
                textBox_from.Visible = false;
                label3.Visible = true;
                label4.Visible = true;
                textBox_duration.Visible = true;
            }
            else
            {
                label2.Visible = true;
                textBox_from.Visible = true;
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

            textBox_file.Text = files[0];
            textBox_from.Text = ParseTime.Stringify(await Render.GetDuration(files[0]));
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBox_start_Validated(object sender, EventArgs e)
        {
            try
            {
                textBox_start.Text = ParseTime.Stringify(ParseTime.Parse(textBox_start.Text));
            }
            catch(FormatException)
            {
                textBox_start.Text = "0:00:00.00";
            }
        }

        private void textBox_from_Validated(object sender, EventArgs e)
        {
            try
            {
                textBox_from.Text = ParseTime.Stringify(ParseTime.Parse(textBox_from.Text));
            }
            catch(FormatException)
            {
                textBox_from.Text = "0:00:00.00";
            }
        }
    }
}