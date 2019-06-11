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
using System.IO;
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
    public class vmEndososRechazados : ViewModelBase<IDialogView>, IDisposable
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
        private DataTable _MyLotsVoidTable;
        private ObservableCollection<infoEndososRechazados> _ItemsSource;
        //private ObservableCollection<Partidos> _infoLot;
        private string _txtTotal;
        private DateTime _dpFchRecibo;
        private infoEndososRechazados _dgSelectedItem;
        private int _dgSelectedIndex;
        private ObservableCollection<String> _filesTmp;
        private bool _isDuplicados;

        public vmEndososRechazados() : base(new wpfEndososRechazados())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdRefresh_Click = new RelayCommand(param => MyCmdRefresh_Click());
            cmdExecute_Click = new RelayCommand(param => MycmdExecute_Click());
            cmdToExcel_Click = new RelayCommand(param => MycmdToExcel_Click());
            DoubleClickCommand = new RelayCommand(param => MyDoubleClickCommand());
            ItemsSource = new ObservableCollection<infoEndososRechazados>(); //para cambiar
            cbPartido = new ObservableCollection<Partidos>();
            _filesTmp = new ObservableCollection<string>();
        }

        #region cmd

        public RelayCommand initWindow { get; private set; }
        public RelayCommand cmdSalir_Click { get; private set; }
        public RelayCommand cmdRefresh_Click { get; private set; }
        public RelayCommand cmdExecute_Click { get; private set; }
        public RelayCommand cmdToExcel_Click { get; private set; }
        public RelayCommand DoubleClickCommand { get; private set; }

        private void MyDoubleClickCommand()
        {
            try
            {
                if (System.DBNull.Value.Equals(dgSelectedIndex))
                    return;

                if (dgSelectedIndex < 1)
                    return;

                infoEndososRechazados  drv = dgSelectedItem;
                

                if (!System.DBNull.Value.Equals(drv.EndosoImage))
                {
                    Byte[] tif = (Byte[])drv.EndosoImage;

                    String numelec = Guid.NewGuid().ToString(); // drv.NumElec.Trim();

                    if (!System.IO.Directory.Exists("c:\\SFTEMP\\"))
                        Directory.CreateDirectory("c:\\SFTEMP\\");

                    String tiffName = "c:\\SFTEMP\\" + numelec + ".tif";

                    _filesTmp.Add(tiffName);
                    
                    File.WriteAllBytes(tiffName, tif);
                    System.Diagnostics.Process process = new System.Diagnostics.Process();

                    process.StartInfo.FileName = tiffName;
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MycmdToExcel_Click()
        {
            try
            {
                jolcode.Code.AplicarEfecto(View as Window);
                jolcode.Code.DoEvents();

                using (jolcode.ToExcel excel = new ToExcel())
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Excel Files|*.xlsx|All Files|*.*";

                    if (sfd.ShowDialog() == true)
                    {
                        excel.TableToExcel(sfd.FileName, ItemsSource);
                        MessageBox.Show("Done!!!", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);

            }finally
            {
                jolcode.Code.QuitarEfecto(View as Window);

            }
        }

        private void MyCmdRefresh_Click()
        {
            MyRefresh();
        }

        private void MycmdExecute_Click()
        {
            try
            {
                ItemsSource.Clear();
                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    if (cbPartido_Item_Id > 1)
                        return;

                    String mpartido = cbPartido[cbPartido_Item_Id].PartidoKey;   // cbPartido_Item.Split('-');

                    _MyLotsVoidTable = get.MyGetLotVoid(mpartido.ToString().Trim(), dpFchRecibo.ToString("MM/dd/yyyy"),isDuplicados);
                    txtTotal = string.Format("{0:N0}", _MyLotsVoidTable.Rows.Count);

                    foreach (DataRow r in _MyLotsVoidTable.Rows)
                    {
                        infoEndososRechazados myLotsVoid = new infoEndososRechazados();
                        myLotsVoid.Partido = r["Partido"].ToString().Trim();
                        myLotsVoid.Lot = r["Lot"].ToString().Trim();
                        myLotsVoid.Batch = r["Batch"].ToString().Trim();
                        myLotsVoid.Formulario = Convert.ToInt32(r["Formulario"].ToString().Trim());
                        myLotsVoid.Rechazo = r["Rechazo"].ToString().Trim();
                        myLotsVoid.Causal = r["Causal"].ToString().Trim();
                        myLotsVoid.NumElec = r["NumElec"].ToString().Trim();
                        myLotsVoid.Status = Convert.ToInt32(r["Status"].ToString().Trim());
                        myLotsVoid.EndosoImage = (Byte[])r["EndosoImage"];
                        myLotsVoid.Fecha_Endoso = r["Fecha_Endoso"].ToString().Trim();
                        ItemsSource.Add(myLotsVoid);
                    }
                }
                MessageBox.Show("Done...", "Done", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

        private void MyCmdSalir_Click()
        {
            try
            {
                foreach(string temp in _filesTmp)
                {
                    try
                    {
                        if (File.Exists(temp))
                            File.Delete(temp);
                    } catch (Exception)  {  }
                }
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

                dpFchRecibo = DateTime.Now;


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

                cbPartido.Clear();
                ItemsSource.Clear();
                txtTotal = "0";
                isDuplicados = false;
                cbPartido_Item = string.Empty;

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
                cbPartido_Item_Id = -1;


            }
        }
        #endregion

        #region property 

        public bool isDuplicados
        {
            get
            {
                return _isDuplicados;
            }set
            {
                _isDuplicados = value;
                this.RaisePropertychanged("isDuplicados");
            }
        }

        public int dgSelectedIndex
        {
            get
            {
                return _dgSelectedIndex;
            }
            set
            {
                _dgSelectedIndex = value;
                this.RaisePropertychanged("dgSelectedIndex");
            }
        }
        public infoEndososRechazados dgSelectedItem
        {
            get
            {
                return _dgSelectedItem;
            }
            set
            {
                _dgSelectedItem = value;
                this.RaisePropertychanged("dgSelectedItem");
            }
        }

        public DateTime dpFchRecibo
        {
            get
            {
                return _dpFchRecibo;
            }
            set
            {
                _dpFchRecibo = value;
                this.RaisePropertychanged("dpFchRecibo");
            }
        }

        public ObservableCollection<infoEndososRechazados> ItemsSource
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
        ~vmEndososRechazados()
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
