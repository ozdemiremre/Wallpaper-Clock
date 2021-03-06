﻿using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WFA_WallpaperClock
{

    public partial class MainForm : Form
    {
        public Color color = DefaultForeColor;
        public Font font = DefaultFont;
        double ratio = Convert.ToDouble(Screen.PrimaryScreen.Bounds.Height) / Convert.ToDouble(Screen.PrimaryScreen.Bounds.Width);
        float fontSize = 96f;
        System.Drawing.Graphics formGraphics;
        bool isDrawing = false;
        public Point startpoint;
        Rectangle theRectangle = new Rectangle(new Point(0, 0), new Size(0, 0));

        [DllImport("user32.dll")]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);
        const uint SPI_SETDESKWALLPAPER = 0x14;
        const uint SPIF_UPDATEINIFILE = 0x01;

        public MainForm()
        {
            Settings.CreateDirectory();
            Settings.CreateSettings();

            startpoint.X = Convert.ToInt32(Settings.ReadSetting(Settings.settings.startPointX));
            startpoint.Y = Convert.ToInt32(Settings.ReadSetting(Settings.settings.startPointY));

            InitializeComponent();

            pictureBox1.Width = Convert.ToInt32(Convert.ToDouble(pictureBox1.Height) / ratio);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            color = Color.FromArgb(Convert.ToInt32(Settings.ReadSetting(Settings.settings.color)));
            buttonSelectColor.BackColor = color;

            numericWallpaperTime.Value = Convert.ToInt32(Settings.ReadSetting(Settings.settings.minuteOfChangeWallpaper));

            font = new Font(Settings.ReadSetting(Settings.settings.font), fontSize);
            buttonSelectFont.Font = new Font(font.Name, 8f);

            Wallpaper.font = font;
            Wallpaper.color = color;
            Wallpaper.startpoint = startpoint;
            Wallpaper.pictureBoxRectangle.Height = pictureBox1.Height;
            Wallpaper.pictureBoxRectangle.Width = pictureBox1.Width;

            if (!String.IsNullOrEmpty(Settings.ReadSetting(Settings.settings.wallpaperFolderDirectory)) && !String.IsNullOrWhiteSpace(Settings.ReadSetting(Settings.settings.wallpaperFolderDirectory))) //If wallpaper folder selected from before.
            {
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, Wallpaper.BurnNewWallpaper(Wallpaper.wallpaperFile.FullName), SPIF_UPDATEINIFILE);
                pictureBox1.LoadAsync(Wallpaper.wallpaperFile.FullName);

                fileSystemWatcher1.Path = Wallpaper.selectedFolderPath;
            }

            checkBoxStartup.Checked = Settings.IsStartupCreated();
            isShuffle.Checked = Convert.ToBoolean(Settings.ReadSetting(Settings.settings.shuffle));
            checkBoxMinimized.Checked = Convert.ToBoolean(Settings.ReadSetting(Settings.settings.startMinimized));


        }

        private void buttonChooseFont_Click(object sender, EventArgs e)
        {

            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                font = new Font(fontDialog1.Font.Name, 96);
                buttonSelectFont.Font = new Font(fontDialog1.Font.Name, buttonSelectFont.Font.Size);

                Settings.ChangeSetting(Settings.settings.font, buttonSelectFont.Font.Name);

                Wallpaper.font = this.font;

                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, Wallpaper.BurnNewWallpaper(Wallpaper.wallpaperFile.FullName), SPIF_UPDATEINIFILE);
            }
        }

        private void buttonSelectColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                color = colorDialog1.Color;
                buttonSelectColor.BackColor = color;

                buttonSelectColor.ForeColor = HSL.GetComplementaryColor(color);

                Wallpaper.color = this.color;
                Settings.ChangeSetting(Settings.settings.color, color.ToArgb().ToString());

                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, Wallpaper.BurnNewWallpaper(Wallpaper.wallpaperFile.FullName), SPIF_UPDATEINIFILE);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            startpoint = e.Location;

            theRectangle.Width = 0;
            theRectangle.Height = 0;

            isDrawing = true;

            Settings.ChangeSetting(Settings.settings.startPointX, e.X.ToString());
            Settings.ChangeSetting(Settings.settings.startPointY, e.Y.ToString());

            Wallpaper.startpoint = this.startpoint;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            labelState.Text = e.Location.X.ToString() + " , " + e.Location.Y.ToString();

            if (isDrawing)
            {
                pictureBox1.Refresh();
                Color newColor = Color.FromArgb(125, Color.Gray);

                Control control = (Control)sender;
                formGraphics = control.CreateGraphics();
                formGraphics.FillRectangle(new SolidBrush(newColor), theRectangle);
                formGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                theRectangle.X = startpoint.X;
                theRectangle.Y = startpoint.Y;
                theRectangle.Height = e.Y - startpoint.Y;
                theRectangle.Width = e.X - startpoint.X;

                fontSize = 96f;
                SizeF stringSize = formGraphics.MeasureString(DateTime.Now.ToShortTimeString(), new Font(font.Name, fontSize));

                if (theRectangle.Width > 15 && theRectangle.Height > 15)
                {
                    while (stringSize.Width > theRectangle.Width || stringSize.Height > theRectangle.Height)
                    {
                        stringSize = formGraphics.MeasureString(DateTime.Now.ToShortTimeString(), new Font(font.Name, fontSize));
                        fontSize -= 0.05f;
                    }

                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                    path.AddString(DateTime.Now.ToShortTimeString(), font.FontFamily, (int)font.Style, fontSize, theRectangle, new StringFormat());
                    formGraphics.DrawPath(new Pen(HSL.GetComplementaryColor(color), 3f), path);
                    formGraphics.FillPath(new SolidBrush(Color.FromArgb(255, color)), path);

                    Settings.ChangeSetting(Settings.settings.fontSize, fontSize.ToString());
                    Wallpaper.fontSize = this.fontSize;
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;

            pictureBox1.Refresh();

            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddString(DateTime.Now.ToShortTimeString(), font.FontFamily, (int)font.Style, fontSize, theRectangle, new StringFormat());
            formGraphics.DrawPath(new Pen(HSL.GetComplementaryColor(color), 3f), path);
            formGraphics.FillPath(new SolidBrush(Color.FromArgb(255, color)), path);

            labelState.Text = e.Location.X.ToString() + " , " + e.Location.Y.ToString();

            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, Wallpaper.BurnNewWallpaper(Wallpaper.wallpaperFile.FullName), SPIF_UPDATEINIFILE);
        }

        int timerCounter = 0;

        private void timer1_Tick(object sender, EventArgs e)    //Every minute.
        {
            timerCounter++;

            if (!String.IsNullOrEmpty(Settings.ReadSetting(Settings.settings.wallpaperFolderDirectory)) && !String.IsNullOrWhiteSpace(Settings.ReadSetting(Settings.settings.wallpaperFolderDirectory))) //Is wallpaperFolderDirectory selected && that selected directory is NullORWhiteSpace
            {
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, Wallpaper.BurnNewWallpaper(Wallpaper.wallpaperFile.FullName), SPIF_UPDATEINIFILE);
                pictureBox1.LoadAsync(Wallpaper.wallpaperFile.FullName);

                if (timerCounter == Convert.ToInt32(numericWallpaperTime.Value)) //If it's time to get a new wallpaper.
                {
                    timerCounter = 0;

                    string path = Wallpaper.GetNewWallpaper(isShuffle.Checked);
                    if (path != null)
                    {
                        pictureBox1.Image = Image.FromFile(Wallpaper.wallpaperFile.FullName);
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                        pictureBox1.LoadAsync(path);

                        SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, Wallpaper.BurnNewWallpaper(Wallpaper.wallpaperFile.FullName), SPIF_UPDATEINIFILE);
                    }
                }

                else if (timerCounter > Convert.ToInt32(numericWallpaperTime.Value)) //Stop timerCounter going over the max minutes.
                    timerCounter = 0;

            }


        }

        private void buttonSelectFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Wallpaper.selectedFolderPath = folderBrowserDialog1.SelectedPath;

                string[] fileInfoJPG = Directory.GetFiles(Wallpaper.selectedFolderPath, "*.jpg", SearchOption.AllDirectories);
                string[] fileInfoPNG = Directory.GetFiles(Wallpaper.selectedFolderPath, "*.png", SearchOption.AllDirectories);

                if (fileInfoJPG.Length == 0 && fileInfoPNG.Length == 0)
                {
                    MessageBox.Show("Please select a folder with PNG or JPG image(s) in it.", "ERROR", MessageBoxButtons.OK);
                    Wallpaper.selectedFolderPath = null;
                    return;
                }
                else
                {
                    Settings.ChangeSetting(Settings.settings.wallpaperFolderDirectory, Wallpaper.selectedFolderPath);
                    Wallpaper.fileInfoJPG = Directory.GetFiles(Wallpaper.selectedFolderPath, "*.jpg", SearchOption.AllDirectories);
                    Wallpaper.fileInfoPNG = Directory.GetFiles(Wallpaper.selectedFolderPath, "*.png", SearchOption.AllDirectories);
                    string path = Wallpaper.GetNewWallpaper(isShuffle.Checked);

                    pictureBox1.Image = Image.FromFile(Wallpaper.wallpaperFile.FullName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                    if (path != null)
                    {
                        pictureBox1.LoadAsync(path);

                        SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, Wallpaper.BurnNewWallpaper(Wallpaper.wallpaperFile.FullName), SPIF_UPDATEINIFILE);
                    }

                }
            }


        }

        private void numericWallpaperTime_ValueChanged(object sender, EventArgs e)
        {
            Settings.ChangeSetting(Settings.settings.minuteOfChangeWallpaper, numericWallpaperTime.Value.ToString());
        }

        private void checkBoxStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxStartup.Checked)
            {
                if (!Settings.IsStartupCreated())
                    Settings.CreateStartupShortcut();
            }

            else
                Settings.DeleteStartupShortcut();

        }

        private void isShuffle_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ChangeSetting(Settings.settings.shuffle, isShuffle.Checked.ToString());
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, Settings.ReadSetting(Settings.settings.lastWallpaperLocation), SPIF_UPDATEINIFILE);
        }

        private void fileSystemWatcher1_Deleted(object sender, FileSystemEventArgs e)
        {
            Wallpaper.ScanSelectedFolder();
        }

        private void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            Wallpaper.ScanSelectedFolder();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void buttonSelectFolder_MouseHover(object sender, EventArgs e)
        {
            Button b = sender as Button;

            toolTip1.Show("Folder that will be scanned and pictures will be used inside of it.", b, 2000);
        }

        private void buttonSelectColor_MouseHover(object sender, EventArgs e)
        {
            Button b = sender as Button;

            toolTip1.Show("Color that the clock will be shown in.", b, 2000);
        }

        private void buttonSelectFont_MouseHover(object sender, EventArgs e)
        {
            Button b = sender as Button;

            toolTip1.Show("Font that the clock will be shown in.", b, 2000);
        }

        private void checkBoxMinimized_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ChangeSetting(Settings.settings.startMinimized, checkBoxStartup.Checked.ToString());
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (checkBoxMinimized.Checked == true)
            {
                this.WindowState = FormWindowState.Minimized;
                Hide();
            }
        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            Wallpaper.ScanSelectedFolder();
        }
    }

}
