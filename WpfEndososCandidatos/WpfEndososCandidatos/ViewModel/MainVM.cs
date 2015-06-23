
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

    
    class MainVM : ViewModelBase<IMainView>
    {
        private RelayCommand _InitWindow;
        private string _Title;
        private const string _REGPATH = "SOFTWARE\\CEE\\Endosos\\Partidos"; // Software\\CEE\\WEBCEE\\Enlaces
        
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

                if ((_SqlServer == "") || (_Username == "") || (_Database == "") || (_Password == "") || (_MastSvr == "") || (_MastUsr == "") || (_MastDB == "") || (_MastPass == "") ||
                    (_ImageSvr == "") || (_ImageUsr == "") || (_ImageDB == "") || (_ImagePass == "") ||  (_ImgPath == "") || (_ValiSvr == "") || (_ValiUsr == "") || (_ValiDB == "") ||
                    (_ValiPass == ""))
                {
                                        

                    vmMantDB frmMantDB = new vmMantDB();

                    frmMantDB.OnShow();

                }//End IF
        
    



            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "OnInitWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }


    }
}
