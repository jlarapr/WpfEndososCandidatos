
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

    class vmfLogin : ViewModelBase<IDialogView>, INotifyPropertyChanged
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

        private RelayCommand _InitWindow;
        private RelayCommand _cancel_Click;
        private string _txtUserName_txt;
        private string _txtPassword_txt;
        private RelayCommand _oK_Click;
        
        public vmfLogin() :
            base(new wpfLogin())
        {
            
        }

        public string WhatIsUserName { get; set; }
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
                    _InitWindow = new RelayCommand(param => OnInitWindow());
                }
                return _InitWindow;
            }
        }

        private void OnInitWindow()
        {
            try
            {
                txtUserName_txt = string.Empty;
                txtPassword_txt = string.Empty;

                
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
                    _cancel_Click = new RelayCommand(param => Cancel_Click());
                }
                return _cancel_Click;
            }
        }

        private void Cancel_Click()
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
                    _oK_Click = new RelayCommand(param => OK_Click(param));
                }
                return _oK_Click;
            }
        }

        private  void OK_Click(object param)
        {
            try
            {
                PasswordBox passwordBox = param as PasswordBox;
                
                txtPassword_txt = passwordBox.Password;

                dbEndososPartidosEntities db = new dbEndososPartidosEntities();

                var user = from u in db.tblUsers
                           where u.UserName == txtUserName_txt
                           select new
                           {
                               passwordHash = u.PasswordHash,
                               salt = u.SecurityStamp
                           };
                           

                if (user.Count() == 0)
                    throw new Exception("Error con el usuario o el password.");


                if (!PasswordMeetsPolicy(txtPassword_txt, PwdPolicy)) return;

                string hashedPassword = user.First().passwordHash;

                _salt = PWDTK.HashHexStringToBytes(user.First().salt);

                
                _hash = PWDTK.HashHexStringToBytes(hashedPassword);           

                if (!PWDTK.ComparePasswordToHash(_salt, txtPassword_txt, _hash, iterations))
                {
                    throw new Exception("Error con el usuario o el password.");
                }

                WhatIsUserName = " " + txtUserName_txt;
               
                this.View.Close();
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
                
            }
        }

       
    }//end
}//end
