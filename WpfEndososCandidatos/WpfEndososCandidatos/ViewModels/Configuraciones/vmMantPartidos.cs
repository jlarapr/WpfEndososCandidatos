﻿
namespace WpfEndososCandidatos.ViewModels.Configuraciones
{
    using jolcode;
    using jolcode.Base;
    using jolcode.MyInterface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using WpfEndososCandidatos.View;

    class vmMantPartidos : ViewModelBase<IDialogView>, IDisposable
    {

        public vmMantPartidos()
            : base(new wpfMantPartidos())
        {
            initWindow = new RelayCommand(param => InitWindow());
            cmdSalir_Click = new RelayCommand(param => CmdSalir_Click(), param => CommandCan);
        }


        #region initWindow OnShow
       private void InitWindow()
       {
           try
           {


           }
           catch (Exception ex)
           {

               MethodBase site = ex.TargetSite;
               MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
           }
       }
       public bool? OnShow()
       {
           return this.View.ShowDialog();
       }
       public RelayCommand initWindow
       {
           get;
           private set;
       }
       #endregion       
        #region Exit
       public RelayCommand cmdSalir_Click
       {
           get;
           private set;
       }
       public void CmdSalir_Click()
       {
           try
           {
               this.View.Close();
           }
           catch (Exception ex)
           {

               MethodBase site = ex.TargetSite;
               MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
           }
       }
       private bool CommandCan
       {
           get
           {
               return true;
           }
       }


       #endregion
        #region Dispose
       
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmMantPartidos()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources AnotherResource 
                //if (managedResource != null)
                //{
                //    managedResource.Dispose();
                //    managedResource = null;
                //}
            }
            // free native resources if there are any.
            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
        }
        
        #endregion

    }
}