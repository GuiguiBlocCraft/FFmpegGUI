namespace ffmpegGui_SimpleCut
{
    partial class DialogDownload
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
            label1 = new Label();
            btn_Link = new Button();
            btn_Download = new Button();
            btn_Folder = new Button();
            folderDialog = new FolderBrowserDialog();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(399, 30);
            label1.TabIndex = 0;
            label1.Text = "Seems you doesn't have FFmpeg in your path.\r\nYou can download this on the official website or find it in your computeur.";
            // 
            // btn_Link
            // 
            btn_Link.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btn_Link.Location = new Point(12, 84);
            btn_Link.Name = "btn_Link";
            btn_Link.Size = new Size(104, 23);
            btn_Link.TabIndex = 1;
            btn_Link.Text = "Official link";
            btn_Link.UseVisualStyleBackColor = true;
            btn_Link.Click += btn_Link_Click;
            // 
            // btn_Download
            // 
            btn_Download.Anchor = AnchorStyles.Bottom;
            btn_Download.Location = new Point(172, 84);
            btn_Download.Name = "btn_Download";
            btn_Download.Size = new Size(104, 23);
            btn_Download.TabIndex = 2;
            btn_Download.Text = "Download";
            btn_Download.UseVisualStyleBackColor = true;
            btn_Download.Click += btn_Download_Click;
            // 
            // btn_Folder
            // 
            btn_Folder.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btn_Folder.Location = new Point(332, 84);
            btn_Folder.Name = "btn_Folder";
            btn_Folder.Size = new Size(104, 23);
            btn_Folder.TabIndex = 3;
            btn_Folder.Text = "Get folder";
            btn_Folder.UseVisualStyleBackColor = true;
            btn_Folder.Click += btn_Folder_Click;
            // 
            // folderDialog
            // 
            folderDialog.RootFolder = Environment.SpecialFolder.Programs;
            // 
            // DialogDownload
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(448, 119);
            Controls.Add(btn_Folder);
            Controls.Add(btn_Download);
            Controls.Add(btn_Link);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "DialogDownload";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FFmpeg GUI";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btn_Link;
        private Button btn_Download;
        private Button btn_Folder;
        private FolderBrowserDialog folderDialog;
    }
}