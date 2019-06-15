using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfEndososCandidatos.ViewModels;
using WpfEndososCandidatos.View;
using WpfEndososCandidatos.Helper;
using WpfEndososCandidatos.Models;
using System.Security.Principal;
using System.Reflection;

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

                ViewModels.MainVM w = new ViewModels.MainVM();
                //AdminRelauncher(); //This is the only important line here, add it to a place it gets run early on.
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
        private void AdminRelauncher()
        {
            if (!IsRunAsAdmin())
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = Assembly.GetEntryAssembly().CodeBase;

                proc.Verb = "runas";

                try
                {
                    Process.Start(proc);
                    Application.Current.Shutdown();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("This program must be run as an administrator! \n\n" + ex.ToString());
                }
            }
        }

        private bool IsRunAsAdmin()
        {
            try
            {
                WindowsIdentity id = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(id);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception)
            {
                return false;
            }
        }


    }//end
}//end
