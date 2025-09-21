namespace ffmpegGui_SimpleCut
{
    partial class FormEditBitrate
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            label_selectBitrateVideo = new Label();
            label_selectBitrateAudio = new Label();
            numeric_VideoBitrate = new NumericUpDown();
            numeric_AudioBitrate = new NumericUpDown();
            btnCancel = new Button();
            btnOk = new Button();
            ((System.ComponentModel.ISupportInitialize)numeric_VideoBitrate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numeric_AudioBitrate).BeginInit();
            SuspendLayout();
            // 
            // label_selectBitrateVideo
            // 
            label_selectBitrateVideo.Anchor = AnchorStyles.Top;
            label_selectBitrateVideo.AutoSize = true;
            label_selectBitrateVideo.Location = new Point(11, 10);
            label_selectBitrateVideo.Name = "label_selectBitrateVideo";
            label_selectBitrateVideo.Size = new Size(142, 15);
            label_selectBitrateVideo.TabIndex = 0;
            label_selectBitrateVideo.Text = "Video bitrate (in kilobits) :";
            // 
            // label_selectBitrateAudio
            // 
            label_selectBitrateAudio.Anchor = AnchorStyles.Top;
            label_selectBitrateAudio.AutoSize = true;
            label_selectBitrateAudio.Location = new Point(9, 39);
            label_selectBitrateAudio.Name = "label_selectBitrateAudio";
            label_selectBitrateAudio.Size = new Size(144, 15);
            label_selectBitrateAudio.TabIndex = 1;
            label_selectBitrateAudio.Text = "Audio bitrate (in kilobits) :";
            // 
            // numeric_VideoBitrate
            // 
            numeric_VideoBitrate.Anchor = AnchorStyles.Top;
            numeric_VideoBitrate.Location = new Point(156, 6);
            numeric_VideoBitrate.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            numeric_VideoBitrate.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numeric_VideoBitrate.Name = "numeric_VideoBitrate";
            numeric_VideoBitrate.Size = new Size(120, 23);
            numeric_VideoBitrate.TabIndex = 3;
            numeric_VideoBitrate.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // numeric_AudioBitrate
            // 
            numeric_AudioBitrate.Anchor = AnchorStyles.Top;
            numeric_AudioBitrate.Location = new Point(156, 35);
            numeric_AudioBitrate.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            numeric_AudioBitrate.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numeric_AudioBitrate.Name = "numeric_AudioBitrate";
            numeric_AudioBitrate.Size = new Size(120, 23);
            numeric_AudioBitrate.TabIndex = 4;
            numeric_AudioBitrate.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom;
            btnCancel.Location = new Point(57, 94);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 25);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnOk
            // 
            btnOk.Anchor = AnchorStyles.Bottom;
            btnOk.Location = new Point(147, 94);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(80, 25);
            btnOk.TabIndex = 7;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // FormEditBitrate
            // 
            AcceptButton = btnOk;
            CancelButton = btnCancel;
            ClientSize = new Size(284, 131);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            Controls.Add(numeric_AudioBitrate);
            Controls.Add(numeric_VideoBitrate);
            Controls.Add(label_selectBitrateAudio);
            Controls.Add(label_selectBitrateVideo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormEditBitrate";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select bitrate";
            ((System.ComponentModel.ISupportInitialize)numeric_VideoBitrate).EndInit();
            ((System.ComponentModel.ISupportInitialize)numeric_AudioBitrate).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_selectBitrateVideo;
        private Label label_selectBitrateAudio;
        private NumericUpDown numeric_VideoBitrate;
        private NumericUpDown numeric_AudioBitrate;
        private Button btnCancel;
        private Button btnOk;
    }
}
