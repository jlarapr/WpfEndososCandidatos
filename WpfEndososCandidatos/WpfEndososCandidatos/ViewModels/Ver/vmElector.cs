namespace WpfEndososCandidatos.ViewModels.Ver
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
    using WpfEndososCandidatos.View;
    class vmElector : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private string _DBCeeMasterCnnStr;
        private Logclass _LogClass;
        private string _TxtElecNum;
        private string _TxtNombre;
        private string _TxtPaterno;
        private string _TxtMaterno;
        private string _TxtPadre;
        private string _TxtMadre;
        private string _TxtDia;
        private string _TxtMes;
        private string _TxtAno;
        private string _TxtPrecinto;
        private string _TxtUnidad;
        private bool _IsChecked_Activo;
        private bool _IsChecked_Inactivo;
        private bool _IsChecked_Excluido;
        private bool _IsChecked_SexF;
        private bool _IsChecked_SexM;

        public vmElector()
            : base(new wpfElector())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click(), param => CommandCan);
            cmdFind_Click = new RelayCommand(param => MyCmdFind_Click(), param => CanFind);
            _LogClass = new Logclass();
        }


        #region MyProerty
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
        public string DBCeeMasterCnnStr
        {
            get
            {
                return _DBCeeMasterCnnStr;
            }set
            {
                _DBCeeMasterCnnStr = value;
            }
        }
        public string TxtElecNum
        {
            get
            {
                return _TxtElecNum;
            }set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int myIntValue = 0;

                    if (int.TryParse(value, out myIntValue))
                    {
                        MyReset();
                        _TxtElecNum = string.Format("{0:0000000}", myIntValue);
                        this.RaisePropertychanged("TxtElecNum");
                    }else
                    {
                        MyReset();
                        _TxtElecNum = string.Empty;
                        this.RaisePropertychanged("TxtElecNum");
                    }
                }
                else
                {
                    MyReset();
                }

            }
        }
        public string TxtNombre
        {
            get
            {
                return _TxtNombre;
            }set
            {
                _TxtNombre = value;
                this.RaisePropertychanged("TxtNombre");
            }
        }
        public string TxtPaterno
        {
            get
            {
                return _TxtPaterno;
            }set
            {
                _TxtPaterno = value;
                this.RaisePropertychanged("TxtPaterno");
            }
        }
        public string TxtMaterno
        {
            get
            {
                return _TxtMaterno;
            }set
            {
                _TxtMaterno = value;
                this.RaisePropertychanged("TxtMaterno");
            }
        }
        public string TxtPadre
        {
            get
            {
                return _TxtPadre;
            }set
            {
                _TxtPadre = value;
                this.RaisePropertychanged("TxtPadre");
            }
        }
        public string TxtMadre
        {
            get
            {
                return _TxtMadre;
            }set
            {
                _TxtMadre = value;
                this.RaisePropertychanged("TxtMadre");
            }
        }
        public string TxtDia
        {
            get
            {
                return _TxtDia;
            }set
            {
                _TxtDia = value;
                this.RaisePropertychanged("TxtDia");
            }
        }
        public string TxtMes
        {
            get
            {
                return _TxtMes;
            }set
            {
                _TxtMes = value;
                this.RaisePropertychanged("TxtMes");
            }
        }
        public string TxtAno
        {
            get
            {
                return _TxtAno;
            }set
            {
                _TxtAno = value;
                this.RaisePropertychanged("TxtAno");
            }
        }
        public string TxtPrecinto
        {
            get
            {
                return _TxtPrecinto;
            }set
            {
                _TxtPrecinto = value;
                this.RaisePropertychanged("TxtPrecinto");
            }
        }
        public string TxtUnidad
        {
            get
            {
                return _TxtUnidad;
            }set
            {
                _TxtUnidad = value;
                this.RaisePropertychanged("TxtUnidad");
            }
        }
        public bool IsChecked_Activo
        {
            get
            {
                return _IsChecked_Activo;
            }set
            {
                _IsChecked_Activo = value;
                this.RaisePropertychanged("IsChecked_Activo");
            }
        }
        public bool IsChecked_Inactivo
        {
            get
            {
                return _IsChecked_Inactivo;
            }set
            {
                _IsChecked_Inactivo = value;
                this.RaisePropertychanged("IsChecked_Inactivo");
            }
        }
        public bool IsChecked_Excluido
        {
            get
            {
                return _IsChecked_Excluido;
            }set
            {
                _IsChecked_Excluido = value;
                this.RaisePropertychanged("IsChecked_Excluido");
            }
        }
        public bool IsChecked_SexM
        {
            get
            {
                return _IsChecked_SexM;
            }set
            {
                _IsChecked_SexM = value;
                this.RaisePropertychanged("IsChecked_SexM");
            }
        }
        public bool IsChecked_SexF
        {
            get
            {
                return _IsChecked_SexF;
            }
            set
            {
                _IsChecked_SexF = value;
                this.RaisePropertychanged("IsChecked_SexF");
            }
        }

        public bool CanFind
        {
            get
            {
                return !string.IsNullOrEmpty(TxtElecNum);
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
                _LogClass.SourceName = "VerElector";
                _LogClass.MessageFile = string.Empty;
                _LogClass.CreateEvent();
                _LogClass.MYEventLog.WriteEntry("Var Elector Start:" + Dia + " " + Hora, EventLogEntryType.Information);

                MyReset();

            }
           catch (Exception ex)
           {

               MethodBase site = ex.TargetSite;
               MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                _LogClass.MYEventLog.WriteEntry(ex.ToString() + "\r\n" + site.Name, EventLogEntryType.Error, 9999);

            }
        }
        public bool? MyOnShow()
       {
           return this.View.ShowDialog();
       }
        public void MyCmdFind_Click()
        {
            try
            {
                MyReset();

                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBCeeMasterCnnStr
                })
                {
                    DataTable myTable;

                    myTable = get.MyGetCitizen(TxtElecNum);

                    TxtNombre = myTable.Rows[0]["FirstName"].ToString();
                    TxtPaterno = myTable.Rows[0]["LastName1"].ToString();
                    TxtMaterno = myTable.Rows[0]["LastName2"].ToString();

                    TxtPadre = string.Empty;
                    TxtMadre = string.Empty;
                    TxtUnidad = string.Empty;
                    
                    string Fechanac = myTable.Rows[0]["Fechanac"].ToString();

                    if (!string.IsNullOrEmpty(Fechanac))
                    {
                        TxtDia = Fechanac.Split('-')[0].Trim();
                        TxtMes = Fechanac.Split('-')[1].Trim();
                        TxtAno = Fechanac.Split('-')[2].Trim();
                    }

                    IsChecked_Activo = false;
                    IsChecked_Excluido = false;
                    IsChecked_Inactivo = false;

                    IsChecked_SexF = false;
                    IsChecked_SexM = false;


                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                _LogClass.MYEventLog.WriteEntry(ex.ToString() + "\r\n" + site.Name, EventLogEntryType.Error, 9999);
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

        public RelayCommand initWindow
       {
           get;
           private set;
       }
        public RelayCommand cmdFind_Click
        {
            get;private set;
        }
        public RelayCommand cmdSalir_Click
        {
            get;
            private set;
        }
        #endregion


        #region MyModulos
        private void MyReset()
        {
            

            TxtNombre = string.Empty;
            TxtPadre = string.Empty;
            TxtPaterno = string.Empty;
            TxtUnidad = string.Empty;
            TxtMaterno = string.Empty;
            TxtMadre = string.Empty;
            TxtDia = string.Empty;
            TxtAno = string.Empty;
            TxtMes = string.Empty;

            IsChecked_Activo = false;
            IsChecked_Excluido = false;
            IsChecked_Inactivo = false;
            IsChecked_SexF = false;
            IsChecked_SexM = false;

        }
        #endregion

       

        #region Dispose
       
       

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmElector()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources AnotherResource 
                if (_LogClass != null)
                {
                    _LogClass.Dispose();
                    _LogClass = null;
                }
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
