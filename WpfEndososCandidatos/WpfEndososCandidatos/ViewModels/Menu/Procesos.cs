namespace WpfEndososCandidatos.ViewModels
{
    using jolcode;
    using System;
    using System.Reflection;
    using System.Windows;
    partial class MainVM
    {
        private RelayCommand _recibirLotes_Click;
        private RelayCommand _autorizarLotes_Click;
        private RelayCommand _procesarLotes_Click;
        private RelayCommand _corregirEndosos_Click;
        private RelayCommand _reversarLote_Click;

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

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
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

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
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
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message,site.Name , MessageBoxButton.OK, MessageBoxImage.Error);
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

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
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
