

namespace WpfEndososCandidatos.ViewModels.Configuraciones
{
    using jolcode;
    using jolcode.Base;
    using jolcode.MyInterface;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using Models;
    using WpfEndososCandidatos.View.Configuraciones;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    class vmMantCriterios : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private Logclass _LogClass;
        private DataTable _MyCriteriosTable;
        private List<Criterios> _Criterios;
        private string _DBEndososCnnStr;
        private ObservableCollection<Criterios> _chk;
        private int _Idx;
        private int _Idx_Warning;
        private string _txtExplicacion;

        public vmMantCriterios()
            : base(new wpfMantCriterios())
        {
            initWindow = new RelayCommand(param => MyInitWindow());

            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());

            cmdActualize_Click = new RelayCommand(param => MyCmdActualize_Click());

            Checked = new RelayCommand(param => MyChecked(param));

            UnChecked = new RelayCommand(param => MyUnChecked(param));

            CheckedWarning = new RelayCommand(param => MyCheckedWarning(param));

            UnCheckedWarning = new RelayCommand(param => MyUnCheckedWarning(param));

            _LogClass = new Logclass();

            _Criterios = new List<Criterios>();

            chk = new ObservableCollection<Criterios>();

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
            }
            set
            {
                _DBEndososCnnStr = value;
            }
        }

        public ObservableCollection<Criterios> chk
        {
            get
            {
                return _chk;
            }set
            {
                if (value != null)
                {
                    _chk = value;
                    this.RaisePropertychanged("chk");
                }
            }

        }

        public string txtExplicacion
        {
            get
            {
                return _txtExplicacion;
            }set
            {
                if (value != null)
                {
                    _txtExplicacion = value;

                    chk[_Idx].Desc = value;

                    this.RaisePropertychanged("txtExplicacion");
                }
            }
        }
        
        #endregion

        #region MyCmd

        private void MyInitWindow()
        {
            try
            {
                string Dia = DateTime.Now.ToString("MMM/dd/yyyy");
                string Hora = DateTime.Now.ToString("hh:mm:ss tt");

                string myBorderBrush = ConfigurationManager.AppSettings["BorderBrush"];

                if (myBorderBrush != null && myBorderBrush.Trim().Length > 0)
                {
                    Type t = typeof(Brushes);
                    Brush b = (Brush)t.GetProperty(myBorderBrush).GetValue(null, null);
                    BorderBrush = b;
                }
                else
                    BorderBrush = Brushes.Black;

                _LogClass.LogName = "Applica";
                _LogClass.SourceName = "Criterios";
                _LogClass.MessageFile = string.Empty;
                _LogClass.CreateEvent();
                _LogClass.MYEventLog.WriteEntry("Criterios Start:" + Dia + " " + Hora, EventLogEntryType.Information);
                
                MyRefresh();
                //MyReset();
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

        public void MyCmdActualize_Click()
        {
            try
            {
                
                bool myUpDate = false;
                string myWhere = string.Empty;
               
                using (SqlExcuteCommand mySqlExe = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    myUpDate = mySqlExe.MyChangeCriterios(chk[_Idx].Campo,chk[_Idx].Editar,chk[_Idx].Desc,chk[_Idx].Warning);
                }

                if (!myUpDate)
                    throw new Exception("Error en la Base de Data");

                MessageBox.Show("Done...", "Save", MessageBoxButton.OK, MessageBoxImage.Information);

                _LogClass.MYEventLog.WriteEntry("Save...", EventLogEntryType.Information, 100);

            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                _LogClass.MYEventLog.WriteEntry(string.Concat(ex.Message, "\r\n", site.Name), EventLogEntryType.Error, 9999);
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

        private void MyChecked(object param)
        {

            if (!int.TryParse(param.ToString(), out _Idx))
                _Idx = -1;

            string myString = chk[_Idx].Txt;

            txtExplicacion = myString.Split('-')[1];
        }

        private void MyUnChecked(object param)
        {
            if (!int.TryParse(param.ToString(), out _Idx))
                _Idx = -1;

            txtExplicacion = string.Empty;
        }

        private void MyCheckedWarning(object param)
        {
            if (!int.TryParse(param.ToString(), out _Idx_Warning))
                _Idx_Warning = -1;


        }

        private void MyUnCheckedWarning(object param)
        {
            if (!int.TryParse(param.ToString(), out _Idx_Warning))
                _Idx_Warning = -1;
        }

        public RelayCommand initWindow
        {
            get;
            private set;
        }

        public RelayCommand cmdActualize_Click
        {
            get;
            private set;
        }

        public RelayCommand cmdSalir_Click
        {
            get;
            private set;
        }

        public RelayCommand Checked
        {
            get;private set;
        }

        public RelayCommand UnChecked
        {
            get; private set;
        }

        public RelayCommand CheckedWarning
        {
            get;private set;
        }

        public RelayCommand UnCheckedWarning
        {
            get;private set;
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

                    foreach (DataRow row in _MyCriteriosTable.Rows)
                    {
                        chk.Add(new Models.Criterios
                        {
                            Campo = row["Campo"].ToString(),
                            Editar = row["Editar"].ToString().Trim() == "1" ? true : false,
                            Desc = row["Desc"].ToString(),
                            Warning = row["Warning"].ToString().Trim() == "1" ? true : false
                        });
                    }
                }

            }
            catch (Exception )
            {
                throw;
            }
        }
        private void MyReset()
        {

        }

        #endregion

        #region Dispose
       
       

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmMantCriterios()
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
