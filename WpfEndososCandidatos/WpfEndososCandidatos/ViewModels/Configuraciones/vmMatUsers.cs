
namespace WpfEndososCandidatos.ViewModels
{
    using jolcode.Base;
    using jolcode.MyInterface;
    using WpfEndososCandidatos.View;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using jolcode;
    using System.Reflection;
    using WpfEndososCandidatos.Helper;
    using WpfEndososCandidatos.Helper.PWDTK;
    using WpfEndososCandidatos.Models;
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using System.Security;
    using System.Windows.Data;
    using System.Runtime.InteropServices;
    using System.Collections.ObjectModel;
    using System.Data.SqlClient;
    using System.Windows.Media;
    using System.Configuration;

    public class vmMatUsers : ViewModelBase<IDialogView>, IDisposable 
    {
        //Below is used to generate a password policy that you may use to check that passwords adhere to this policy
        private const int numberUpper = 1;
        private const int numberNonAlphaNumeric = 1;
        private const int numberNumeric = 2;
        private const int minPwdLength = 6;
        private const int maxPwdLength = Int32.MaxValue;
        private Byte[] _salt;
        private Byte[] _hash;
        //Salt length
        private const int saltSize = PWDTK.CDefaultSaltLength + 2;
        private const int iterations = 10002;
        PWDTK.PasswordPolicy PwdPolicy = new PWDTK.PasswordPolicy(numberUpper, numberNonAlphaNumeric, numberNumeric, minPwdLength, maxPwdLength);
        PWDTK.UserPolicy UserPolicy = new PWDTK.UserPolicy(4, Int32.MaxValue);

        dbEndososPartidosEntities _db = new dbEndososPartidosEntities();
        private int _Operation;
        private string[] _AreasDeAcceso = new string[9]; 
        private bool _cambiarPassword_IsChecked;
        private bool _autorizarLotes_IsChecked;        
        private bool _procesarLotes_IsChecked;        
        private bool _verElector_IsCheked;        
        private bool _reportes_IsChecked;
        private bool _reversarLote_IsChecked;
        private bool _configuraciones_IsChecked;
        private bool _corregirEndosos_IsChecked;
        private SecureString _password;
        private SecureString _passwordVerification;
        private ObservableCollection<string> _CbUser;
        private string _CbUser_SelectedItem;
        private int _CbUser_SelectedIndex;
        private string _password_Cls;
        private string _verificacionPassword_Cls;

        public string _sqlServer { get; set; }
        public string _userName { get;set; }
        public string _userPassword { get;set; }
        public string _database { get; set; }
        
