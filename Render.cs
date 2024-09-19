using System.Diagnostics;

namespace ffmpegGui_SimpleCut;

internal class Render
{
    public bool UseGraphicCard { get; set; } = false;
    private string InputFile { get; set; }
    private string OutputFile { get; set; }
    private float StartPos { get; set; }
    private float Duration { get; set; }
    private int BitRate { get; set; }

    private string GetArguments()
    {
        return $"{(UseGraphicCard ? "-hwaccel cuda " : "")} -y -i \"{InputFile}\" -ss {StartPos} -t {Duration} -b:v {Math.Floor((float)BitRate / 1000000)}M -c:a copy {(UseGraphicCard ? "-c:v h264_nvenc " : "")}\"{OutputFile}\"";
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
        InputFile = inputFile;
        OutputFile = outputFile;
    }

    public int SetBitrate()
    {
        var p = new Process();
        p.StartInfo.FileName = "ffprobe.exe";
        p.StartInfo.Arguments = $"-i \"{InputFile}\" -v quiet -select_streams v:0 -show_entries format=bit_rate -of default=noprint_wrappers=1";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.Start();
        p.WaitForExit();

        string result = p.StandardOutput.ReadToEnd();
        int bitrate = int.Parse(result.Replace("bit_rate=", string.Empty).Trim());

        BitRate = bitrate;

        return BitRate;
    }

    public void Execute()
    {
        string arguments = GetArguments();

        Process.Start("ffmpeg.exe", arguments);
    }
}
