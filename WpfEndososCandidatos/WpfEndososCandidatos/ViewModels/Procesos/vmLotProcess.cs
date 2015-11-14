namespace WpfEndososCandidatos.ViewModels.Procesos
{
    using jolcode;
    using jolcode.Base;
    using jolcode.MyInterface;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using Models;
    using WpfEndososCandidatos.View.Procesos;

     class vmLotProcess : ViewModelBase<IDialogView>,IDisposable 
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private string _DBEndososCnnStr;
        private DataTable _MyCriteriosTable;
        private ObservableCollection<Criterios> _CollCriterios;

        public vmLotProcess()
            : base(new wpfLotProcess())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());

            CollCriterios = new ObservableCollection<Criterios>();
        }

        #region MyProperty
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

        public ObservableCollection<Criterios> CollCriterios
        {
            get
            {
                return _CollCriterios;
            }
            set
            {
                if (value != null)
                {
                    _CollCriterios = value;
                    this.RaisePropertychanged("CollCriterios");
                }
            }

        }

        #endregion

        #region MyCmd
        private void MyInitWindow()
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
        public void MyCmdSalir_Click()
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


      
        

        #region MyMetodos


        private void MyRefresh()
        {
            try
            {
                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {

                    _MyCriteriosTable = get.MyGetCriterios();



                    CollCriterios.Clear();

                    foreach (DataRow row in _MyCriteriosTable.Rows)
                    {
                        CollCriterios.Add(new Models.Criterios
                        {
                            Campo = row["Campo"].ToString(),
                            Editar = row["Editar"].ToString().Trim() == "1" ? true : false,
                            Desc = row["Desc"].ToString(),
                            Warning = row["Warning"].ToString().Trim() == "1" ? true : false
                        });
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        #endregion



        #region Dispose



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmLotProcess()
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
