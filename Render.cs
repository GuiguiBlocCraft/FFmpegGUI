using System.Globalization;
using FFmpeg.NET;
using FFmpeg.NET.Events;

namespace ffmpegGui_SimpleCut;

internal class Render
{
    public static string FFmpeg = "ffmpeg.exe";
    public static string FFprobe = "ffprobe.exe";

    private Engine Engine = new Engine(GetFullPath(FFmpeg));
    private string InputFile { get; set; }
    public int BitRateVideo { get; set; }
    public int BitRateAudio { get; set; }
    private List<Split> Splits { get; set; } = new List<Split>();
    public ConversionProgressEventArgs Progress { get; set; }
    private CancellationTokenSource cts = new CancellationTokenSource();

    public bool UseGraphicCard { get; set; } = false;
    public Graphic GraphicMethod { get; set; } = Graphic.Unknown;
    public string Codec { get; set; } = string.Empty;
    public Preset Preset { get; set; } = Preset.Medium;
    public StateRender StateRender { get; set; } = StateRender.Idle;
    public string LastErrorMessage { get; set; }

    private void OnProgress(object sender, ConversionProgressEventArgs e)
    {
        Progress = e;
    }

    private void OnError(object sender, ConversionErrorEventArgs e)
    {
        StateRender = StateRender.Error;
        LastErrorMessage = e.Exception.Message;
    }

    public string GetArguments()
    {
        string graphicEncoder = "";
        string preset = "";

        // Encoder used
        if(GraphicMethod == Graphic.NVidia)
            graphicEncoder = "nvenc";
        else if(GraphicMethod == Graphic.AMD)
            graphicEncoder = "amf";
        else if(GraphicMethod == Graphic.Intel)
            graphicEncoder = "qsv";

        // Preset used
        if(Preset == Preset.UltraFast)
            preset = "ultrafast";
        else if(Preset == Preset.SuperFast)
            preset = "superfast";
        else if(Preset == Preset.VeryFast)
            preset = "veryfast";
        else if(Preset == Preset.Faster)
            preset = "faster";
        else if(Preset == Preset.Fast)
            preset = "fast";
        else if(Preset == Preset.Medium)
            preset = "medium";
        else if(Preset == Preset.Slow)
            preset = "slow";
        else if(Preset == Preset.Slower)
            preset = "slower";

        return $"{(UseGraphicCard ? "-hwaccel auto " : "")}-i \"{InputFile}\" "
            + string.Join(" ", Splits.Select(s => $"-ss {s.StartPos.ToString(CultureInfo.InvariantCulture)} -t {s.Duration.ToString(CultureInfo.InvariantCulture)} -b:v {BitRateVideo} -b:a {BitRateAudio} {(UseGraphicCard ? $"-c:v {Codec}_" + graphicEncoder : $"-c:v {Codec}")} -preset {preset} \"{s.OutputFile}\""));
    }

    public void SetData(string inputFile, List<Split> splits)
    {
        InputFile = inputFile;
        Splits = splits;
    }

    public async Task DetectAndSetValue()
    {
        BitRateVideo = await MediaInfo.GetBitRateVideo(InputFile);
        BitRateAudio = await MediaInfo.GetBitRateAudio(InputFile);

        string videoName = GraphicUtil.Detect();
        if(videoName.Contains("NVIDIA"))
            GraphicMethod = Graphic.NVidia;
        else if(videoName.Contains("AMD"))
            GraphicMethod = Graphic.AMD;
        else if(videoName.Contains("Intel"))
            GraphicMethod = Graphic.Intel;
    }

    public List<Split> GetSplits()
    {
        return Splits;
    }

    public float GetTotalDuration()
    {
        float duration = 0;

        foreach(Split split in Splits)
        {
            duration += split.Duration;
        }

        return duration;
    }

    public async Task Execute()
    {
        StateRender = StateRender.Running;
        Engine.Progress += OnProgress;
        Engine.Error += OnError;

        try
        {
            await Engine.ExecuteAsync(GetArguments(), cts.Token);

            if(StateRender != StateRender.Error)
                StateRender = StateRender.Idle;
        }
        catch(TaskCanceledException ex)
        {
            if(StateRender != StateRender.Error)
            {
                StateRender = StateRender.Cancelled;
                LastErrorMessage = ex.Message;
            }
        }
        catch(Exception ex)
        {
            StateRender = StateRender.Error;
            LastErrorMessage = ex.Message;
        }

        Engine.Progress -= OnProgress;
        Engine.Error -= OnError;
    }

    public void Stop()
    {
        cts.Cancel();
    }

    public static string GetFullPath(string filename)
    {
        string path = RegUtil.GetPath();

        if(!FileUtils.IsFileExistsInPath(filename) && Directory.Exists(path))
            return Path.Combine(path, filename);
        return filename;
    }
}
