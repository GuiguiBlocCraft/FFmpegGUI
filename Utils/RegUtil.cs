using Microsoft.Win32;

namespace ffmpegGui_SimpleCut;

internal class RegUtil
{
    public readonly static string Root = "HKEY_CURRENT_USER\\SOFTWARE\\FFmpegCut";
    public readonly static string Save = "FFmpegFolder";

    public static string GetPath()
    {
        return (string)Registry.GetValue(Root, Save, "");
    }

    public static void SetPath(string folder)
    {
        Registry.SetValue(Root, Save, folder, RegistryValueKind.ExpandString);
    }
}
