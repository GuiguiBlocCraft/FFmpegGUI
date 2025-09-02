namespace ffmpegGui_SimpleCut;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        // Check ffmpeg and ffprobe
        if(!FileUtils.IsFileExistsInPath(Render.FFmpeg) || !FileUtils.IsFileExistsInPath(Render.FFprobe))
        {
            MessageBox.Show("FFmpeg was not found in your PATH. Please install it before launch this app.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(1);
        }

        Form1 form = new Form1();
        form.SetTitleVersion(typeof(Form1).Assembly.GetName().Version);

        if(args.Length > 0)
            form.SetFileName(Path.GetFullPath(args[0]));

        Application.Run(form);
    }
}
