using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using savefiledecoder.Properties;

namespace savefiledecoder
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            lblCurrentVersion.Text = $"Version {Program.GetApplicationVersionStr()}";
            llProjectHomepage.Text = Resources.ProjectHomepage;

            Task.Run(async () =>
            {
                var result = await UpdateChecker.CheckForUpdates();
                if (result == null || !result.CanBeUpdated)
                {
                    this.InvokeEx(() =>
                    {
                        lblNoUpdatesWasFound.Visible = true;
                    });
                    return;
                }

                this.InvokeEx(() =>
                {
                    llNewVersionIsAvailable.Text = $"New version is available: {result.ServerVersion}";
                    llNewVersionIsAvailable.Visible = true;
                });
            });
        }

        private void llNewVersionIsAvailable_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UpdateChecker.VisitDownloadPage();
        }

        private void llDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Resources.ProjectHomepage);
        }
    }
}
