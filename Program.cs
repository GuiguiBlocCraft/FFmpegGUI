namespace ffmpegGui_SimpleCut;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        // Check ffmpeg and ffprobe
        if(!FileUtils.CheckFFmpeg())
        {
            Application.Run(new DialogDownload());

            if(!FileUtils.CheckFFmpeg())
                Environment.Exit(0);
        }

        Form1 form = new Form1();

        if(args.Length > 0)
            form.SetFileName(Path.GetFullPath(args[0]));

        Application.Run(form);
    }
}
