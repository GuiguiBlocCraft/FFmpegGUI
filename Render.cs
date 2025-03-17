using System.Diagnostics;
using System.Globalization;
using FFmpeg.NET;
using FFmpeg.NET.Events;

namespace ffmpegGui_SimpleCut;

internal class Render
{
    public static string FFmpeg = "ffmpeg.exe";
    public static string FFprobe = "ffprobe.exe";

    private Engine Engine = new Engine();
    private string InputFile { get; set; }
    private List<Split> Splits { get; set; } = new List<Split>();
    public ConversionProgressEventArgs Progress { get; set; }
    private CancellationTokenSource cts = new CancellationTokenSource();

    public bool UseGraphicCard { get; set; } = false;
    public StateRender StateRender { get; set; } = StateRender.Idle;
    public string LastErrorMessage { get; set; }

    private void OnProgress(object sender, ConversionProgressEventArgs e)
    {
        Progress = e;
    }

    private string GetArguments()
    {
        return $"{(UseGraphicCard ? "-hwaccel cuda " : "")} -i \"{InputFile}\" "
            + string.Join(" ", Splits.Select(s => $"-ss {s.StartPos.ToString(CultureInfo.InvariantCulture)} -t {s.Duration.ToString(CultureInfo.InvariantCulture)} {(UseGraphicCard ? "-c:v h264_nvenc " : "")}\"{s.OutputFile}\""));
    }

    public void SetData(string inputFile, List<Split> splits)
    {
        InputFile = inputFile;
        Splits = splits;
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

    public static async Task<float> GetDuration(string fileName)
    {
        var p = new Process();
        p.StartInfo.FileName = "ffprobe";
        p.StartInfo.Arguments = $"-i \"{fileName}\" -v 0 -show_entries stream=duration -of default=noprint_wrappers=1";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.Start();
        await p.WaitForExitAsync();

        string result = p.StandardOutput.ReadToEnd();

        foreach(string line in result.Split(Environment.NewLine))
        {
            string[] data = line.Split('=');

            if(data[0] == "duration")
            {
                if(float.TryParse(data[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float value))
                    return value;
                return 0f;
            }
        }

        return 0f;
    }

    public async Task Execute()
    {
        StateRender = StateRender.Running;
        Engine.Progress += OnProgress;

        try
        {
            await Engine.ExecuteAsync(GetArguments(), cts.Token);
        }
        catch(TaskCanceledException ex)
        {
            StateRender = StateRender.Cancelled;
            LastErrorMessage = ex.Message;
        }
        catch(Exception ex)
        {
            StateRender = StateRender.Error;
            LastErrorMessage = ex.Message;
        }

        if(StateRender == StateRender.Running)
            StateRender = StateRender.Idle;
        Engine.Progress -= OnProgress;
    }

    public void Stop()
    {
        cts.Cancel();
    }
}
