﻿

namespace WpfEndososCandidatos.ViewModels
{
    using jolcode;
    using System;
    using System.Reflection;
    using System.Windows;
    using WpfEndososCandidatos.ViewModels.Configuraciones;
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
        private bool _mnuAreas_IsEnabled;
        private bool _mnuPartidos_IsEnabled;
        private bool _mnuNotarios_IsEnabled;
        private bool _mnuValidaciones_IsEnabled;
        private bool _mnuUsuarios_IsEnabled;
        private bool _mnuBaseDeDatos_IsEnabled;
        private bool _mnuInicializarLotes_IsEnabled;



        public bool mnuBaseDeDatos_IsEnabled
        {
            get
            {
                return _mnuBaseDeDatos_IsEnabled;
            }
            set
            {
                if (_mnuBaseDeDatos_IsEnabled != value)
                {
                    _mnuBaseDeDatos_IsEnabled = value;
                    this.RaisePropertychanged("mnuBaseDeDatos_IsEnabled");
                }
            }
        }
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
                    using (vmMantDB frmMantDB = new vmMantDB(_REGPATH))
                    {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.TargetSite.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public bool mnuAreas_IsEnabled
        {
            get
            {
                return _mnuAreas_IsEnabled;
            }
            set
            {
                if (_mnuAreas_IsEnabled != value)
                {
                    _mnuAreas_IsEnabled = value;
                    this.RaisePropertychanged("mnuAreas_IsEnabled");
                }
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
                using (vmMantAreas frmMantAreas = new vmMantAreas())
                {
                    frmMantAreas.View.Owner = this.View as Window;
                    frmMantAreas.OnShow();
                }
                
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public bool mnuPartidos_IsEnabled
        {
            get
            {
                return _mnuPartidos_IsEnabled;
            }
            set
            {
                if (_mnuPartidos_IsEnabled != value)
                {
                    _mnuPartidos_IsEnabled = value;
                    this.RaisePropertychanged("mnuPartidos_IsEnabled");
                }
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
                using (vmMantPartidos frmMantPartidos = new vmMantPartidos())
                {
                    frmMantPartidos.View.Owner = this.View as Window;
                    frmMantPartidos.OnShow();

                }
                
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool mnuNotarios_IsEnabled
        {
            get
            {
                return _mnuNotarios_IsEnabled;
            }
            set
            {
                if (_mnuNotarios_IsEnabled != value)
                {
                    _mnuNotarios_IsEnabled = value;
                    this.RaisePropertychanged("mnuNotarios_IsEnabled");
                }
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
                using (vmMantNotarios frmMantNotarios = new vmMantNotarios())
                {
                    frmMantNotarios.View.Owner = this.View as Window;
                    frmMantNotarios.OnShow();
                }
                
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
        }

        public bool mnuValidaciones_IsEnabled
        {
            get
            {
                return _mnuValidaciones_IsEnabled;
            }
            set
            {
                if (_mnuValidaciones_IsEnabled != value)
                {
                    _mnuValidaciones_IsEnabled = value;
                    this.RaisePropertychanged("mnuValidaciones_IsEnabled");
                }
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
                using (vmMantCriterios frmMantCriterios = new vmMantCriterios())
                {
                    frmMantCriterios.View.Owner = this.View as Window;
                    frmMantCriterios.OnShow();
                }
                
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool mnuUsuarios_IsEnabled
        {
            get
            {
                return _mnuUsuarios_IsEnabled;
            }
            set
            {
                if (_mnuUsuarios_IsEnabled != value)
                {
                    _mnuUsuarios_IsEnabled = value;
                    this.RaisePropertychanged("mnuUsuarios_IsEnabled");
                }
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
                using (vmMatUsers frmMatUsers = new vmMatUsers())
                {
                    frmMatUsers.View.Owner = this.View as Window;
                    frmMatUsers._sqlServer = _SqlServer;
                    frmMatUsers._userName = _Username;
                    frmMatUsers._userPassword = _Password;
                    frmMatUsers._database = _Database;
                    frmMatUsers.OnShow();
                }   
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool mnuInicializarLotes_IsEnabled
        {
            get
            {
                return _mnuInicializarLotes_IsEnabled;
            }
            set
            {
                if (_mnuInicializarLotes_IsEnabled != value)
                {
                    _mnuInicializarLotes_IsEnabled = value;
                    this.RaisePropertychanged("mnuInicializarLotes_IsEnabled");
                }
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
                using (vmAbout frmAbout = new vmAbout())
                {
                    frmAbout.View.Owner = this.View as Window;
                    frmAbout.OnShow();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.TargetSite.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
