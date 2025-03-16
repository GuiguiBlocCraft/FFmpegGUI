namespace ffmpegGui_SimpleCut;

internal class FileUtils
{
    public static string[] MakeFileOutput(string inputFile, int limit = 1)
    {
        string[] list = inputFile.Split('.');
        string extension = "." + list[list.Length - 1];
        string filename = inputFile.Substring(0, inputFile.Length - extension.Length);

        List<string> listFileName = new List<string>();
        string result = $"{filename}_splited{extension}";

        for(int i = 0; i < limit; i++)
        {
            if(listFileName.Any(a => a == result) || File.Exists(result))
            {
                int n = 0;

                do
                {
                    n++;
                    result = $"{filename}_splited-{n}{extension}";
                } while(listFileName.Any(a => a == result) || File.Exists(result));
            }

            listFileName.Add(result);
        }

        return listFileName.ToArray();
    }

    public static bool IsFileExistsInPath(string filename)
    {
        return Environment.GetEnvironmentVariable("PATH")
            .Split(';')
            .Where(s => File.Exists(Path.Combine(s, filename)))
            .Any();
    }
}
