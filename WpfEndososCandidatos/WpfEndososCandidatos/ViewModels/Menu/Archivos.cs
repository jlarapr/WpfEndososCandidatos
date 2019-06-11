namespace WpfEndososCandidatos.ViewModels
{
    using jolcode;
    using System;
    using System.Reflection;
    using System.Windows;
    using WpfEndososCandidatos.Models;
    using System.Linq;
    using System.Windows.Input;
    using System.Configuration;

    partial class MainVM
    {
        private RelayCommand _login_Click;
        private RelayCommand _logout_Click;
        private RelayCommand _cambiarPassword_Click;
        private RelayCommand _close_Click;
        private bool _mnuChangePassword_IsEnabled;
        private bool _mnuLogout_IsEnabled;
        private bool _mnuLogin_IsEnabled;
      //  dbEndososPartidosEntities _db = new dbEndososPartidosEntities();
        private Guid _Id;
        
        public bool mnuLogin_IsEnabled
        {
            get
            {
                return _mnuLogin_IsEnabled;
            }
            set
            {
                if (_mnuLogin_IsEnabled != value)
                {
                    _mnuLogin_IsEnabled = value;
                    this.RaisePropertychanged("mnuLogin_IsEnabled");
                }
            }
        }
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
            MiCursor =  Cursors.Wait;

            try
            {
                using (vmfLogin frmfLogin = new vmfLogin())
                {
                    frmfLogin.View.Owner = this.View as Window;
                    frmfLogin.DBEndososCnnStr = _DBEndososCnnStr;
                    frmfLogin.DBCeeMasterCnnStr = _DBCeeMasterCnnStr;
                    frmfLogin.DBImagenesCnnStr = _DBImagenesCnnStr;

                    frmfLogin.OnShow();

                    Title = String.Format("CEE Sistema de Validación de Endosos " + "Version {0}", AssemblyVersion);
                    //Title = String.Format("CEE Endosos Candidatos 2015 Version {0}", AssemblyVersion);
                    if (frmfLogin.View.DialogResult == false)
                        return;

                    WhatIsUserName = frmfLogin.WhatIsUserName;

                    Title += string.Concat(" UserName:", frmfLogin.WhatIsUserName, " ", frmfLogin._Id.ToString());
                    _Id = frmfLogin._Id;
                    mnuLogin_IsEnabled = false;
                    mnuLogout_IsEnabled = true;

                    foreach (char c in frmfLogin._AreasDeAcceso.ToCharArray())
                    {
                        switch (c)
                        {
                            case 'A'://CambiarPassword
                                mnuChangePassword_IsEnabled = true;
                                break;
                            case 'B':// AutorizarLotes
                                mnuAutoRizarLotes_IsEnabled = true;
                                break;
                            case 'C':// ProcesarLotes
                                mnuRecibirLotes_IsEnabled = true;
                                mnuProcesarLotes_IsEnabled = true;
                                mnuVerEndosos_IsEnabled = true;

                                bool ReydiMenuIsEnable;
                                bool.TryParse(ConfigurationManager.AppSettings["ReydiMenuIsEnable"], out ReydiMenuIsEnable);
                                mnuReydi_IsEnabled = ReydiMenuIsEnable;
                                
                                break;
                            case 'D':// VerElector
                                mnuVerElector_IsEnabled = true;
                                break;
                            case 'E'://Reportes
                                mnuInformeEndosos_IsEnabled = true;
                                mnuInformeDuplicados_IsEnable = false;

                                bool InformeDuplicadosIsEnable;
                                bool.TryParse(ConfigurationManager.AppSettings["InformeDuplicadosIsEnable"], out InformeDuplicadosIsEnable);
                                mnuInformeDuplicados_IsEnable = InformeDuplicadosIsEnable;


                                break;
                            case 'F'://ReversarLote
                                mnuRevLote_IsEnabled = true;
                                break;
                            case 'G'://Configuracione
                                mnuAreas_IsEnabled = true;
                                mnuPartidos_IsEnabled = true;
                                mnuNotarios_IsEnabled = true;
                                mnuValidaciones_IsEnabled = true;
                                mnuUsuarios_IsEnabled = true;
                                mnuBaseDeDatos_IsEnabled = true;
                                mnuInicializarLotes_IsEnabled = true;
                                mnuCandidatos_IsEnabled = true;
                                break;
                            case 'H'://corregirEndosos
                                mnuCorregirEndosos_IsEnabled = true;
                                break;
                        }
                    }
                }//end using
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }finally
            {
                MiCursor = Cursors.Arrow;

            }
        }

        public bool mnuLogout_IsEnabled
        {
            get
            {
                return _mnuLogout_IsEnabled;
            }
            set
            {
                if (_mnuLogout_IsEnabled != value)
                {
                    _mnuLogout_IsEnabled = value;
                    this.RaisePropertychanged("mnuLogout_IsEnabled");
                }
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
                mnuChangePassword_IsEnabled = false;
                mnuLogout_IsEnabled = false;
                mnuLogin_IsEnabled = true;
                mnuVerElector_IsEnabled = false;
                mnuRecibirLotes_IsEnabled = false;
                mnuAutoRizarLotes_IsEnabled = false;
                mnuProcesarLotes_IsEnabled = false;
                mnuCorregirEndosos_IsEnabled = false;
                mnuRevLote_IsEnabled = false;
                mnuAreas_IsEnabled = false;
                mnuPartidos_IsEnabled = false;
                mnuNotarios_IsEnabled = false;
                mnuValidaciones_IsEnabled = false;
                mnuUsuarios_IsEnabled = false;
                mnuBaseDeDatos_IsEnabled = false;
                mnuInicializarLotes_IsEnabled = false;
                mnuCandidatos_IsEnabled = false;
                mnuVerEndosos_IsEnabled = false;
                mnuInformeEndosos_IsEnabled = false;
                mnuReydi_IsEnabled = false;
                Login_Click();
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool mnuChangePassword_IsEnabled
        {
            get
            {
                return _mnuChangePassword_IsEnabled;
            }
            set
            {
                if (_mnuChangePassword_IsEnabled != value)
                {
                    _mnuChangePassword_IsEnabled = value;
                    this.RaisePropertychanged("mnuChangePassword_IsEnabled");
                }
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
                using (vmMantPass frmMantPass = new vmMantPass())
                {
                    frmMantPass.View.Owner = this.View as Window;
                    frmMantPass._Id = _Id;
                    frmMantPass.DBEndososCnnStr = DBEndososCnnStr;
                    frmMantPass.OnShow();
                }
              
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
