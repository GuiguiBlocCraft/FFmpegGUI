using System.Diagnostics;
using FFmpeg.NET;
using System.Globalization;

namespace ffmpegGui_SimpleCut;

internal class Render
{
    public Engine FFmpeg = new Engine("ffmpeg.exe");

    public bool UseGraphicCard { get; set; } = false;
    private InputFile FileIn { get; set; }
    private OutputFile FileOut { get; set; }
    private float StartPos { get; set; }
    private float Duration { get; set; }
    private int BitRateVideo { get; set; }
    private int BitRateAudio { get; set; }

    private string GetArguments()
    {
        return $"{(UseGraphicCard ? "-hwaccel cuda " : "")} -y -i \"{FileIn.Name}\" -ss {StartPos.ToString(CultureInfo.InvariantCulture)} -t {Duration.ToString(CultureInfo.InvariantCulture)} -b:v {BitRateVideo} -b:a {BitRateAudio} {(UseGraphicCard ? "-c:v h264_nvenc " : "")}\"{FileOut.Name}\"";
    }

    public void SetStartToFrom(float start, float from)
    {
        StartPos = start;
        Duration = from - start;
    }

    public void SetStartDuration(float start, int duration)
    {
        StartPos = start;
        Duration = duration;
    }

    public void SetFiles(string inputFile, string outputFile)
    {
        FileIn = new InputFile(inputFile);
        FileOut = new OutputFile(outputFile);
    }

    public void SetBitrate()
    {
        var p = new Process();
        p.StartInfo.FileName = "ffprobe";
        p.StartInfo.Arguments = $"-i \"{FileIn}\" -v 0 -show_entries stream=bit_rate -of default=noprint_wrappers=1";
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
                return float.Parse(data[1], CultureInfo.InvariantCulture);
            }
        }

        return 0f;
    }

    public async Task Execute()
    {
        ConversionOptions options = new ConversionOptions();
        options.CutMedia(TimeSpan.FromSeconds(StartPos), TimeSpan.FromSeconds(Duration));
        options.VideoBitRate = BitRateVideo;
        options.AudioBitRate = BitRateAudio;

        await FFmpeg.ConvertAsync(FileIn, FileOut, options, CancellationToken.None);
    }
}
