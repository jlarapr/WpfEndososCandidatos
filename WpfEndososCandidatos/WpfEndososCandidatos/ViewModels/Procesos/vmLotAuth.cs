namespace WpfEndososCandidatos.ViewModels.Procesos
{
    using jolcode;
    using jolcode.Base;
    using jolcode.MyInterface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using WpfEndososCandidatos.View;

    class vmLotAuth : ViewModelBase<IDialogView>
    {
        private int _numLote;
        private int _cantidad;

        public vmLotAuth()
            : base(new wpfLotAuth())
        {
            initWindow = new RelayCommand(param => InitWindow());
            cmdSalir_Click = new RelayCommand(param => CmdSalir_Click());
            cmdAddLot_Click = new RelayCommand(param => CmdAddLot_Click());
            cmdAddTodoLot_Click = new RelayCommand(param => CmdAddTodoLot_Click());
        }

        #region initWindow OnShow
        private void InitWindow()
        {
            try
            {


            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool? OnShow()
        {
            return this.View.ShowDialog();
        }
        public RelayCommand initWindow
        {
            get;
            private set;
        }
        #endregion

        #region property
        public int numLote
        {
            get
            {
                return _numLote;
            }
            set
            {
                if (_numLote != value)
                {
                    _numLote = value;
                    this.RaisePropertychanged("numLote");
                }
            }
        }

        public int cantidad
        {
            get
            {
                return _cantidad;
            }
            set
            {
                if (_cantidad != value)
                {
                    _cantidad = value;
                    this.RaisePropertychanged("cantidad");
                }
            }
            
        }
        #endregion

        #region Exit
        public RelayCommand cmdSalir_Click
        {
            get;
            private set;
        }
        public void CmdSalir_Click()
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

        #region cmd
        public RelayCommand cmdAddLot_Click
        {
            get;
            private set;
        }
        public void CmdAddLot_Click()
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

        public RelayCommand cmdAddTodoLot_Click
        {
            get;
            private set;

        }
        public void CmdAddTodoLot_Click()
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

        #endregion

    }//end
}//end
