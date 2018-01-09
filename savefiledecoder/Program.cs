using System;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace savefiledecoder
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

        public static string GetApplicationVersionStr()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
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
