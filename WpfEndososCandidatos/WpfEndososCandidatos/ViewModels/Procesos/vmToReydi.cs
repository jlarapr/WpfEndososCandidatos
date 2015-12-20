using jolcode;
using jolcode.Base;
using jolcode.MyInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfEndososCandidatos.Models;
using WpfEndososCandidatos.View.Procesos;
using System.Data;

namespace WpfEndososCandidatos.ViewModels.Procesos
{
    class vmToReydi : ViewModelBase<IDialogViewListBox>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private string _DBEndososCnnStr;
        private string _DBMasterCeeCnnStr;
        private string _DBCeeMasterImgCnnStr;
        private string _DBRadicacionesCEECnnStr;
        private string _SysUser;
        private Brush _BorderBrush;
        private ObservableCollection<LotsToReydi> _lsAllLots;
        private LotsToReydi _lsAllLots_Item;
        private int _lsAllLots_Items_Item_Id;
        private ObservableCollection<LotsToReydi> _lsValidLots;
        private LotsToReydi _lsValidLots_Item;
        private bool _IsEnabled_CmdAddAllLot;
        private bool _IsEnabled_CmdAddLot;
        private bool _IsEnabled_CmdRemoveLot;
        private bool _IsEnabled_CmdRemoveAllLot;
        private DataTable _MyLotsTable;
        private string _Partido;

        public vmToReydi() : base (new wpfToReydi())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdAddAllLot_Click = new RelayCommand(param => MyCmdAddAllLot_Click());
            cmdAddLot_Click = new RelayCommand(param => MyCmdAddLot_Click());
            cmdRemoveLot_Click = new RelayCommand(param => MyCmdRemoveLot_Click());
            cmdRemoveAllLot_Click = new RelayCommand(param => MyCmdRemoveAllLot_Click());
            cmdSendToReydi_Click = new RelayCommand(param => MycmdSendToReydi_Click(), param => canSenToReydi);

            lsAllLots = new ObservableCollection<LotsToReydi>();
            lsValidLots = new ObservableCollection<LotsToReydi>();
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
        public string DBMasterCeeCnnStr
        {
            get
            {
                return _DBMasterCeeCnnStr;
            }
            set
            {
                _DBMasterCeeCnnStr = value;
            }
        }
        public string DBCeeMasterImgCnnStr
        {
            get
            {
                return _DBCeeMasterImgCnnStr;
            }
            set
            {
                _DBCeeMasterImgCnnStr = value;
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
            }
            set
            {
                _SysUser = value;
            }
        }
        public ObservableCollection<LotsToReydi> lsAllLots
        {
            get
            {
                return _lsAllLots;
            }
            set
            {
                _lsAllLots = value;
                this.RaisePropertychanged("lsAllLots");
            }
        }
        public LotsToReydi lsAllLots_Item
        {
            get
            {
                return _lsAllLots_Item;
            }
            set
            {
                _lsAllLots_Item = value;
                this.RaisePropertychanged("lsAllLots_Item");
            }
        }
        public int lsAllLots_Item_Item_Id
        {
            get
            {
                return _lsAllLots_Items_Item_Id;
            }
            set
            {
                _lsAllLots_Items_Item_Id = value;
                this.RaisePropertychanged("lsAllLots_Item");
            }
        }
        public ObservableCollection<LotsToReydi> lsValidLots
        {
            get
            {
                return _lsValidLots;
            }
            set
            {
                _lsValidLots = value;
                this.RaisePropertychanged("lsValidLots");
            }
        }
        public LotsToReydi lsValidLots_Item
        {
            get
            {
                return _lsValidLots_Item;
            }
            set
            {
                _lsValidLots_Item = value;
                this.RaisePropertychanged("lsValidLots_Item");
            }
        }


