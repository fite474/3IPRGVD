using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SharedData.Helpers
{
    static class ThreadExtension
    {
        public static void ActionInvoke(this Control control, Action action)
        {
            control.Invoke(action);
        }
    }
}
