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
    static class Wallpaper
    {

        static public FileInfo wallpaperFile;
        //string WallpaperPth = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Wallpaper", 0).ToString(); // Get the wallpaper path.
        static public string selectedFolderPath = null;

        static Random rnd = new Random(DateTime.Now.Millisecond);
        static public Font font;
        static public Color color;
        static public float fontSize = 96f;
        static string BurntWallpaperPath = null;
        static public Point startpoint;
        static public Rectangle pictureBoxRectangle;
        static Wallpaper()
        {
            if (!String.IsNullOrWhiteSpace(Settings.ReadSetting(Settings.settings.lastWallpaperLocation))) //If a wallpaper is selected from before.
            {
                wallpaperFile = new FileInfo(Settings.ReadSetting(Settings.settings.lastWallpaperLocation));
            }
            else
                wallpaperFile = new FileInfo(getNewWallpaper(false));

            selectedFolderPath = Settings.ReadSetting(Settings.settings.wallpaperFolderDirectory);
        }
        static public string burnNewWallpaper(string originalWallpaperPath)   //Burns image with current time and returns the path of the new picture.
        {
            Bitmap wallpaper = new Bitmap(originalWallpaperPath);
            Graphics wallpaperGraph = Graphics.FromImage(wallpaper);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            wallpaperGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            wallpaperGraph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            path.FillMode = System.Drawing.Drawing2D.FillMode.Winding;

            Point relativePoint = new Point((wallpaper.Width * startpoint.X) / pictureBoxRectangle.Width, (wallpaper.Height * startpoint.Y) / pictureBoxRectangle.Height);

            path.AddString(DateTime.Now.ToShortTimeString(), font.FontFamily, (int)font.Style, (float)(fontSize * Convert.ToDouble(wallpaper.Width) / Convert.ToDouble(pictureBoxRectangle.Width)), relativePoint, new StringFormat());
            wallpaperGraph.DrawPath(new Pen(HSL.GetComplementaryColor(color), 3f), path);
            wallpaperGraph.FillPath(new SolidBrush(Color.FromArgb(255, color)), path);
            BurntWallpaperPath = Settings.rootDirectory + "\\wallpaper.png";

            wallpaper.Save(BurntWallpaperPath);

            return BurntWallpaperPath;
        }

        static public string getNewWallpaper(bool isShuffle) //IsShuffle not working atm.
        {
            if (System.IO.Directory.Exists(selectedFolderPath) && selectedFolderPath != null)     //Check f folder exists, folderPath string != null
            {                                                                                                                                              //User is going to *select* a folder. So it already exists. Don't think I need to check again.

#if DEBUG
                int seed = DateTime.Now.Millisecond;
#endif 
                DirectoryInfo dirInfo = new DirectoryInfo(selectedFolderPath);
                FileInfo[] fileInfoJPG = dirInfo.GetFiles("*.jpg");
                FileInfo[] fileInfoPNG = dirInfo.GetFiles("*.png");

                int JPGorPNG;

                if (fileInfoJPG.Length == 0 && fileInfoPNG.Length == 0)
                {
                    MessageBox.Show("No images found at the selected path.\n Did you move images from the folder?", "ERROR", MessageBoxButtons.OK);
                    return null;
                }

                if (fileInfoJPG.Length == 0)    //Because I checked if the folder has JPG or PNG files, it has at least a JPG or a PNG.
                    JPGorPNG = 1;

                else
                    JPGorPNG = 0;

                

                if (fileInfoJPG.Length != 0 && fileInfoPNG.Length != 0)
                    JPGorPNG = rnd.Next(0, 2);
                

                if (JPGorPNG == 0)
                    wallpaperFile = fileInfoJPG[rnd.Next(fileInfoJPG.Length)];

                else
                    wallpaperFile = fileInfoPNG[rnd.Next(fileInfoPNG.Length)];



#if DEBUG
                Console.WriteLine("File Info JPG size: " + fileInfoJPG.Length);
                Console.WriteLine("File Info PNG size: " + fileInfoPNG.Length);
                Console.WriteLine("JPG or PNG: " + JPGorPNG);
                Console.WriteLine("Seed in milisecond is: " + seed);
#endif

                Settings.ChangeSetting(Settings.settings.lastWallpaperLocation, wallpaperFile.FullName);

                return wallpaperFile.FullName;
            }

            else
                return null;

        }
    }
}
