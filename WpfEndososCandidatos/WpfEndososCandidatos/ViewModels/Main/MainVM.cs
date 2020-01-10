
namespace WpfEndososCandidatos.ViewModels
{
    using jolcode;
    using jolcode.Base;
    using jolcode.MyInterface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using WpfEndososCandidatos.View;
    using System.Windows.Input;
    using Helper;
    using System.Diagnostics;

    partial class MainVM : ViewModelBase<IMainView>
    {
        private RelayCommand _InitWindow;
        private string _Title;
        private const string _REGPATH = "SOFTWARE\\CEE\\Endosos\\Partidos";

        private string _SqlServer = string.Empty;
        private string _Username = string.Empty;
        private string _Password = string.Empty;
        private string _Database = string.Empty;

        private string _MastSvr = string.Empty;
        private string _MastUsr = string.Empty;
        private string _MastPass = string.Empty;
        private string _MastDB = string.Empty;

        private string _ImageSvr = string.Empty;
        private string _ImageUsr = string.Empty;
        private string _ImagePass = string.Empty;
        private string _ImageDB = string.Empty;

        private string _RadicacionesSvr = string.Empty;
        private string _RadicacionesUsr = string.Empty;
        private string _RadicacionesPass = string.Empty;
        private string _RadicacionesDB = string.Empty;

        private string _PDFPath = string.Empty;
        private string _Hora;
        private string _Dia;
        private Cursor _MiCursor;
        private string _DBEndososCnnStr;
        private string _DBCeeMasterCnnStr;
        private string _DBImagenesCnnStr;
        private string _DBRadicacionesCnnStr;
        private string _WhatIsUserName;
        private string _WhatIsModo;

        public MainVM()
            : base(new MainWindow())
        {
            mnuduplicado_click = new RelayCommand(param => Mymnuduplicado_clickC());
            mnuEndososRechazados_click = new RelayCommand(param => MymnuEndososRechazados_click());
            mnuEstatus_click = new RelayCommand(param => MymnuEstatus_click());
            mnuduplicadopornumelectoral_click = new RelayCommand(param => Mymnuduplicadopornumelectoral_click());


            help_click = new RelayCommand(param => Myhelp_click());
        }
        public string WhatIsModo
        {
            get
            {
                return _WhatIsModo;
            }
            set
            {
                _WhatIsModo = value;
            }
        }
        public string WhatIsUserName
        {
            get
            {
                return _WhatIsUserName;
            }
            set
            {
                _WhatIsUserName = value;
            }
        }
        public Cursor MiCursor
        {
            get
            {
                return _MiCursor;
            }
            set
            {
                _MiCursor = value;
                this.RaisePropertychanged("MiCursor");
            }

        }

        public RelayCommand InitWindow
        {
            get
            {
                if (_InitWindow == null)
                {
                    _InitWindow = new RelayCommand(param => MyOnInitWindow());
                }
                return _InitWindow;
            }
        }

        public string Hora
        {
            get
            {
                return _Hora;
            }
            set
            {
                _Hora = value;
                this.RaisePropertychanged("Hora");
            }
        }
        public string Dia
        {
            get
            {
                return _Dia;
            }
            set
            {
                _Dia = value;
                this.RaisePropertychanged("Dia");
            }
        }


        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    this.RaisePropertychanged("Title");
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
        public string DBEndososCnnStr
        {
            get
            {
                return _DBEndososCnnStr;
            }
        }
        public string DBCeeMasterCnnStr
        {
            get
            {
                return _DBCeeMasterCnnStr;
            }
        }
        public string DBImagenesCnnStr
        {
            get
            {
                return _DBImagenesCnnStr;
            }
        }
        public string DBRadicacionesCEECnnStr
        {
            get
            {
                return _DBRadicacionesCnnStr;
            }
        }

        public void OnShow()
        {
            this.View.Show();
        }

