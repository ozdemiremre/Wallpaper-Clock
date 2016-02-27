namespace WFA_WallpaperClock
{
    partial class MainForm
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
            if (disposing && (components != null))
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.buttonSelectFont = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.buttonSelectColor = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelState = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.numericWallpaperTime = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.isShuffle = new System.Windows.Forms.CheckBox();
            this.buttonSelectFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.checkBoxStartup = new System.Windows.Forms.CheckBox();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericWallpaperTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSelectFont
            // 
            this.buttonSelectFont.Location = new System.Drawing.Point(13, 13);
            this.buttonSelectFont.Name = "buttonSelectFont";
            this.buttonSelectFont.Size = new System.Drawing.Size(113, 23);
            this.buttonSelectFont.TabIndex = 0;
            this.buttonSelectFont.Text = "Select Font";
            this.buttonSelectFont.UseVisualStyleBackColor = true;
            this.buttonSelectFont.Click += new System.EventHandler(this.buttonChooseFont_Click);
            // 
            // buttonSelectColor
            // 
            this.buttonSelectColor.Location = new System.Drawing.Point(13, 64);
            this.buttonSelectColor.Name = "buttonSelectColor";
            this.buttonSelectColor.Size = new System.Drawing.Size(113, 23);
            this.buttonSelectColor.TabIndex = 1;
            this.buttonSelectColor.Text = "Select Color";
            this.buttonSelectColor.UseVisualStyleBackColor = true;
            this.buttonSelectColor.Click += new System.EventHandler(this.buttonSelectColor_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(198, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(503, 236);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Location = new System.Drawing.Point(136, 236);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(35, 13);
            this.labelState.TabIndex = 4;
            this.labelState.Text = "label1";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 6000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // numericWallpaperTime
            // 
            this.numericWallpaperTime.Location = new System.Drawing.Point(137, 155);
            this.numericWallpaperTime.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericWallpaperTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericWallpaperTime.Name = "numericWallpaperTime";
            this.numericWallpaperTime.ReadOnly = true;
            this.numericWallpaperTime.Size = new System.Drawing.Size(34, 20);
            this.numericWallpaperTime.TabIndex = 6;
            this.numericWallpaperTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericWallpaperTime.ValueChanged += new System.EventHandler(this.numericWallpaperTime_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Change wallpaper every";
            // 
            // isShuffle
            // 
            this.isShuffle.AutoSize = true;
            this.isShuffle.Location = new System.Drawing.Point(13, 191);
            this.isShuffle.Name = "isShuffle";
            this.isShuffle.Size = new System.Drawing.Size(59, 17);
            this.isShuffle.TabIndex = 8;
            this.isShuffle.Text = "Shuffle";
            this.isShuffle.UseVisualStyleBackColor = true;
            this.isShuffle.CheckedChanged += new System.EventHandler(this.isShuffle_CheckedChanged);
            // 
            // buttonSelectFolder
            // 
            this.buttonSelectFolder.Location = new System.Drawing.Point(13, 113);
            this.buttonSelectFolder.Name = "buttonSelectFolder";
            this.buttonSelectFolder.Size = new System.Drawing.Size(113, 23);
            this.buttonSelectFolder.TabIndex = 9;
            this.buttonSelectFolder.Text = "Select Folder";
            this.buttonSelectFolder.UseVisualStyleBackColor = true;
            this.buttonSelectFolder.Click += new System.EventHandler(this.buttonSelectFolder_Click);
            // 
            // checkBoxStartup
            // 
            this.checkBoxStartup.AutoSize = true;
            this.checkBoxStartup.Location = new System.Drawing.Point(13, 215);
            this.checkBoxStartup.Name = "checkBoxStartup";
            this.checkBoxStartup.Size = new System.Drawing.Size(115, 17);
            this.checkBoxStartup.TabIndex = 10;
            this.checkBoxStartup.Text = "Set as AutoStartup";
            this.checkBoxStartup.UseVisualStyleBackColor = true;
            this.checkBoxStartup.CheckedChanged += new System.EventHandler(this.checkBoxStartup_CheckedChanged);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.IncludeSubdirectories = true;
            this.fileSystemWatcher1.NotifyFilter = ((System.IO.NotifyFilters)((System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.DirectoryName)));
            this.fileSystemWatcher1.SynchronizingObject = this;
            this.fileSystemWatcher1.Created += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Created);
            this.fileSystemWatcher1.Deleted += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Deleted);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(713, 261);
            this.Controls.Add(this.checkBoxStartup);
            this.Controls.Add(this.buttonSelectFolder);
            this.Controls.Add(this.isShuffle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericWallpaperTime);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonSelectColor);
            this.Controls.Add(this.buttonSelectFont);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Wallpaper Clock";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericWallpaperTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button buttonSelectColor;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NumericUpDown numericWallpaperTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox isShuffle;
        private System.Windows.Forms.Button buttonSelectFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button buttonSelectFont;
        private System.Windows.Forms.CheckBox checkBoxStartup;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
    }
}

