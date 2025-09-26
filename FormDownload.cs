namespace ffmpegGui_SimpleCut;

public partial class FormDownload : Form
{
    public FormDownload()
    {
        InitializeComponent();
    }

    public void SetText(string text)
    {
        label_DownloadInfo.Text = text;
    }

    public void SetProgress(int value)
    {
        progressBar.Value = value;
    }

    public void SetProgressState(ProgressBarStyle style)
    {
        progressBar.Style = style;
    }
}
