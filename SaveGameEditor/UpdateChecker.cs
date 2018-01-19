using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace SaveGameEditor
{
    public class UpdateCheckingResult
    {
        public Version ServerVersion { get; set; }

        public string ServerVersionStr { get; set; }

        public Version LocalVersion { get; set; }

        public bool CanBeUpdated { get; set; }
    }

    public static class UpdateChecker
    {
        private const string GetVersionUrl = "https://raw.githubusercontent.com/IgelRM/LiS-BtS-Savegame-viewer/master/LatestVersion.txt";
        private const string DownloadUpdateUrl = "https://github.com/IgelRM/LiS-BtS-Savegame-viewer/releases";

        public static void VisitDownloadPage()
        {
            Process.Start(DownloadUpdateUrl);
        }

        public static async Task<UpdateCheckingResult> CheckForUpdates()
        {
            var client = new WebClient();
            try
            {
                var latestVersionStr = await client.DownloadStringTaskAsync(GetVersionUrl);
                Version latestVersion;
                if (!Version.TryParse(latestVersionStr, out latestVersion))
                {
                    return null;
                }

                var currentVersion = Program.GetApplicationVersion();

                return new UpdateCheckingResult
                {
                    ServerVersion = latestVersion,
                    ServerVersionStr = latestVersionStr,
                    LocalVersion = currentVersion,
                    CanBeUpdated = latestVersion.CompareTo(currentVersion) == 1
                };
            }
            // Unable to retrieve latest version from server
            catch (WebException)
            {
                return null;
            }
        }
    }
}
