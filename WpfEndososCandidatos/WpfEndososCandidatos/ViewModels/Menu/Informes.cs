using jolcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfEndososCandidatos.ViewModels.Informes;

namespace WpfEndososCandidatos.ViewModels
{
    partial class MainVM
    {
        private RelayCommand _mnuRechazo_click;
        private RelayCommand _mnuReydi_click;
        private RelayCommand _mnuRechazoReydi_click;
        private RelayCommand _help_click;

        public RelayCommand help_click
        {
            get; private set;
        }
        public RelayCommand mnuduplicado_click
        {
            get; private set;
        }

        public RelayCommand mnuEndososRechazados_click
        {
            get; private set;
        }
        public RelayCommand mnuEstatus_click
        {
            get; private set;
        }


        public RelayCommand mnuRechazo_click
        {
            get
            {
                if (_mnuRechazo_click == null)
                {
                    _mnuRechazo_click = new RelayCommand(param => MymnuRechazo_click());
                }
                return _mnuRechazo_click;
            }
        }
        public RelayCommand mnuReydi_click
        {
            get
            {
                if (_mnuReydi_click == null)
                {
                    _mnuReydi_click = new RelayCommand(param => MymnuReydi_click());
                }
                return _mnuReydi_click;
            }
        }

        private void MymnuReydi_click()
        {
            try
            {
                using (vmInformeReydi frm = new vmInformeReydi())
                {
                    frm.View.Owner = this.View as Window;
                    frm.DBMasterCeeCnnStr = DBCeeMasterCnnStr;
                    frm.DBEndososCnnStr = DBEndososCnnStr;
                    frm.DBCeeMasterImgCnnStr = DBImagenesCnnStr;
                    frm.DBRadicacionesCEECnnStr = DBRadicacionesCEECnnStr;
                    frm.PDFPath = _PDFPath;
                    frm.MyOnShow();
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommand mnuRechazoReydi_click
        {
            get
            {
                if (_mnuRechazoReydi_click == null)
                {
                    _mnuRechazoReydi_click = new RelayCommand(param => MymnuRechazoReydi_click());
                }
                return _mnuRechazoReydi_click;
            }
        }
        private void MymnuRechazoReydi_click()
        {
            try
            {
                using (vmInforme frm = new vmInforme())
                {
                    frm.View.Owner = this.View as Window;
                    frm.DBMasterCeeCnnStr = DBCeeMasterCnnStr;
                    frm.DBEndososCnnStr = DBEndososCnnStr;
                    frm.DBCeeMasterImgCnnStr = DBImagenesCnnStr;
                    frm.PDFPath = _PDFPath;
                    frm.StatusReydi = "1";
                    frm.MyOnShow();
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MymnuRechazo_click()
        {
            try
            {
                using (vmInforme frm = new vmInforme())
                {
                    frm.View.Owner = this.View as Window;
                    frm.DBMasterCeeCnnStr = DBCeeMasterCnnStr;
                    frm.DBEndososCnnStr = DBEndososCnnStr;
                    frm.DBCeeMasterImgCnnStr = DBImagenesCnnStr;
                    frm.PDFPath = _PDFPath;
                    frm.StatusReydi = "0";
                    frm.MyOnShow();
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void Mymnuduplicado_clickC()
        {
            try
            {
                using (vmDuplicados frm = new vmDuplicados())
                {
                    frm.View.Owner = this.View as Window;
                    frm.DBMasterCeeCnnStr = DBCeeMasterCnnStr;
                    frm.DBEndososCnnStr = DBEndososCnnStr;
                    frm.DBCeeMasterImgCnnStr = DBImagenesCnnStr;
                    //frm.PDFPath = _PDFPath;
                    frm.StatusReydi = "0,1";
                    frm.MyOnShow();
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Myhelp_click()
        {
            try{
                //System.Windows.Forms.HelpProvider h = new System.Windows.Forms.HelpProvider();
                //h.HelpNamespace = "Sistema_de_Endosos.chm";
                //System.Windows.Forms.Help.ShowHelp(null, h.HelpNamespace);
                System.Windows.Forms.Help.ShowHelp(null, @"Sistema_de_Endosos.chm");
                
            }catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
        private void MymnuEstatus_click()
        {
            try
            {
                using (vmEstatus frm = new vmEstatus())
                {
                    frm.View.Owner = this.View as Window;
                    frm.DBMasterCeeCnnStr = DBCeeMasterCnnStr;
                    frm.DBEndososCnnStr = DBEndososCnnStr;
                    frm.DBCeeMasterImgCnnStr = DBImagenesCnnStr;

                    frm.MyOnShow();
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MymnuEndososRechazados_click()
        {
            try
            {
                using (vmEndososRechazados frm = new vmEndososRechazados())
                {
                    frm.View.Owner = this.View as Window;
                    frm.DBMasterCeeCnnStr = DBCeeMasterCnnStr;
                    frm.DBEndososCnnStr = DBEndososCnnStr;
                    frm.DBCeeMasterImgCnnStr = DBImagenesCnnStr;
                    frm.MyOnShow();
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
