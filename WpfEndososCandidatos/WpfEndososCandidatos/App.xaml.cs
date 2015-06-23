using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfEndososCandidatos
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string miProcessName = Process.GetCurrentProcess().ProcessName;

            Process[] miProcess = Process.GetProcessesByName(miProcessName);

            if (miProcess.Length > 1)
            {
                MessageBox.Show(miProcessName + " already running");

                Process.GetCurrentProcess().Kill();
            }
            else
            {
                //View.MainWindow w = new View.MainWindow();
                //w.Closing += OnClosing;                        
                //w.Show();

                ViewModel.MainVM w = new ViewModel.MainVM();
                w.View.Closing += OnClosing;
                w.OnShow();
            }

        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Application.Current.Shutdown();
        }
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = this.OnSessionEnding();
            //Process.GetCurrentProcess().Kill();
        }

        private bool OnSessionEnding()
        {
            var response = MessageBox.Show("!!!Do you really want to exit?", "Exiting...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (response == MessageBoxResult.Yes)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }//end
}//end
