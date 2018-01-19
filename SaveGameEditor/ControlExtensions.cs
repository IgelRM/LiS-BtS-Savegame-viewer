using System;
using System.Windows.Forms;

namespace SaveGameEditor
{
    public static class ControlExtensions
    {
        public static void InvokeEx(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}
