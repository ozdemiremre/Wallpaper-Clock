using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace WFA_WallpaperClock
{

    public partial class MainForm : Form
    {
        public Font font = DefaultFont;
        public Color color = DefaultForeColor;
        string WallpaperPth = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Wallpaper", 0).ToString(); // Get the wallpaper path.
        double ratio = Convert.ToDouble(Screen.PrimaryScreen.Bounds.Height) / Convert.ToDouble(Screen.PrimaryScreen.Bounds.Width);

        string BurntWallpaperPath = null;

        string selectedFolderPath = null;
        Point startpoint;
        Point lastpoint;
        float fontSize = 96f;
        System.Drawing.Graphics formGraphics;
        bool isDrawing = false;
        Rectangle theRectangle = new Rectangle(new Point(0, 0), new Size(0, 0));

        [DllImport("user32.dll")]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);
        const uint SPI_SETDESKWALLPAPER = 0x14;
        const uint SPIF_UPDATEINIFILE = 0x01;

        public MainForm()
        {
            Settings.CreateDirectory();
            Settings.CreateSettings();
            InitializeComponent();

            pictureBox1.Width = Convert.ToInt32(Convert.ToDouble(pictureBox1.Height) / ratio);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;


            font = new Font(Settings.ReadSetting(Settings.settings.font), fontSize);
            buttonSelectFont.Font = new Font(font.Name, 8f);

            color = Color.FromArgb(Convert.ToInt32(Settings.ReadSetting(Settings.settings.color)));
            buttonSelectColor.BackColor = color;

            selectedFolderPath = Settings.ReadSetting(Settings.settings.wallpaperFolderDirectory);            
            numericWallpaperTime.Value = Convert.ToInt32(Settings.ReadSetting(Settings.settings.minuteOfChangeWallpaper));
        }

        private void buttonChooseFont_Click(object sender, EventArgs e)
        {

            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                font = new Font(fontDialog1.Font.Name, 96);
                buttonSelectFont.Font = new Font(fontDialog1.Font.Name, buttonSelectFont.Font.Size);

                Settings.ChangeSetting(Settings.settings.font, buttonSelectFont.Font.Name);
            }
        }

        private void buttonSelectColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                color = colorDialog1.Color;
                buttonSelectColor.BackColor = color;

                buttonSelectColor.ForeColor = HSL.GetComplementaryColor(color);

                Settings.ChangeSetting(Settings.settings.color, color.ToArgb().ToString());
            }
        }
        private void buttonResize_Click(object sender, EventArgs e)
        {
            //string WallpaperPth = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Wallpaper", 0).ToString(); // Get the wallpaper path.
            //string WallpaperStyle = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WallpaperStyle", 0).ToString();

            //pictureBox1.Width = Convert.ToInt32(Convert.ToDouble(pictureBox1.Height) / ratio);
            //pictureBox1.LoadAsync(WallpaperPth);
            //Bitmap btmp = new Bitmap(WallpaperPth);
            //Graphics GrapBitmap = Graphics.FromImage(btmp);

            //pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            //buttonBurnClock.Enabled = true;
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            startpoint = e.Location;

            theRectangle.Width = 0;
            theRectangle.Height = 0;

            isDrawing = true;
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
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;

            lastpoint.X = e.X;
            lastpoint.Y = e.Y;

            pictureBox1.Refresh();

            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddString(DateTime.Now.ToShortTimeString(), font.FontFamily, (int)font.Style, fontSize, theRectangle, new StringFormat());
            formGraphics.DrawPath(new Pen(HSL.GetComplementaryColor(color), 3f), path);
            formGraphics.FillPath(new SolidBrush(Color.FromArgb(255, color)), path);

            labelState.Text = e.Location.X.ToString() + " , " + e.Location.Y.ToString();
        }

        private void buttonBurnClock_Click(object sender, EventArgs e)
        {
            Bitmap wallpaper = new Bitmap(pictureBox1.Image);
            Graphics wallpaperGraph = Graphics.FromImage(wallpaper);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            Point relativePoint = new Point((wallpaper.Width * startpoint.X) / pictureBox1.Width, (wallpaper.Height * startpoint.Y) / pictureBox1.Height);

            path.AddString(DateTime.Now.ToShortTimeString(), font.FontFamily, (int)font.Style, (float)(fontSize * Convert.ToDouble(wallpaper.Width) / Convert.ToDouble(pictureBox1.Width)), relativePoint, new StringFormat());
            wallpaperGraph.DrawPath(new Pen(HSL.GetComplementaryColor(color), 3f), path);
            wallpaperGraph.FillPath(new SolidBrush(Color.FromArgb(255, color)), path);
        }

        int timerCounter = 0;

        private void timer1_Tick(object sender, EventArgs e)    //Every minute.
        {
            timerCounter++;

            if (timerCounter == Convert.ToInt32(numericWallpaperTime.Value))
            {
                timerCounter = 0;

                if (System.IO.Directory.Exists(selectedFolderPath) && selectedFolderPath != null)     //Check f folder exists, folderPath string != null
                {                                                                                                                                                                                //User is going to *select* a folder. So it already exists. Don't think I need to check again.
                    Random rnd = new Random(DateTime.Now.Millisecond);

                    DirectoryInfo dirInfo = new DirectoryInfo(selectedFolderPath);
                    FileInfo[] fileInfoJPG = dirInfo.GetFiles("*.jpg");
                    FileInfo[] fileInfoPNG = dirInfo.GetFiles("*.png");
                    FileInfo wallpaperFile;

                    int JPGorPNG;

                    if (fileInfoJPG.Length == 0 && fileInfoPNG.Length == 0)
                    {
                        MessageBox.Show("No images found at the selected path.\n Did you move images from the folder?", "ERROR", MessageBoxButtons.OK);
                        return;
                    }

                    if (fileInfoJPG.Length == 0)    //Because I checked if the folder has JPG or PNG files, it has at least a JPG or a PNG.
                        JPGorPNG = 1;

                    else
                        JPGorPNG = 0;

                    if (fileInfoJPG.Length != 0 && fileInfoPNG.Length != 0)
                        JPGorPNG = rnd.Next(0, 2);

                    if (JPGorPNG == 0)
                        wallpaperFile = fileInfoJPG[rnd.Next(0, fileInfoJPG.Length)];

                    else
                        wallpaperFile = fileInfoPNG[rnd.Next(0, fileInfoPNG.Length)];

                    pictureBox1.Image = Image.FromFile(wallpaperFile.FullName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                    pictureBox1.LoadAsync(wallpaperFile.FullName);

                    SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, burnNewWallpaper(wallpaperFile.FullName), SPIF_UPDATEINIFILE);
                }
            }
        }

        private void buttonSelectFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                selectedFolderPath = folderBrowserDialog1.SelectedPath;

                DirectoryInfo dirInfo = new DirectoryInfo(selectedFolderPath);
                FileInfo[] fileInfoJPG = dirInfo.GetFiles("*.jpg");
                FileInfo[] fileInfoPNG = dirInfo.GetFiles("*.png");

                if (fileInfoJPG.Length == 0 && fileInfoPNG.Length == 0)
                {
                    MessageBox.Show("Please select a folder with PNG or JPG image(s) in it.", "ERROR", MessageBoxButtons.OK);
                    selectedFolderPath = null;
                    return;
                }
                else
                    Settings.ChangeSetting(Settings.settings.wallpaperFolderDirectory, selectedFolderPath);

            }


        }

        private string burnNewWallpaper(string originalWallpaperPath)   //Burns image with current time and returns the path.
        {
            Bitmap wallpaper = new Bitmap(pictureBox1.Image);
            Graphics wallpaperGraph = Graphics.FromImage(wallpaper);
            wallpaperGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            Point relativePoint = new Point((wallpaper.Width * startpoint.X) / pictureBox1.Width, (wallpaper.Height * startpoint.Y) / pictureBox1.Height);

            path.AddString(DateTime.Now.ToShortTimeString(), font.FontFamily, (int)font.Style, (float)(fontSize * Convert.ToDouble(wallpaper.Width) / Convert.ToDouble(pictureBox1.Width)), relativePoint, new StringFormat());
            wallpaperGraph.DrawPath(new Pen(HSL.GetComplementaryColor(color), 3f), path);
            wallpaperGraph.FillPath(new SolidBrush(Color.FromArgb(255, color)), path);
            BurntWallpaperPath = Settings.rootDirectory + "\\wallpaper.jpg";
            wallpaper.Save(BurntWallpaperPath);

            return BurntWallpaperPath;
        }

        private void numericWallpaperTime_ValueChanged(object sender, EventArgs e)
        {
            Settings.ChangeSetting(Settings.settings.minuteOfChangeWallpaper, numericWallpaperTime.Value.ToString());
        }
    }

}
