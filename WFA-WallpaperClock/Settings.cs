using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WFA_WallpaperClock
{
    static class Settings
    {

        public static string rootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\WallpaperClock";
        public static string settingsDirectory = rootDirectory + "\\WallpaperClockSettings.txt";

        static string[] settingStrings = new string[8];

        public enum settings
        {
            font = 0,
            color,
            shuffle,
            minuteOfChangeWallpaper,
            wallpaperFolderDirectory,
            startPointX,
            startPointY,
            lastWallpaperLocation
        }

        public static void CreateDirectory()
        {
            if (!Directory.Exists(rootDirectory))
            {
                Directory.CreateDirectory(rootDirectory);
            }
        }

        public static void CreateSettings()
        {
            if (!File.Exists(settingsDirectory))
            {
                File.Create(settingsDirectory).Close();
                settingStrings[0] = MainForm.DefaultFont.Name;                                  //Font
                settingStrings[1] = MainForm.DefaultBackColor.ToArgb().ToString(); //Color
                settingStrings[2] = "0";                                                                            //Shuffle
                settingStrings[3] = "1";                                                                             //minuteOfChangeWallpaper
                settingStrings[4] = "";                                                                              //WallpaperFolderDirectory
                settingStrings[5] = "0";
                settingStrings[6] = "0";
                settingStrings[7] = "";

                File.WriteAllLines(settingsDirectory, settingStrings);
            }
        }

        public static string ReadSetting(settings SettingToRead)
        {
            settingStrings = File.ReadAllLines(settingsDirectory);

            switch (SettingToRead)
            {
                case settings.font:
                    return settingStrings[0];
                    break;
                case settings.color:
                    return settingStrings[1];
                    break;
                case settings.shuffle:
                    return settingStrings[2];
                    break;
                case settings.minuteOfChangeWallpaper:
                    return settingStrings[3];
                    break;
                case settings.wallpaperFolderDirectory:
                    return settingStrings[4];
                    break;
                case settings.startPointX:
                    return settingStrings[5];
                    break;
                case settings.startPointY:
                    return settingStrings[6];
                    break;
                case settings.lastWallpaperLocation:
                    return settingStrings[7];
                    break;
                default:
                    return null;
                    break;
            }
        }

        public static void ChangeSetting(settings settingIndex, string setting) // As of now, it doesn't check the validity of the setting.For example, if the passed value is bool or not for isShuffle.
        {
            string[] originalSettings = File.ReadAllLines(settingsDirectory);

            originalSettings[(int)settingIndex] = setting;

            File.WriteAllLines(settingsDirectory, originalSettings);
        }


    }
}
