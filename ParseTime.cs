namespace ffmpegGui_SimpleCut;

internal class ParseTime
{
    public static float Parse(string time)
    {
        string[] listNumbers = time.Replace('.', ',').Split(':');

        int hours = listNumbers.Length > 2 ? Int32.Parse(listNumbers[listNumbers.Length - 3]) : 0;
        int minutes = listNumbers.Length > 1 ? Int32.Parse(listNumbers[listNumbers.Length - 2]) : 0;
        float seconds = float.Parse(listNumbers[listNumbers.Length - 1]);

        return (hours * 3600) + (minutes * 60) + seconds;
    }
}
