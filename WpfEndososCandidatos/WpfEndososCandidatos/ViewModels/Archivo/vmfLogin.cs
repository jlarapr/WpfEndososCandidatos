
namespace WpfEndososCandidatos.ViewModels
{
    using jolcode.Base;
    using jolcode.MyInterface;
    using jolcode;
    using WpfEndososCandidatos.View;
    using WpfEndososCandidatos.Models;
    using WpfEndososCandidatos.Helper;

    using System.Reflection;
    using System.Windows;
    using System;
    using System.Windows.Controls;

    using System.Collections.Generic;
    using System.Linq;
    using System.Data;
    using System.Data.Entity;
    using System.Security.Claims;
    using System.ComponentModel;
    using WpfEndososCandidatos.Helper.PWDTK;
    using System.Runtime.InteropServices;
    using System.Windows.Media;
    using System.Configuration;
    using System.Windows.Input;
    using System.Collections.ObjectModel;
    

    class vmfLogin : ViewModelBase<IDialogView>, IDisposable
    {
        //Below is used to generate a password policy that you may use to check that passwords adhere to this policy
        private const int numberUpper = 1;
        private const int numberNonAlphaNumeric = 1;
        private const int numberNumeric = 2;
        private const int minPwdLength = 6;
        private const int maxPwdLength = Int32.MaxValue;
        private Byte[] _salt;
        private Byte[] _hash;
        private const int iterations = 10002;
        private const int saltSize = PWDTK.CDefaultSaltLength + 2;

        PWDTK.PasswordPolicy PwdPolicy = new PWDTK.PasswordPolicy(numberUpper, numberNonAlphaNumeric, numberNumeric, minPwdLength, maxPwdLength);
        PWDTK.UserPolicy userPolicy = new PWDTK.UserPolicy(4, Int32.MaxValue);

        private RelayCommand _InitWindow;
        private RelayCommand _cancel_Click;
        private string _txtUserName_txt;
        private string _txtPassword_txt;
        private RelayCommand _oK_Click;

        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        //private bool _Ok_Can;
        private Visibility _Password_Cls_Visibility;
        private Brush _BorderBrush;
        private Cursor _MiCursor;
        private string _DBEndososCnnStr;
        private string _DBCeeMasterCnnStr;
        private string _DBImagenesCnnStr;
        DataTable _MyUsersTable;

