namespace ffmpegGui_SimpleCut
{
    partial class FormLoading
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
            label_Loading = new Label();
            SuspendLayout();
            // 
            // label_Loading
            // 
            label_Loading.AutoSize = true;
            label_Loading.Font = new Font("Segoe UI", 36F);
            label_Loading.Location = new Point(36, 43);
            label_Loading.Name = "label_Loading";
            label_Loading.Size = new Size(228, 65);
            label_Loading.TabIndex = 1;
            label_Loading.Text = "Loading...";
            label_Loading.TextAlign = ContentAlignment.MiddleCenter;
            label_Loading.UseWaitCursor = true;
            // 
            // FormLoading
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(300, 150);
            Controls.Add(label_Loading);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormLoading";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormLoading";
            TopMost = true;
            UseWaitCursor = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_Loading;
    }
}