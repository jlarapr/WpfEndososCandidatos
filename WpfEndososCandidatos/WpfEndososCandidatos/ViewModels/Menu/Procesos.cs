﻿namespace WpfEndososCandidatos.ViewModels
{
    using jolcode;
    using System;
    using System.Reflection;
    using System.Windows;
    using View.Informes;
    using WpfEndososCandidatos.ViewModels.Procesos;
    partial class MainVM
    {
        private RelayCommand _recibirLotes_Click;
        private RelayCommand _autorizarLotes_Click;
        private RelayCommand _procesarLotes_Click;
        private RelayCommand _corregirEndosos_Click;
        private RelayCommand _reversarLote_Click;
        private bool _mnuRecibirLotes_IsEnabled;
        private bool _mnuAutoRizarLotes_IsEnabled;
        private bool _mnuProcesarLotes_IsEnabled;
        private bool _mnuCorregirEndosos_IsEnabled;
        private bool _mnuRevLote_IsEnabled;
        private bool _mnuVerEndosos_IsEnabled;
        private bool _mnuInformeEndosos_IsEnabled;
        private bool _mnuReydi_IsEnabled;
        private RelayCommand _verEndosos_Click;
        private RelayCommand _FixLot_Click;

        public bool mnuRecibirLotes_IsEnabled
        {
            get
            {
                return _mnuRecibirLotes_IsEnabled;
            }
            set
            {
                if (_mnuRecibirLotes_IsEnabled != value)
                {
                    _mnuRecibirLotes_IsEnabled = value;
                    this.RaisePropertychanged("mnuRecibirLotes_IsEnabled");
                }
            }
        }
        
        public RelayCommand recibirLotes_Click
        {
            get
            {
                if (_recibirLotes_Click == null)
                {
                    _recibirLotes_Click = new RelayCommand(Param => RecibirLotes_Click());
                }
                return _recibirLotes_Click;
            }
        }

        private void RecibirLotes_Click()
        {
            try
            {
                using (vmLotReceive frmLotReceive = new vmLotReceive())
                {
                    frmLotReceive.View.Owner = this.View as Window;
                    frmLotReceive.OnShow();
                }
               
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public bool mnuAutoRizarLotes_IsEnabled
        {
            get
            {
                return _mnuAutoRizarLotes_IsEnabled;
            }
            set
            {
                if (_mnuAutoRizarLotes_IsEnabled !=value)
                {
                    _mnuAutoRizarLotes_IsEnabled = value;
                    this.RaisePropertychanged("mnuAutoRizarLotes_IsEnabled");
                }
            }
        }
        public RelayCommand autorizarLotes_Click
        {
            get
            {
                if (_autorizarLotes_Click == null)
                {
                    _autorizarLotes_Click = new RelayCommand(param => AutorizarLotes_Click());
                }
                return _autorizarLotes_Click;
                     
            }
        }

        private void AutorizarLotes_Click()
        {
            try
            {
                using (vmLotAuth frmLotAuth = new vmLotAuth())
                {
                    frmLotAuth.View.Owner = this.View as Window;
                    frmLotAuth.DBEndososCnnStr = DBEndososCnnStr;
                    frmLotAuth.DBRadicacionesCEECnnStr = DBRadicacionesCEECnnStr;
                    frmLotAuth.SysUser = WhatIsUserName;
                    frmLotAuth.MyOnShow();
                }
               
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool mnuProcesarLotes_IsEnabled
        {
            get
            {
                return _mnuProcesarLotes_IsEnabled;
            }
            set
            {
                if (_mnuProcesarLotes_IsEnabled !=value)
                {
                    _mnuProcesarLotes_IsEnabled = value;
                    this.RaisePropertychanged("mnuProcesarLotes_IsEnabled");
                }
            }
        }
        
        public RelayCommand procesarLotes_Click
        {
            get
            {
                if (_procesarLotes_Click == null)
                {
                    _procesarLotes_Click = new RelayCommand(param => ProcesarLotes_Click());
                }
                return _procesarLotes_Click;
            }
        }

        private void ProcesarLotes_Click()
        {
            try
            {
                using (vmLotProcess frmLotProcess = new vmLotProcess())
                {
                    frmLotProcess.View.Owner = this.View as Window;
                    frmLotProcess.DBEndososCnnStr = DBEndososCnnStr;
                    frmLotProcess.DBCeeMasterCnnStr = DBCeeMasterCnnStr;
                    frmLotProcess.DBImagenesCnnStr = DBImagenesCnnStr;
                    frmLotProcess.DBRadicacionesCEECnnStr = DBRadicacionesCEECnnStr;
                    frmLotProcess.SysUser = WhatIsUserName;
                    frmLotProcess.MyOnShow();
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message,site.Name , MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public bool mnuCorregirEndosos_IsEnabled
        {
            get
            {
                return _mnuCorregirEndosos_IsEnabled;
            }
            set
            {
                if (_mnuCorregirEndosos_IsEnabled != value)
                {
                    _mnuCorregirEndosos_IsEnabled = value;
                    this.RaisePropertychanged("mnuCorregirEndosos_IsEnabled");
                }
            }
        }

        public bool mnuVerEndosos_IsEnabled
        {
            get
            {
                return _mnuVerEndosos_IsEnabled;
            }
            set
            {
                if (_mnuVerEndosos_IsEnabled != value)
                {
                    _mnuVerEndosos_IsEnabled = value;
                    this.RaisePropertychanged("mnuVerEndosos_IsEnabled");
                }
            }
        }
        public bool mnuInformeEndosos_IsEnabled
        {
            get
            {
                return _mnuInformeEndosos_IsEnabled;
            }
            set
            {
                if (_mnuInformeEndosos_IsEnabled != value)
                {
                    _mnuInformeEndosos_IsEnabled = value;
                    this.RaisePropertychanged("mnuInformeEndosos_IsEnabled");
                }
            }
        }
        public bool mnuReydi_IsEnabled
        {
            get
            {
                return _mnuReydi_IsEnabled;
            }
            set
            {
                if (_mnuReydi_IsEnabled != value)
                {
                    _mnuReydi_IsEnabled = value;
                    this.RaisePropertychanged("mnuReydi_IsEnabled");
                }
            }
        }
        public RelayCommand verEndosos_Click
        {
            get
            {
                if (_verEndosos_Click == null)
                {
                    _verEndosos_Click = new RelayCommand(param => MyverEndosos_Click());
                }
                return _verEndosos_Click;
            }
        }

       public RelayCommand FixLot_Click
        {
            get
            {
                if (_FixLot_Click ==null)
                {
                    _FixLot_Click = new RelayCommand(param => MyFixLot_Click());
                }
                return _FixLot_Click;
            }
        }
        private void MyFixLot_Click()
        {
            try
            {
                using (vmFixLot frm = new vmFixLot())
                {
                    frm.View.Owner = this.View as Window;
                    frm.MyOnShow();

                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MyverEndosos_Click()
        {
            try
            {
                using ( vmVerendosos frm = new vmVerendosos())
                {
                    frm.View.Owner = this.View as Window;
                    frm.DBEndososCnnStr = DBEndososCnnStr;
                    frm.DBMasterCeeCnnStr = DBCeeMasterCnnStr;
                    frm.DBCeeMasterImgCnnStr = DBImagenesCnnStr;
                    frm.SysUser = WhatIsUserName;
                    frm.MyOnShow();
                }

            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        public RelayCommand  corregirEndosos_Click
        {
            get
            {
                if (_corregirEndosos_Click == null)
                {
                    _corregirEndosos_Click = new RelayCommand(param => CorregirEndosos_Click());
                }
                return _corregirEndosos_Click;
            }
        }

        private void CorregirEndosos_Click()
        {
            try
            {
                using (vmLotFix frmLotFix = new vmLotFix())
                {
                    frmLotFix.View.Owner = this.View as Window;
                    frmLotFix.DBEndososCnnStr = DBEndososCnnStr;
                    frmLotFix.DBMasterCeeCnnStr = DBCeeMasterCnnStr;
                    frmLotFix.DBCeeMasterImgCnnStr = DBImagenesCnnStr;
                    frmLotFix.SysUser = WhatIsUserName;
                    frmLotFix.MyOnShow();
                }
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        public bool mnuRevLote_IsEnabled
        {
            get
            {
                return _mnuRevLote_IsEnabled;
            }
            set
            {
                if (_mnuRevLote_IsEnabled != value)
                {
                    _mnuRevLote_IsEnabled = value;
                    this.RaisePropertychanged("mnuRevLote_IsEnabled");
                }
            }
        }
        public RelayCommand reversarLote_Click
        {
            get
            {
                if (_reversarLote_Click == null)
                {
                    _reversarLote_Click = new RelayCommand(param => ReversarLote_Click());
                }
                return _reversarLote_Click;
            }
        }

        private void ReversarLote_Click()
        {
            try
            {
                using (vmLotReverse frmLotReverse = new vmLotReverse())
                {
                    frmLotReverse.View.Owner = this.View as Window;
                    frmLotReverse.DBEndososCnnStr = DBEndososCnnStr;
                    frmLotReverse.SysUser = WhatIsUserName;
                    frmLotReverse.MyOnShow();
                }
               
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }//end
}//end
