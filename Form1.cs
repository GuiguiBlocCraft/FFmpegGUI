using System.Diagnostics;
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
        }

        private void btn_openFile_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "";
            openFileDialog.Filter = "All Videos Files |*.wmv; *.avi; *.flv; *.mkv; *.mov; *.mp4; *.mpeg; *.webm";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();
        }

        private void OpenFileDialog_FileOk(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            textBox_file.Text = openFileDialog.FileName;
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            Render render = new Render();

            if(String.IsNullOrEmpty(textBox_file.Text))
            {
                MessageBox.Show("Select a video file to start", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(checkBox_durationMode.Checked)
            {
                float start;
                int duration;

                try
                {
                    start = ParseTime.Parse(tBox_start.Text);
                    duration = Int32.Parse(tBox_duration.Text);
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
                    start = ParseTime.Parse(tBox_start.Text);
                    from = ParseTime.Parse(tBox_from.Text);
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
                tBox_from.Visible = false;
                label3.Visible = true;
                label4.Visible = true;
                tBox_duration.Visible = true;
            }
            else
            {
                label2.Visible = true;
                tBox_from.Visible = true;
                label3.Visible = false;
                label4.Visible = false;
                tBox_duration.Visible = false;
            }
        }

        private void label_Author_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/GuiguiBlocCraft/FFmpegGUI";

            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Process.Start("xdg-open", url);
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                Process.Start("open", url);
        }
    }
}