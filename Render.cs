using System.Diagnostics;
using FFmpeg.NET;
using System.Globalization;

namespace ffmpegGui_SimpleCut;

internal class Render
{
    public Engine FFmpeg = new Engine("ffmpeg.exe");

    public bool UseGraphicCard { get; set; } = false;
    private string InputFile { get; set; }
    private List<Split> Splits { get; set; } = new List<Split>();
    private int BitRateVideo { get; set; }
    private int BitRateAudio { get; set; }

    private string GetArguments()
    {
        return $"{(UseGraphicCard ? "-hwaccel cuda " : "")} -y -i \"{InputFile}\" "
            + string.Join(" ", Splits.Select(s => $"-ss {s.StartPos.ToString(CultureInfo.InvariantCulture)} -t {s.Duration.ToString(CultureInfo.InvariantCulture)} -b:v {BitRateVideo} -b:a {BitRateAudio} {(UseGraphicCard ? "-c:v h264_nvenc " : "")}\"{s.OutputFile}\""));
    }

    public void SetSplits(string inputFile, List<Split> splits)
    {
        InputFile = inputFile;
        Splits = splits;
    }

    public void SetBitrate()
    {
        var p = new Process();
        p.StartInfo.FileName = "ffprobe";
        p.StartInfo.Arguments = $"-i \"{InputFile}\" -v 0 -show_entries stream=bit_rate -of default=noprint_wrappers=1";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.Start();
        p.WaitForExit();

        string result = p.StandardOutput.ReadToEnd();
        int index = 0;

        foreach(string line in result.Split(Environment.NewLine))
        {
            string[] data = line.Split('=');

            if(data[0] == "bit_rate")
            {
                int bitrate = Int32.Parse(data[1], CultureInfo.InvariantCulture);

                if(index == 0)
                    BitRateVideo = bitrate;
                else
                    BitRateAudio = bitrate;
                index++;
            }
        }
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
        await FFmpeg.ExecuteAsync(GetArguments(), CancellationToken.None);
    }
}