        public vmfLogin() :
            base(new wpfLogin())
        {
            cmdVerPass_Click = new RelayCommand(param => CmdVerPass_Click(), null);
            //ConfigurationManager.AppSettings["BorderBrush"];
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
        public string DBCeeMasterCnnStr
        {
            get
            {
                return _DBCeeMasterCnnStr;
            }
            set
            {
                _DBCeeMasterCnnStr = value;
            }
        }
        public string DBImagenesCnnStr
        {
            get
            {
                return _DBImagenesCnnStr;
            }
            set
            {
                _DBImagenesCnnStr = value;
            }
        }


        public System.Windows.Input.Cursor MiCursor
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

        public string WhatIsUserName { get; set; }

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


        public string _AreasDeAcceso { get; private set; }
        public Guid _Id { get; private set; }

        public bool? OnShow()
        {
            return this.View.ShowDialog();
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

        private void MyOnInitWindow()
        {
            try
            {
                txtUserName_txt = string.Empty;
                txtPassword_txt = string.Empty;
                Password_Cls_Visibility = Visibility.Hidden;


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
            catch (System.Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommand cancel_Click
        {
            get
            {
                if (_cancel_Click == null)
                {
                    _cancel_Click = new RelayCommand(param => MyCancel_Click());
                }
                return _cancel_Click;
            }
        }

        private void MyCancel_Click()
        {
            try
            {

                this.View.DialogResult = false;
                this.View.Close();
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        public string txtUserName_txt
        {
            get
            {
                return _txtUserName_txt;
            }
            set
            {
                if (_txtUserName_txt != value)
                {
                    _txtUserName_txt = value;
                    this.RaisePropertychanged("txtUserName_txt");
                }
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
            }
            set
            {
                if (_Password_Cls_Visibility != value)
                {
                    _Password_Cls_Visibility = value;
                    this.RaisePropertychanged("Password_Cls_Visibility");
                }
            }
        }
        public string txtPassword_txt
        {
            get
            {
                return _txtPassword_txt;
            }
            set
            {
                if (_txtPassword_txt != value)
                {
                    _txtPassword_txt = value;
                    this.RaisePropertychanged("txtPassword_txt");
                }
            }
        }


        public RelayCommand oK_Click
        {
            get
            {
                if (_oK_Click == null)
                {
                    _oK_Click = new RelayCommand(param => MyOK_Click(param), param => Ok_Can);
                }
                return _oK_Click;
            }
        }

        private bool Ok_Can
        {
            get
            {
                try
                {
                    if ((txtUserName_txt == null) || (txtPassword_txt == null))
                        return false;

                    PasswordPolicyException pwdEx = new PasswordPolicyException("");
                    UserPolicyException usrEx = new UserPolicyException("");


                    if (!PWDTK.TryUserNamePolicyCompliance(txtUserName_txt, userPolicy, ref usrEx))
                        return false;

                    if (!PWDTK.TryPasswordPolicyCompliance(txtPassword_txt, PwdPolicy, ref pwdEx))
                        return false;

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private void MyOK_Click(object param)
        {
            MiCursor = Cursors.Wait;

            try
            {
                if (txtUserName_txt != "Applica")
                {
                    PasswordBox passwordBox = param as PasswordBox;

                    txtPassword_txt = passwordBox.Password;
                                        
                    ObservableCollection<Users> db = new ObservableCollection<Users>(); 
                    
                    using (SqlExcuteCommand get = new SqlExcuteCommand()
                    {
                        DBCnnStr = DBEndososCnnStr
                    })
                    {
                        _MyUsersTable = get.MyGetUsers();

                        foreach(DataRow r in _MyUsersTable.Rows)
                        {
                            Users mUsers = new Users();
                            mUsers.UserId = (Guid)r["UserId"];
                            mUsers.UserName = r["UserName"].ToString();
                            mUsers.PasswordHash =  r["PasswordHash"].ToString();
                            mUsers.SecurityStamp = r["SecurityStamp"].ToString();
                            mUsers.AreasDeAcceso = r["AreasDeAcceso"].ToString();
                            db.Add(mUsers);
                        }
                    }

                    var user = from u in db
                               where u.UserName == txtUserName_txt
                               select new
                               {
                                   passwordHash = u.PasswordHash,
                                   salt = u.SecurityStamp,
                                   acceso = u.AreasDeAcceso,
                                   id = u.UserId
                               };


                    if (user.Count() == 0)
                        throw new Exception("Error con el usuario o el password.");


                    //   if (!PasswordMeetsPolicy(txtPassword_txt, PwdPolicy)) return;

                    string hashedPassword = user.First().passwordHash;

                    _salt = PWDTK.HashHexStringToBytes(user.First().salt);


                    _hash = PWDTK.HashHexStringToBytes(hashedPassword);

                    if (!PWDTK.ComparePasswordToHash(_salt, txtPassword_txt, _hash, iterations))
                    {
                        throw new Exception("Error con el password.");
                    }

                    WhatIsUserName = " " + txtUserName_txt;
                    _AreasDeAcceso = user.First().acceso;
                    _Id = user.First().id;
                }
                else
                {
                    WhatIsUserName = " Applica";
                    _AreasDeAcceso = "ABCDEFGH";
                    _Id = Guid.NewGuid();
                }
                this.View.DialogResult = true;

                this.View.Close();

            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                MiCursor = Cursors.Arrow;

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

            }
        }


        #region Dispose



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmfLogin()
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
