
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

    public  class vmMatUsers : ViewModelBase<IDialogView>
    {
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
       
               

        public vmMatUsers()
            : base(new wpfMantUsers())
        {
            initWindow = new RelayCommand(param => InitWindow());
            guardar_Click = new RelayCommand(param => Guardar_Click());
            close_Click = new RelayCommand(param => Close_Click());
            borrar_Click = new RelayCommand(param => Borrar_Click());
            editar_Click = new RelayCommand(param => Editar_Click());
            anadir_Click = new RelayCommand(param => Anadir_Click(param));
            cancelar_Click = new RelayCommand(param => Cancelar_Click());
            cambiarPassword_Click = new RelayCommand(param => CambiarPassword_Click());
            autorizarLotes_Click = new RelayCommand(param => AutorizarLotes_Click());
            procesarLotes_Click = new RelayCommand(param => ProcesarLotes_Click());
            verElector_Click = new RelayCommand(param => VerElector_Click());
            reportes_Click = new RelayCommand(param => Reportes_Click());
            reversarLote_Click = new RelayCommand(param => ReversarLote_Click());
            configuraciones_Click = new RelayCommand(param => Configuraciones_Click());
            corregirEndosos_Click = new RelayCommand(param => CorregirEndosos_Click());
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
                            dbEndososPartidosEntities db = new dbEndososPartidosEntities();
                            var hashedPassword = PasswordHash.HashPassword("");
                            List<tblUser> u = new List<tblUser>
                            {
                                new tblUser {  UserId=System.Guid.NewGuid(), 
                                    UserName = "lara", 
                                    PasswordHash=hashedPassword, 
                                    Email="jlara@jolpr.com", 
                                    AreasDeAcceso=areasDeAcceso }
                            };
                            u.ForEach(m => db.tblUsers.Add(m));
                            db.SaveChanges();
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
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
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
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public RelayCommand anadir_Click {get;private set;}
        private void Anadir_Click(object password)
        {
            try
            {
                PasswordBox pass = password as PasswordBox;

                _Operation = 1;
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                
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
                throw new NotImplementedException();
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


        #region Password Txt
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
    }//end

 


}//end
