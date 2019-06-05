using jolcode;
using jolcode.Base;
using jolcode.MyInterface;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfEndososCandidatos.Models;
using WpfEndososCandidatos.View.Informes;

namespace WpfEndososCandidatos.ViewModels.Informes
{
    public class vmEstatus : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private string _DBEndososCnnStr;
        private string _DBMasterCeeCnnStr;
        private string _DBCeeMasterImgCnnStr;
        private string _StatusReydi;
        private ObservableCollection<Partidos> _cbPartido;
        private string _cbPartido_Item;
        private int _cbPartido_Item_Id;
        private DataTable _MyPartidoTable;
        private ObservableCollection<InfoDuplicado> _ItemsSource;
        //private ObservableCollection<Partidos> _infoLot;
        private string _txtTotal;


        public vmEstatus() : base(new wpfEstatus())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdRefresh_Click = new RelayCommand(param => MyCmdRefresh_Click());
            cmdExecute_Click = new RelayCommand(param => MycmdExecute_Click());
            cmdToExcel_Click = new RelayCommand(param => MycmdToExcel_Click());
            ItemsSource = new ObservableCollection<InfoDuplicado>();
            cbPartido = new ObservableCollection<Partidos>();

        }

        #region cmd
        public RelayCommand initWindow { get; private set; }
        public RelayCommand cmdSalir_Click { get; private set; }
        public RelayCommand cmdRefresh_Click { get; private set; }
        public RelayCommand cmdExecute_Click { get; private set; }
        public RelayCommand cmdToExcel_Click { get; private set; }



        private void MycmdToExcel_Click()
        {

        }

        private void MyCmdRefresh_Click()
        {

        }

        private void MycmdExecute_Click()
        {

        }

        public bool? MyOnShow()
        {
            return this.View.ShowDialog();
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
        private void MyRefresh()
        {

            using (SqlExcuteCommand get = new SqlExcuteCommand()
            {
                DBCnnStr = DBEndososCnnStr
            })
            {
                _MyPartidoTable = get.MyGetPartidos();

                foreach (DataRow row in _MyPartidoTable.Rows)
                {
                    Partidos mypartido = new Partidos();
                    mypartido.Id = (int)row["Id"];
                    mypartido.PartidoKey = row["Partido"].ToString();
                    mypartido.Desc = row["Desc"].ToString();
                    mypartido.EndoReq = (int)row["EndoReq"];
                    mypartido.Area = row["Area"].ToString();

                    cbPartido.Add(mypartido);
                }
                cbPartido.Sort();

            }
        }
        #endregion

        #region property 

        public ObservableCollection<InfoDuplicado> ItemsSource
        {
            get
            {
                return _ItemsSource;
            }
            set
            {
                _ItemsSource = value;
                this.RaisePropertychanged("ItemsSource");
            }
        }

        public ObservableCollection<Partidos> cbPartido
        {
            get
            {
                return _cbPartido;
            }
            set
            {
                _cbPartido = value;
                this.RaisePropertychanged("cbPartido");
            }
        }

        public string cbPartido_Item
        {
            get
            {
                return _cbPartido_Item;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _cbPartido_Item = value;
                    this.RaisePropertychanged("cbPartido_Item");
                }
            }
        }
        public int cbPartido_Item_Id
        {
            get
            {
                return _cbPartido_Item_Id;
            }
            set
            {
                ItemsSource.Clear();
                _cbPartido_Item_Id = value;
                this.RaisePropertychanged("cbPartido_Item_Id");
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
        public string StatusReydi
        {
            get
            {
                return _StatusReydi;
            }
            set
            {
                _StatusReydi = value;
            }
        }
        public string txtTotal
        {
            get
            {
                return _txtTotal;
            }
            set
            {

                _txtTotal = value;
                this.RaisePropertychanged("txtTotal");
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~vmEstatus()
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
