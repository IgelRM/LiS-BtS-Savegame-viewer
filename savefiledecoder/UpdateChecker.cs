using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace savefiledecoder
{
    public static class UpdateChecker
    {
        private const string GetVersionUrl = "https://raw.githubusercontent.com/IgelRM/LiS-BtS-Savegame-viewer/master/LatestVersion.txt";
        private const string DownloadUpdateUrl = "https://github.com/IgelRM/LiS-BtS-Savegame-viewer/releases";

        public static async void CheckForUpdates(Form form)
        {
            await Task.Run(async () =>
            {
                var client = new WebClient();
                try
                {
                    var latestVersionStr = await client.DownloadStringTaskAsync(GetVersionUrl);
                    Version latestVersion;
                    if (!Version.TryParse(latestVersionStr, out latestVersion))
                    {
                        return;
                    }

                    var currentVersion = Program.GetApplicationVersion();

                    // Latest available version is higher than the current one
                    if (latestVersion.CompareTo(currentVersion) == 1)
                    {
                        var message = new StringBuilder();
                        message.AppendLine($"A newer version ({latestVersionStr}) is available.");
                        message.AppendLine();
                        message.AppendLine("Would you like to go to the release download page?");

                        form.Invoke(new Action(() =>
                        {
                            if (MessageBox.Show(message.ToString(), "Application Update",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                Process.Start(DownloadUpdateUrl);
                            }
                        }));
                    }
                }
                // Unable to retrieve latest version from server
                catch (WebException)
                {
                }
            });
        }
    }
}
