namespace WpfEndososCandidatos.ViewModels
{
    using jolcode;
    using System;
    using System.Reflection;
    using System.Windows;
    
    partial class MainVM
    {
        private RelayCommand _login_Click;
        private RelayCommand _logout_Click;
        private RelayCommand _cambiarPassword_Click;
        private RelayCommand _close_Click;
        public RelayCommand login_Click
        {
            get
            {
                if (_login_Click == null)
                {
                    _login_Click = new RelayCommand(param => Login_Click());
                }
                return _login_Click;
            }
        }
        private void Login_Click()
        {
            try
            {

                vmfLogin frmfLogin = new vmfLogin();
                frmfLogin.View.Owner = this.View as Window;
                frmfLogin.OnShow();
                Title += string.Concat(" UserName:", frmfLogin.WhatIsUserName);

                
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public RelayCommand logout_Click
        {
            get
            {
                if (_logout_Click == null)
                {
                    _logout_Click = new RelayCommand(param => Logout_Click());
                }
                return _logout_Click;
            }
        }
        private void Logout_Click()
        {
            try
            {

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public RelayCommand cambiarPassword_Click
        {
            get
            {
                if (_cambiarPassword_Click == null)
                {
                    _cambiarPassword_Click = new RelayCommand(param => CambiarPassword_Click());
                }
                return _cambiarPassword_Click;
            }
        }

        private void CambiarPassword_Click()
        {
            try
            {

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommand close_Click
        {
            get
            {
                if (_close_Click == null)
                {
                    _close_Click = new RelayCommand(param => Close_Click());
                }
                return _close_Click;
            }
        }

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
    }//end
}//end
