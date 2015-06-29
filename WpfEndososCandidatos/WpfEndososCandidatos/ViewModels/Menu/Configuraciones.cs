﻿

namespace WpfEndososCandidatos.ViewModels
{
    using jolcode;
    using System;
    using System.Reflection;
    using System.Windows;
    partial class MainVM
    {
        private RelayCommand _baseDeDatos;
        private RelayCommand _areas_Click;
        private RelayCommand _partidos_Click;
        private RelayCommand _notarios_Click;
        private RelayCommand _validaciones_Click;
        private RelayCommand _usuarios_Click;
        private RelayCommand _inicializarLotes_Click;
        private RelayCommand _about_Click;

        public RelayCommand baseDeDatos_Click
        {
            get
            {
                if (_baseDeDatos == null)
                {
                    _baseDeDatos = new RelayCommand(param => BaseDeDatos_Click());
                }
                return _baseDeDatos;
            }
        }

        private void BaseDeDatos_Click()
        {
            try
            {
                {//Get values from register
                    _SqlServer = jolcode.Registry.read(_REGPATH, "DBServer");
                    _Username = jolcode.Registry.read(_REGPATH, "DBUser");
                    _Password = jolcode.Registry.read(_REGPATH, "DBPass");
                    _Database = jolcode.Registry.read(_REGPATH, "DBName");

                    _MastSvr = jolcode.Registry.read(_REGPATH, "MastSvr");
                    _MastUsr = jolcode.Registry.read(_REGPATH, "MastUsr");
                    _MastPass = jolcode.Registry.read(_REGPATH, "MastPass");
                    _MastDB = jolcode.Registry.read(_REGPATH, "MastDB");

                    _ImageSvr = jolcode.Registry.read(_REGPATH, "ImageSvr");
                    _ImageUsr = jolcode.Registry.read(_REGPATH, "ImageUsr");
                    _ImagePass = jolcode.Registry.read(_REGPATH, "ImagePass");
                    _ImageDB = jolcode.Registry.read(_REGPATH, "ImageDB");

                    _ValiSvr = jolcode.Registry.read(_REGPATH, "ValiSvr");
                    _ValiUsr = jolcode.Registry.read(_REGPATH, "ValiUsr");
                    _ValiPass = jolcode.Registry.read(_REGPATH, "ValiPass");
                    _ValiDB = jolcode.Registry.read(_REGPATH, "ValiDB");

                    _ImgPath = jolcode.Registry.read(_REGPATH, "ImagePathNew");
                }

                
                {
                    vmMantDB frmMantDB = new vmMantDB(_REGPATH);

                    frmMantDB.sqlServer = _SqlServer;
                    frmMantDB.userName = _Username;
                    frmMantDB.password = _Password;
                    frmMantDB.database = _Database;

                    frmMantDB.mastSvr = _MastSvr;
                    frmMantDB.mastUsr = _MastUsr;
                    frmMantDB.mastPass = _MastPass;
                    frmMantDB.mastDB = _MastDB;

                    frmMantDB.imageSvr = _ImageSvr;
                    frmMantDB.imageUsr = _ImageUsr;
                    frmMantDB.imagePass = _ImagePass;
                    frmMantDB.imageDB = _ImageDB;

                    frmMantDB.valiSvr = _ValiSvr;
                    frmMantDB.valiUsr = _ValiUsr;
                    frmMantDB.valiPass = _ValiPass;
                    frmMantDB.valiDB = _ValiDB;

                    frmMantDB.imgPath = _ImgPath;

                    frmMantDB.View.Owner = this.View as Window;
                    frmMantDB.OnShow();

                    _SqlServer = frmMantDB.sqlServer;
                    _Username = frmMantDB.userName;
                    _Password = frmMantDB.password;
                    _Database = frmMantDB.database;

                    _MastSvr = frmMantDB.mastSvr;
                    _MastUsr = frmMantDB.mastUsr;
                    _MastPass = frmMantDB.mastPass;
                    _MastDB = frmMantDB.mastDB;

                    _ImageSvr = frmMantDB.imageSvr;
                    _ImageUsr = frmMantDB.imageUsr;
                    _ImagePass = frmMantDB.imagePass;
                    _ImageDB = frmMantDB.imageDB;

                    _ValiSvr = frmMantDB.valiSvr;
                    _ValiUsr = frmMantDB.valiUsr;
                    _ValiPass = frmMantDB.valiPass;
                    _ValiDB = frmMantDB.valiDB;

                    _ImgPath = frmMantDB.imgPath;

                }//End IF

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.TargetSite.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommand areas_Click
        {
            get
            {
                if (_areas_Click == null)
                {
                    _areas_Click = new RelayCommand(param => Areas_Click());
                }
                return _areas_Click;
            }
        }

        private void Areas_Click()
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

        public RelayCommand partidos_Click
        {
            get
            {
                if (_partidos_Click == null)
                {
                    _partidos_Click = new RelayCommand(param => Partidos_Click());
                }
                return _partidos_Click;
            }
        }

        private void Partidos_Click()
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

        public RelayCommand notarios_Click
        {
            get
            {
                if (_notarios_Click == null)
                {
                    _notarios_Click = new RelayCommand(param => Notarios_Click());
                }
                return _notarios_Click;
            }
        }

        private void Notarios_Click()
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

        public RelayCommand validaciones_Click
        {
            get
            {
                if (_validaciones_Click==null) 
                {
                    _validaciones_Click = new RelayCommand(param => Validaciones_Click());
                }
                return _validaciones_Click;
            }
        }
        private void Validaciones_Click()
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

        public RelayCommand usuarios_Click
        {
            get
            {
                if (_usuarios_Click == null)
                {
                    _usuarios_Click = new RelayCommand(param => Usuarios_Click());
                }
                return _usuarios_Click;
            }
        }
        private void Usuarios_Click()
        {
            try
            {
                vmMatUsers frmMatUsers = new vmMatUsers();
                frmMatUsers.View.Owner = this.View as Window;
                frmMatUsers.OnShow();
                     

                
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommand inicializarLotes_Click
        {
            get
            {
                if (_inicializarLotes_Click==null)
                {
                    _inicializarLotes_Click = new RelayCommand(param => InicializarLotes_Click());
                }
                return _inicializarLotes_Click;
            }
        }

        private void InicializarLotes_Click()
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

        public RelayCommand about_Click
        {
            get
            {
                if (_about_Click == null)
                {
                    _about_Click = new RelayCommand(param => About_Click());
                }
                return _about_Click;
            }
        }

        private void About_Click()
        {
            try
            {
                vmAbout frmAbout = new vmAbout();
                frmAbout.View.Owner = this.View as Window;
                frmAbout.OnShow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.TargetSite.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}