using System.Globalization;

namespace ffmpegGui_SimpleCut;

internal class ParseTime
{
    public static float Parse(string time)
    {
        string[] listNumbers = time.Split(':');

        int hours = listNumbers.Length > 2 ? Int32.Parse(listNumbers[listNumbers.Length - 3]) : 0;
        int minutes = listNumbers.Length > 1 ? Int32.Parse(listNumbers[listNumbers.Length - 2]) : 0;
        float seconds = float.Parse(listNumbers[listNumbers.Length - 1], CultureInfo.InvariantCulture);

        return (hours * 3600) + (minutes * 60) + seconds;
    }

    public static string Stringify(float time)
    {
        int hours = (int)Math.Floor(time / 3600);
        int minutes = (int)Math.Floor(time / 60) % 60;
        float seconds = time % 60;
        string result = string.Empty;

        result += hours.ToString() + ":";

        if(minutes < 10)
            result += "0";
        result += minutes.ToString() + ":";

        if(seconds < 10)
            result += "0";
        result += seconds.ToString("N2", CultureInfo.InvariantCulture);

        return result;
    }
}
