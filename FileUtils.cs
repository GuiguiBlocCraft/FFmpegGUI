namespace ffmpegGui_SimpleCut;

internal class FileUtils
{
    public static string MakeFileOutput(string inputFile)
    {
        string[] list = inputFile.Split('.');
        string extension = "." + list[list.Length - 1];
        string filename = inputFile.Substring(0, inputFile.Length - extension.Length);

        string result = $"{filename}_splited{extension}";

        if(File.Exists(result))
        {
            int n = 0;

            do
            {
                n++;
                result = $"{filename}_splited-{n}{extension}";
            } while(File.Exists(result));
        }

        return result;
    }

    public static bool IsFileExistsInPath(string filename)
    {
        return Environment.GetEnvironmentVariable("PATH")
            .Split(';')
            .Where(s => File.Exists(Path.Combine(s, filename)))
            .Any();
    }
}
