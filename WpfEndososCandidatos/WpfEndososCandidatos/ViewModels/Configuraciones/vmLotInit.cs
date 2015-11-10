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
using WpfEndososCandidatos.Models;
using WpfEndososCandidatos.View;

namespace WpfEndososCandidatos.ViewModels.Configuraciones
{
    class vmLotInit : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private DataTable _MyLotsTable;
        private string _DBEndososCnnStr;
        private ObservableCollection<string> _cbLots;
        private string _cbLots_Item;
        private int _cbLots_Item_Id;
        private Logclass _LogClass;


        //ObservableCollection
        public vmLotInit()
            : base(new wpfLotInit())
        {
            initWindow = new RelayCommand(param => MyInitWindow(),null);
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click(), param => CommandCan);
            cmdReverse_Click = new RelayCommand(param => MyCmdReverse_Click(),param=>CanCmdReverse);

            _LogClass = new Logclass();
            cbLots = new ObservableCollection<string>();
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
        public string DBEndososCnnStr
        {
            get
            {
                return _DBEndososCnnStr;
            }
            set
            {
                _DBEndososCnnStr = value;
            }
        }
        private bool CommandCan
        {
            get
            {
                return true;
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
            }set
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
            }set
            {
                _cbLots_Item_Id = value;
                this.RaisePropertychanged("cbLots_Item_Id");
            }
        }

        public bool CanCmdReverse
        {
            get
            {
                return !string.IsNullOrEmpty(cbLots_Item);
            }
        }


        #endregion

        #region MyCmd
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
        public bool? MyOnShow()
        {
            return this.View.ShowDialog();
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
                _LogClass.SourceName = "LotIni";
                _LogClass.MessageFile = string.Empty;
                _LogClass.CreateEvent();
                _LogClass.MYEventLog.WriteEntry("LotInt Start:" + Dia + " " + Hora, EventLogEntryType.Information);

                MyReset();

            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                _LogClass.MYEventLog.WriteEntry(ex.ToString() + "\r\n" + site.Name, EventLogEntryType.Error, 9999);

            }
        }
        private void MyCmdReverse_Click()
        {
            try
            {
                var response = MessageBox.Show("!!!Esta Acción es Irreversible " + cbLots_Item + " Desea Continuar ?", "Lots...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (response == MessageBoxResult.Yes)
                {
                    using (SqlExcuteCommand get = new SqlExcuteCommand()
                    {
                        DBCnnStr = DBEndososCnnStr
                    })
                    {
                        if (!string.IsNullOrEmpty(cbLots_Item))
                            if (get.MyInitLot(cbLots_Item,_LogClass))
                            {
                                MessageBox.Show("Done..");
                                MyReset();
                            }else
                            {
                                MessageBox.Show("Error en la base de datos");
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                _LogClass.MYEventLog.WriteEntry(ex.ToString() + "\r\n" + site.Name, EventLogEntryType.Error, 9999);
            }
        }

        public RelayCommand cmdReverse_Click
        {
            get;private set;
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
        #endregion

        #region MyMetodos


        private void MyReset()
        {
            using (SqlExcuteCommand get = new SqlExcuteCommand()
            {
                DBCnnStr = DBEndososCnnStr
            })
            {
                _MyLotsTable = get.MyGetLot();
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
            }
        }

       
    
       #endregion       
      
        #region Dispose
       
      

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmLotInit() 
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

    }
}