        private bool _CbUser_IsEditable;
        private bool _Password_IsEnabled;
        private bool _AreasdeAcceso_IsEnabled;
        private string _CbUser_Text;
        private bool _cmdAdd_IsEnabled;
        private bool _cmdEdit_IsEnabled;
        private bool _cmdDelete_IsEnabled;
        private bool _cmdSave_IsEnabled;
        private bool _cmdCancel_IsEnabled;
        private bool _cbUser_IsEnabled;
        private Guid _Id;
        private bool _cmdEditPass_IsEnabled;
        private Visibility _Password_Cls_Visibility;
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        public vmMatUsers()
            : base(new wpfMantUsers())
        {
            initWindow = new RelayCommand(param => InitWindow());
            guardar_Click = new RelayCommand(param => Guardar_Click());
            close_Click = new RelayCommand(param => Close_Click());
            borrar_Click = new RelayCommand(param => Borrar_Click());
            editar_Click = new RelayCommand(param => Editar_Click());
            editPass_Click = new RelayCommand(param => EditPass_Click());
            anadir_Click = new RelayCommand(param => Anadir_Click());
            cancelar_Click = new RelayCommand(param => Cancelar_Click());
            cambiarPassword_Click = new RelayCommand(param => CambiarPassword_Click());
            autorizarLotes_Click = new RelayCommand(param => AutorizarLotes_Click());
            procesarLotes_Click = new RelayCommand(param => ProcesarLotes_Click());
            verElector_Click = new RelayCommand(param => VerElector_Click());
            reportes_Click = new RelayCommand(param => Reportes_Click());
            reversarLote_Click = new RelayCommand(param => ReversarLote_Click());
            configuraciones_Click = new RelayCommand(param => Configuraciones_Click());
            corregirEndosos_Click = new RelayCommand(param => CorregirEndosos_Click());
            cbUser_ChangeItem = new RelayCommand(param => CbUser_ChangeItem());
            cmdVerPass_Click = new RelayCommand(param => CmdVerPass_Click());
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
        public bool AreasdeAcceso_IsEnabled
        {
            get
            {
                return _AreasdeAcceso_IsEnabled;
            }
            set
            {
                if (_AreasdeAcceso_IsEnabled != value)
                {
                    _AreasdeAcceso_IsEnabled = value;
                    this.RaisePropertychanged("AreasdeAcceso_IsEnabled");
                }
            }
        }

        public bool cmdAdd_IsEnabled
        {
            get
            {
                return _cmdAdd_IsEnabled;
            }
            set
            {
                if (_cmdAdd_IsEnabled != value)
                {
                    _cmdAdd_IsEnabled = value;
                    this.RaisePropertychanged("cmdAdd_IsEnabled");
                }
            }
        }

        public bool cmdEdit_IsEnabled
        {
            get
            {
                return _cmdEdit_IsEnabled;
            }
            set
            {
                if (_cmdEdit_IsEnabled != value)
                {
                    _cmdEdit_IsEnabled = value;
                    this.RaisePropertychanged("cmdEdit_IsEnabled");
                }
            }
        }

        public bool cmdDelete_IsEnabled
        {
            get
            {
                return _cmdDelete_IsEnabled;
            }
            set
            {
                if (_cmdDelete_IsEnabled != value)
                {
                    _cmdDelete_IsEnabled = value;
                    this.RaisePropertychanged("cmdDelete_IsEnabled");
                }
            }
        }
        public bool cmdSave_IsEnabled
        {
            get
            {
                return _cmdSave_IsEnabled;
            }
            set
            {
                if (_cmdSave_IsEnabled != value)
                {
                    _cmdSave_IsEnabled = value;
                    this.RaisePropertychanged("cmdSave_IsEnabled");
                }
            }
        }
        public bool cmdEditPass_IsEnabled
        {
            get
            {
                return _cmdEditPass_IsEnabled;
            }
            set
            {
                if (_cmdEditPass_IsEnabled != value)
                {
                    _cmdEditPass_IsEnabled = value;
                    this.RaisePropertychanged("cmdEditPass_IsEnabled");
                }
            }

        }
        public bool cmdCancel_IsEnabled
        {
            get
            {
                return _cmdCancel_IsEnabled;
            }
            set
            {
                if (_cmdCancel_IsEnabled != value)
                {
                    _cmdCancel_IsEnabled = value;
                    this.RaisePropertychanged("cmdCancel_IsEnabled");
                }
            }
        }

        #region SaveBorrarEdiatarAnadir

        public RelayCommand guardar_Click { get; private set; }

        private void Guardar_Click()
        {
            try
            {
                string areasDeAcceso = string.Empty;

                foreach (string s in _AreasDeAcceso)
                {
                    areasDeAcceso += s;
                }
                switch(_Operation)
                {
                    case 1:
                        {//Anadir
                            IntPtr passwordBSTR = default(IntPtr);
                            string insecurePassword = "";
                            passwordBSTR = Marshal.SecureStringToBSTR(Password);
                            insecurePassword = Marshal.PtrToStringBSTR(passwordBSTR);

                            IntPtr passwordVerificationBSTR = default(IntPtr);
                            string insecurePasswordVerification = string.Empty;

                            passwordVerificationBSTR = Marshal.SecureStringToBSTR(PasswordVerification);
                            insecurePasswordVerification = Marshal.PtrToStringBSTR(passwordVerificationBSTR);

                            if (!insecurePassword.Equals(insecurePasswordVerification))
                            {
                                throw new Exception("Error con el Password");
                            }

                            //Policy 
                            if (!userMeetsPolicy(CbUser_Text, UserPolicy)) return;

                            if (!PasswordMeetsPolicy(insecurePassword, PwdPolicy)) return;
                            
                            //Hash password
                            _salt = PWDTK.GetRandomSalt(saltSize);

                            string salt = PWDTK.GetSaltHexString(_salt);

                            _hash = PWDTK.PasswordToHash(_salt, insecurePassword, iterations);
                            
                            var hashedPassword = PWDTK.HashBytesToHexString(_hash);

                            List<tblUser> u = new List<tblUser>
                            {
                                new tblUser 
                                {  
                                    UserId=System.Guid.NewGuid(), 
                                    UserName = CbUser_Text ,
                                    PasswordHash=hashedPassword, 
                                    SecurityStamp = salt,
                                    Email= CbUser_Text +  "@jolpr.com", 
                                    AreasDeAcceso=areasDeAcceso 
                                }
                            };
                            u.ForEach(m => _db.tblUsers.Add(m));
                            _db.SaveChanges();
                        }
                        break;
                    case 2://Editar Areas De Acceso
                        {
                            tblUser tbluser = _db.tblUsers.Find(_Id);
                            _db.Entry(tbluser).State = System.Data.Entity.EntityState.Modified;
                                                       
                            tbluser.AreasDeAcceso = areasDeAcceso;

                            _db.SaveChanges();
                        }
                        break;
                    case 3://Delete
                        {
                            string msg = "You are about to delete 1 user\r";
                            msg += "Click yes to permanently delete this user( " + CbUser_Text + " ).\r";
                            msg += "You won't be able to undo those changes.";

                            var response = MessageBox.Show("!!!" + msg, "Delete...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                            if (response == MessageBoxResult.Yes)
                            {
                                tblUser tbluser = _db.tblUsers.Find(_Id);
                                _db.tblUsers.Remove(tbluser);
                                _db.SaveChanges();
                            }
                        }
                        break;
                    case 4: //Edit Pass
                        {
                            tblUser tbluser = _db.tblUsers.Find(_Id);
                            _db.Entry(tbluser).State = System.Data.Entity.EntityState.Modified;

                            IntPtr passwordBSTR = default(IntPtr);
                            string insecurePassword = "";
                            passwordBSTR = Marshal.SecureStringToBSTR(Password);
                            insecurePassword = Marshal.PtrToStringBSTR(passwordBSTR);

                            IntPtr passwordVerificationBSTR = default(IntPtr);
                            string insecurePasswordVerification = string.Empty;

                            passwordVerificationBSTR = Marshal.SecureStringToBSTR(PasswordVerification);
                            insecurePasswordVerification = Marshal.PtrToStringBSTR(passwordVerificationBSTR);

                            if (!insecurePassword.Equals(insecurePasswordVerification))
                            {
                                throw new Exception("Error con el Password");
                            }

                            //Policy
                            if (!userMeetsPolicy(CbUser_Text, UserPolicy)) return;

                            if (!PasswordMeetsPolicy(insecurePassword, PwdPolicy)) return;

                            //Hash password                                                        
                            _salt = PWDTK.GetRandomSalt(saltSize);

                            string salt = PWDTK.GetSaltHexString(_salt);

                            _hash = PWDTK.PasswordToHash(_salt, insecurePassword, iterations);

                            var hashedPassword = PWDTK.HashBytesToHexString(_hash);

                            tbluser.SecurityStamp = salt;
                            tbluser.PasswordHash = hashedPassword;

                            _db.SaveChanges();
                        }
                        break;
                }
                Cancelar_Click();
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.ToString(), site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public RelayCommand borrar_Click { get; private set; }
        
        private void Borrar_Click()
        {
            try
            {
                _Operation = 3;
                cmdSave_IsEnabled = true;
                cmdEdit_IsEnabled = false;                
                cbUser_IsEnabled = false;
                cmdDelete_IsEnabled = false;
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public RelayCommand editar_Click { get; private set; }
        private void Editar_Click()
        {
            try
            {
                _Operation = 2;

                cbUser_IsEnabled = false;
                cmdAdd_IsEnabled = false;
                cmdDelete_IsEnabled = false;
                cmdEdit_IsEnabled = false;
                cmdEditPass_IsEnabled = false;

                cmdSave_IsEnabled = true;
                Password_IsEnabled = false;
                AreasdeAcceso_IsEnabled = true;
            }
            catch (Exception ex)
            {
                AreasdeAcceso_IsEnabled = false;
                CbUser_IsEditable = false;
                Password_IsEnabled = false;
            
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommand editPass_Click { get; private set; }

        private void EditPass_Click()
        {
            try
            {
                _Operation = 4;
                cbUser_IsEnabled = false;
                cmdAdd_IsEnabled = false;
                cmdDelete_IsEnabled = false;
                cmdEdit_IsEnabled = false;
                cmdEditPass_IsEnabled = false;

                password_Cls = string.Empty;
                verificacionPassword_Cls = string.Empty;

                cmdSave_IsEnabled = true;
                Password_IsEnabled = true;
                AreasdeAcceso_IsEnabled = false;


            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public RelayCommand anadir_Click {get;private set;}
        private void Anadir_Click()
        {
            try
            {
                _Operation = 1;

                Password_IsEnabled = true;
                CbUser_IsEditable = true;
                AreasdeAcceso_IsEnabled = true;
                CbUser.Clear();
                CbUser = null;
                CbUser = new ObservableCollection<string>();

                cmdCancel_IsEnabled = true;
                cmdSave_IsEnabled = true;
                cmdAdd_IsEnabled = false;
            }
            catch (Exception ex)
            {
                CbUser_IsEditable = false;
                Password_IsEnabled = false;
                AreasdeAcceso_IsEnabled = false;
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommand cancelar_Click { get; private set;}

        private void Cancelar_Click()
        {
            try
            {
                _Operation = 5;
                _AreasDeAcceso = new string[9];
                cambiarPassword_IsChecked = false;
                autorizarLotes_IsChecked = false;
                procesarLotes_IsChecked = false;
                verElector_IsChecked = false;
                reportes_IsChecked = false;
                reversarLote_IsChecked = false;
                configuraciones_IsChecked = false;
                corregirEndosos_IsChecked = false;
                Password_Cls_Visibility = Visibility.Hidden;


                CbUser_IsEditable = false;
                Password_IsEnabled = false;
                AreasdeAcceso_IsEnabled = false;

                password_Cls = string.Empty;
                verificacionPassword_Cls = string.Empty;
                CbUser_Text = string.Empty;


                CbUser = new ObservableCollection<string>();

                var usernames = from u in _db.tblUsers
                                orderby u.UserName
                                select u;


                foreach (var s in usernames)
                {
                    CbUser.Add(s.UserName);
                }

                cbUser_IsEnabled = true;
                cmdAdd_IsEnabled = true;

                cmdCancel_IsEnabled = false;
                cmdEdit_IsEnabled = false;
                cmdDelete_IsEnabled = false;
                cmdSave_IsEnabled = false;
                cmdEditPass_IsEnabled = false;
                
                Id = Guid.Empty.ToString();

            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public RelayCommand cmdVerPass_Click
        {
            get;
            private set;

        }

        private void CmdVerPass_Click() 
        {
            try
            {

                if (Password_Cls_Visibility == Visibility.Visible)
                {
                    Password_Cls_Visibility = Visibility.Hidden;
                }
                else
                {
                    Password_Cls_Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        } 
        public Visibility Password_Cls_Visibility
        {
            get
            {
                return _Password_Cls_Visibility;
            }set
            {
                if (_Password_Cls_Visibility != value)
                {
                    _Password_Cls_Visibility = value;
                    this.RaisePropertychanged("Password_Cls_Visibility");
                }
            }
        }


        #endregion
        
        #region cambiarPassword


        public bool cambiarPassword_IsChecked
        {
            get
            {
                return _cambiarPassword_IsChecked;
            }
            set
            {
                if (_cambiarPassword_IsChecked != value)
                {
                    _cambiarPassword_IsChecked = value;
                    this.RaisePropertychanged("cambiarPassword_IsChecked");
                }
            }
        }
        public RelayCommand cambiarPassword_Click {get;private set;}

        private void CambiarPassword_Click()
        {
            try
            {

                if (cambiarPassword_IsChecked)
                {
                    _AreasDeAcceso[1] = "A";
                }
                else
                {
                    _AreasDeAcceso[1] = string.Empty;

                }
                
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        #endregion

        #region Autorizar Lotes

        public bool autorizarLotes_IsChecked
        {
            get
            {
                return _autorizarLotes_IsChecked;
            }
            set
            {
                if (_autorizarLotes_IsChecked != value)
                {
                    _autorizarLotes_IsChecked = value;
                    this.RaisePropertychanged("autorizarLotes_IsChecked");
                }
            }

        }

        public RelayCommand autorizarLotes_Click
        {
            get;
            private set;
        }

        private void AutorizarLotes_Click()
        {
            try
            {

                if (autorizarLotes_IsChecked)
                {
                    _AreasDeAcceso[2] = "B";
                }
                else
                {
                    _AreasDeAcceso[2] = string.Empty;
                }

                
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Procesar Lotes
        
        public bool procesarLotes_IsChecked
        {
            get
            {
                return _procesarLotes_IsChecked;
            }
            set
            {
                if (_procesarLotes_IsChecked != value)
                {
                    _procesarLotes_IsChecked = value;
                    this.RaisePropertychanged("procesarLotes_IsChecked");
                }
            }
        }
        public RelayCommand procesarLotes_Click
        {
            get;
            private set;

        }

        private void ProcesarLotes_Click()
        {
            try
            {
                if (procesarLotes_IsChecked)
                {
                    _AreasDeAcceso[3] = "C";
                }
                else
                {
                    _AreasDeAcceso[3] = string.Empty;
                }

                
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        #endregion

        #region Ver Elector

        public bool verElector_IsChecked
        {
            get
            {
                return _verElector_IsCheked;
            }
            set
            {
                if (_verElector_IsCheked != value)
                {
                    _verElector_IsCheked = value;
                    this.RaisePropertychanged("verElector_IsChecked");
                }
            }
        }
        public RelayCommand verElector_Click
        {
            get;
            private set;

        }

        private void VerElector_Click()
        {
            try
            {
                if (verElector_IsChecked)
                {
                    _AreasDeAcceso[4] = "D";
                }
                else
                {
                    _AreasDeAcceso[4] = string.Empty;
                }

                
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Reportes

        public bool reportes_IsChecked
        {
            get
            {
                return _reportes_IsChecked;
            }
            set
            {
                if (_reportes_IsChecked != value)
                {
                    _reportes_IsChecked = value;
                    this.RaisePropertychanged("reportes_IsChecked");
                }
            }
        }
        public RelayCommand reportes_Click
        {
            get;
            private set;

        }

        private void Reportes_Click()
        {
            try
            {
                if (reportes_IsChecked)
                {
                    _AreasDeAcceso[5] = "E";
                }
                else
                {
                    _AreasDeAcceso[5] = string.Empty;
                }
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        #endregion

        #region Reversar Lote


        public bool reversarLote_IsChecked
        {
            get
            {
                return _reversarLote_IsChecked;
            }
            set
            {
                if (_reversarLote_IsChecked != value)
                {
                    _reversarLote_IsChecked = value;
                    this.RaisePropertychanged("reversarLote_IsChecked");
                }
            }

        }

        public RelayCommand reversarLote_Click
        {
            get;
            private set;
        }

        private void ReversarLote_Click()
        {
            try
            {
                if (reversarLote_IsChecked)
                {
                    _AreasDeAcceso[6] = "F";
                }
                else
                {
                    _AreasDeAcceso[6] = string.Empty;
                }
                
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Configuraciones
         
        public bool configuraciones_IsChecked
        {
            get
            {
                return _configuraciones_IsChecked;
            }
            set
            {
                if (_configuraciones_IsChecked != value)
                {
                    _configuraciones_IsChecked = value;
                    this.RaisePropertychanged("configuraciones_IsChecked");
                }
            }
        }
        public RelayCommand configuraciones_Click
        {
            get;
            private set;
        }

        private void Configuraciones_Click()
        {
            try
            {
                if (configuraciones_IsChecked)
                {
                    _AreasDeAcceso[7] = "G";
                }
                else
                {
                    _AreasDeAcceso[7] = string.Empty;
                }
                
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        



        #endregion

        #region Corregir Endosos

        public bool corregirEndosos_IsChecked
        {
            get
            {
                return _corregirEndosos_IsChecked;
            }
            set
            {
                if (_corregirEndosos_IsChecked != value)
                {
                    _corregirEndosos_IsChecked = value;
                    this.RaisePropertychanged("corregirEndosos_IsChecked");
                }
            }
        }

        public RelayCommand corregirEndosos_Click
        {
            get;
            private set;
        }

        private void CorregirEndosos_Click()
        {
            try
            {
                if (corregirEndosos_IsChecked)
                {
                    _AreasDeAcceso[8] = "H";
                }
                else
                {
                    _AreasDeAcceso[8] = string.Empty;
                }
                
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region cbUser

        public bool CbUser_IsEditable
        {
            get
            {
                return _CbUser_IsEditable;
            }
            set
            {
                if (_CbUser_IsEditable != value)
                {
                    _CbUser_IsEditable = value;
                    this.RaisePropertychanged("CbUser_IsEditable");
                }
            }
        }

        public bool cbUser_IsEnabled
        {
            get
            {
                return _cbUser_IsEnabled;
            }
            set
            {
                if (_cbUser_IsEnabled != value)
                {
                    _cbUser_IsEnabled = value;
                    this.RaisePropertychanged("cbUser_IsEnabled");
                }
            }
        }

        public ObservableCollection<string> CbUser
        {
            get
            {
                return _CbUser;
            }
            set
            {
                _CbUser = value;
                this.RaisePropertychanged("CbUser");
           
            }
        }

        public string CbUser_Text
        {
            get
            {
                return _CbUser_Text;
            }
            set
            {
                if (_CbUser_Text != value)
                {
                    _CbUser_Text = value;
                    this.RaisePropertychanged("CbUser_Text");
                }
            }
        }
        public string   CbUser_SelectedItem
        {
            get
            {
                return _CbUser_SelectedItem;
            }
            set
            {
                _CbUser_SelectedItem = value;
                this.RaisePropertychanged("CbUser_SelectedItem");
                
            }
        }

        public int CbUser_SelectedIndex
        {
            get
            {
                return _CbUser_SelectedIndex;
            }
            set
            {
                _CbUser_SelectedIndex = value;
                this.RaisePropertychanged("CbUser_SelectedIndex");
            }
        }
        public RelayCommand cbUser_ChangeItem
        {
            get;
            private set;
        }
        public string Id
        {
            get
            {
                return _Id.ToString();
            }       
            set
            {
                if (_Id.ToString() != value)
                {
                    _Id = Guid.Parse(value);
                    this.RaisePropertychanged("Id");
                }
            }
        }

        private void CbUser_ChangeItem()
        {
            try
            {
                var pass = from p in _db.tblUsers
                           where p.UserName == CbUser_SelectedItem
                           select p;

                cmdEdit_IsEnabled = true;
                cmdEditPass_IsEnabled = true;
                cmdCancel_IsEnabled = true;

                cmdAdd_IsEnabled = false;
                cmdDelete_IsEnabled = true;
                Password_IsEnabled = false;
                Password_Cls_Visibility = Visibility.Hidden;


                _AreasDeAcceso = new string[9]; 

                foreach(var pss in pass)
                {
                     
                    Byte[] hash = PWDTK.HashHexStringToBytes (pss.PasswordHash);

                    password_Cls = PWDTK.HashBytesToHexString(hash);  // Helper.PasswordHash.HashPasswordDecrypt(pss.PasswordHash);  // Helper.PasswordHash.Decrypt(pss.PasswordHash);

                    verificacionPassword_Cls = password_Cls;
                    //_Id = pss.UserId;
                    Id = pss.UserId.ToString();
                    foreach (char c in pss.AreasDeAcceso.ToCharArray())
                    {
                        switch (c)
                        {
                            case 'A':
                                _AreasDeAcceso[1] = "A";
                                cambiarPassword_IsChecked = true;
                                break;
                            case 'B' :
                                _AreasDeAcceso[2] ="B";
                                autorizarLotes_IsChecked = true;
                                break;
                            case 'C':
                                _AreasDeAcceso[3] = "C";
                                procesarLotes_IsChecked = true;
                                break;
                            case 'D':
                                _AreasDeAcceso[4] ="D";
                                verElector_IsChecked = true;
                                break;
                            case 'E':
                                _AreasDeAcceso[5] ="E";
                                reportes_IsChecked = true;
                                break;
                            case 'F':
                                _AreasDeAcceso[6] ="F";
                                reversarLote_IsChecked = true;
                                break;
                            case 'G':
                                _AreasDeAcceso[7] ="G";
                                configuraciones_IsChecked = true;
                                break;
                            case 'H':
                                _AreasDeAcceso[8] ="H";
                                corregirEndosos_IsChecked = true;
                                break;

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Password Txt

        public bool Password_IsEnabled
        {
            get
            {
                return _Password_IsEnabled;
            }
            set
            {
                if (_Password_IsEnabled != value)
                {
                    _Password_IsEnabled = value;
                    this.RaisePropertychanged("Password_IsEnabled");
                }
            }
        }
        
        public SecureString Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    this.RaisePropertychanged("Password");
                }
            }
        }

        public string password_Cls
        {
            get
            {
                return _password_Cls;
            }
            set
            {
                if (_password_Cls != value)
                {
                    _password_Cls = value;
                    this.RaisePropertychanged("password_Cls");
                }
            }
        }
        
        public string verificacionPassword_Cls
        {
            get
            {
                return _verificacionPassword_Cls;
            }
            set
            {
                if (_verificacionPassword_Cls != value)
                {
                    _verificacionPassword_Cls = value;
                    this.RaisePropertychanged("verificacionPassword_Cls");

                }
            }
        }
        
        public SecureString PasswordVerification
          {
              get
              {
                  return _passwordVerification;
              }
              set
              {
                  if (_passwordVerification != value)
                  {
                      _passwordVerification = value;
                      this.RaisePropertychanged("PasswordVerification");
                  }
              }
          }
      
        #endregion
      
        #region Swowwindow
        public bool? OnShow()
        {
            return this.View.ShowDialog();
        }
        
      public RelayCommand initWindow { get; private set; }
            
      private void InitWindow()
        {
            try
            {
                //cambiarPassword_IsChecked = false;
                CbUser = new ObservableCollection<string>();
                CbUser_IsEditable = false;
                Password_IsEnabled = false;
                AreasdeAcceso_IsEnabled = false;
                
                cmdAdd_IsEnabled = true;
                cbUser_IsEnabled = true;

                cmdEdit_IsEnabled = false;
                cmdDelete_IsEnabled = false;
                cmdSave_IsEnabled = false;
                cmdCancel_IsEnabled = false;
                cmdEditPass_IsEnabled = false;

                Password_Cls_Visibility =  Visibility.Hidden;

                var usernames = from u in _db.tblUsers
                             orderby u.UserName
                             select u;


                foreach (var s in usernames)
                {
                    CbUser.Add(s.UserName);
                }
                if (CbUser.Count > 0)
                    CbUser_SelectedIndex = -1;
                //GetUsers(_sqlServer,_database, _userName, PasswordHash.Decrypt(_userPassword),this.CbUser);

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

        #endregion

        #region Close
        public RelayCommand close_Click { get; private set; }
        
        private void Close_Click()
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
                
        #region Tools

        private void GetUsers(string server,string database, string user, string insecurePassword, ObservableCollection<string> ListDatabase) 
        {
            try
            {

                string query = "select * from tblUsers order by UserName";
                string cnnString;
                //Persist Security Info=False;Data Source=[server name];Initial Catalog=[DataBase Name];User ID=myUsername;Password=myPassword
                cnnString = string.Concat("Persist Security Info=False;",
                                          "Data Source=", server,
                                          ";Initial Catalog=",database ,
                                          ";User ID=", user,
                                          ";Password=", insecurePassword);
                                
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
                            ListDatabase.Add(dr["username"].ToString());
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


        private bool PasswordMeetsPolicy(String Password, PWDTK.PasswordPolicy PassPolicy)
        {
            PasswordPolicyException pwdEx = new PasswordPolicyException("");

            if (PWDTK.TryPasswordPolicyCompliance(Password, PassPolicy, ref pwdEx))
            {
                return true;
            }
            else
            {
                //Password does not comply with PasswordPolicy so we get the error message from the PasswordPolicyException to display to the user
                //errorPasswd.SetError(txtPassword, pwdEx.Message);
                throw new Exception(pwdEx.Message);

                //return false;
            }
        }
        private bool userMeetsPolicy(string username, PWDTK.UserPolicy userPolicy)
        {
            UserPolicyException usrEx = new UserPolicyException("");
            if (PWDTK.TryUserNamePolicyCompliance(username, userPolicy, ref usrEx))
            {
                return true;
            }
            else
            {
                throw new Exception(usrEx.Message);
            }
        }


        #endregion

        #region Dispose
       
       

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmMatUsers()
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
