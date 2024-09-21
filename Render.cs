using System.Diagnostics;

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
        return $"{(UseGraphicCard ? "-hwaccel cuda " : "")} -y -i \"{InputFile}\" -ss {StartPos} -t {Duration} -b:v {BitRateVideo} -b:a {BitRateAudio} {(UseGraphicCard ? "-c:v h264_nvenc " : "")}\"{OutputFile}\"";
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
                int bitrate = Int32.Parse(data[1]);

                if(index == 0)
                    BitRateVideo = bitrate;
                else
                    BitRateAudio = bitrate;
                index++;
            }
        }
    }

    public void Execute()
    {
        string arguments = GetArguments();

        Process.Start("ffmpeg", arguments);
    }
}
