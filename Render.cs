using System.Diagnostics;
using System.Globalization;

namespace ffmpegGui_SimpleCut;

internal class Render
{
    public bool UseGraphicCard { get; set; } = false;
    private string InputFile { get; set; }
    private string OutputFile { get; set; }
    private float StartPos { get; set; }
    private float Duration { get; set; }
    private int BitRateVideo { get; set; }
    private int BitRateAudio { get; set; }

    private string GetArguments()
    {
        return $"{(UseGraphicCard ? "-hwaccel cuda " : "")} -y -i \"{InputFile}\" -ss {StartPos.ToString(CultureInfo.InvariantCulture)} -t {Duration.ToString(CultureInfo.InvariantCulture)} -b:v {BitRateVideo} -b:a {BitRateAudio} {(UseGraphicCard ? "-c:v h264_nvenc " : "")}\"{OutputFile}\"";
    }

    public void SetStartToFrom(float start, float from)
    {
        StartPos = start;
        Duration = from - start;
    }

    public void SetStartDuration(float start, float duration)
    {
        StartPos = start;
        Duration = duration;
    }

    public void SetFiles(string inputFile, string outputFile)
    {
        InputFile = inputFile;
        OutputFile = outputFile;
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
                return float.Parse(data[1], CultureInfo.InvariantCulture);
            }
        }

        return 0f;
    }

    public void Execute()
    {
        string arguments = GetArguments();

        Process.Start("ffmpeg", arguments);
    }
}
