
namespace WpfEndososCandidatos.ViewModel
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
    using WpfEndososCandidatos.View;

    
    partial class  MainVM : ViewModelBase<IMainView>
    {
        private RelayCommand _InitWindow;
        private string _Title;
        private const string _REGPATH = "SOFTWARE\\CEE\\Endosos\\Partidos"; 
        
        private string _SqlServer=string.Empty;
        private string _Username =string.Empty;
        private string _Password =string.Empty;
        private string _Database =string.Empty;
        
        private string _MastSvr =string.Empty;
        private string _MastUsr =string.Empty;
        private string _MastPass =string.Empty;
        private string _MastDB = string.Empty;
        
        private string _ImageSvr =string.Empty;
        private string _ImageUsr =string.Empty;
        private string _ImagePass=string.Empty;
        private string _ImageDB =string.Empty;
        
        private string _ValiSvr =string.Empty;
        private string _ValiUsr =string.Empty;
        private string _ValiPass =string.Empty;
        private string _ValiDB = string.Empty;

        private string _ImgPath = string.Empty;
        private RelayCommand _Close;
        
        public MainVM()
            : base(new MainWindow())
        {

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

        public RelayCommand close
        {
            get
            {
                if (_Close == null)
                {
                    _Close = new RelayCommand(param => Close());
                }
                return _Close;
            }
        }

        private void Close()
        {
            
            this.View.Close();
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

        public void OnShow()
        {
            this.View.Show();
        }
       

        private void OnInitWindow()
        {
            Title = String.Format("CEE Endosos Candidatos 2015 Version {0}", AssemblyVersion);

            try
            {
                {//Get values from register
                    _SqlServer = jolcode.Registry.read(_REGPATH, "DBServer");
                    _Username = jolcode.Registry.read(_REGPATH, "DBUser");
                    _Password = jolcode.Registry.read(_REGPATH, "DBPass");
                    _Database = jolcode.Registry.read(_REGPATH, "DBName");

                    _MastSvr = jolcode.Registry.read(_REGPATH, "MastSvr");
                    _MastUsr = jolcode.Registry.read(_REGPATH, "MastUsr");
                    _MastPass = jolcode.Registry.read(_REGPATH, "MastPass");
                    _MastDB = jolcode.Registry.read(_REGPATH, "MastDB");

                    _ImageSvr = jolcode.Registry.read(_REGPATH, "ImageSvr");
                    _ImageUsr = jolcode.Registry.read(_REGPATH, "ImageUsr");
                    _ImagePass = jolcode.Registry.read(_REGPATH, "ImagePass");
                    _ImageDB = jolcode.Registry.read(_REGPATH, "ImageDB");

                    _ValiSvr = jolcode.Registry.read(_REGPATH, "ValiSvr");
                    _ValiUsr = jolcode.Registry.read(_REGPATH, "ValiUsr");
                    _ValiPass = jolcode.Registry.read(_REGPATH, "ValiPass");
                    _ValiDB = jolcode.Registry.read(_REGPATH, "ValiDB");

                    _ImgPath = jolcode.Registry.read(_REGPATH, "ImagePathNew");
                }

                //if ((_SqlServer == "") || (_Username == "") || (_Database == "") || (_Password == "") || (_MastSvr == "") || (_MastUsr == "") || (_MastDB == "") || (_MastPass == "") ||
                //    (_ImageSvr == "") || (_ImageUsr == "") || (_ImageDB == "") || (_ImagePass == "") ||  (_ImgPath == "") || (_ValiSvr == "") || (_ValiUsr == "") || (_ValiDB == "") ||
                //    (_ValiPass == ""))

                if ((_SqlServer.Trim().Length == 0)     ||
                        (_Username.Trim().Length == 0)  ||
                        (_Database.Trim().Length == 0)  ||
                       (_Password.Trim().Length == 0)   ||
                       (_MastSvr.Trim().Length == 0)    ||
                       (_MastUsr.Trim().Length == 0)    ||
                       (_MastDB.Trim().Length == 0)     ||
                       (_MastPass.Trim().Length == 0)   ||
                       (_ImageSvr.Trim().Length == 0)   ||
                       (_ImageUsr.Trim().Length == 0)   ||
                       (_ImageDB.Trim().Length == 0)    ||
                       (_ImagePass.Trim().Length == 0)  ||
                       (_ImgPath.Trim().Length == 0)    ||
                       (_ValiSvr.Trim().Length == 0)    ||
                       (_ValiUsr.Trim().Length == 0)    ||
                       (_ValiDB.Trim().Length == 0)     ||
                       (_ValiPass.Trim().Length == 0))
                {
                    vmMantDB frmMantDB = new vmMantDB(_REGPATH);

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

                    frmMantDB.valiSvr = _ValiSvr;
                    frmMantDB.valiUsr = _ValiUsr;
                    frmMantDB.valiPass = _ValiPass;
                    frmMantDB.valiDB = _ValiDB;

                    frmMantDB.imgPath = _ImgPath;

                    frmMantDB.View.Owner = this.View as Window;

                    frmMantDB.OnShow();

                    _SqlServer=frmMantDB.sqlServer;
                    _Username=frmMantDB.userName;
                    _Password=frmMantDB.password;
                    _Database=frmMantDB.database;

                    _MastSvr=frmMantDB.mastSvr;
                    _MastUsr=frmMantDB.mastUsr;
                    _MastPass=frmMantDB.mastPass;
                    _MastDB=frmMantDB.mastDB;

                    _ImageSvr=frmMantDB.imageSvr;
                    _ImageUsr=frmMantDB.imageUsr;
                    _ImagePass=frmMantDB.imagePass;
                    _ImageDB=frmMantDB.imageDB;

                    _ValiSvr=frmMantDB.valiSvr;
                    _ValiUsr=frmMantDB.valiUsr;
                    _ValiPass=frmMantDB.valiPass;
                    _ValiDB=frmMantDB.valiDB;

                    _ImgPath = frmMantDB.imgPath;

                }//End IF

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "OnInitWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }


        





    }

     

}


