using Shell32;
using System;
using System.IO;


namespace WFA_WallpaperClock
{
    static class Settings
    {

        public static string rootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\WallpaperClock";
        public static string settingsDirectory = rootDirectory + "\\WallpaperClockSettings.txt";
        public static string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        public static string currentPath = System.Windows.Forms.Application.ExecutablePath;

        static string[] settingStrings = new string[11];
        /// <summary>
        /// Enum of the settings.
        /// </summary>
        public enum settings
        {
            font = 0,
            color,
            shuffle,
            minuteOfChangeWallpaper,
            wallpaperFolderDirectory,
            startPointX,
            startPointY,
            lastWallpaperLocation,
            fontSize,
            lastWallpaperIndex,
            startMinimized
        }
        /// <summary>
        /// Creates folders if they don't exist.
        /// </summary>
        public static void CreateDirectory()
        {
            if (!Directory.Exists(rootDirectory))
            {
                Directory.CreateDirectory(rootDirectory);
            }
        }
        /// <summary>
        /// Creates settings file with default settings.
        /// </summary>
        public static void CreateSettings()
        {
            if (!File.Exists(settingsDirectory))
            {
                File.Create(settingsDirectory).Close();
                settingStrings[0] = MainForm.DefaultFont.Name;                                  //Font
                settingStrings[1] = MainForm.DefaultBackColor.ToArgb().ToString(); //Color
                settingStrings[2] = "False";                                                                       //Shuffle
                settingStrings[3] = "1";                                                                             //minuteOfChangeWallpaper
                settingStrings[4] = "";                                                                              //WallpaperFolderDirectory
                settingStrings[5] = "0";                                                                           //StartPointX
                settingStrings[6] = "0";                                                                            //StartPointY
                settingStrings[7] = null;                                                                           //LastWallpaperLocation
                settingStrings[8] = "96";                                                                       //FontSize
                settingStrings[9] = "0";
                settingStrings[10] = "False";
                File.WriteAllLines(settingsDirectory, settingStrings);
            }
        }
        /// <summary>
        /// Reads the setting from the file and returns it as string.
        /// </summary>
        /// <param name="SettingToRead">Index of the setting to read.</param>
        /// <returns></returns>
        public static string ReadSetting(settings SettingToRead)
        {
            settingStrings = File.ReadAllLines(settingsDirectory);

            switch (SettingToRead)
            {
                case settings.font:
                    return settingStrings[0];
                case settings.color:
                    return settingStrings[1];
                case settings.shuffle:
                    return settingStrings[2];
                case settings.minuteOfChangeWallpaper:
                    return settingStrings[3];
                case settings.wallpaperFolderDirectory:
                    return settingStrings[4];
                case settings.startPointX:
                    return settingStrings[5];
                case settings.startPointY:
                    return settingStrings[6];
                case settings.lastWallpaperLocation:
                    return settingStrings[7];
                case settings.fontSize:
                    return settingStrings[8];
                case settings.lastWallpaperIndex:
                    return settingStrings[9];
                case settings.startMinimized:
                    return settingStrings[10];
                default:
                    return null;
            }
        }
        /// <summary>
        /// Changes the settings file at AppData/Local/WallpaperClock/WallpaperClockSettings.
        /// </summary>
        /// <param name="settingIndex">Index of the setting to be changed.</param>
        /// <param name="setting">Content of the setting.</param>
        public static void ChangeSetting(settings settingIndex, string setting) // As of now, it doesn't check the validity of the setting.For example, if the passed value is bool or not for isShuffle.
        {
            string[] originalSettings = File.ReadAllLines(settingsDirectory);

            originalSettings[(int)settingIndex] = setting;

            File.WriteAllLines(settingsDirectory, originalSettings);
        }
        /// <summary>
        /// Checks wheter if the shortcut is created.
        /// </summary>
        /// <returns></returns>
        public static bool IsStartupCreated() //If there is a shortcut called WallpaperClock and it's target is current directory.
        {
            Shell shell = new Shell();

            if (File.Exists(startupPath + "\\WallpaperClock.lnk"))
            {
                Folder folder = shell.NameSpace(startupPath);
                FolderItem folderItem = folder.ParseName("WallpaperClock.lnk");
                Shell32.ShellLinkObject link = folderItem.GetLink as Shell32.ShellLinkObject;


                if (string.Equals(link.Path, currentPath, StringComparison.OrdinalIgnoreCase)) //If the current shortcut's target equals current .exe's location.
                    return true;

            }
            return false;
        }
        /// <summary>
        /// Creates a Shortcut of the current .exe at the Startup folder.
        /// </summary>
        public static void CreateStartupShortcut()
        {
            object shDesktop = (object)"Startup";
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\WallpaperClock.lnk";
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.TargetPath = System.Windows.Forms.Application.ExecutablePath;
            shortcut.Save();
        }
        /// <summary>
        /// Deletes the startup shortcut if it exists.
        /// </summary>
        public static void DeleteStartupShortcut()
        {
            if (IsStartupCreated())
                File.Delete(startupPath + "\\WallpaperClock.lnk");

        }
    }
}
