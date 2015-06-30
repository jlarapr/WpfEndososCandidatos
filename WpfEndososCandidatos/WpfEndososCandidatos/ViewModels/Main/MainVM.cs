
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
                    try { _SqlServer = jolcode.Registry.read(_REGPATH, "DBServer"); }
                    catch
                    {
                        _SqlServer = string.Empty;
                        jolcode.Registry.write(_REGPATH, "DBServer",string.Empty);

                    }

                    try { _Username = jolcode.Registry.read(_REGPATH, "DBUser"); }
                    catch
                    {
                        _Username = string.Empty;
                        jolcode.Registry.write(_REGPATH, "DBUser",string.Empty); 
                    }

                    try { _Password = jolcode.Registry.read(_REGPATH, "DBPass"); }
                    catch
                    {
                        _Password = string.Empty;
                        jolcode.Registry.write(_REGPATH, "DBPass",string.Empty);
                    }

                    try { _Database = jolcode.Registry.read(_REGPATH, "DBName"); }
                    catch
                    {
                        _Database = string.Empty;
                        jolcode.Registry.write(_REGPATH, "DBName",string.Empty); 
                    }

                    try { _MastSvr = jolcode.Registry.read(_REGPATH, "MastSvr"); }
                    catch
                    {
                        _MastSvr = string.Empty;
                        jolcode.Registry.write(_REGPATH, "MastSvr",string.Empty);
                    }

                    try { _MastUsr = jolcode.Registry.read(_REGPATH, "MastUsr"); }
                    catch
                    {
                        _MastUsr = string.Empty;
                        jolcode.Registry.write(_REGPATH, "MastUsr",string.Empty);
                    }

                    try { _MastPass = jolcode.Registry.read(_REGPATH, "MastPass"); }
                    catch
                    {
                        _MastPass = string.Empty;
                        jolcode.Registry.write(_REGPATH, "MastPass",string.Empty);
                    }

                    try { _MastDB = jolcode.Registry.read(_REGPATH, "MastDB"); }
                    catch
                    {
                        _MastDB = string.Empty;
                        jolcode.Registry.write(_REGPATH, "MastDB",string.Empty);
                    }

                    try { _ImageSvr = jolcode.Registry.read(_REGPATH, "ImageSvr"); }
                    catch
                    {
                        _ImageSvr = string.Empty;
                        jolcode.Registry.write(_REGPATH, "ImageSvr",string.Empty);
                    }

                    try { _ImageUsr = jolcode.Registry.read(_REGPATH, "ImageUsr"); }
                    catch
                    {
                        _ImageUsr = string.Empty;
                        jolcode.Registry.write(_REGPATH, "ImageUsr",string.Empty);
                    }

                    try { _ImagePass = jolcode.Registry.read(_REGPATH, "ImagePass"); }
                    catch
                    {
                        _ImagePass = string.Empty;
                        jolcode.Registry.write(_REGPATH, "ImagePass",string.Empty);
                    }

                    try { _ImageDB = jolcode.Registry.read(_REGPATH, "ImageDB"); }
                    catch
                    {
                        _ImageDB = string.Empty;
                        jolcode.Registry.write(_REGPATH, "ImageDB",string.Empty);
                    }

                    try { _ValiSvr = jolcode.Registry.read(_REGPATH, "ValiSvr"); }
                    catch
                    {
                        _ValiSvr = string.Empty;
                        jolcode.Registry.write(_REGPATH, "ValiSvr",string.Empty);
                    }

                    try { _ValiUsr = jolcode.Registry.read(_REGPATH, "ValiUsr"); }
                    catch
                    {
                        _ValiUsr = string.Empty;
                        jolcode.Registry.write(_REGPATH, "ValiUsr",string.Empty);
                    }

                    try { _ValiPass = jolcode.Registry.read(_REGPATH, "ValiPass"); }
                    catch
                    {
                        _ValiPass = string.Empty;
                        jolcode.Registry.write(_REGPATH, "ValiPass",string.Empty);
                    }

                    try { _ValiDB = jolcode.Registry.read(_REGPATH, "ValiDB"); }
                    catch
                    {
                        _ValiDB = string.Empty;
                        jolcode.Registry.write(_REGPATH, "ValiDB",string.Empty);
                    }

                    try { _ImgPath = jolcode.Registry.read(_REGPATH, "ImagePathNew"); }
                    catch
                    {
                        _ImgPath = string.Empty;
                        jolcode.Registry.write(_REGPATH, "ImagePathNew",string.Empty);
                    }
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


