using System;
using System.IO;
using System.Text.RegularExpressions;

namespace savefiledecoder
{
    public static class PathHelper
    {
        private const string CSharpAssemblyRelativePath = @"Life is Strange - Before the Storm_Data\Managed\Assembly-CSharp.dll";
        private const string InitialDataRelativePath = @"Life is Strange - Before the Storm_Data\StreamingAssets\Data\InitialData.et.bytes";
        private const string SteamIdFoldersRelativePath = @"AppData\LocalLow\Square Enix\Life Is Strange_ Before The Storm\Saves";

        public const string ExportObjectivesFileName = "export_objectives.txt";
        public const string ExportCheckpointsFileName = "export_checkpoints.txt";
        public const string ExportVariablesFileName = "export_variables.txt";

        /// <summary>
        /// Default save header file name
        /// </summary>
        public const string SaveHeaderFileName = "Header.Save";

        /// <summary>
        /// Default save data file name
        /// </summary>
        public const string SaveDataFileName = "Data.Save";

        /// <summary>
        /// Gets path to folder where bot Header.Save and Data.Save files are located
        /// </summary>
        /// <param name="steamId">SteamID</param>
        /// <param name="slot">Selected save slot</param>
        /// <returns>Path to save folder</returns>
        public static string GetSaveFolder(string steamId, SaveSlot slot)
        {
            return Path.Combine(GetSteamIdFolderBySteamId(steamId), $"SLOT_0{(int)slot}");
        }

        /// <summary>
        /// Gets path to Header.Save file
        /// </summary>
        /// <param name="steamId">SteamID</param>
        /// <param name="slot">Selected save slot</param>
        /// <returns>Path to Header.Save file</returns>
        public static string GetSaveHeaderFilePath(string steamId, SaveSlot slot)
        {
            return Path.Combine(GetSaveFolder(steamId, slot), SaveHeaderFileName);
        }

        /// <summary>
        /// Gets path to Data.Save file
        /// </summary>
        /// <param name="steamId">SteamID</param>
        /// <param name="slot">Selected save slot</param>
        /// <returns>Path to Data.Save file</returns>
        public static string GetSaveDataFilePath(string steamId, SaveSlot slot)
        {
            return Path.Combine(GetSaveFolder(steamId, slot), SaveDataFileName);
        }

        public static string GetInitialDataFilePath(string gameFolderPath)
        {
            return Path.Combine(gameFolderPath, InitialDataRelativePath);
        }

        public static string GetCSharpAssemblyPath(string gameFolderPath)
        {
            return Path.Combine(gameFolderPath, CSharpAssemblyRelativePath);
        }

        public static string[] GetSteamIdFolders()
        {
            return Directory.GetDirectories(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), SteamIdFoldersRelativePath));
        }

        public static string GetSteamIdFolderBySteamId(string steamId)
        {
            if (string.IsNullOrWhiteSpace(steamId))
            {
                throw new ArgumentException("Argument should not be empty", nameof(steamId));
            }
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                SteamIdFoldersRelativePath, steamId);
            //return Directory.Exists(path) ? path : null;
            return path;
        }

        public static string GetSteamIdFromPath(string path)
        {
            var re = new Regex(@".*\\Saves\\(?<steamId>\d*).*");
            var result = re.Match(path);
            if (result.Success)
            {
                return result.Groups["steamId"].Value;
            }
            return null;
        }

        public static SaveSlot? GetSlotFromPath(string path)
        {
            var re = new Regex(@".*\\Saves\\\d*\\SLOT_0(?<slotNumber>\d).*");
            var result = re.Match(path);
            if (result.Success)
            {
                int slotNumber;
                if (!int.TryParse(result.Groups["slotNumber"].Value, out slotNumber))
                {
                    return null;
                }
                if (!Enum.IsDefined(typeof(SaveSlot), slotNumber))
                {
                    return null;
                }
                return (SaveSlot) slotNumber;
            }
            return null;
        }
    }
}
