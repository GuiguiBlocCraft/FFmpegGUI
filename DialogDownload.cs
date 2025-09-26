using System.Diagnostics;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace ffmpegGui_SimpleCut;

public partial class DialogDownload : Form
{
    private const string BaseUrl = "https://www.gyan.dev/ffmpeg/builds/";

    private readonly HttpClient httpClient = new HttpClient();

    public DialogDownload()
    {
        InitializeComponent();
    }

    private void btn_Link_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://github.com/GyanD/codexffmpeg/releases") { UseShellExecute = true });
    }

    private async void btn_Download_Click(object sender, EventArgs e)
    {
        FormDownload formDownload = new FormDownload();
        Hide();
        formDownload.Show();
        formDownload.SetText("Prepare to download...");

        string lastVersion, hash;

        try
        {
            hash = await GetHash();
        }
        catch(Exception ex)
        {
            SetErrorAndClose($"Unable to get hash ({ex.Message})", formDownload);
            return;
        }

        try
        {
            lastVersion = await GetLastVersion();
        }
        catch(Exception ex)
        {
            SetErrorAndClose($"Unable to get last version ({ex.Message})", formDownload);
            return;
        }

        string name = $"ffmpeg-{lastVersion}-essentials_build";
        string fileName = $"{name}.zip";
        string fileDownloaded = Path.Combine(Path.GetTempPath(), fileName);

        // Delete folder if is exists
        if(Directory.Exists(name))
            Directory.Delete(name, true);

        // Download
        formDownload.SetText($"Download {fileName}...");

        try
        {
            using(HttpResponseMessage response = await DownloadFFmpeg(lastVersion))
            {
                response.EnsureSuccessStatusCode();

                long totalBytes = response.Content.Headers.ContentLength ?? -1L;

                if(totalBytes > 0)
                    formDownload.SetProgressState(ProgressBarStyle.Blocks);
                else
                    formDownload.SetProgressState(ProgressBarStyle.Marquee);

                using(Stream stream = await response.Content.ReadAsStreamAsync())
                using(FileStream fileStream = new FileStream(fileDownloaded, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    byte[] buffer = new byte[8192];
                    long totalRead = 0;
                    int read;

                    do
                    {
                        read = await stream.ReadAsync(buffer);

                        if(read == 0)
                            break;

                        await fileStream.WriteAsync(buffer.AsMemory(0, read));
                        totalRead += read;

                        decimal percent = totalRead * 100 / totalBytes;

                        formDownload.SetText($"{totalRead / 1048576} on {totalBytes / 1048576} Mo");
                        formDownload.SetProgress((int)percent);
                    } while(read > 0);
                }
            }
        }
        catch(Exception ex)
        {
            SetErrorAndClose($"Unable to download {fileName} ({ex.Message})", formDownload);
            return;
        }

        // Check hash
        string fileHashed = await HashFile(fileDownloaded);

        if(hash != fileHashed)
        {
            SetErrorAndClose("Hash's file downloaded is invalid", formDownload);
            return;
        }

        // Extract
        formDownload.SetText($"Extracting files...");
        formDownload.SetProgressState(ProgressBarStyle.Marquee);

        try
        {
            await Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(fileDownloaded, ".");
                File.Delete(fileDownloaded);
            });
        }
        catch(Exception ex)
        {
            SetErrorAndClose($"Extracting error: {ex.Message}", formDownload);
        }

        SaveSelectedPath(Path.Combine(name, "bin"));

        formDownload.Close();
        Close();
    }

    private void btn_Folder_Click(object sender, EventArgs e)
    {
        DialogResult dialogResult = folderDialog.ShowDialog();

        if(dialogResult == DialogResult.Cancel)
            return;

        string pathFFmpeg = folderDialog.SelectedPath;

        // Root's ffmpeg selected
        if(Directory.Exists(Path.Combine(pathFFmpeg, "bin")))
        {
            pathFFmpeg = Path.Combine(pathFFmpeg, "bin");
        }

        if(!File.Exists(Path.Combine(pathFFmpeg, "ffmpeg.exe")) || !File.Exists(Path.Combine(pathFFmpeg, "ffprobe.exe")))
        {
            MessageBox.Show("Folder selected is not a FFmpeg's path\nPlease select folder with bin's FFmpeg", "Invalid folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SaveSelectedPath(pathFFmpeg);
        Close();
    }

    private async Task<string> GetLastVersion()
    {
        return await httpClient.GetStringAsync(BaseUrl + "release-version");
    }

    private async Task<string> GetHash()
    {
        return await httpClient.GetStringAsync(BaseUrl + "ffmpeg-release-essentials.zip.sha256");
    }

    private async Task<HttpResponseMessage> DownloadFFmpeg(string version)
    {
        return await httpClient.GetAsync($"https://github.com/GyanD/codexffmpeg/releases/download/{version}/ffmpeg-{version}-essentials_build.zip", HttpCompletionOption.ResponseHeadersRead);
    }

    private async Task<string> HashFile(string file)
    {
        using(SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(await File.ReadAllBytesAsync(file));
            StringBuilder builder = new StringBuilder();

            foreach(byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }
    }

    private void SetErrorAndClose(string text, FormDownload formDownload)
    {
        formDownload.SetText(text);
        MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        formDownload.Close();
        Close();
    }

    private void SaveSelectedPath(string folder)
    {
        RegUtil.SetPath(folder);
    }
}
