
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
    using WpfEndososCandidatos.Models;
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using System.Security;
    using System.Windows.Data;
    using System.Runtime.InteropServices;
    using System.Collections.ObjectModel;
    using System.Data.SqlClient;

    public  class vmMatUsers : ViewModelBase<IDialogView>
    {
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

        public vmMatUsers()
            : base(new wpfMantUsers())
        {
            initWindow = new RelayCommand(param => InitWindow());
            guardar_Click = new RelayCommand(param => Guardar_Click());
            close_Click = new RelayCommand(param => Close_Click());
            borrar_Click = new RelayCommand(param => Borrar_Click());
            editar_Click = new RelayCommand(param => Editar_Click());
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

                            var hashedPassword = Helper.PasswordHash.Encrypt(insecurePassword);  //PasswordHash.HashPassword(insecurePassword);

                            List<tblUser> u = new List<tblUser>
                            {
                                new tblUser 
                                {  
                                    UserId=System.Guid.NewGuid(), 
                                    UserName = CbUser_Text ,
                                    PasswordHash=hashedPassword, 
                                    Email= CbUser_SelectedItem +  "@jolpr.com", 
                                    AreasDeAcceso=areasDeAcceso 
                                }
                            };
                            u.ForEach(m => _db.tblUsers.Add(m));
                            _db.SaveChanges();
                        }
                        break;
                    //case 2://Editar
                    //    break;
                    //case 3://Delete
                    //    break;
                    //case 5: // Cancelar
                     //   break;

                }
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

                throw new NotImplementedException();
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
                CbUser_IsEditable = true;
                Password_IsEnabled = true;
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
        public RelayCommand anadir_Click {get;private set;}
        private void Anadir_Click()
        {
            try
            {
                Password_IsEnabled = true;
                CbUser_IsEditable = true;
                AreasdeAcceso_IsEnabled = true;
                CbUser.Clear();
                CbUser = null;
                CbUser = new ObservableCollection<string>();
               

                _Operation = 1;
                
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


                CbUser_IsEditable = false;
                Password_IsEnabled = false;
                AreasdeAcceso_IsEnabled = false;

                CbUser = new ObservableCollection<string>();

                var usernames = from u in _db.tblUsers
                                orderby u.UserName
                                select u;


                foreach (var s in usernames)
                {
                    CbUser.Add(s.UserName);
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
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
        private void CbUser_ChangeItem()
        {
            try
            {
                var pass = from p in _db.tblUsers
                           where p.UserName == CbUser_SelectedItem
                           select p;
        
                _AreasDeAcceso = new string[9]; 

                foreach(var pss in pass)
                {
                    password_Cls = Helper.PasswordHash.Decrypt(pss.PasswordHash);
                    verificacionPassword_Cls = password_Cls;
                    
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
        
        #endregion

    }//end
}//end
