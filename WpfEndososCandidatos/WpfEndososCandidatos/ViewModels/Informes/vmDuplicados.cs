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
    public class vmDuplicados: ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private string _DBEndososCnnStr;
        private string _DBMasterCeeCnnStr;
        private string _DBCeeMasterImgCnnStr;
        private string _StatusReydi;
        private ObservableCollection<string> _cbLots;
        private string _cbLots_Item;
        private int _cbLots_Item_Id;
        private DataTable _MyLotsTable;
        private ObservableCollection<InfoDuplicado> _ItemsSource;
        private ObservableCollection<Lots> _infoLot;
        private string _txtTotal;

        public vmDuplicados() : base(new wpfDuplicados())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdRefresh_Click = new RelayCommand(param => MyCmdRefresh_Click());
            cmdExecute_Click = new RelayCommand(param => MycmdExecute_Click());
            cmdToExcel_Click = new RelayCommand(param => MycmdToExcel_Click());
            cbLots = new ObservableCollection<string>();
            ItemsSource = new ObservableCollection<InfoDuplicado>();

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
                if (!string.IsNullOrEmpty(value))
                {
                    _cbLots_Item = value;
                    this.RaisePropertychanged("cbLots_Item");
                }
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
                ItemsSource.Clear();
                _cbLots_Item_Id = value;
                this.RaisePropertychanged("cbLots_Item_Id");
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

        public RelayCommand initWindow { get; private set; }
        public RelayCommand cmdSalir_Click { get; private set; }
        public RelayCommand cmdRefresh_Click { get; private set; }
        public RelayCommand cmdExecute_Click { get; private set; }
        public RelayCommand cmdToExcel_Click { get; private set; }
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
       private void MycmdExecute_Click()
        {
            try
            {
                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr,
                    DBCeeMasterCnnStr = DBCeeMasterImgCnnStr
                    //DBRadicacionesCEECnnStr = DBRadicacionesCEECnnStr
                })
                {
                    txtTotal = "0";
                    DataTable T = new DataTable();

                    T = get.MyGetDuplicados(cbLots_Item);
                    txtTotal = string.Format("{0:0,0}", T.Rows.Count);
                    ItemsSource = new ObservableCollection<InfoDuplicado>();

                    SqlConnection myCnnDBEndososValidarDatos = new SqlConnection();
                    myCnnDBEndososValidarDatos.ConnectionString = DBEndososCnnStr;
                    myCnnDBEndososValidarDatos.Open();

                    SqlConnection myCnnDBCeeMaster = new SqlConnection();
                    myCnnDBCeeMaster.ConnectionString = DBMasterCeeCnnStr;
                    myCnnDBCeeMaster.Open();

                    foreach (DataRow row in T.Rows)
                    {
                        int m_Cargo = -1;
                        string m_NumElec = row["NumElec"].ToString();
                        string m_PARTIDO = row["Partido"].ToString();
                        string m_lot = row["lot"].ToString();
                        bool isError = false;
                        try { int.TryParse(row["Cargo"].ToString().Trim(), out m_Cargo); } catch { throw; }
                        object FirstGeoCode = null;

                        string[] sqlstrPrecinto = { "SELECT FirstGeoCode ",
                                                "From [usercid].[Citizen] ",
                                                " Where [CitizenID] = '", FixNum( m_NumElec) , "'"};

                        if (MyValidarDatosScalar(string.Concat(sqlstrPrecinto), out FirstGeoCode, myCnnDBCeeMaster) == null)
                        {
                            FirstGeoCode = "000";
                        }
                        
                        string[] sqlstr = { "SELECT count(*) as total,[NumElec],lot,[Status] ",
                                                "From [LotsEndo]  ",
                                                "Where [NumElec] = '",  FixNum(m_NumElec.Trim()) , "' ",
                                                "and ", "Cargo='",m_Cargo.ToString(), "' ",
                                                "and lot not in ('", m_lot ,"') ",
                                                "group by NumElec,lot,[Status]"};

                        List<string> total = null;
                        
                        if (MyValidarDatos(string.Concat(sqlstr), out total, myCnnDBEndososValidarDatos) != null)
                        {
                            switch (m_Cargo)
                            {
                                case 0:
                                    {
                                        if ((total.Count + 1) >= 1)
                                        {
                                            isError = true;
                                        }
                                    }
                                    break;
                                case 1:// 'Gobernador
                                    {
                                        if ((total.Count + 1) >= 1)
                                        {
                                            isError = true ;
                                        }
                                    }
                                    break;

                                case 2://'Comisionado Residente
                                    {
                                        if ((total.Count + 1) >= 1)
                                        {
                                            isError = true ;
                                        }
                                    }
                                    break;

                                case 3: //'Senador Distrito
                                    {
                                        if ((total.Count + 1) >= 2)
                                        {
                                            isError = true ;
                                        }
                                    }
                                    break;

                                case 4://'Senador Acumulación
                                    {
                                        int permitidos = 11;
                                        if (m_PARTIDO == "PNP")
                                            permitidos = 6;

                                        if (m_PARTIDO == "PPD")
                                            permitidos = 6;

                                        if ((total.Count + 1) >= permitidos)
                                        {
                                            isError = true ;
                                        }
                                    }
                                    break;

                                case 5: //'Representante Distrito
                                    {
                                        if ((total.Count + 1) >= 1)
                                        {
                                            isError = true ;
                                        }
                                    }
                                    break;

                                case 6: //'Representante Acumulación
                                    {
                                        int permitidos = 11;
                                        if (m_PARTIDO == "PNP")
                                            permitidos = 6;

                                        if (m_PARTIDO == "PPD")
                                            permitidos = 6;


                                        if ((total.Count + 1) >= permitidos)
                                        {
                                            isError = true ;
                                        }
                                    }
                                    break;

                                case 7: //'Alcalde
                                    {
                                        if ((total.Count + 1) >= 1)
                                        {
                                            isError = true ;
                                        }
                                    }
                                    break;

                                case 8: //'Legislador Municipales
                                    {//Falta tabala 
                                        object totalPermitidos = null;
                                        string precintoMasterCee = FirstGeoCode.ToString().Trim().PadLeft(3, '0');

                                        string[] sql = {
                                            "Select [Postulados] from [dbo].[tblLegisladoresMunicipalesPostulados] ",
                                            "where [Precinto] like '%",precintoMasterCee,"%'"
                                        };

                                       if ( MyValidarDatosScalar(string.Concat(sqlstr), out totalPermitidos, myCnnDBEndososValidarDatos)!=null)
                                        {
                                            if ((total.Count + 1) >= (int)totalPermitidos)
                                            {
                                                isError = true;
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        isError = true;
                                        break;
                                    }
                            }
                            //  VerDate = DateTimeUtil.MyValidarFechaMMddyy( row["VerDate"].ToString()),
                            //   FinDate = DateTimeUtil.MyValidarFechaMMddyy(row["FinDate"].ToString()),
                            if (isError)
                            {
                                var vd = GetDate(row["lot"].ToString());
                                DateTime VerDate;
                                DateTime? VerDateNull = null;

                                DateTime FinDate;
                                DateTime? FinDateNull = null;

                                string StatusReydi = vd.ToList<Lots>()[0].StatusReydi;


                                if (DateTime.TryParse(vd.ToList<Lots>()[0].VerDate, out VerDate))
                                    VerDateNull = VerDate;

                                if (DateTime.TryParse(vd.ToList<Lots>()[0].FinDate, out FinDate))
                                    FinDateNull = FinDate;
                                foreach (string slot in total)
                                {
                                    ItemsSource.Add(new InfoDuplicado()
                                    {
                                        Lot = row["lot"].ToString(),
                                        Batch = row["Batch"].ToString(),
                                        Formulario = row["Formulario"].ToString(),
                                        NumElec = row["NumElec"].ToString(),
                                        Cargo = m_Cargo.ToString(),
                                        VerDate = VerDateNull,
                                        FinDate = FinDateNull,
                                        StatusReydi = StatusReydi.Trim() == "1" ? "SI" : "NO",
                                        Status = row["Status"].ToString(),
                                        LotDuplicado = slot

                                    });
                                }
                            }
                            //list.Where(x=>x.Title == title)
                            //var listItem = list.Single(i => i.Title == title);
                        }
                    }
                    MessageBox.Show("Done...", "Done", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.ToString(), site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
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
                        //excel.TableToExcel(sfd.SafeFileName, _T);
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
        #endregion

        #region MyModules
        private void MyRefresh()
        {
          
            using (SqlExcuteCommand get = new SqlExcuteCommand()
            {
                DBCnnStr = DBEndososCnnStr
            })
            {
                _MyLotsTable = get.MyGetLot(StatusReydi, "1,2,3,4");
                cbLots.Clear();

                ItemsSource.Clear();
               
                if (_MyLotsTable.Rows.Count == 0)
                    MessageBox.Show("No hay lotes ", "No Hay", MessageBoxButton.OK, MessageBoxImage.Information);
                _infoLot = new ObservableCollection<Lots>();

                foreach (DataRow row in _MyLotsTable.Rows)
                {

                    _infoLot.Add(new Lots()
                    {
                        Partido = row["Partido"].ToString(),
                        Lot = row["Lot"].ToString(),
                        Amount = row["Amount"].ToString(),
                        Usercode = row["Usercode"].ToString(),
                        AuthDate = row["AuthDate"].ToString(),
                        Status = row["Status"].ToString(),
                        VerDate = row["VerDate"].ToString(),
                        VerUser = row["VerUser"].ToString(),
                        FinUser = row["FinUser"].ToString(),
                        FinDate = row["FinDate"].ToString(),
                        RevDate = row["RevDate"].ToString(),
                        RevUser = row["RevUser"].ToString(),
                        conditions = row["conditions"].ToString(),
                        ImportDate = row["ImportDate"].ToString(),
                        StatusReydi = row["StatusReydi"].ToString(),

                    });
                    Lots myLots = new Lots();

                    myLots.Lot = row["Lot"].ToString();
                    cbLots.Add(myLots.Lot);

                  
                }



                cbLots_Item_Id = -1;

            }

        }
       
        private IEnumerable<Lots> GetDate (string lot)
        {
            IEnumerable<Lots> query = from radicacion in _infoLot
                        where radicacion.Lot == lot
                        select new Lots()
                        {
                            VerDate = radicacion.VerDate,
                            FinDate = radicacion.FinDate,
                            StatusReydi = radicacion.StatusReydi
                        };

            return query.ToList();
        }
        private string FixNum(string param)
        {
            int FixNum = 0;

            if (int.TryParse(param, out FixNum))
                return param;
            else
                return "0";
        }
        private object MyValidarDatos(string sql, out List<string> returnValue, SqlConnection cnn)
        {

            returnValue = null;

            using (SqlCommand cmd = new SqlCommand(sql, cnn)
            {
                CommandType = CommandType.Text,

            })
            {
                try
                {
                    SqlDataAdapter dr = new SqlDataAdapter();
                    dr.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    List<string> mylist = new List<string>();
                    dr.Fill(ds);
                    
                    foreach(DataRow row in ds.Tables[0].Rows)
                    {
                        mylist.Add(row["lot"].ToString() + " Status: " + row["Status"].ToString());
                    }

                    returnValue = mylist;

                }
                catch (Exception ex)
                {
                    throw new Exception("Error " + sql + "\r\n" + ex.ToString());
                }
            }

            return returnValue;
        }
        private object MyValidarDatosScalar(string sql, out object returnValue, SqlConnection cnn)
        {

            returnValue = null;

            using (SqlCommand cmd = new SqlCommand(sql, cnn)
            {
                CommandType = CommandType.Text,

            })
            {
                try
                {
                    returnValue = cmd.ExecuteScalar();
                }
                catch
                {
                    throw new Exception("Error " + sql);
                }
            }

            return returnValue;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmDuplicados()
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
