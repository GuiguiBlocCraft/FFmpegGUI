namespace ffmpegGui_SimpleCut;

internal class FileUtils
{
    public static string MakeFileOutput(string inputFile)
    {
        string[] list = inputFile.Split('.');
        string extension = "." + list[list.Length - 1];
        string filename = inputFile.Substring(0, inputFile.Length - extension.Length);

        return $"{filename}_splited{extension}";
    }

    public static bool IsFileExistsInPath(string filename)
    {
        return Environment.GetEnvironmentVariable("PATH")
            .Split(';')
            .Where(s => File.Exists(Path.Combine(s, filename)))
            .Any();
    }
}
