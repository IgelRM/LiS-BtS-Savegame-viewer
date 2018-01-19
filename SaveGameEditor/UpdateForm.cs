using System.Text;
using System.Windows.Forms;

namespace SaveGameEditor
{
    public partial class UpdateForm : Form
    {
        public bool DontShowAgainIsChecked { get; private set; } = false;

        public UpdateForm(UpdateCheckingResult updateCheckingResult)
        {
            InitializeComponent();

            var message = new StringBuilder();
            message.AppendLine($"A newer version ({updateCheckingResult.ServerVersion}) is available.");
            message.AppendLine();
            message.AppendLine("Would you like to go to the release download page?");

            lblMessage.Text = message.ToString();
        }

        private void UpdateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DontShowAgainIsChecked = cbDontShow.Checked;
        }
    }
}
