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
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using WpfEndososCandidatos.View;

    class vmLotAuth : ViewModelBase<IDialogView>,IDisposable 
    {
        private string _numLote;
        private int  _cantidad;
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private string _DBEndososCnnStr;
        private Logclass _LogClass;
        private string _SysUser;
        private ObservableCollection<string> _cbLots;
        private int _cbLots_Item_Id;
        private string _cbLots_Item;
        private string _lblCount;
        private int _cantidadEntregada;

        public vmLotAuth()
            : base(new wpfLotAuth())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdAddLot_Click = new RelayCommand(param => MyCmdAddLot_Click(),param=>CanAddLot);
            cmdAddTodoLot_Click = new RelayCommand(param => MyCmdAddTodoLot_Click());

            _LogClass = new Logclass();
            cbLots = new ObservableCollection<string>();
        }

        #region MyProperty
        public int cantidadEntregada
        {
            get
            {
                return _cantidadEntregada;
            }set
            {
          
                    _cantidadEntregada = value;
                this.RaisePropertychanged("cantidadEntregada");
            }
        }
        public string numLote
        {
            get
            {
                return _numLote;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _numLote = value;
                    this.RaisePropertychanged("numLote");
                }
                    
            }
        }
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
        public int cantidad
        {
            get
            {
                return _cantidad;
            }
            set
            {
                    _cantidad = value;
                    this.RaisePropertychanged("cantidad");
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
        public bool CanAddLot
        {
            get
            {
                return !string.IsNullOrEmpty(numLote);
            }
        }
        public string lblCount
        {
            get
            {
                return _lblCount;
            }set
            {
                _lblCount = "Total de Lotes Disponibles: " + value;
                this.RaisePropertychanged("lblCount");
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

                if (value != null)
                {

                   // string[] tmp = value.Split('-');
                    //numLote = tmp[0].ToString();
                    numLote = value.Trim();
                    //cantidad = 0;
                    //int k = 0;

                    //if (int.TryParse(tmp[1].ToString(), out k))
                    //    cantidad = k;




                    using (SqlExcuteCommand get = new SqlExcuteCommand()
                    {
                        DBCnnStr = DBEndososCnnStr
                    })
                    {
                        cantidadEntregada = get.MyGetCatntidadEntregada(numLote);
                        cantidad = get.MyGetCatntidadDigitalizada(numLote);
                    }

                }
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

        #endregion

        #region Mycmd
        public void MyCmdAddLot_Click()
        {
            try
            {
                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    if (cantidad != cantidadEntregada)
                        throw new Exception("Error en la Cantidad");

                    if (!get.MyChangeTF(get.MyGetSelectLotes(numLote,cantidad.ToString()), SysUser))
                        throw new Exception("Error en la base de datos.");
                    else
                        MessageBox.Show("Done...", "Done.", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                MyReset();
            }
            catch (Exception ex)
            {
                
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void MyCmdAddTodoLot_Click()
        {
            try
            {
                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    
                    if (!get.MyChangeTF(get.MyGetTodosLotes(), SysUser))
                        throw new Exception("Error en la base de datos.");
                    else
                        MessageBox.Show("Done...", "Done.", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                MyReset();

            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                _LogClass.MYEventLog.WriteEntry(ex.ToString() + "\r\n" + site.Name, EventLogEntryType.Error, 9999);
            }

        }
        public void MyCmdSalir_Click()
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
        private void MyInitWindow()
        {
            try
            {
                string Dia = DateTime.Now.ToString("MMM/dd/yyyy");
                string Hora = DateTime.Now.ToString("hh:mm:ss tt");

                string myBorderBrush = ConfigurationManager.AppSettings["BorderBrush"];

                if (myBorderBrush != null && myBorderBrush.Trim().Length > 0)
                {
                    Type t = typeof(Brushes);
                    Brush b = (Brush)t.GetProperty(myBorderBrush).GetValue(null, null);
                    BorderBrush = b;
                }
                else
                    BorderBrush = Brushes.Black;

                _LogClass.LogName = "Applica";
                _LogClass.SourceName = "Autorizar_Lotes";
                _LogClass.MessageFile = string.Empty;
                _LogClass.CreateEvent();
                _LogClass.MYEventLog.WriteEntry("Autorizar_Lotes Start:" + Dia + " " + Hora, EventLogEntryType.Information);

                MyReset();

            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                _LogClass.MYEventLog.WriteEntry(ex.ToString() + "\r\n" + site.Name, EventLogEntryType.Error, 9999);

            }
        }
        public bool? MyOnShow()
        {
            return this.View.ShowDialog();
        }

        public RelayCommand cmdAddTodoLot_Click
        {
            get;
            private set;

        }
        public RelayCommand cmdAddLot_Click
        {
            get;
            private set;
        }
        public RelayCommand cmdSalir_Click
        {
            get;
            private set;
        }
        public RelayCommand initWindow
        {
            get;
            private set;
        }

        #endregion

        #region MyMetodos

        private void MyReset()
        {
         

            using (SqlExcuteCommand get = new SqlExcuteCommand()
            {
                DBCnnStr = DBEndososCnnStr
            })
            {
                DataTable myLotsTable = get.MyGetLotFromTF();
                cbLots.Clear();

                foreach (DataRow row in myLotsTable.Rows)
                {
                    cbLots.Add(row["BatchTrack"].ToString());
                    //cbLots.Add(row["BatchTrack"].ToString() + "-" + row["Amount"].ToString() + "-" + row["Partido"].ToString());
                }

                cbLots_Item_Id = -1;
                numLote = "0";
                cantidad = 0;
                cantidadEntregada = 0;
                lblCount = string.Format("{0:0}", _cbLots.Count);
            }


        }
        #endregion


        #region Dispose



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmLotAuth()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources AnotherResource 
                if (_LogClass != null)
                {
                    _LogClass.Dispose();
                    _LogClass = null;
                }
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
