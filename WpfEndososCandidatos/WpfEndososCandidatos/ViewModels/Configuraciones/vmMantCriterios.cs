

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
    using System.Collections;

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
        private ObservableCollection<Brush> _Background_0;
        private string _Explicacion = string.Empty;
        private bool _IsEnabled_txtExplicacion;
        private bool _isEdit;

        public vmMantCriterios()
            : base(new wpfMantCriterios())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click(),param=> CanSalir);
            cmdActualize_Click = new RelayCommand(param => MyCmdActualize_Click(), param => CanActualize);
            cmdEditar_Click = new RelayCommand(param => MyCmdEditar_Click(),param => CanEditar);
            cmdCancel_Click = new RelayCommand(param => MyCmdCancel_Click(), param => CanCancel);

            Checked = new RelayCommand(param => MyChecked(param));
            UnChecked = new RelayCommand(param => MyUnChecked(param));
            CheckedWarning = new RelayCommand(param => MyCheckedWarning(param));
            UnCheckedWarning = new RelayCommand(param => MyUnCheckedWarning(param));

            _LogClass = new Logclass();
            _Criterios = new List<Criterios>();
            chk = new ObservableCollection<Criterios>();
            Background_0 = new ObservableCollection<Brush>();
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
        public ObservableCollection <Brush> Background_0 {
            get
            {
                return _Background_0;
            }
            set
            {
                _Background_0 = value;
                this.RaisePropertychanged("Background_0");
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
                _txtExplicacion = value;
                if (!string.IsNullOrEmpty(value))
                {
                    if (chk[_Idx].Desc != value.Trim())
                        chk[_Idx].Desc = value.Trim();
                  
                }
                this.RaisePropertychanged("txtExplicacion");
            }
        }
        public string Explicacion
        {
            get
            {
                return "Explicación:" + _Explicacion;
            }set
            {
                _Explicacion = value;
                this.RaisePropertychanged("Explicacion");

            }
                
        }
        public bool IsEnabled_txtExplicacion
        {
            get
            {
                return _IsEnabled_txtExplicacion;
            }set
            {
                _IsEnabled_txtExplicacion = value;
                this.RaisePropertychanged("IsEnabled_txtExplicacion");
            }
        }
        public bool isEdit
        {
            get
            {
                return _isEdit;
            }
            set
            {
                _isEdit = value;
                this.RaisePropertychanged("isEdit");
            }
        }
        public bool CanActualize
        {
            get; private set;
        }
        public bool CanEditar
        {
            get;
            private set;
        }
        public bool CanCancel
        {
            get; private set;
        }
        public bool CanSalir
        {
            get; private set;
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
                MyReset();
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

                using (SqlExcuteCommand mySqlExe = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    for (int i = 0; i < 21; i++)
                    {
                        myUpDate = mySqlExe.MyChangeCriterios(chk[i].Campo, chk[i].Editar, chk[i].Desc, chk[i].Warning);
                    }

                    if (!myUpDate)
                        throw new Exception("Error en la Base de Data");

                    MyRefresh();

                    MyReset();

                    MessageBox.Show("Done...", "Save", MessageBoxButton.OK, MessageBoxImage.Information);

                    _LogClass.MYEventLog.WriteEntry("Save...", EventLogEntryType.Information, 100);
                }
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

            for (int i = 0; i < Background_0.Count; i++)
                Background_0[i] = Brushes.White;
            
            Background_0[_Idx] = (Brushes.Red);

            Explicacion = (_Idx + 1).ToString();

            txtExplicacion = myString.Split('-')[1];

            IsEnabled_txtExplicacion = true;

            CanActualize = true;

        }
        private void MyUnChecked(object param)
        {
            if (!int.TryParse(param.ToString(), out _Idx))
                _Idx = -1;

            for (int i = 0; i < Background_0.Count; i++)
                Background_0[i] = Brushes.White;

            Explicacion = string.Empty;

            txtExplicacion = string.Empty;

            IsEnabled_txtExplicacion = false;

            CanActualize = true;


        }
        private void MyCheckedWarning(object param)
        {
            if (!int.TryParse(param.ToString(), out _Idx_Warning))
                _Idx_Warning = -1;

            CanActualize = true;

        }
        private void MyUnCheckedWarning(object param)
        {
            if (!int.TryParse(param.ToString(), out _Idx_Warning))
                _Idx_Warning = -1;

            CanActualize = true;

        }
        private void MyCmdEditar_Click()
        {
            CanEditar = false;
            CanSalir = false;
            IsEnabled_txtExplicacion = false;
            CanActualize = false;

            CanCancel = true;
            isEdit = true;
        }
        private void MyCmdCancel_Click()
        {
            if (CanActualize)
                MyRefresh();

            MyReset();
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
        public RelayCommand cmdEditar_Click
        {
            get;private set;
        }
        public RelayCommand cmdCancel_Click
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

                    Explicacion = string.Empty;

                    chk.Clear();

                    foreach (DataRow row in _MyCriteriosTable.Rows)
                    {
                        chk.Add(new Criterios
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
            CanCancel = false;
            CanActualize = false;
            IsEnabled_txtExplicacion = false;

            CanEditar = true;
            CanSalir = true;
            isEdit = false;

            txtExplicacion = string.Empty;

            Explicacion = string.Empty;

            Background_0.Clear();

            for (int i=0; i <=21;i++ )
                Background_0.Add(Brushes.White);
            
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
