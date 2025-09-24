using System.Diagnostics;
using System.Globalization;

namespace ffmpegGui_SimpleCut;

public class MediaInfo
{
    public static async Task<int> GetBitRateVideo(string fileName)
    {
        string result = await ExecuteAsync($"-i \"{fileName}\" -v quiet -select_streams v:0 -show_entries stream=bit_rate -of default=noprint_wrappers=1");
        var resStr = GetResult(result, "bit_rate");

        if(int.TryParse(resStr, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out int value))
            return value;

        return 0;
    }

    public static async Task<int> GetBitRateAudio(string fileName)
    {
        string result = await ExecuteAsync($"-i \"{fileName}\" -v quiet -select_streams a:0 -show_entries stream=bit_rate -of default=noprint_wrappers=1");
        var resStr = GetResult(result, "bit_rate");

        if(int.TryParse(resStr, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out int value))
            return value;

        return 0;
    }

    public static async Task<float> GetDuration(string fileName)
    {
        string result = await ExecuteAsync($"-i \"{fileName}\" -v 0 -show_entries stream=duration -of default=noprint_wrappers=1");
        var resStr = GetResult(result, "duration");

        if(float.TryParse(resStr, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float value))
            return value;

        return 0f;
    }

    public static async Task<List<Codec>> GetCodecsList()
    {
        string result = await ExecuteAsync("-codecs");
        List<Codec> codecs = new List<Codec>();

        bool listed = false;

        foreach(string line in result.Split(Environment.NewLine))
        {
            string lineTrimed = line.Trim();

            if(listed)
            {
                string[] data = lineTrimed.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);

                if(data.Length == 0)
                    continue;

                string[] supports = data[0].Select(c => c.ToString()).ToArray();

                // Encode support + Video codec
                if(supports.Length > 0 && supports[1] == "E" && supports[2] == "V" && supports[3] != "I")
                    codecs.Add(new Codec()
                    {
                        Value = data[1],
                        Name = data[2]
                    });
            }
            else if(lineTrimed == "-------")
            {
                listed = true;
            }
        }

        return codecs;
    }

    private static async Task<string> ExecuteAsync(string arguments)
    {
        var p = new Process()
        {
            StartInfo = {
                FileName = Render.FFprobe,
                Arguments = "-hide_banner " + arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            }
        };

        p.Start();

        return await p.StandardOutput.ReadToEndAsync();
    }

    private static string? GetResult(string result, string search)
    {
        foreach(string line in result.Split(Environment.NewLine))
        {
            string[] data = line.Split('=');

            if(data[0] == search)
                return data[1];
        }

        return null;
    }
}