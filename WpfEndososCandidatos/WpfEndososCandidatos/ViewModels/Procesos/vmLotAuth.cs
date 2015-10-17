namespace WpfEndososCandidatos.ViewModels.Procesos
{
    using jolcode;
    using jolcode.Base;
    using jolcode.MyInterface;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using WpfEndososCandidatos.View;

    class vmLotAuth : ViewModelBase<IDialogView>,IDisposable 
    {
        private int _numLote;
        private int _cantidad;
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;

        public vmLotAuth()
            : base(new wpfLotAuth())
        {
            initWindow = new RelayCommand(param => InitWindow());
            cmdSalir_Click = new RelayCommand(param => CmdSalir_Click());
            cmdAddLot_Click = new RelayCommand(param => CmdAddLot_Click());
            cmdAddTodoLot_Click = new RelayCommand(param => CmdAddTodoLot_Click());
        }
        public Brush BorderBrush
        {
            get
            {
                return _BorderBrush;
            }
            set
            {
                if (_BorderBrush != value)
                {
                    _BorderBrush = value;
                    this.RaisePropertychanged("BorderBrush");
                }

            }
        }
        #region initWindow OnShow
        private void InitWindow()
        {
            try
            {
                string myBorderBrush = ConfigurationManager.AppSettings["BorderBrush"];

                if (myBorderBrush != null && myBorderBrush.Trim().Length > 0)
                {
                    Type t = typeof(Brushes);
                    Brush b = (Brush)t.GetProperty(myBorderBrush).GetValue(null, null);
                    BorderBrush = b;
                }
                else
                    BorderBrush = Brushes.Black;

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

        #region property
        public int numLote
        {
            get
            {
                return _numLote;
            }
            set
            {
                if (_numLote != value)
                {
                    _numLote = value;
                    this.RaisePropertychanged("numLote");
                }
            }
        }

        public int cantidad
        {
            get
            {
                return _cantidad;
            }
            set
            {
                if (_cantidad != value)
                {
                    _cantidad = value;
                    this.RaisePropertychanged("cantidad");
                }
            }
            
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



        #endregion

        #region cmd
        public RelayCommand cmdAddLot_Click
        {
            get;
            private set;
        }
        public void CmdAddLot_Click()
        {
            try
            {

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommand cmdAddTodoLot_Click
        {
            get;
            private set;

        }
        public void CmdAddTodoLot_Click()
        {
            try
            {

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        #endregion

        #region Dispose
       
       

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmLotAuth()
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

    }//end
}//end
