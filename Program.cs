namespace ffmpegGui_SimpleCut;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        var form = new Form1(args.Length > 0 ? args[0] : null);
        form.SetTitleVersion(typeof(Form1).Assembly.GetName().Version);
        Application.Run(form);
    }
}
