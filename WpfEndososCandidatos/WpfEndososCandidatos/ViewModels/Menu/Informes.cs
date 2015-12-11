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

        public RelayCommand mnuRechazo_click
        {
            get
            {
                if (_mnuRechazo_click ==null)
                {
                    _mnuRechazo_click = new RelayCommand(param => MymnuRechazo_click());
                }
                return _mnuRechazo_click;
            }
        }
     
        private void MymnuRechazo_click()
        {
            try
            {
                using (vmInforme frm = new vmInforme())
                {
                    frm.View.Owner =this.View  as Window;
                    frm.DBMasterCeeCnnStr = DBCeeMasterCnnStr;
                    frm.DBEndososCnnStr = DBEndososCnnStr;
                    frm.DBCeeMasterImgCnnStr = DBImagenesCnnStr;
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
    }
}
