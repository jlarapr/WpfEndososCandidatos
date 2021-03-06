﻿namespace WpfEndososCandidatos.ViewModels
{
    using jolcode;
    using jolcode.Base;
    using jolcode.MyInterface;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using WpfEndososCandidatos.Helper;
    using WpfEndososCandidatos.View;
    class vmMantDB : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private string _dfPathtoPictures_txt;

        private RelayCommand _InitWindow;
        private RelayCommand _cmdBrowseImagesPath_Click;
        private RelayCommand _cmdCancel_Click;
        private RelayCommand _cmdOk_Click;
        private RelayCommand _cmdReloadDBs_Click;
        private RelayCommand _cmdReloadMastDBs_Click;
        private RelayCommand _cmdReloadImageDBs_Click;

        private string _dfServer_txt;
        private string _dfUsername_txt;
        private string _dfPassword_txt;
        private ObservableCollection<string> _cbDatabase;

        private string _dfMastSvr_txt;
        private string _dfMastUsr_txt;
        private string _dfMastPass_txt;
        private ObservableCollection<string> _cbMastDB;

        private string _dfImageSvr_txt;
        private string _dfImageUsr_txt;
        private string _dfImagePass_txt;
        private ObservableCollection<string> _cbImageDB;

        private string _cbImageDB_Item;
        private string _cbMastDB_Item;
        private string _cbDatabase_Item;

        private int _cbDatabase_Item_Id;
        private int _cbMastDB_Item_Id;
        private int _cbImageDB_Item_Id;

        private string _REGPATH;
        private bool _SaveReg;
        private Brush _BorderBrush;
        private string _RadicacionesSvr_txt;
        private string _RadicacionesUsr_txt;
        private string _RadicacionesUPass_txt;
        private ObservableCollection<string> _cbRadicacionesDB;
        private string _cbRadicacionesDB_Item;
        private int _cbRadicacionesDB_Item_Id;
        private RelayCommand _cmdReloadRadicacionesDBs_Click;

        public string sqlServer { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string database { get; set; }

        public string mastSvr { get; set; }
        public string mastUsr { get; set; }
        public string mastPass { get; set; }
        public string mastDB { get; set; }

        public string imageSvr { get; set; }
        public string imageUsr { get; set; }
        public string imagePass { get; set; }
        public string imageDB { get; set; }
        public string RadicacionesSvr { get; set; }
        public string RadicacionesUsr { get; set; }
        public string RadicacionesPass { get; set; }
        public string RadicacionesDB { get; set; }
        public string imgPath { get; set; }

        public vmMantDB(string regpath)
            : base(new wpfMantDB())
        {
            _REGPATH = regpath;
        }


        #region Property
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

        public string dfPathtoPictures_txt
        {
            get
            {
                return _dfPathtoPictures_txt;
            }
            set
            {
                if (_dfPathtoPictures_txt != value)
                {
                    _dfPathtoPictures_txt = value;
                    this.RaisePropertychanged("dfPathtoPictures_txt");
                }
            }
        }
        public string dfServer_txt
        {
            get
            {
                return _dfServer_txt;
            }
            set
            {
                if (_dfServer_txt != value)
                {
                    _dfServer_txt = value;
                    this.RaisePropertychanged("dfServer_txt");
                }
            }
        }
        public string dfUsername_txt
        {
            get
            {
                return _dfUsername_txt;
            }
            set
            {
                if (_dfUsername_txt != value)
                {
                    _dfUsername_txt = value;
                    this.RaisePropertychanged("dfUsername_txt");
                }
            }
        }
        public string dfPassword_txt
        {
            get
            {
                return _dfPassword_txt;
            }
            set
            {
                if (dfPassword_txt != value)
                {
                    _dfPassword_txt = value;

                    this.RaisePropertychanged("dfPassword_txt");
                }
            }
        }

        public ObservableCollection<string> cbDatabaseEndoso
        {
            get
            {
                return _cbDatabase;
            }
            set
            {
                //  if (_cbDatabase != value)
                {
                    _cbDatabase = value;
                    this.RaisePropertychanged("cbDatabaseEndoso");
                }
            }
        }
        public string cbDatabase_Item
        {
            get
            {
                return _cbDatabase_Item;
            }
            set
            {
                //if (_cbDatabase_Item != value)
                {
                    _cbDatabase_Item = value;
                    this.RaisePropertychanged("cbDatabase_Item");
                    //cbDatabase = null;
                    //cbDatabase = new List<string>();
                }
            }
        }
        public int cbDatabase_Item_Id
        {
            get
            {
                return _cbDatabase_Item_Id;
            }
            set
            {
                _cbDatabase_Item_Id = value;
                this.RaisePropertychanged("cbDatabase_Item_Id");
            }

        }
        public string dfMastSvr_txt
        {
            get
            {
                return _dfMastSvr_txt;
            }
            set
            {
                if (_dfMastSvr_txt != value)
                {
                    _dfMastSvr_txt = value;
                    this.RaisePropertychanged("dfMastSvr_txt");
                }
            }
        }
        public string dfMastUsr_txt
        {
            get
            {
                return _dfMastUsr_txt;
            }
            set
            {
                if (_dfMastUsr_txt != value)
                {
                    _dfMastUsr_txt = value;
                    this.RaisePropertychanged("dfMastUsr_txt");
                }
            }
        }
        public string dfMastPass_txt
        {
            get
            {
                return _dfMastPass_txt;
            }
            set
            {
                if (_dfMastPass_txt != value)
                {
                    _dfMastPass_txt = value;
                    this.RaisePropertychanged("dfMastPass_txt");
                }
            }
        }

        public ObservableCollection<string> cbMastDB
        {
            get
            {
                return _cbMastDB;
            }
            set
            {

                _cbMastDB = value;
                this.RaisePropertychanged("cbMastDB");
            }
        }
        public string cbMastDB_Item
        {
            get
            {
                return _cbMastDB_Item;
            }
            set
            {
                if (_cbMastDB_Item != value)
                {
                    _cbMastDB_Item = value;
                    this.RaisePropertychanged("cbMastDB_Item");
                }
            }
        }
        public int cbMastDB_Item_Id
        {
            get
            {
                return _cbMastDB_Item_Id;
            }
            set
            {
                _cbMastDB_Item_Id = value;
                this.RaisePropertychanged("cbMastDB_Item_Id");
            }
        }
        public string dfImageSvr_txt
        {
            get
            {
                return _dfImageSvr_txt;
            }
            set
            {
                if (_dfImageSvr_txt != value)
                {
                    _dfImageSvr_txt = value;
                    this.RaisePropertychanged("dfImageSvr_txt");
                }
            }
        }
        public string dfImageUsr_txt
        {
            get
            {
                return _dfImageUsr_txt;
            }
            set
            {
                if (_dfImageUsr_txt != value)
                {
                    _dfImageUsr_txt = value;
                    this.RaisePropertychanged("dfImageUsr_txt");
                }
            }
        }
        public string dfImagePass_txt
        {
            get
            {
                return _dfImagePass_txt;
            }
            set
            {
                if (_dfImagePass_txt != value)
                {
                    _dfImagePass_txt = value;
                    this.RaisePropertychanged("dfImagePass_txt");
                }
            }
        }

        public ObservableCollection<string> cbImageDB
        {
            get
            {
                return _cbImageDB;
            }
            set
            {
                _cbImageDB = value;
                this.RaisePropertychanged("cbImageDB");
            }
        }
        public string cbImageDB_Item
        {
            get
            {
                return _cbImageDB_Item;
            }
            set
            {
                if (_cbImageDB_Item != value)
                {
                    _cbImageDB_Item = value;
                    this.RaisePropertychanged("cbImageDB_Item");
                    //cbImageDB = null;
                    //cbImageDB = new List<string>();
                }
            }
        }
        public int cbImageDB_Item_Id
        {
            get
            {
                return _cbImageDB_Item_Id;
            }
            set
            {
                _cbImageDB_Item_Id = value;
                this.RaisePropertychanged("cbImageDB_Item_Id");
            }
        }

        public string RadicacionesSvr_txt
        {
            get
            {
                return _RadicacionesSvr_txt;
            } set
            {
                _RadicacionesSvr_txt = value;
                this.RaisePropertychanged("RadicacionesSvr_txt");
            }
        }
        public string RadicacionesUsr_txt
        {
            get
            {
                return _RadicacionesUsr_txt;
            }
            set
            {
                _RadicacionesUsr_txt = value;
                this.RaisePropertychanged("RadicacionesUsr_txt");
            }
        }
        public string RadicacionesUPass_txt
        {
            get
            {
                return _RadicacionesUPass_txt;
            } set
            {
                if (_RadicacionesUPass_txt != value)
                {
                    _RadicacionesUPass_txt = value;
                    this.RaisePropertychanged("RadicacionesUPass_txt");
                }
            }
        }

        public ObservableCollection<string> cbRadicacionesDB
        {
            get
            {
                return _cbRadicacionesDB;
            }
            set
            {
                _cbRadicacionesDB = value;
                this.RaisePropertychanged("cbRadicacionesDB");
            }
        }
        public string cbRadicacionesDB_Item
        {
            get
            {
                return _cbRadicacionesDB_Item;
            }
            set
            {
                if (_cbRadicacionesDB_Item != value)
                {
                    _cbRadicacionesDB_Item = value;
                    this.RaisePropertychanged("cbRadicacionesDB_Item");
                }
            }
        }
        public int cbRadicacionesDB_Item_Id
        {
            get
            {
                return _cbRadicacionesDB_Item_Id;
            }
            set
            {
                _cbRadicacionesDB_Item_Id = value;
                this.RaisePropertychanged("cbRadicacionesDB_Item_Id");
            }
        }
        #endregion

        #region MyCmd


        public RelayCommand cmdBrowseImagesPath_Click
        {
            get
            {
                if (_cmdBrowseImagesPath_Click == null)
                {
                    _cmdBrowseImagesPath_Click = new RelayCommand(param => CmdBrowseImagesPath_Click());
                }
                return _cmdBrowseImagesPath_Click;
            }
        }
        public RelayCommand cmdCancel_Click
        {
            get
            {
                if (_cmdCancel_Click == null)
                {
                    _cmdCancel_Click = new RelayCommand(param => CmdCancel_Click());
                }
                return _cmdCancel_Click;
            }
        }

        public RelayCommand cmdReloadDBs_Click
        {
            get
            {
                if (_cmdReloadDBs_Click == null)
                {
                    _cmdReloadDBs_Click = new RelayCommand(param => CmdReloadDBs_Click());
                }
                return _cmdReloadDBs_Click;
            }
        }
        public RelayCommand cmdReloadMastDBs_Click
        {
            get
            {
                if (_cmdReloadMastDBs_Click == null)
                {
                    _cmdReloadMastDBs_Click = new RelayCommand(param => CmdReloadMastDBs_Click());
                }
                return _cmdReloadMastDBs_Click;
            }
        }
        public RelayCommand cmdReloadImageDBs_Click
        {
            get
            {
                if (_cmdReloadImageDBs_Click == null)
                {
                    _cmdReloadImageDBs_Click = new RelayCommand(param => CmdReloadImageDBs_Click());
                }
                return _cmdReloadImageDBs_Click;
            }
        }
        public RelayCommand cmdOk_Click
        {
            get
            {
                if (_cmdOk_Click == null)
                {
                    _cmdOk_Click = new RelayCommand(param => CmdOk_Click());
                }
                return _cmdOk_Click;
            }
        }
        public RelayCommand cmdReloadRadicacionesDBs_Click
        {
            get
            {
                if (_cmdReloadRadicacionesDBs_Click == null)
                {
                    _cmdReloadRadicacionesDBs_Click = new RelayCommand(param => MyCmdReloadRadicacionesDBs_Click());
                }
                return _cmdReloadRadicacionesDBs_Click;
            }
        }


        private void MyCmdReloadRadicacionesDBs_Click()
        {
            try
            {
                if (RadicacionesSvr_txt.Trim().Length > 0)
                {
                    cbRadicacionesDB = null;
                    cbRadicacionesDB = new ObservableCollection<string>();
                    cbRadicacionesDB.Clear();

                    GetDataBaseName(RadicacionesSvr_txt, RadicacionesUsr_txt, RadicacionesUPass_txt, cbRadicacionesDB);


                    if (cbRadicacionesDB.Count > 0)
                    {
                        cbRadicacionesDB_Item_Id = 0;
                    }
                    else
                    {
                        cbRadicacionesDB_Item_Id = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "MyCmdReloadRadicacionesDBs_Click", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CmdOk_Click()
        {
            try
            {
                _SaveReg = false;

                sqlServer = dfServer_txt.Trim();
                userName = dfUsername_txt.Trim();

                // IntPtr passwordBSTR = default(IntPtr);
                //   string insecurePassword = "";
                // passwordBSTR = Marshal.SecureStringToBSTR(dfPassword_txt);
                //   insecurePassword = Marshal.PtrToStringBSTR(passwordBSTR);

                password = dfPassword_txt;// insecurePassword;
                database = cbDatabase_Item.Trim();

                mastSvr = dfMastSvr_txt.Trim();
                mastUsr = dfMastUsr_txt.Trim();

                //     insecurePassword = "";
                //passwordBSTR = Marshal.SecureStringToBSTR(dfMastPass_txt);
                // insecurePassword = Marshal.PtrToStringBSTR(passwordBSTR);

                mastPass = dfMastPass_txt;//insecurePassword;
                mastDB = cbMastDB_Item.Trim();

                imageSvr = dfImageSvr_txt.Trim();
                imageUsr = dfImageUsr_txt.Trim();

                //   insecurePassword = "";
                // passwordBSTR = Marshal.SecureStringToBSTR(dfImagePass_txt);
                // insecurePassword = Marshal.PtrToStringBSTR(passwordBSTR);

                imagePass = dfImagePass_txt;//insecurePassword;
                imageDB = cbImageDB_Item.Trim();

                RadicacionesSvr = RadicacionesSvr_txt.Trim();
                RadicacionesUsr = RadicacionesUsr_txt.Trim();
                RadicacionesPass = RadicacionesUPass_txt.Trim();
                RadicacionesDB = cbRadicacionesDB_Item.Trim();


                string PDFPath = dfPathtoPictures_txt.Trim();

                if (PDFPath.Trim().Length == 0)
                    throw new Exception("Error con los Path");

                imgPath = PDFPath;

                if ((sqlServer.Trim().Length == 0) ||
                    (userName.Trim().Length == 0) ||
                    (database.Trim().Length == 0) ||
                    (password.Trim().Length == 0) ||
                    (mastSvr.Trim().Length == 0) ||
                    (mastUsr.Trim().Length == 0) ||
                    (mastDB.Trim().Length == 0) ||
                    (mastPass.Trim().Length == 0) ||
                    (imageSvr.Trim().Length == 0) ||
                    (imageUsr.Trim().Length == 0) ||
                    (imageDB.Trim().Length == 0) ||
                    (imagePass.Trim().Length == 0) ||

                    (RadicacionesSvr.Trim().Length == 0) ||
                    (RadicacionesUsr.Trim().Length == 0) ||
                    (RadicacionesDB.Trim().Length == 0) ||
                    (RadicacionesPass.Trim().Length == 0))
                {

                    throw new Exception("Error con los Parametros de la Base de datos");
                }
                else
                {

                    //jolcode.Registry.write(_REGPATH,)


                    jolcode.Registry.write(_REGPATH, "DBServer", sqlServer);
                    jolcode.Registry.write(_REGPATH, "DBUser", userName);
                    jolcode.Registry.write(_REGPATH, "DBPass", PasswordHash.Encrypt1(password));
                    jolcode.Registry.write(_REGPATH, "DBName", database);

                    jolcode.Registry.write(_REGPATH, "MastSvr", mastSvr);
                    jolcode.Registry.write(_REGPATH, "MastUsr", mastUsr);
                    jolcode.Registry.write(_REGPATH, "MastPass", PasswordHash.Encrypt1(mastPass));
                    jolcode.Registry.write(_REGPATH, "MastDB", mastDB);

                    jolcode.Registry.write(_REGPATH, "ImageSvr", imageSvr);
                    jolcode.Registry.write(_REGPATH, "ImageUsr", imageUsr);
                    jolcode.Registry.write(_REGPATH, "ImagePass", PasswordHash.Encrypt1(imagePass));
                    jolcode.Registry.write(_REGPATH, "ImageDB", imageDB);

                    jolcode.Registry.write(_REGPATH, "RadicacionesSvr", RadicacionesSvr);
                    jolcode.Registry.write(_REGPATH, "RadicacionesUsr", RadicacionesUsr);
                    jolcode.Registry.write(_REGPATH, "RadicacionesPass", PasswordHash.Encrypt1(RadicacionesPass));
                    jolcode.Registry.write(_REGPATH, "RadicacionesDB", RadicacionesDB);

                    jolcode.Registry.write(_REGPATH, "ImagePathNew", PDFPath);

                    _SaveReg = true;

                    MessageBox.Show("Done...", "CmdOk_Click", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.View.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CmdOk_Click", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CmdReloadImageDBs_Click()
        {
            try
            {
                if (dfImageSvr_txt.Trim().Length > 0)
                {
                    //IntPtr passwordBSTR = default(IntPtr);
                    //string insecurePassword = "";
                    //passwordBSTR = Marshal.SecureStringToBSTR(dfImagePass_txt);
                    //insecurePassword = Marshal.PtrToStringBSTR(passwordBSTR);
                    cbImageDB = null;
                    cbImageDB = new ObservableCollection<string>();
                    cbImageDB.Clear();



                    GetDataBaseName(_dfImageSvr_txt, _dfImageUsr_txt, dfImagePass_txt, cbImageDB);



                    if (cbImageDB.Count > 0)
                    {
                        cbImageDB_Item_Id = 0;
                    }
                    else
                    {
                        cbImageDB_Item_Id = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "CmdReloadImageDBs_Click", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CmdReloadMastDBs_Click()
        {
            try
            {
                if (dfMastSvr_txt.Trim().Length > 0)
                {
                    //IntPtr passwordBSTR = default(IntPtr);
                    //string insecurePassword = "";
                    //passwordBSTR = Marshal.SecureStringToBSTR(dfMastPass_txt);
                    //insecurePassword = Marshal.PtrToStringBSTR(passwordBSTR);
                    cbMastDB = null;
                    cbMastDB = new ObservableCollection<string>();
                    cbMastDB.Clear();

                    GetDataBaseName(dfMastSvr_txt, dfMastUsr_txt, dfMastPass_txt, cbMastDB);


                    if (cbMastDB.Count > 0)
                    {
                        cbMastDB_Item_Id = 0;
                    }
                    else
                    {
                        cbMastDB_Item_Id = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "CmdReloadMastDBs_Click", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CmdReloadDBs_Click()
        {
            try
            {

                if (dfServer_txt.Trim().Length > 0)
                {
                    // IntPtr passwordBSTR = default(IntPtr);
                    //string insecurePassword = "";
                    // passwordBSTR = Marshal.SecureStringToBSTR(dfPassword_txt);
                    // insecurePassword = Marshal.PtrToStringBSTR(passwordBSTR);

                    cbDatabaseEndoso.Clear();
                    cbDatabaseEndoso = null;
                    cbDatabaseEndoso = new ObservableCollection<string>();

                    GetDataBaseName(dfServer_txt, dfUsername_txt, dfPassword_txt, cbDatabaseEndoso);


                    if (cbDatabaseEndoso.Count > 0)
                    {
                        cbDatabase_Item_Id = 0;
                    }
                    else
                    {
                        cbDatabase_Item_Id = -1;
                    }


                }
                else
                {
                    cbDatabaseEndoso = null;
                    cbDatabaseEndoso = new ObservableCollection<string>();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.TargetSite.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CmdCancel_Click()
        {
            try
            {
                this.View.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "cmdCancel_Click", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CmdBrowseImagesPath_Click()
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();


                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    dfPathtoPictures_txt = fbd.SelectedPath;

                }
                else
                {
                    dfPathtoPictures_txt = string.Empty;
                }




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "CmdBrowseImagesPath_Click", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region ShowWindows
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

     

        private void OnInitWindow()
        {
            try
            {
                dfServer_txt = string.Empty;
                dfUsername_txt = string.Empty;

                dfMastSvr_txt = string.Empty;
                dfMastUsr_txt = string.Empty;
                // dfMastPass_txt = string.Empty;

                dfImageSvr_txt = string.Empty;
                dfImageUsr_txt = string.Empty;
                //  dfImagePass_txt = new SecureString();

                dfPathtoPictures_txt = string.Empty;

                cbDatabaseEndoso = new ObservableCollection<string>();
                cbImageDB = new ObservableCollection<string>();
                cbMastDB = new ObservableCollection<string>();
                cbRadicacionesDB = new ObservableCollection<string>();

                dfServer_txt = sqlServer;
                dfUsername_txt = userName;
                string decryptPassword;

                if (password.Trim().Length > 0)
                {
                    decryptPassword = PasswordHash.Decrypt1(password);

                    GetDataBaseName(sqlServer, userName, decryptPassword, cbDatabaseEndoso);
                    
                    cbDatabase_Item = database;
                    //dfPassword_txt = ToSecureString(decryptPassword);
                    dfPassword_txt = decryptPassword;
                    password = decryptPassword;
                }

                if (mastPass.Trim().Length > 0)
                {
                    decryptPassword = PasswordHash.Decrypt1(mastPass);

                    GetDataBaseName(mastSvr, mastUsr, decryptPassword, cbMastDB);

                    cbMastDB_Item = mastDB;
                    dfMastSvr_txt = mastSvr;
                    dfMastUsr_txt = mastUsr;

                    //dfMastPass_txt = ToSecureString(decryptPassword);
                    dfMastPass_txt = decryptPassword;
                    mastPass = decryptPassword;
                }

                if (imagePass.Trim().Length >0)
                {
                    decryptPassword = PasswordHash.Decrypt1(imagePass);
                    
                    GetDataBaseName(imageSvr, imageUsr, decryptPassword,cbImageDB);
                    
                    cbImageDB_Item = imageDB;
                    dfImageSvr_txt = imageSvr;
                    dfImageUsr_txt = imageUsr;

                    //dfImagePass_txt = ToSecureString(decryptPassword);
                    dfImagePass_txt = decryptPassword;
                    imagePass = decryptPassword;
                }
                if (RadicacionesPass.Trim().Length > 0)
                {
                    decryptPassword = PasswordHash.Decrypt1(RadicacionesPass);

                    GetDataBaseName(RadicacionesSvr, RadicacionesUsr, decryptPassword, cbRadicacionesDB);

                    cbRadicacionesDB_Item = RadicacionesDB;
                    RadicacionesSvr_txt = RadicacionesSvr;
                    RadicacionesUsr_txt = RadicacionesUsr;

                    RadicacionesUPass_txt = decryptPassword;
                    RadicacionesPass = decryptPassword;
                }



                dfPathtoPictures_txt = imgPath;

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
                MessageBox.Show(ex.ToString(), "OnInitWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        
        public bool? OnShow()
        {
            this.View.Closing += View_Closing;

            return this.View.ShowDialog();
        }
        #endregion

        #region Close
        void View_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_SaveReg)
            {
                e.Cancel = this.OnSessionEnding();
            }
            else
            {
                e.Cancel = false;
            }
        }
        private bool OnSessionEnding()
        {
            var response = MessageBox.Show("!!!Do you really want to Cancel?", "Cancel...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (response == MessageBoxResult.Yes)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Helper


        private void GetDataBaseName(string server, string user, string insecurePassword, BindingList<string> ListDatabase)
        {
            try
            {

                string query = "select name from sys.databases order by name";
                string cnnString;
                //Persist Security Info=False;Data Source=[server name];Initial Catalog=[DataBase Name];User ID=myUsername;Password=myPassword
                cnnString = string.Concat("Persist Security Info=False;",
                                          "Data Source=", server,
                                          ";Initial Catalog=master;",
                                          "User ID=", user,
                                          ";Password=", insecurePassword);

                //ListDatabase.Clear();

                using (SqlConnection cnn = new SqlConnection(cnnString))
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = System.Data.CommandType.Text,
                        CommandText = query
                    })
                    {
                        SqlDataReader dr;

                        dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            ListDatabase.Add(dr["name"].ToString());
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                ListDatabase.Clear();
                ListDatabase = null;
                ListDatabase = new BindingList<string>();
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void GetDataBaseName(string server, string user, string insecurePassword, ObservableCollection<string> ListDatabase)
        {
            try
            {

                string query = "select name from sys.databases order by name";
                string cnnString;
                //Persist Security Info=False;Data Source=[server name];Initial Catalog=[DataBase Name];User ID=myUsername;Password=myPassword
                cnnString = string.Concat("Persist Security Info=False;",
                                          "Data Source=", server,
                                          ";Initial Catalog=master;",
                                          "User ID=", user,
                                          ";Password=", insecurePassword);

                //ListDatabase.Clear();
                
                using (SqlConnection cnn = new SqlConnection(cnnString))
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = cnn,
                        CommandType = System.Data.CommandType.Text,
                        CommandText = query
                    })
                    {
                        SqlDataReader dr;

                        dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            ListDatabase.Add(dr["name"].ToString());
                        }

                    }
                }                              
                    
            }
            catch (Exception ex)
            {
                ListDatabase.Clear();
                ListDatabase = null;
                ListDatabase = new ObservableCollection<string>();
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private SecureString ToSecureString(string normalString)
        {
            var ss = new SecureString();
            if (!string.IsNullOrEmpty(normalString))
            {
                foreach (char cr in normalString)
                {
                    ss.AppendChar(cr);
                }
            }
            ss.MakeReadOnly();
            return ss;
        }
        
        #endregion

        #region Dispose
       


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmMantDB()
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
