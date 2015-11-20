namespace WpfEndososCandidatos.ViewModels.Procesos
{
    using jolcode;
    using jolcode.Base;
    using jolcode.MyInterface;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using Models;
    using WpfEndososCandidatos.View.Procesos;
    using System.Windows.Input;

    class vmLotProcess : ViewModelBase<IDialogView>,IDisposable 
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private string _DBEndososCnnStr;
        private DataTable _MyCriteriosTable;
        private ObservableCollection<Criterios> _CollCriterios;
        private DataTable _MyLotsTable;
        private ObservableCollection<string> _cbLots;
        private string _cbLots_Item;
        private int _cbLots_Item_Id;
        private bool _CanView;
        private Cursor _MiCursor;
        private string _lblTota;
        private string _lblLote;
        private string _lblProcesadas;
        private string _lblAprobadas;
        private string _lblRechazadas;
        private string _lblWarnings;
        private ObservableCollection<string> _lblNReasons;
        private string _DBCeeMasterCnnStr;
        private string _DBImagenesCnnStr;
        private string _SysUser;
        private ObservableCollection<Brush> _Foreground_Desc;
        private string _DBRadicacionesCEECnnStr;

        public vmLotProcess()
            : base(new wpfLotProcess())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdRefresh_Click = new RelayCommand(param => MyCmdRefresh_Click());
            cmdProcess_Click = new RelayCommand(param=>MyCmdProcess_Click(), param => CanCmdProcess);
            cmdView_Click = new RelayCommand(param => MyCmdView_Click(), param => CanView);

            CollCriterios = new ObservableCollection<Criterios>();
            cbLots = new ObservableCollection<string>();
            lblNReasons = new ObservableCollection<string>();
            Foreground_Desc = new ObservableCollection<Brush>();
            CanView = false;
        }

        #region MyProperty

        public Brush BorderBrush
        {
            get
            {
                return _BorderBrush;
            }
            set
            {
                if (_BorderBrush != value)
                {
                    _BorderBrush = value;
                    this.RaisePropertychanged("BorderBrush");
                }

            }
        }
        public System.Windows.Input.Cursor MiCursor
        {
            get
            {
                return _MiCursor;
            }
            set
            {
                _MiCursor = value;
                this.RaisePropertychanged("MiCursor");
            }

        }
        public string DBEndososCnnStr
        {
            get
            {
                return _DBEndososCnnStr;
            }set
            {
                _DBEndososCnnStr = value;
            }
        }
        public string DBCeeMasterCnnStr
        {
            get
            {
                return _DBCeeMasterCnnStr;
            }
            set
            {
                _DBCeeMasterCnnStr = value;
            }
        }
        public string DBImagenesCnnStr
        {
            get
            {
                return _DBImagenesCnnStr;
            }
            set
            {
                _DBImagenesCnnStr = value;
            }
        }
        public string DBRadicacionesCEECnnStr
        {
            get
            {
                return _DBRadicacionesCEECnnStr;
            }set
            {
                _DBRadicacionesCEECnnStr = value;
            }
        }

        public string SysUser
        {
            get
            {
                return _SysUser;
            }set
            {
                _SysUser = value;
            }
        }

        public string lblTota
        {
            get
            {
                return _lblTota;
            }set
            {
                _lblTota = value;
                this.RaisePropertychanged("lblTota");
            }
        }
        public string lblLote
        {
            get
            {
                return _lblLote;
            }
            set
            {
                _lblLote = value;
                this.RaisePropertychanged("lblLote");
            }
        }
        public string lblProcesadas
        {
            get
            {
                return _lblProcesadas;
            }
            set
            {
                _lblProcesadas = value;
                this.RaisePropertychanged("lblProcesadas");
            }
        }
        public string lblAprobadas
        {
            get
            {
                return _lblAprobadas;
            }
            set
            {
                _lblAprobadas = value;
                this.RaisePropertychanged("lblAprobadas");
            }
        }
        public string lblRechazadas
        {
            get
            {
                return _lblRechazadas;
            }
            set
            {
                _lblRechazadas = value;
                this.RaisePropertychanged("lblRechazadas");
            }
        }
        public string lblWarnings
        {
            get
            {
                return _lblWarnings;
            }
            set
            {
                _lblWarnings = value;
                this.RaisePropertychanged("lblWarnings");
            }
        }

        public ObservableCollection<Brush> Foreground_Desc
        {
            get
            {
                return _Foreground_Desc;
            }set
            {
                _Foreground_Desc = value;
                this.RaisePropertychanged("Foreground_Desc");
            }
        }
        public ObservableCollection<Criterios> CollCriterios
        {
            get
            {
                return _CollCriterios;
            }
            set
            {
                if (value != null)
                {
                    _CollCriterios = value;
                    this.RaisePropertychanged("CollCriterios");
                }
            }

        }
        public ObservableCollection<string> lblNReasons
        {
            get
            {
                return _lblNReasons;
            }set
            {
                _lblNReasons = value;
                this.RaisePropertychanged("lblNReasons");
            }
        }
        public ObservableCollection<string> cbLots
        {
            get
            {
                return _cbLots;
            }
            set
            {
                _cbLots = value;
                this.RaisePropertychanged("cbLots");
            }
        }
        public string cbLots_Item
        {
            get
            {
                return _cbLots_Item;
            }
            set
            {
               
                _cbLots_Item = value;
                this.RaisePropertychanged("cbLots_Item");
            }
        }
        public int cbLots_Item_Id
        {
            get
            {
                return _cbLots_Item_Id;
            }
            set
            {
                _cbLots_Item_Id = value;
                this.RaisePropertychanged("cbLots_Item_Id");
            }
        }

        public bool CanCmdProcess
        {
            get
            {
                return cbLots_Item_Id > -1 ? true : false;
            }
        }
        public bool CanView
        {
            get
            {
                return _CanView;
            }set
            {
                _CanView = value;
            }
        }

        #endregion

        #region MyCmd
        private void MyInitWindow()
        {
            try
            {
                string myBorderBrush = ConfigurationManager.AppSettings["BorderBrush"];

                if (myBorderBrush != null && myBorderBrush.Trim().Length > 0)
                {
                    Type t = typeof(Brushes);
                    Brush b = (Brush)t.GetProperty(myBorderBrush).GetValue(null, null);
                    BorderBrush = b;
                }
                else
                    BorderBrush = Brushes.Black;

                MyRefresh();

            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }

          

        }
        private void MyCmdSalir_Click()
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
        internal bool? MyOnShow()
        {
            return this.View.ShowDialog();
        }
        private void MyCmdRefresh_Click()
        {
            try
            {
                MyRefresh();
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void MyCmdProcess_Click()
        {
            try
            {
                MiCursor = Cursors.Wait;
                CanView = true;

                using (SqlExcuteCommand exe = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr,
                    DBCeeMasterCnnStr = DBCeeMasterCnnStr,
                    DBImagenesCnnStr = DBImagenesCnnStr,
                    DBRadicacionesCEECnnStr = DBRadicacionesCEECnnStr
                })
                {
                    exe.MyProcessLot(cbLots_Item, SysUser, CollCriterios,lblNReasons);
                }

            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                MiCursor = Cursors.Arrow;
            }
        }

        private void MyCmdView_Click()
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

        public RelayCommand initWindow
        {
            get;
            private set;
        }
        public RelayCommand cmdSalir_Click
        {
            get;
            private set;
        }
        public RelayCommand cmdRefresh_Click
        {
            get;private set;
        }
        public RelayCommand cmdProcess_Click
        {
            get;private set;
        }
        public RelayCommand cmdView_Click
        {
            get;private set;
        }

        #endregion

        
        #region MyMetodos


        private void MyRefresh()
        {
            try
            {
                CanView = false;
                lblLote = "0";
                lblTota = "0";
                lblProcesadas = "0";
                lblAprobadas = "0";
                lblRechazadas = "0";
                lblWarnings = "0";

                for (int i = 0; i < 17; i++)
                    lblNReasons.Add("0");

                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr,
                    DBCeeMasterCnnStr=DBCeeMasterCnnStr,
                    DBImagenesCnnStr=DBImagenesCnnStr
                })
                {

                    _MyCriteriosTable = get.MyGetCriterios();
                    
                    CollCriterios.Clear();

                    Foreground_Desc.Clear();


                    foreach (DataRow row in _MyCriteriosTable.Rows)
                    {
                        
                        CollCriterios.Add(new Criterios
                        {
                            Campo = row["Campo"].ToString(),
                            Editar = row["Editar"].ToString().Trim() == "1" ? true : false,
                            Desc = row["Desc"].ToString(),
                            Warning = row["Warning"].ToString().Trim() == "1" ? true : false
                        });

                        if (row["Editar"].ToString().Trim() != "1")
                        {
                            Foreground_Desc.Add(Brushes.Yellow);
                        }
                        else
                        {
                            Foreground_Desc.Add(Brushes.Black);
                        }
                    }

                    _MyLotsTable = get.MyGetLotToProcess();

                    cbLots.Clear();

                    foreach (DataRow row in _MyLotsTable.Rows)
                    {
                        Lots myLots = new Lots();

                        myLots.Partido = row["Partido"].ToString();
                        myLots.Lot = row["Lot"].ToString();
                        myLots.Amount = row["Amount"].ToString();
                        myLots.Usercode = row["Usercode"].ToString();
                        myLots.AuthDate = row["AuthDate"].ToString();
                        myLots.Status = row["Status"].ToString();
                        myLots.VerDate = row["VerDate"].ToString();
                        myLots.VerUser = row["VerUser"].ToString();
                        myLots.FinUser = row["FinUser"].ToString();
                        myLots.FinDate = row["FinDate"].ToString();
                        myLots.RevDate = row["RevDate"].ToString();
                        myLots.RevUser = row["RevUser"].ToString();
                        myLots.conditions = row["conditions"].ToString();
                        myLots.ImportDate = row["ImportDate"].ToString();
                      
                        cbLots.Add(myLots.Lot);

                    }
                    cbLots_Item_Id = -1;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MySendTab()
        {
            try
            {
                KeyEventArgs e1 = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
                e1.RoutedEvent = Keyboard.KeyDownEvent;
                InputManager.Current.ProcessInput(e1);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion



        #region Dispose



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmLotProcess()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources AnotherResource 
                //if (managedResource != null)
                //{
                //    managedResource.Dispose();
                //    managedResource = null;
                //}
            }
            // free native resources if there are any.
            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
        }
        
        #endregion

    }//end
}//end
