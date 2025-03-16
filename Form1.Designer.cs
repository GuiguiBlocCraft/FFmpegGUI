namespace ffmpegGui_SimpleCut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            lblFile = new Label();
            textBox_file = new TextBox();
            btn_openFile = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            textBox_from = new TextBox();
            textBox_to = new TextBox();
            textBox_duration = new TextBox();
            btn_Start = new Button();
            checkBox_useGC = new CheckBox();
            checkBox_durationMode = new CheckBox();
            openFileDialog = new OpenFileDialog();
            label_Title = new Label();
            label_Author = new Label();
            label_createdBy = new Label();
            lblPagination = new Label();
            btnPagePrev = new Button();
            btnPageNext = new Button();
            btnAddList = new Button();
            btnRemoveList = new Button();
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
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(176, 90);
            label3.Name = "label3";
            label3.Size = new Size(91, 15);
            label3.TabIndex = 5;
            label3.Text = "with duration in";
            label3.Visible = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(367, 91);
            label4.Name = "label4";
            label4.Size = new Size(50, 15);
            label4.TabIndex = 6;
            label4.Text = "seconds";
            label4.Visible = false;
            // 
            // textBox_from
            // 
            textBox_from.Location = new Point(74, 87);
            textBox_from.Name = "textBox_from";
            textBox_from.Size = new Size(96, 23);
            textBox_from.TabIndex = 7;
            textBox_from.Text = "0:00:00.00";
            textBox_from.Validated += textBox_from_Validated;
            // 
            // textBox_to
            // 
            textBox_to.Location = new Point(197, 87);
            textBox_to.Name = "textBox_to";
            textBox_to.Size = new Size(96, 23);
            textBox_to.TabIndex = 8;
            textBox_to.Text = "0:00:00.00";
            textBox_to.Validated += textBox_to_Validated;
            // 
            // textBox_duration
            // 
            textBox_duration.Location = new Point(268, 87);
            textBox_duration.Name = "textBox_duration";
            textBox_duration.Size = new Size(96, 23);
            textBox_duration.TabIndex = 9;
            textBox_duration.Text = "0";
            textBox_duration.TextAlign = HorizontalAlignment.Right;
            textBox_duration.Visible = false;
            textBox_duration.Validated += textBox_duration_Validated;
            // 
            // btn_Start
            // 
            btn_Start.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btn_Start.Location = new Point(326, 187);
            btn_Start.Name = "btn_Start";
            btn_Start.Size = new Size(148, 33);
            btn_Start.TabIndex = 17;
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
            checkBox_useGC.TabIndex = 11;
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
            checkBox_durationMode.TabIndex = 10;
            checkBox_durationMode.Text = "Duration mode";
            checkBox_durationMode.UseVisualStyleBackColor = true;
            checkBox_durationMode.CheckedChanged += checkBox_durationMode_CheckedChanged;
            // 
            // label_Title
            // 
            label_Title.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label_Title.AutoSize = true;
            label_Title.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            label_Title.Location = new Point(302, 9);
            label_Title.Name = "label_Title";
            label_Title.Size = new Size(197, 45);
            label_Title.TabIndex = 14;
            label_Title.Text = "FFmpeg GUI";
            // 
            // label_Author
            // 
            label_Author.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label_Author.AutoSize = true;
            label_Author.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label_Author.Location = new Point(704, 210);
            label_Author.Name = "label_Author";
            label_Author.Size = new Size(88, 13);
            label_Author.TabIndex = 15;
            label_Author.Text = "GuiguiBlocCraft";
            label_Author.TextAlign = ContentAlignment.TopRight;
            label_Author.Click += label_Author_Click;
            // 
            // label_createdBy
            // 
            label_createdBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label_createdBy.AutoSize = true;
            label_createdBy.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label_createdBy.Location = new Point(646, 210);
            label_createdBy.Name = "label_createdBy";
            label_createdBy.Size = new Size(62, 13);
            label_createdBy.TabIndex = 16;
            label_createdBy.Text = "Created by";
            label_createdBy.TextAlign = ContentAlignment.TopRight;
            // 
            // lblPagination
            // 
            lblPagination.AutoSize = true;
            lblPagination.Location = new Point(8, 122);
            lblPagination.Name = "lblPagination";
            lblPagination.Size = new Size(59, 15);
            lblPagination.TabIndex = 9;
            lblPagination.Text = "Split 0 / 0";
            // 
            // btnPagePrev
            // 
            btnPagePrev.Location = new Point(93, 118);
            btnPagePrev.Name = "btnPagePrev";
            btnPagePrev.Size = new Size(75, 23);
            btnPagePrev.TabIndex = 10;
            btnPagePrev.Text = "Previous";
            btnPagePrev.UseVisualStyleBackColor = true;
            btnPagePrev.Click += btnPagePrev_Click;
            // 
            // btnPageNext
            // 
            btnPageNext.Location = new Point(174, 118);
            btnPageNext.Name = "btnPageNext";
            btnPageNext.Size = new Size(75, 23);
            btnPageNext.TabIndex = 11;
            btnPageNext.Text = "Next";
            btnPageNext.UseVisualStyleBackColor = true;
            btnPageNext.Click += btnPageNext_Click;
            // 
            // btnAddList
            // 
            btnAddList.Location = new Point(255, 118);
            btnAddList.Name = "btnAddList";
            btnAddList.Size = new Size(75, 23);
            btnAddList.TabIndex = 12;
            btnAddList.Text = "Add";
            btnAddList.UseVisualStyleBackColor = true;
            btnAddList.Click += btnAddList_Click;
            // 
            // btnRemoveList
            // 
            btnRemoveList.Location = new Point(336, 118);
            btnRemoveList.Name = "btnRemoveList";
            btnRemoveList.Size = new Size(75, 23);
            btnRemoveList.TabIndex = 13;
            btnRemoveList.Text = "Remove";
            btnRemoveList.UseVisualStyleBackColor = true;
            btnRemoveList.Click += btnRemoveList_Click;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 232);
            Controls.Add(btnRemoveList);
            Controls.Add(btnAddList);
            Controls.Add(btnPageNext);
            Controls.Add(btnPagePrev);
            Controls.Add(lblPagination);
            Controls.Add(label_createdBy);
            Controls.Add(label_Author);
            Controls.Add(label_Title);
            Controls.Add(label4);
            Controls.Add(checkBox_durationMode);
            Controls.Add(checkBox_useGC);
            Controls.Add(btn_Start);
            Controls.Add(textBox_duration);
            Controls.Add(label3);
            Controls.Add(textBox_to);
            Controls.Add(textBox_from);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btn_openFile);
            Controls.Add(textBox_file);
            Controls.Add(lblFile);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FFmpeg GUI";
            DragDrop += Form1_DragDrop;
            DragEnter += Form1_DragEnter;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFile;
        private TextBox textBox_file;
        private Button btn_openFile;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox textBox_from;
        private TextBox textBox_to;
        private TextBox textBox_duration;
        private Button btn_Start;
        private CheckBox checkBox_useGC;
        private CheckBox checkBox_durationMode;
        private OpenFileDialog openFileDialog;
        private Label label_Title;
        private Label label_Author;
        private Label label_createdBy;
        private Label lblPagination;
        private Button btnPagePrev;
        private Button btnPageNext;
        private Button btnAddList;
        private Button btnRemoveList;
    }
}