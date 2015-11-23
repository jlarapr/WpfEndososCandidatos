using jolcode;
using jolcode.Base;
using jolcode.MyInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WpfEndososCandidatos.View;

namespace WpfEndososCandidatos.ViewModels.Procesos
{
    class vmFixVoid : ViewModelBase<IDialogView>, IDisposable
    {

        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private string _DBEndososCnnStr;

        public vmFixVoid(string lot) : base(new wpfFixVoid())
        {
            Lot = lot;
        }

        #region MyProperty

        public string DBEndososCnnStr
        {
            get
            {
                return _DBEndososCnnStr;
            }set
            {
                _DBEndososCnnStr = value;
            }
        }
        public string Lot
        {
            get;private set;
        }

        #endregion

        #region MyCmd

        public bool? MyOnShow()
        {
            return this.View.ShowDialog();
        }


        public RelayCommand initWindow
        {
            get;
            private set;
        }
        public RelayCommand cmdSalir_Click
        {
            get;
            private set;
        }

        #endregion








        #region Dispose



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmFixVoid()
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