        private void MyDispatcherTimer_Tick(object sender, EventArgs e)
        {
            Hora = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void MyOnInitWindow()
        {
            String yy = System.DateTime.Now.ToString("yyyy");
            Title = String.Format("CEE Sistema de Validación de Endosos " + yy + " Version {0}", AssemblyVersion);
            Dia = DateTime.Now.ToString("MMM/dd/yyyy");
            Hora = DateTime.Now.ToString("hh:mm:ss tt");

            //DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            //    {
            //        Hora = DateTime.Now.ToString("hh:mm:ss tt");
            //    },this.View.Dispatcher);
            //  DispatcherTimer setup

            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(MyDispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            Logclass myLogClass = new Logclass();

            try
            {


                myLogClass.LogName = "Applica";
                myLogClass.MessageFile = string.Empty;
                myLogClass.SourceName = "MainVM";
                myLogClass.CategoryCount = 0;
                myLogClass.DisplayNameMsgId = 256;
                myLogClass.CreateEvent();

                myLogClass.MYEventLog.WriteEntry("APP Start:" + Dia + " " + Hora, EventLogEntryType.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error-MyOnInitWindow", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception("Error en el EventLog " + ex.ToString());
            }

            try
            {
                {//Get values from register
                    try { _SqlServer = jolcode.Registry.read(_REGPATH, "DBServer"); }
                    catch
                    {
                        _SqlServer = string.Empty;
                        jolcode.Registry.write(_REGPATH, "DBServer", string.Empty);

                    }

                    try { _Username = jolcode.Registry.read(_REGPATH, "DBUser"); }
                    catch
                    {
                        _Username = string.Empty;
                        jolcode.Registry.write(_REGPATH, "DBUser", string.Empty);
                    }

                    try { _Password = jolcode.Registry.read(_REGPATH, "DBPass"); }
                    catch
                    {
                        _Password = string.Empty;
                        jolcode.Registry.write(_REGPATH, "DBPass", string.Empty);
                    }

                    try { _Database = jolcode.Registry.read(_REGPATH, "DBName"); }
                    catch
                    {
                        _Database = string.Empty;
                        jolcode.Registry.write(_REGPATH, "DBName", string.Empty);
                    }

                    try { _MastSvr = jolcode.Registry.read(_REGPATH, "MastSvr"); }
                    catch
                    {
                        _MastSvr = string.Empty;
                        jolcode.Registry.write(_REGPATH, "MastSvr", string.Empty);
                    }

                    try { _MastUsr = jolcode.Registry.read(_REGPATH, "MastUsr"); }
                    catch
                    {
                        _MastUsr = string.Empty;
                        jolcode.Registry.write(_REGPATH, "MastUsr", string.Empty);
                    }

                    try { _MastPass = jolcode.Registry.read(_REGPATH, "MastPass"); }
                    catch
                    {
                        _MastPass = string.Empty;
                        jolcode.Registry.write(_REGPATH, "MastPass", string.Empty);
                    }

                    try { _MastDB = jolcode.Registry.read(_REGPATH, "MastDB"); }
                    catch
                    {
                        _MastDB = string.Empty;
                        jolcode.Registry.write(_REGPATH, "MastDB", string.Empty);
                    }

                    try { _ImageSvr = jolcode.Registry.read(_REGPATH, "ImageSvr"); }
                    catch
                    {
                        _ImageSvr = string.Empty;
                        jolcode.Registry.write(_REGPATH, "ImageSvr", string.Empty);
                    }

                    try { _ImageUsr = jolcode.Registry.read(_REGPATH, "ImageUsr"); }
                    catch
                    {
                        _ImageUsr = string.Empty;
                        jolcode.Registry.write(_REGPATH, "ImageUsr", string.Empty);
                    }

                    try { _ImagePass = jolcode.Registry.read(_REGPATH, "ImagePass"); }
                    catch
                    {
                        _ImagePass = string.Empty;
                        jolcode.Registry.write(_REGPATH, "ImagePass", string.Empty);
                    }

                    try { _ImageDB = jolcode.Registry.read(_REGPATH, "ImageDB"); }
                    catch
                    {
                        _ImageDB = string.Empty;
                        jolcode.Registry.write(_REGPATH, "ImageDB", string.Empty);
                    }

                    try { _RadicacionesSvr = jolcode.Registry.read(_REGPATH, "RadicacionesSvr"); }
                    catch
                    {
                        _RadicacionesSvr = string.Empty;
                        jolcode.Registry.write(_REGPATH, "RadicacionesSvr", string.Empty);
                    }

                    try { _RadicacionesUsr = jolcode.Registry.read(_REGPATH, "RadicacionesUsr"); }
                    catch
                    {
                        _RadicacionesUsr = string.Empty;
                        jolcode.Registry.write(_REGPATH, "RadicacionesUsr", string.Empty);
                    }

                    try { _RadicacionesPass = jolcode.Registry.read(_REGPATH, "RadicacionesPass"); }
                    catch
                    {
                        _RadicacionesPass = string.Empty;
                        jolcode.Registry.write(_REGPATH, "RadicacionesPass", string.Empty);
                    }

                    try { _RadicacionesDB = jolcode.Registry.read(_REGPATH, "RadicacionesDB"); }
                    catch
                    {
                        _RadicacionesDB = string.Empty;
                        jolcode.Registry.write(_REGPATH, "RadicacionesDB", string.Empty);
                    }

                    try { _PDFPath = jolcode.Registry.read(_REGPATH, "ImagePathNew"); }
                    catch
                    {
                        _PDFPath = string.Empty;
                        jolcode.Registry.write(_REGPATH, "ImagePathNew", string.Empty);
                    }
                }

                //if ((_SqlServer == "") || (_Username == "") || (_Database == "") || (_Password == "") || (_MastSvr == "") || (_MastUsr == "") || (_MastDB == "") || (_MastPass == "") ||
                //    (_ImageSvr == "") || (_ImageUsr == "") || (_ImageDB == "") || (_ImagePass == "") ||  (_ImgPath == "") || (_ValiSvr == "") || (_ValiUsr == "") || (_ValiDB == "") ||
                //    (_ValiPass == ""))

                if ((_SqlServer.Trim().Length == 0) ||
                        (_Username.Trim().Length == 0) ||
                        (_Database.Trim().Length == 0) ||
                       (_Password.Trim().Length == 0) ||
                       (_MastSvr.Trim().Length == 0) ||
                       (_MastUsr.Trim().Length == 0) ||
                       (_MastDB.Trim().Length == 0) ||
                       (_MastPass.Trim().Length == 0) ||
                       (_ImageSvr.Trim().Length == 0) ||
                       (_ImageUsr.Trim().Length == 0) ||
                       (_ImageDB.Trim().Length == 0) ||
                       (_ImagePass.Trim().Length == 0) ||
                       (_PDFPath.Trim().Length == 0) ||
                       (_RadicacionesSvr.Trim().Length == 0) ||
                       (_RadicacionesUsr.Trim().Length == 0) ||
                       (_RadicacionesDB.Trim().Length == 0) ||
                       (_RadicacionesPass.Trim().Length == 0))
                {
                    using (vmMantDB frmMantDB = new vmMantDB(_REGPATH))
                    {
                        frmMantDB.sqlServer = _SqlServer;
                        frmMantDB.userName = _Username;
                        frmMantDB.password = _Password;
                        frmMantDB.database = _Database;

                        frmMantDB.mastSvr = _MastSvr;
                        frmMantDB.mastUsr = _MastUsr;
                        frmMantDB.mastPass = _MastPass;
                        frmMantDB.mastDB = _MastDB;

                        frmMantDB.imageSvr = _ImageSvr;
                        frmMantDB.imageUsr = _ImageUsr;
                        frmMantDB.imagePass = _ImagePass;
                        frmMantDB.imageDB = _ImageDB;

                        frmMantDB.RadicacionesSvr = _RadicacionesSvr;
                        frmMantDB.RadicacionesUsr = _RadicacionesUsr;
                        frmMantDB.RadicacionesPass = _RadicacionesPass;
                        frmMantDB.RadicacionesDB = _RadicacionesDB;

                        frmMantDB.imgPath = _PDFPath;

                        frmMantDB.View.Owner = this.View as Window;

                        frmMantDB.OnShow();

                        _SqlServer = frmMantDB.sqlServer;
                        _Username = frmMantDB.userName;
                        _Password = PasswordHash.Encrypt1(frmMantDB.password);
                        _Database = frmMantDB.database;

                        _MastSvr = frmMantDB.mastSvr;
                        _MastUsr = frmMantDB.mastUsr;
                        _MastPass = PasswordHash.Encrypt1(frmMantDB.mastPass);
                        _MastDB = frmMantDB.mastDB;

                        _ImageSvr = frmMantDB.imageSvr;
                        _ImageUsr = frmMantDB.imageUsr;
                        _ImagePass = PasswordHash.Encrypt1(frmMantDB.imagePass);
                        _ImageDB = frmMantDB.imageDB;

                        _RadicacionesSvr = frmMantDB.RadicacionesSvr;
                        _RadicacionesUsr = frmMantDB.RadicacionesUsr;
                        _RadicacionesPass = PasswordHash.Encrypt1(frmMantDB.RadicacionesPass);
                        _RadicacionesDB = frmMantDB.RadicacionesDB;

                        _PDFPath = frmMantDB.imgPath;
                    }// end using
                }//End IF
                mnuChangePassword_IsEnabled = false;
                mnuLogout_IsEnabled = false;
                mnuLogin_IsEnabled = true;
                mnuVerElector_IsEnabled = false;
                mnuInformeEndosos_IsEnabled = false;
                mnuRecibirLotes_IsEnabled = false;
                mnuAutoRizarLotes_IsEnabled = false;
                mnuProcesarLotes_IsEnabled = false;
                mnuCorregirEndosos_IsEnabled = false;
                mnuRevLote_IsEnabled = false;
                mnuAreas_IsEnabled = false;
                mnuPartidos_IsEnabled = false;
                mnuNotarios_IsEnabled = false;
                mnuValidaciones_IsEnabled = false;
                mnuUsuarios_IsEnabled = false;
                mnuBaseDeDatos_IsEnabled = false;
                mnuInicializarLotes_IsEnabled = false;
                mnuVerEndosos_IsEnabled = false;
                mnuInformeEndosos_IsEnabled = false;
                mnuReydi_IsEnabled = false;
                mnuInformeDuplicados_IsEnable = false;
                mnuduplicadopornumelectoral_IsEnable = false;

                _DBEndososCnnStr = string.Concat("Persist Security Info=False;Data Source=", _SqlServer, ";Initial Catalog=", _Database, ";User ID=", _Username, ";Password=", PasswordHash.Decrypt1(_Password));
                _DBCeeMasterCnnStr = string.Concat("Persist Security Info=False;Data Source=", _MastSvr, ";Initial Catalog=", _MastDB, ";User ID=", _MastUsr, ";Password=", PasswordHash.Decrypt1(_MastPass));
                _DBImagenesCnnStr = string.Concat("Persist Security Info=False;Data Source=", _ImageSvr, ";Initial Catalog=", _ImageDB, ";User ID=", _ImageUsr, ";Password=", PasswordHash.Decrypt1(_ImagePass));
                _DBRadicacionesCnnStr = string.Concat("Persist Security Info=False;Data Source=", _RadicacionesSvr, ";Initial Catalog=", _RadicacionesDB, ";User ID=", _RadicacionesUsr, ";Password=", PasswordHash.Decrypt1(_RadicacionesPass));

                Login_Click();


            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    MessageBox.Show(ex.ToString(), "MyOnInitWindow", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.View.Close();

                }
                else
                {
                    MessageBox.Show(ex.ToString(), "MyOnInitWindow", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                try
                {
                    myLogClass.MYEventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error, 9000);
                }
                catch { }


            }

        }
    }
}


