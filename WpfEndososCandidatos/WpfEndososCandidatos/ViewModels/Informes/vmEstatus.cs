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
        private ObservableCollection<infoEstatus> _ItemsSource;
        private DataTable _MyEstatusTable;
        //private ObservableCollection<Partidos> _infoLot;
        private string _txtTotal;
        private bool _isPartidos;

        public vmEstatus() : base(new wpfEstatus())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdRefresh_Click = new RelayCommand(param => MyCmdRefresh_Click());
            cmdExecute_Click = new RelayCommand(param => MycmdExecute_Click());
            cmdToExcel_Click = new RelayCommand(param => MycmdToExcel_Click());
            ItemsSource = new ObservableCollection<infoEstatus>();
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

            }
            finally
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

                    //if (cbPartido_Item_Id > 1)
                    //    return;

                    String mpartido = isPartidos == true ? cbPartido[cbPartido_Item_Id].PartidoKey : "";

                    _MyEstatusTable = get.MyGetInfoEstatus(mpartido, isPartidos);
                    int Endosos_Requeridos = 0;
                    int Endosos_Aceptados = 0;
                    int Endosos_Rechazados = 0;
                    int Endosos_Procesados = 0;
                    int Endosos_Entregados = 0;
                    txtTotal = string.Format("{0:N0}", _MyEstatusTable.Rows.Count);


                    if (!isPartidos)
                    {
                        int count = 0;
                        String mPartido = String.Empty;
                        String mTitulo = String.Empty;
                        infoEstatus myinfoEstatus = new infoEstatus();
                        bool misStart = true;

                        foreach (DataRow d in _MyEstatusTable.Rows)
                        {
                            
                            mTitulo = d["Titulo"].ToString();
                           if (mpartido != d["partido"].ToString().Trim())
                            {
                                if (!misStart)
                                {
                                    myinfoEstatus.Partido = mpartido;
                                    ItemsSource.Add(myinfoEstatus);
                                }
                                count++;
                                mpartido = d["partido"].ToString().Trim();
                                myinfoEstatus = new infoEstatus();
                                misStart = false;
                            }

                           switch (mTitulo)
                            {
                                case "Endosos_Requeridos":
                                    {
                                        int.TryParse(d["Total"].ToString().Trim(), out Endosos_Requeridos);
                                        myinfoEstatus.Endosos_Requeridos = Endosos_Requeridos;
                                    }
                                    break;
                                case "Endosos_Aceptados":
                                    {
                                        int.TryParse(d["Total"].ToString().Trim(), out Endosos_Aceptados);
                                        myinfoEstatus.Endosos_Aceptados = Endosos_Aceptados;
                                    }
                                    break;
                                case "Endosos_Rechazados":
                                    {
                                        int.TryParse(d["Total"].ToString().Trim(), out Endosos_Rechazados);
                                        myinfoEstatus.Endosos_Rechazados = Endosos_Rechazados;
                                    }
                                    break;
                                case "Endosos_Procesados":
                                    {
                                        int.TryParse(d["Total"].ToString().Trim(), out Endosos_Procesados);
                                        myinfoEstatus.Endosos_Procesados = Endosos_Procesados;
                                    }
                                    break;
                                case "Endosos_Entregados":
                                    {
                                        int.TryParse(d["Total"].ToString().Trim(), out Endosos_Entregados);
                                        myinfoEstatus.Endosos_Entregados = Endosos_Entregados;
                                    }
                                    break;
                            }
                        }
                        myinfoEstatus.Partido = mpartido;
                        ItemsSource.Add(myinfoEstatus);
                        txtTotal = string.Format("{0:N0}", count);
                    }
                    else
                    {
                        foreach (DataRow d in _MyEstatusTable.Rows)
                        {
                            infoEstatus myinfoEstatus = new infoEstatus();
                            myinfoEstatus.Partido = d["Partido"].ToString();
                            int.TryParse(d["Endosos_Requeridos"].ToString().Trim(), out Endosos_Requeridos);
                            int.TryParse(d["Endosos_Aceptados"].ToString().Trim(), out Endosos_Aceptados);
                            int.TryParse(d["Endosos_Rechazados"].ToString().Trim(), out Endosos_Rechazados);
                            int.TryParse(d["Endosos_Procesados"].ToString().Trim(), out Endosos_Procesados);
                            int.TryParse(d["Endosos_Entregados"].ToString().Trim(), out Endosos_Entregados);
                            myinfoEstatus.Endosos_Requeridos = Endosos_Requeridos;
                            myinfoEstatus.Endosos_Aceptados = Endosos_Aceptados;
                            myinfoEstatus.Endosos_Rechazados = Endosos_Rechazados;
                            myinfoEstatus.Endosos_Procesados = Endosos_Procesados;
                            myinfoEstatus.Endosos_Entregados = Endosos_Entregados;
                            ItemsSource.Add(myinfoEstatus);
                        }
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
                cbPartido.Clear();
                ItemsSource.Clear();
                isPartidos = true;
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

        public bool isPartidos
        {
            get
            {
                return _isPartidos;
            }
            set
            {
                _isPartidos = value;
                this.RaisePropertychanged("isPartidos");
            }
        }

        public ObservableCollection<infoEstatus> ItemsSource
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
