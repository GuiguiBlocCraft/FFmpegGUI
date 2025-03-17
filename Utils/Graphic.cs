using System.Management;

namespace ffmpegGui_SimpleCut;

public class GraphicUtil
{
    public static string Detect()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");

        foreach(ManagementObject mo in searcher.Get())
        {
            string videoName = mo.Properties["Name"].Value.ToString();

            if(videoName != null && (videoName.Contains("NVIDIA") || videoName.Contains("AMD") || videoName.Contains("Intel")))
                return videoName;
        }

        return "";
    }
}