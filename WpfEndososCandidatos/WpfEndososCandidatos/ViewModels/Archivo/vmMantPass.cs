
namespace WpfEndososCandidatos.ViewModels
{
    using jolcode;
    using jolcode.Base;
    using jolcode.MyInterface;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using WpfEndososCandidatos.Helper.PWDTK;
    using WpfEndososCandidatos.Models;
    using WpfEndososCandidatos.View;

    public class vmMantPass : ViewModelBase<IDialogView>, IDisposable
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
        private dbEndososPartidosEntities _db = new dbEndososPartidosEntities();

        PWDTK.PasswordPolicy PwdPolicy = new PWDTK.PasswordPolicy(numberUpper, numberNonAlphaNumeric, numberNumeric, minPwdLength, maxPwdLength);
        private Visibility _Password_Cls_Visibility;
        private SecureString _password;
        private SecureString _passwordVerification;
        public Guid _Id { get; set; }
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;

        public vmMantPass() : base (new wpfMantPass())
        {
            initWindow = new RelayCommand(param => MyInitWindow(),null );
            cmdSalir_Click = new RelayCommand(param => CmdSalir_Click(),null);
            cmdVerPass_Click = new RelayCommand(param => CmdVerPass_Click(), null);
            cmdGuardar_Click = new RelayCommand(param => CmdGuardar_Click(), null);
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
        public bool? OnShow()
        {
            return this.View.ShowDialog();
        }
        public RelayCommand initWindow
        {
            get;
            private set;
        }
        private void MyInitWindow()
        {
            try
            {
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
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public RelayCommand cmdSalir_Click
        {
            get;
            private set;
        }
        public void CmdSalir_Click() 
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
        public RelayCommand cmdVerPass_Click
        {
            get;
            private set;

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

        public RelayCommand cmdGuardar_Click
        {
            get;
            private set;
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
        private void CmdGuardar_Click()
        {
            try
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

                //Hash password
                if (!PasswordMeetsPolicy(insecurePassword, PwdPolicy)) return;

                _salt = PWDTK.GetRandomSalt(saltSize);

                string salt = PWDTK.GetSaltHexString(_salt);

                _hash = PWDTK.PasswordToHash(_salt, insecurePassword, iterations);

                var hashedPassword = PWDTK.HashBytesToHexString(_hash);

                tbluser.SecurityStamp = salt;
                tbluser.PasswordHash = hashedPassword;

                _db.SaveChanges();
                MessageBox.Show("Dones...", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                CmdSalir_Click();
            }
            catch (Exception ex)
            {
                
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
    
    
        #region Dispose
       
      

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmMantPass()
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
