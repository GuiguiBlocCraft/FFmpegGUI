﻿namespace ffmpegGui_SimpleCut
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblFile = new Label();
            textBox_file = new TextBox();
            btn_openFile = new Button();
            label1 = new Label();
            label2 = new Label();
            tBox_start = new TextBox();
            tBox_from = new TextBox();
            label3 = new Label();
            tBox_duration = new TextBox();
            btn_Start = new Button();
            checkBox_useGC = new CheckBox();
            checkBox_durationMode = new CheckBox();
            label4 = new Label();
            openFileDialog = new OpenFileDialog();
            label_Title = new Label();
            SuspendLayout();
            // 
            // lblFile
            // 
            lblFile.AutoSize = true;
            lblFile.Location = new Point(12, 61);
            lblFile.Name = "lblFile";
            lblFile.Size = new Size(31, 15);
            lblFile.TabIndex = 0;
            lblFile.Text = "File :";
            // 
            // textBox_file
            // 
            textBox_file.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox_file.Location = new Point(49, 58);
            textBox_file.Name = "textBox_file";
            textBox_file.Size = new Size(701, 23);
            textBox_file.TabIndex = 1;
            // 
            // btn_openFile
            // 
            btn_openFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn_openFile.Location = new Point(756, 58);
            btn_openFile.Name = "btn_openFile";
            btn_openFile.Size = new Size(32, 23);
            btn_openFile.TabIndex = 2;
            btn_openFile.Text = "...";
            btn_openFile.UseVisualStyleBackColor = true;
            btn_openFile.Click += btn_openFile_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 90);
            label1.Name = "label1";
            label1.Size = new Size(60, 15);
            label1.TabIndex = 3;
            label1.Text = "Start from";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(176, 90);
            label2.Name = "label2";
            label2.Size = new Size(18, 15);
            label2.TabIndex = 4;
            label2.Text = "to";
            // 
            // tBox_start
            // 
            tBox_start.Location = new Point(74, 87);
            tBox_start.Name = "tBox_start";
            tBox_start.Size = new Size(96, 23);
            tBox_start.TabIndex = 5;
            tBox_start.Text = "0:00:00.00";
            // 
            // tBox_from
            // 
            tBox_from.Location = new Point(197, 87);
            tBox_from.Name = "tBox_from";
            tBox_from.Size = new Size(96, 23);
            tBox_from.TabIndex = 6;
            tBox_from.Text = "0:00:00.00";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(176, 90);
            label3.Name = "label3";
            label3.Size = new Size(91, 15);
            label3.TabIndex = 7;
            label3.Text = "with duration in";
            label3.Visible = false;
            // 
            // tBox_duration
            // 
            tBox_duration.Location = new Point(268, 87);
            tBox_duration.Name = "tBox_duration";
            tBox_duration.Size = new Size(96, 23);
            tBox_duration.TabIndex = 8;
            tBox_duration.Text = "0";
            tBox_duration.TextAlign = HorizontalAlignment.Right;
            tBox_duration.Visible = false;
            // 
            // btn_Start
            // 
            btn_Start.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btn_Start.Location = new Point(326, 187);
            btn_Start.Name = "btn_Start";
            btn_Start.Size = new Size(148, 33);
            btn_Start.TabIndex = 9;
            btn_Start.Text = "Start render";
            btn_Start.UseVisualStyleBackColor = true;
            btn_Start.Click += btn_Start_Click;
            // 
            // checkBox_useGC
            // 
            checkBox_useGC.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            checkBox_useGC.AutoSize = true;
            checkBox_useGC.Location = new Point(12, 201);
            checkBox_useGC.Name = "checkBox_useGC";
            checkBox_useGC.Size = new Size(158, 19);
            checkBox_useGC.TabIndex = 10;
            checkBox_useGC.Text = "Render with graphic card";
            checkBox_useGC.UseVisualStyleBackColor = true;
            // 
            // checkBox_durationMode
            // 
            checkBox_durationMode.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            checkBox_durationMode.AutoSize = true;
            checkBox_durationMode.Location = new Point(12, 176);
            checkBox_durationMode.Name = "checkBox_durationMode";
            checkBox_durationMode.Size = new Size(106, 19);
            checkBox_durationMode.TabIndex = 11;
            checkBox_durationMode.Text = "Duration mode";
            checkBox_durationMode.UseVisualStyleBackColor = true;
            checkBox_durationMode.CheckedChanged += checkBox_durationMode_CheckedChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(367, 91);
            label4.Name = "label4";
            label4.Size = new Size(50, 15);
            label4.TabIndex = 12;
            label4.Text = "seconds";
            label4.Visible = false;
            // 
            // label_Title
            // 
            label_Title.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label_Title.AutoSize = true;
            label_Title.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            label_Title.Location = new Point(302, 9);
            label_Title.Name = "label_Title";
            label_Title.Size = new Size(197, 45);
            label_Title.TabIndex = 13;
            label_Title.Text = "FFmpeg GUI";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 232);
            Controls.Add(label_Title);
            Controls.Add(label4);
            Controls.Add(checkBox_durationMode);
            Controls.Add(checkBox_useGC);
            Controls.Add(btn_Start);
            Controls.Add(tBox_duration);
            Controls.Add(label3);
            Controls.Add(tBox_from);
            Controls.Add(tBox_start);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btn_openFile);
            Controls.Add(textBox_file);
            Controls.Add(lblFile);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FFmpeg GUI";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFile;
        private TextBox textBox_file;
        private Button btn_openFile;
        private Label label1;
        private Label label2;
        private TextBox tBox_start;
        private TextBox tBox_from;
        private Label label3;
        private TextBox tBox_duration;
        private Button btn_Start;
        private CheckBox checkBox_useGC;
        private CheckBox checkBox_durationMode;
        private Label label4;
        private OpenFileDialog openFileDialog;
        private Label label_Title;
    }
}