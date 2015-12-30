using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace jolcode
{
 public   class Code
    {
        public static void AplicarEfecto(Window win)
        {
            var objBlur = new System.Windows.Media.Effects.BlurEffect();
            objBlur.Radius = 5;
            win.Effect = objBlur;
        }

        public static void QuitarEfecto(Window win)
        {
            win.Effect = null;
        }
        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  new Action(delegate { }));
        }
        #region MyAlzheimer
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);
        public static void MyAlzheimer()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
        }
        #endregion
    }
}
