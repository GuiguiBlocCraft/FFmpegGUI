namespace ffmpegGui_SimpleCut;

partial class FormEditBitrate : Form
{
    public int BitrateVideo { get; set; }
    public int BitrateAudio { get; set; }

    public FormEditBitrate()
    {
        InitializeComponent();
    }

    public void SetValue(int bitrateVideo, int bitrateAudio)
    {
        if(bitrateVideo > numeric_VideoBitrate.Minimum)
            numeric_VideoBitrate.Value = bitrateVideo / 1000;
        else
            numeric_VideoBitrate.Value = numeric_VideoBitrate.Minimum;

        if(bitrateAudio > numeric_AudioBitrate.Minimum)
            numeric_AudioBitrate.Value = bitrateAudio / 1000;
        else
            numeric_AudioBitrate.Value = numeric_AudioBitrate.Minimum;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        BitrateVideo = (int)numeric_VideoBitrate.Value * 1000;
        BitrateAudio = (int)numeric_AudioBitrate.Value * 1000;

        DialogResult = DialogResult.OK;
        Close();
    }
}
