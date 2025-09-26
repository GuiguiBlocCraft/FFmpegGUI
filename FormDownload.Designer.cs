namespace ffmpegGui_SimpleCut
{
    partial class FormDownload
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label_DownloadInfo = new Label();
            progressBar = new ProgressBar();
            SuspendLayout();
            // 
            // label_DownloadInfo
            // 
            label_DownloadInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label_DownloadInfo.Location = new Point(10, 10);
            label_DownloadInfo.Name = "label_DownloadInfo";
            label_DownloadInfo.Size = new Size(306, 15);
            label_DownloadInfo.TabIndex = 7;
            label_DownloadInfo.Text = "Loading";
            label_DownloadInfo.TextAlign = ContentAlignment.TopCenter;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(10, 29);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(308, 23);
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.TabIndex = 6;
            // 
            // FormDownload
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(328, 58);
            ControlBox = false;
            Controls.Add(label_DownloadInfo);
            Controls.Add(progressBar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormDownload";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Download";
            ResumeLayout(false);
        }

        #endregion

        private Label label_DownloadInfo;
        private ProgressBar progressBar;
    }
}