        public bool IsEnabled_CmdAddAllLot
        {
            get
            {
                return _IsEnabled_CmdAddAllLot;
            }
            set
            {
                _IsEnabled_CmdAddAllLot = value;
                this.RaisePropertychanged("IsEnabled_CmdAddAllLot");
            }
        }
        public bool IsEnabled_CmdAddLot
        {
            get
            {
                return _IsEnabled_CmdAddLot;
            }
            set
            {
                _IsEnabled_CmdAddLot = value;
                this.RaisePropertychanged("IsEnabled_CmdAddLot");
            }
        }
        public bool IsEnabled_CmdRemoveLot
        {
            get
            {
                return _IsEnabled_CmdRemoveLot;
            }
            set
            {
                _IsEnabled_CmdRemoveLot = value;
                this.RaisePropertychanged("IsEnabled_CmdRemoveLot");
            }
        }
        public bool IsEnabled_CmdRemoveAllLot
        {
            get
            {
                return _IsEnabled_CmdRemoveAllLot;
            }
            set
            {
                _IsEnabled_CmdRemoveAllLot = value;
                this.RaisePropertychanged("IsEnabled_CmdRemoveAllLot");
            }
        }
        private bool canSenToReydi
        {
            get
            {
                if (lsValidLots.Count > 0)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region #MyCmd
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

                lsAllLots = new ObservableCollection<LotsToReydi>();
                lsValidLots = new ObservableCollection<LotsToReydi>();

                IsEnabled_CmdAddAllLot = true;
                IsEnabled_CmdAddLot = true;
                IsEnabled_CmdRemoveLot = true;
                IsEnabled_CmdRemoveAllLot = true;

                MyReset();



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
        private void MyCmdRemoveLot_Click()
        {
            try
            {
                while (this.View.lsValid.SelectedItems.Count > 0)
                {
                    lsAllLots.Add(lsValidLots_Item);
                    lsValidLots.Remove(lsValidLots_Item);
                }
                lsAllLots.Sort();
            }
            catch
            {
                throw;
            }
        }
        private void MyCmdAddAllLot_Click()
        {
            try
            {
                var myCopy = new ObservableCollection<LotsToReydi>(this.lsAllLots);
                foreach (var item in myCopy)
                {
                    this.lsValidLots.Add(item);
                    this.lsAllLots.Remove(item);
                }
                lsValidLots.Sort();

            }
            catch (Exception)
            {
                throw;
            }
        }
        private void MyCmdRemoveAllLot_Click()
        {
            try
            {
                var myCopy = new ObservableCollection<LotsToReydi>(this.lsValidLots);
                foreach (var item in myCopy)
                {
                    this.lsAllLots.Add(item);
                    this.lsValidLots.Remove(item);
                }
                lsAllLots.Sort();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void MyCmdAddLot_Click()
        {
            try
            {
                if (this.View.lsAll != null)
                {
                    while (this.View.lsAll.SelectedItems.Count > 0)
                    {
                        this.lsValidLots.Add(lsAllLots_Item);
                        this.lsAllLots.Remove(lsAllLots_Item);
                    }
                    lsValidLots.Sort();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void MycmdSendToReydi_Click()
        {
            try
            {
                var response = MessageBox.Show("!!!Esta Acción es Irreversible  Desea Continuar ?", "Send Lot to Reydi.", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (response == MessageBoxResult.Yes)
                {
                    using (SqlExcuteCommand send = new SqlExcuteCommand()
                    {
                        DBCnnStr = DBEndososCnnStr,
                        DBCeeMasterCnnStr = DBMasterCeeCnnStr,
                        DBRadicacionesCEECnnStr = DBRadicacionesCEECnnStr,
                        DBImagenesCnnStr = DBCeeMasterImgCnnStr
                    })
                    {
                        if (!send.MySendToReydi(this.lsValidLots, SysUser, _Partido))
                        {
                            throw new Exception("Hay Problemas enviando la data a REYDI!!!");
                        }
                        else
                        {
                            MessageBox.Show("Done...", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }finally
            {
                MyReset();
            }
        }

        public RelayCommand cmdAddAllLot_Click { get; private set; }
        public RelayCommand cmdAddLot_Click { get; private set; }
        public RelayCommand cmdRemoveLot_Click { get; private set; }
        public RelayCommand cmdRemoveAllLot_Click { get; private set; }
        public RelayCommand initWindow
        {
            get;
            private set;
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
        public RelayCommand cmdSalir_Click
        {
            get;private set;
        }
        public RelayCommand cmdSendToReydi_Click { get; private set; }
        #endregion


        #region MyMetodos
        private void MyReset()
        {
            using (SqlExcuteCommand get = new SqlExcuteCommand()
            {
                DBCnnStr = DBEndososCnnStr,
                DBCeeMasterCnnStr = DBMasterCeeCnnStr,
                DBRadicacionesCEECnnStr = DBRadicacionesCEECnnStr,
                DBImagenesCnnStr = DBCeeMasterImgCnnStr
            })
            {
                _MyLotsTable = get.MyGetLot("2,3,4");

                this.View.lsAll.ItemsSource = lsAllLots;
                this.View.lsValid.ItemsSource = lsValidLots;

                MySetAllLots(_MyLotsTable);
            }
        }
        private bool MySetAllLots(DataTable Table)
        {
            bool myBoolOut = false;
            try
            {
                lsAllLots.Clear();
                lsValidLots.Clear();

                foreach (DataRow row in Table.Rows)
                {
                    string myLot = row["Lot"].ToString();
                    LotsToReydi item = new LotsToReydi();
                    _Partido = row["Partido"].ToString();
                    item.Lot = myLot;
                    item.EndorsementGroupCode = myLot;
                    item.ValidatedEndorsements = (int)row["ValidatedEndorsements"];
                    item.RejectedEndorsements = (int)row["RejectedEndorsements"];
                    item.EndorsementValidationDate = row["VerDate"].ToString();
                    item.Num_Candidato = row["Num_Candidato"].ToString();


                   lsAllLots.Add(item);
                }
                lsAllLots.Sort();
                myBoolOut = true;
            }
            catch (Exception)
            {
                throw;
            }
            return myBoolOut;
        }
        #endregion















        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmToReydi()
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
