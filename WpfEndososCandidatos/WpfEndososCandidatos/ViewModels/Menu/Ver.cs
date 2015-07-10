namespace WpfEndososCandidatos.ViewModels
{
    using jolcode;
    using System;
    using System.Reflection;
    using System.Windows;
     partial class MainVM
     {
         private RelayCommand _verElector_click;
         private bool _mnuVerElector_IsEnabled;



         public bool mnuVerElector_IsEnabled
         {
             get
             {
                 return _mnuVerElector_IsEnabled;
             }
             set
             {
                 if (_mnuVerElector_IsEnabled != value)
                 {
                     _mnuVerElector_IsEnabled = value;
                     this.RaisePropertychanged("mnuVerElector_IsEnabled");
                 }
             }
         }
         public RelayCommand verElector_click
         {
             get
             {
                 if (_verElector_click == null)
                 {
                     _verElector_click = new RelayCommand(param => VerElector_click());
                 }
                 return _verElector_click;

                      
             }
         }
         private void VerElector_click()
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


     }//end
}//end
