namespace ffmpegGui_SimpleCut;

public class ListSplits
{
    private readonly List<Split> Splits = new List<Split>();
    private string DirectoryOutput = string.Empty;
    public int Page { get; set; } = 1;
    public int MaxPage { get { return Splits.Count; }}

    public void NextPage()
    {
        if(Page < MaxPage)
            Page++;
    }

    public void PreviousPage()
    {
        if(Page > 1)
            Page--;
    }

    public void Add(float start, float duration)
    {
        Splits.Add(new Split()
        {
            StartPos = start,
            Duration = duration,
            OutputFile = ""
        });

        Page = MaxPage;
    }

    public void Update(float start, float duration)
    {
        Split split = Splits[Page - 1];
        split.StartPos = start;
        split.Duration = duration < 0 ? 0 : duration;
    }

    public void Remove()
    {
        Splits.RemoveAt(Page - 1);

        if(Page == MaxPage + 1)
            Page--;
    }

    public Split GetData()
    {
        return Splits[Page - 1];
    }

    public List<Split> ToList()
    {
        return Splits;
    }

    public void InitializeNames(string inputFile)
    {
        string[] fileNames = FileUtils.MakeFileOutput(inputFile, MaxPage);

        for(int n = 0; n < Splits.Count; n++)
        {
            Splits[n].OutputFile = DirectoryOutput == string.Empty
                ? fileNames[n]
                : Path.Combine(DirectoryOutput, Path.GetFileName(fileNames[n]));
        }
    }

    public void SetOutputDirectory(string directory)
    {
        DirectoryOutput = directory;
    }

    public bool CheckOutputDirectory()
    {
        return DirectoryOutput == string.Empty || Directory.Exists(DirectoryOutput);
    }
}