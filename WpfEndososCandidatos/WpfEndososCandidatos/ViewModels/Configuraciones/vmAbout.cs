namespace WpfEndososCandidatos.ViewModels
{
    using jolcode;
using jolcode.Base;
using jolcode.MyInterface;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using WpfEndososCandidatos.View;

    class vmAbout : ViewModelBase<IDialogView>, IDisposable
    {
        private string _mensaje;
        private RelayCommand _InitWindow;
        private RelayCommand _ok_Click;

        public vmAbout()
            : base(new wpfAbout())
        {

        }

        public RelayCommand ok_Click
        {
            get
            {
                if (_ok_Click == null)
                {
                    _ok_Click = new RelayCommand(param => Ok_Click());
                }
                return _ok_Click;
            }
        }

        private void Ok_Click()
        {
            this.View.Close();
        }
        
        public bool? OnShow()
        {
            return this.View.ShowDialog();
        }

        public RelayCommand InitWindow
        {
            get
            {
                if (_InitWindow == null)
                {
                    _InitWindow = new RelayCommand(param => OnInitWindow());
                }
                return _InitWindow;
            }
        }

        private void OnInitWindow()
        {

            mensaje = String.Format("CEE Endosos Candidatos 2015 Version {0}", AssemblyVersion);
            mensaje  += "\rEsta aplicación procesa las peticiones de endoso para\rlos candidatos de las primarias.";

        }


        public string mensaje
        {
            get
            {
                return _mensaje;
            }
            set
            {
                if (_mensaje != value)
                {
                    _mensaje = value;
                    this.RaisePropertychanged("mensaje");
                }
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        #region Dispose
       
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmAbout()
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
