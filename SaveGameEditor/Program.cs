using System;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SaveGameEditor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static Version GetApplicationVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }

        public static string GetApplicationVersionStr()
        {
            var version = GetApplicationVersion();
            var sb = new StringBuilder();
            sb.Append($"{version.Major}.{version.Minor}");
            if (version.Build != 0)
            {
                sb.Append($".{version.Build}");
            }
            return sb.ToString();
        }
    }
}
