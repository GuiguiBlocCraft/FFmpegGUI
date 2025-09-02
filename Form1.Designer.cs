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
            lblInfo = new Label();
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            presetToolStripMenuItem = new ToolStripMenuItem();
            ultrafastToolStripMenuItem = new ToolStripMenuItem();
            superfastToolStripMenuItem = new ToolStripMenuItem();
            veryFastToolStripMenuItem = new ToolStripMenuItem();
            fasterToolStripMenuItem = new ToolStripMenuItem();
            fastToolStripMenuItem = new ToolStripMenuItem();
            mediumToolStripMenuItem = new ToolStripMenuItem();
            slowToolStripMenuItem = new ToolStripMenuItem();
            slowerToolStripMenuItem = new ToolStripMenuItem();
            panelPlayerVideo = new Panel();
            panelPlayerButtons = new Panel();
            btn_TakePositionEnd = new Button();
            btn_TakePositionStart = new Button();
            trackBar_Player = new TrackBar();
            label_Position = new Label();
            btn_VideoPlay = new Button();
            menuStrip1.SuspendLayout();
            panelPlayerButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar_Player).BeginInit();
            SuspendLayout();
            // 
            // lblFile
            // 
            lblFile.AutoSize = true;
            lblFile.Location = new Point(12, 81);
            lblFile.Name = "lblFile";
            lblFile.Size = new Size(31, 15);
            lblFile.TabIndex = 0;
            lblFile.Text = "File :";
            // 
            // textBox_file
            // 
            textBox_file.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox_file.Location = new Point(49, 78);
            textBox_file.Name = "textBox_file";
            textBox_file.Size = new Size(701, 23);
            textBox_file.TabIndex = 1;
            // 
            // btn_openFile
            // 
            btn_openFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn_openFile.Location = new Point(756, 78);
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
            label1.Location = new Point(8, 110);
            label1.Name = "label1";
            label1.Size = new Size(60, 15);
            label1.TabIndex = 3;
            label1.Text = "Start from";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(176, 110);
            label2.Name = "label2";
            label2.Size = new Size(18, 15);
            label2.TabIndex = 4;
            label2.Text = "to";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(176, 110);
            label3.Name = "label3";
            label3.Size = new Size(91, 15);
            label3.TabIndex = 5;
            label3.Text = "with duration in";
            label3.Visible = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(367, 111);
            label4.Name = "label4";
            label4.Size = new Size(50, 15);
            label4.TabIndex = 6;
            label4.Text = "seconds";
            label4.Visible = false;
            // 
            // textBox_from
            // 
            textBox_from.Location = new Point(74, 107);
            textBox_from.Name = "textBox_from";
            textBox_from.Size = new Size(96, 23);
            textBox_from.TabIndex = 7;
            textBox_from.Text = "0:00:00.00";
            textBox_from.Validated += textBox_from_Validated;
            // 
            // textBox_to
            // 
            textBox_to.Location = new Point(197, 107);
            textBox_to.Name = "textBox_to";
            textBox_to.Size = new Size(96, 23);
            textBox_to.TabIndex = 8;
            textBox_to.Text = "0:00:00.00";
            textBox_to.Validated += textBox_to_Validated;
            // 
            // textBox_duration
            // 
            textBox_duration.Location = new Point(268, 107);
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
            btn_Start.Anchor = AnchorStyles.Bottom;
            btn_Start.Location = new Point(326, 682);
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
            checkBox_useGC.Location = new Point(12, 696);
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
            checkBox_durationMode.Location = new Point(12, 671);
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
            label_Title.Font = new Font("Segoe UI", 24F);
            label_Title.Location = new Point(302, 29);
            label_Title.Name = "label_Title";
            label_Title.Size = new Size(197, 45);
            label_Title.TabIndex = 14;
            label_Title.Text = "FFmpeg GUI";
            // 
            // label_Author
            // 
            label_Author.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label_Author.AutoSize = true;
            label_Author.Font = new Font("Segoe UI", 8.25F);
            label_Author.Location = new Point(704, 705);
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
            label_createdBy.Font = new Font("Segoe UI", 8.25F);
            label_createdBy.Location = new Point(646, 705);
            label_createdBy.Name = "label_createdBy";
            label_createdBy.Size = new Size(62, 13);
            label_createdBy.TabIndex = 16;
            label_createdBy.Text = "Created by";
            label_createdBy.TextAlign = ContentAlignment.TopRight;
            // 
            // lblPagination
            // 
            lblPagination.AutoSize = true;
            lblPagination.Location = new Point(8, 142);
            lblPagination.Name = "lblPagination";
            lblPagination.Size = new Size(56, 15);
            lblPagination.TabIndex = 9;
            lblPagination.Text = "Split 0 / 0";
            // 
            // btnPagePrev
            // 
            btnPagePrev.Location = new Point(93, 138);
            btnPagePrev.Name = "btnPagePrev";
            btnPagePrev.Size = new Size(75, 23);
            btnPagePrev.TabIndex = 10;
            btnPagePrev.Text = "Previous";
            btnPagePrev.UseVisualStyleBackColor = true;
            btnPagePrev.Click += btnPagePrev_Click;
            // 
            // btnPageNext
            // 
            btnPageNext.Location = new Point(174, 138);
            btnPageNext.Name = "btnPageNext";
            btnPageNext.Size = new Size(75, 23);
            btnPageNext.TabIndex = 11;
            btnPageNext.Text = "Next";
            btnPageNext.UseVisualStyleBackColor = true;
            btnPageNext.Click += btnPageNext_Click;
            // 
            // btnAddList
            // 
            btnAddList.Location = new Point(255, 138);
            btnAddList.Name = "btnAddList";
            btnAddList.Size = new Size(75, 23);
            btnAddList.TabIndex = 12;
            btnAddList.Text = "Add";
            btnAddList.UseVisualStyleBackColor = true;
            btnAddList.Click += btnAddList_Click;
            // 
            // btnRemoveList
            // 
            btnRemoveList.Location = new Point(336, 138);
            btnRemoveList.Name = "btnRemoveList";
            btnRemoveList.Size = new Size(75, 23);
            btnRemoveList.TabIndex = 13;
            btnRemoveList.Text = "Remove";
            btnRemoveList.UseVisualStyleBackColor = true;
            btnRemoveList.Click += btnRemoveList_Click;
            // 
            // lblInfo
            // 
            lblInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblInfo.AutoSize = true;
            lblInfo.Location = new Point(275, 663);
            lblInfo.MinimumSize = new Size(250, 0);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(250, 15);
            lblInfo.TabIndex = 18;
            lblInfo.TextAlign = ContentAlignment.TopCenter;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, optionsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 19;
            menuStrip1.Text = "menuStrip";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(12, 20);
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { presetToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(61, 20);
            optionsToolStripMenuItem.Text = "&Options";
            // 
            // presetToolStripMenuItem
            // 
            presetToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ultrafastToolStripMenuItem, superfastToolStripMenuItem, veryFastToolStripMenuItem, fasterToolStripMenuItem, fastToolStripMenuItem, mediumToolStripMenuItem, slowToolStripMenuItem, slowerToolStripMenuItem });
            presetToolStripMenuItem.Name = "presetToolStripMenuItem";
            presetToolStripMenuItem.Size = new Size(180, 22);
            presetToolStripMenuItem.Text = "&Preset";
            // 
            // ultrafastToolStripMenuItem
            // 
            ultrafastToolStripMenuItem.Name = "ultrafastToolStripMenuItem";
            ultrafastToolStripMenuItem.Size = new Size(180, 22);
            ultrafastToolStripMenuItem.Text = "&Ultra fast";
            ultrafastToolStripMenuItem.Click += ultrafastToolStripMenuItem_Click;
            // 
            // superfastToolStripMenuItem
            // 
            superfastToolStripMenuItem.Name = "superfastToolStripMenuItem";
            superfastToolStripMenuItem.Size = new Size(180, 22);
            superfastToolStripMenuItem.Text = "&Super fast";
            superfastToolStripMenuItem.Click += superfastToolStripMenuItem_Click;
            // 
            // veryFastToolStripMenuItem
            // 
            veryFastToolStripMenuItem.Name = "veryFastToolStripMenuItem";
            veryFastToolStripMenuItem.Size = new Size(180, 22);
            veryFastToolStripMenuItem.Text = "&Very fast";
            veryFastToolStripMenuItem.Click += veryFastToolStripMenuItem_Click;
            // 
            // fasterToolStripMenuItem
            // 
            fasterToolStripMenuItem.Name = "fasterToolStripMenuItem";
            fasterToolStripMenuItem.Size = new Size(180, 22);
            fasterToolStripMenuItem.Text = "F&aster";
            fasterToolStripMenuItem.Click += fasterToolStripMenuItem_Click;
            // 
            // fastToolStripMenuItem
            // 
            fastToolStripMenuItem.Name = "fastToolStripMenuItem";
            fastToolStripMenuItem.Size = new Size(180, 22);
            fastToolStripMenuItem.Text = "&Fast";
            fastToolStripMenuItem.Click += fastToolStripMenuItem_Click;
            // 
            // mediumToolStripMenuItem
            // 
            mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
            mediumToolStripMenuItem.Size = new Size(180, 22);
            mediumToolStripMenuItem.Text = "&Medium";
            mediumToolStripMenuItem.Click += mediumToolStripMenuItem_Click;
            // 
            // slowToolStripMenuItem
            // 
            slowToolStripMenuItem.Name = "slowToolStripMenuItem";
            slowToolStripMenuItem.Size = new Size(180, 22);
            slowToolStripMenuItem.Text = "&Slow";
            slowToolStripMenuItem.Click += slowToolStripMenuItem_Click;
            // 
            // slowerToolStripMenuItem
            // 
            slowerToolStripMenuItem.Name = "slowerToolStripMenuItem";
            slowerToolStripMenuItem.Size = new Size(180, 22);
            slowerToolStripMenuItem.Text = "S&lower";
            slowerToolStripMenuItem.Click += slowerToolStripMenuItem_Click;
            // 
            // panelPlayerVideo
            // 
            panelPlayerVideo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelPlayerVideo.Location = new Point(12, 177);
            panelPlayerVideo.Name = "panelPlayerVideo";
            panelPlayerVideo.Size = new Size(776, 400);
            panelPlayerVideo.TabIndex = 20;
            // 
            // panelPlayerButtons
            // 
            panelPlayerButtons.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelPlayerButtons.Controls.Add(btn_TakePositionEnd);
            panelPlayerButtons.Controls.Add(btn_TakePositionStart);
            panelPlayerButtons.Controls.Add(trackBar_Player);
            panelPlayerButtons.Controls.Add(label_Position);
            panelPlayerButtons.Controls.Add(btn_VideoPlay);
            panelPlayerButtons.Location = new Point(12, 581);
            panelPlayerButtons.Name = "panelPlayerButtons";
            panelPlayerButtons.Size = new Size(776, 79);
            panelPlayerButtons.TabIndex = 21;
            // 
            // btn_TakePositionEnd
            // 
            btn_TakePositionEnd.Anchor = AnchorStyles.Bottom;
            btn_TakePositionEnd.Location = new Point(430, 52);
            btn_TakePositionEnd.Name = "btn_TakePositionEnd";
            btn_TakePositionEnd.Size = new Size(75, 23);
            btn_TakePositionEnd.TabIndex = 2;
            btn_TakePositionEnd.Text = "Take end";
            btn_TakePositionEnd.UseVisualStyleBackColor = true;
            btn_TakePositionEnd.Click += btn_TakePositionEnd_Click;
            // 
            // btn_TakePositionStart
            // 
            btn_TakePositionStart.Anchor = AnchorStyles.Bottom;
            btn_TakePositionStart.Location = new Point(351, 52);
            btn_TakePositionStart.Name = "btn_TakePositionStart";
            btn_TakePositionStart.Size = new Size(75, 23);
            btn_TakePositionStart.TabIndex = 1;
            btn_TakePositionStart.Text = "Take start";
            btn_TakePositionStart.UseVisualStyleBackColor = true;
            btn_TakePositionStart.Click += btn_TakePositionStart_Click;
            // 
            // trackBar_Player
            // 
            trackBar_Player.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            trackBar_Player.Location = new Point(0, 1);
            trackBar_Player.Name = "trackBar_Player";
            trackBar_Player.Size = new Size(775, 45);
            trackBar_Player.TabIndex = 3;
            trackBar_Player.Scroll += trackBar_Player_Scroll;
            // 
            // label_Position
            // 
            label_Position.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label_Position.AutoSize = true;
            label_Position.Location = new Point(7, 55);
            label_Position.Name = "label_Position";
            label_Position.Size = new Size(53, 15);
            label_Position.TabIndex = 4;
            label_Position.Text = "Position:";
            // 
            // btn_VideoPlay
            // 
            btn_VideoPlay.Anchor = AnchorStyles.Bottom;
            btn_VideoPlay.Location = new Point(272, 52);
            btn_VideoPlay.Name = "btn_VideoPlay";
            btn_VideoPlay.Size = new Size(75, 23);
            btn_VideoPlay.TabIndex = 0;
            btn_VideoPlay.Text = "Play";
            btn_VideoPlay.UseVisualStyleBackColor = true;
            btn_VideoPlay.Click += btn_VideoPlay_Click;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 727);
            Controls.Add(panelPlayerButtons);
            Controls.Add(panelPlayerVideo);
            Controls.Add(lblInfo);
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
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FFmpeg GUI";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            DragDrop += Form1_DragDrop;
            DragEnter += Form1_DragEnter;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panelPlayerButtons.ResumeLayout(false);
            panelPlayerButtons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar_Player).EndInit();
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
        private Label lblInfo;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem presetToolStripMenuItem;
        private ToolStripMenuItem ultrafastToolStripMenuItem;
        private ToolStripMenuItem superfastToolStripMenuItem;
        private ToolStripMenuItem veryFastToolStripMenuItem;
        private ToolStripMenuItem fasterToolStripMenuItem;
        private ToolStripMenuItem fastToolStripMenuItem;
        private ToolStripMenuItem mediumToolStripMenuItem;
        private ToolStripMenuItem slowToolStripMenuItem;
        private ToolStripMenuItem slowerToolStripMenuItem;
        private Panel panelPlayerVideo;
        private Panel panelPlayerButtons;
        private Button btn_VideoPlay;
        private Label label_Position;
        private TrackBar trackBar_Player;
        private Button btn_TakePositionStart;
        private Button btn_TakePositionEnd;
    }
}