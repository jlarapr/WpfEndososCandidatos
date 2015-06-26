
namespace WpfEndososCandidatos.ViewModel
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

    class vmMatUsers : ViewModelBase<IDialogView>
    {
        private int _Operation;
        private RelayCommand _InitWindow;
        private RelayCommand _Close_Click;
        private RelayCommand _guardar_Click;
        private RelayCommand _borrar_Click;
        private RelayCommand _editar_Click;
        private RelayCommand _anadir_Click;
        private RelayCommand _cancelar_Click;
        

        public vmMatUsers()
            : base(new wpfMantUsers())
        {

        }

        #region SaveBorrarEdiatarAnadir
        public RelayCommand guardar_Click
        {
            get
            {
                if (_guardar_Click == null)
                {
                    _guardar_Click = new RelayCommand(param => Guardar_Click());
                }
                return _guardar_Click;
            }
        }
        private void Guardar_Click()
        {
            try
            {

                throw new NotImplementedException();

                switch(_Operation)
                {
                    case 1:
                        {//Anadir
                            dbEndososPartidosEntities db = new dbEndososPartidosEntities();
                            var hashedPassword = PasswordHash.HashPassword("");
                            List<tblUser> u = new List<tblUser>
                            {
                                new tblUser {  UserId=System.Guid.NewGuid(), UserName = "lara", PasswordHash=hashedPassword, Email="jlara@jolpr.com" }
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
        public RelayCommand borrar_Click
        {
            get
            {
                if (_borrar_Click == null)
                {
                    _borrar_Click = new RelayCommand(param => Borrar_Click());
                }
                return _borrar_Click;
            }
        }
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
        public RelayCommand editar_Click
        {
            get
            {
                if (_editar_Click == null)
                {
                    _editar_Click = new RelayCommand(param => Editar_Click());
                }
                return _editar_Click;
            }
        }
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
        public RelayCommand anadir_Click
        {
            get
            {
                if (_anadir_Click == null)
                {
                    _anadir_Click = new RelayCommand(param => Anadir_Click());
                }
                return _anadir_Click;
            }
        }
        private void Anadir_Click()
        {
            try
            {
                _Operation = 1;
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommand cancelar_Click
        {
            get
            {
                if (_cancelar_Click ==null)
                {
                    _cancelar_Click = new RelayCommand(param => Cancelar_Click());
                }
                return _cancelar_Click;
            }
        }

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


        #region Swowwindow
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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "OnInitWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        #endregion

        #region Close
        public RelayCommand close_Click
        {
            get
            {
                if (_Close_Click == null)
                {
                    _Close_Click = new RelayCommand(param => Close_Click());
                }
                return _Close_Click;
                     
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



        #endregion
    }//end
}//end
