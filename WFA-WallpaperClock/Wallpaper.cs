﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WFA_WallpaperClock
{
    static class Wallpaper
    {

        public static FileInfo wallpaperFile;
        public static string selectedFolderPath = null;

        static Random rnd = new Random(DateTime.Now.Millisecond);
        public static Font font;
        public static Color color;
        public static float fontSize = 96f;
        static string BurntWallpaperPath = null;
        public static Point startpoint;
        public static Rectangle pictureBoxRectangle;
        public static string[] fileInfoPNG;
        public static string[] fileInfoJPG;

        [DllImport("user32.dll")]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

        static Wallpaper()
        {
            if (!String.IsNullOrEmpty(Settings.ReadSetting(Settings.settings.lastWallpaperLocation))) //If a wallpaper is selected from before.
                wallpaperFile = new FileInfo(Settings.ReadSetting(Settings.settings.lastWallpaperLocation));

            if (!String.IsNullOrEmpty(Settings.ReadSetting(Settings.settings.wallpaperFolderDirectory)) && !String.IsNullOrWhiteSpace(Settings.ReadSetting(Settings.settings.wallpaperFolderDirectory))) //If wallpaper folder selected from before.
            {
                selectedFolderPath = Settings.ReadSetting(Settings.settings.wallpaperFolderDirectory);
                fileInfoJPG = Directory.GetFiles(selectedFolderPath, "*.jpg", SearchOption.AllDirectories);
                fileInfoPNG = Directory.GetFiles(selectedFolderPath, "*.png", SearchOption.AllDirectories);
            }

            if (!String.IsNullOrEmpty(Settings.ReadSetting(Settings.settings.fontSize)) && !String.IsNullOrWhiteSpace(Settings.ReadSetting(Settings.settings.fontSize))) //If this program has run before and font size has been calculated.
                fontSize = float.Parse(Settings.ReadSetting(Settings.settings.fontSize));


        }

        /// <summary>
        /// Bakes the current time on the original wallpaper and returns the path of the baked wallpaper.
        /// </summary>
        /// <param name="originalWallpaperPath">The full path of the original wallpaper.</param>
        /// <returns></returns>
        public static string BurnNewWallpaper(string originalWallpaperPath)
        {
            Image wallpaper = Image.FromFile(originalWallpaperPath);
            Bitmap bufferBtmp = new Bitmap(wallpaper);
            Graphics wallpaperGraph = Graphics.FromImage(bufferBtmp);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            wallpaperGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            wallpaperGraph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            path.FillMode = System.Drawing.Drawing2D.FillMode.Winding;

            path.AddString(DateTime.Now.ToShortTimeString(), font.FontFamily, (int)font.Style, (float)(fontSize * Convert.ToDouble(wallpaper.Width) / Convert.ToDouble(pictureBoxRectangle.Width)), CalculateTheRelativePoint(), new StringFormat());
            wallpaperGraph.DrawPath(new Pen(HSL.GetComplementaryColor(color), 3f), path);
            wallpaperGraph.FillPath(new SolidBrush(Color.FromArgb(255, color)), path);
            BurntWallpaperPath = Settings.rootDirectory + "\\wallpaper.png";

            bufferBtmp.Save(BurntWallpaperPath);
            bufferBtmp.Dispose();
            wallpaper.Dispose();
            wallpaperGraph.Dispose();

            return BurntWallpaperPath;
        }

        /// <summary>
        /// Calculates the relative Top-Left point of the rectangle, depending of the resolution of the wallpaper. W.I.P.
        /// </summary>
        /// <returns></returns>
        public static Point CalculateTheRelativePoint() // This calculates as if the wallpaper fit is "Fill".
        {
            Bitmap wallppr = new Bitmap(wallpaperFile.FullName);
            double wallpaperRatio = Convert.ToDouble(wallppr.Height) / Convert.ToDouble(wallppr.Width);
            double monitorRatio = Convert.ToDouble(Screen.PrimaryScreen.Bounds.Height) / Convert.ToDouble(Screen.PrimaryScreen.Bounds.Width);
            Point relativePoint = Point.Empty;
            double margin;


            if (wallpaperRatio < monitorRatio) //Means Width is bigger.
            {
                margin = (wallppr.Width - (wallppr.Height / monitorRatio)) / 2;
                relativePoint.X = ((wallppr.Width * startpoint.X) / pictureBoxRectangle.Width) + Convert.ToInt32(margin);
                relativePoint.Y = (wallppr.Height * startpoint.Y) / pictureBoxRectangle.Height;
            }

            else if (wallpaperRatio > monitorRatio) //Means Height is bigger.
            {
                margin = (wallppr.Height - (wallppr.Width * monitorRatio)) / 2;
                relativePoint.X = (wallppr.Width * startpoint.X) / pictureBoxRectangle.Width;
                relativePoint.Y = (wallppr.Height * startpoint.Y) / pictureBoxRectangle.Height + Convert.ToInt32(margin);
            }

            else //Means ratios are equal.
            {
                relativePoint.X = (wallppr.Width * startpoint.X) / pictureBoxRectangle.Width;
                relativePoint.Y = (wallppr.Height * startpoint.Y) / pictureBoxRectangle.Height;
            }


            return relativePoint;
        }

        /// <summary>
        /// Finds a new JPG or PNG file from the selected file.
        /// </summary>
        public static string GetNewWallpaper(bool isShuffle) //Atm number of pictures should be limited to UShort.Max(64,000-ish). Possible overflow.
        {

            if (Directory.Exists(selectedFolderPath) && selectedFolderPath != null)     //Check f folder exists, folderPath string != null
            {                                                                                                                                              //User is going to *select* a folder. So it already exists. Don't think I need to check again.

                ushort wallpaperIndex = Convert.ToUInt16(Settings.ReadSetting(Settings.settings.lastWallpaperIndex));

                string[] pictureFiles = fileInfoJPG.Concat(fileInfoPNG).ToArray();

                Array.Sort(pictureFiles);

                if (!isShuffle)
                {
                    wallpaperIndex = Convert.ToUInt16(rnd.Next(pictureFiles.Length));
                    wallpaperFile = new FileInfo(pictureFiles[wallpaperIndex]);
                    Settings.ChangeSetting(Settings.settings.lastWallpaperIndex, wallpaperIndex.ToString());
                }

                else
                {

                    if (wallpaperIndex >= pictureFiles.Length)
                        wallpaperIndex = 0;

                    wallpaperFile = new FileInfo(pictureFiles[wallpaperIndex]);

                    wallpaperIndex++;

                    Settings.ChangeSetting(Settings.settings.lastWallpaperIndex, wallpaperIndex.ToString());
                }

                Settings.ChangeSetting(Settings.settings.lastWallpaperLocation, wallpaperFile.FullName);

                return wallpaperFile.FullName;

            }

            else
                return null;

        }

        public static void ScanSelectedFolder()
        {
            fileInfoJPG = Directory.GetFiles(selectedFolderPath, "*.jpg", SearchOption.AllDirectories);
            fileInfoPNG = Directory.GetFiles(selectedFolderPath, "*.png", SearchOption.AllDirectories);

            if (fileInfoJPG.Length == 0 && fileInfoPNG.Length == 0)
            {
                Wallpaper.selectedFolderPath = null;
                MessageBox.Show("No pictures detected in selected folder.\n Did you delete them?", "ERROR", MessageBoxButtons.OK);
            }
            else if (fileInfoJPG.Length != 0 || fileInfoPNG.Length != 0)
            {
                GetNewWallpaper(Convert.ToBoolean(Settings.ReadSetting(Settings.settings.shuffle)));
            }
        }
    }
}
