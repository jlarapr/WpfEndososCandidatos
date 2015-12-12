using CrystalDecisions.Shared;
using jolcode;
using jolcode.Base;
using jolcode.MyInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfEndososCandidatos.Models;
using WpfEndososCandidatos.View.Informes;

namespace WpfEndososCandidatos.ViewModels.Informes
{
    class vmInforme : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private ObservableCollection<string> _cbLots;
        private string _cbLots_Item;
        private int _cbLots_Item_Id;
        private string _DBEndososCnnStr;
        private DataTable _MyLotsTable;
        private string _DBMasterCeeCnnStr;
        private string _SysUser;
        private string _DBCeeMasterImgCnnStr;
        private string _PDFPath;
        private string _txtPDFPath;

        public vmInforme()
           : base(new wpfInformes())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdRefresh_Click = new RelayCommand(param => MyCmdRefresh_Click());
            cmdOpen_Click = new RelayCommand(param => MyCmdOpen_Click(), param => CanOpen);
            cbLots = new ObservableCollection<string>();
        }
        #region MyProperty
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
                _cbLots_Item_Id = value;
                this.RaisePropertychanged("cbLots_Item_Id");
            }
        }

        private bool CanOpen
        {
            get
            {
                return cbLots_Item_Id > -1 ? true : false;
            }
        }
        public string txtPDFPath
        {
            get
            {
                return _txtPDFPath;
            }
            set
            {
                _txtPDFPath = value;
                this.RaisePropertychanged("txtPDFPath");
            }
        }
        public string PDFPath
        {
            get
            {
                return _PDFPath;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (_PDFPath != value)
                    {
                        _PDFPath = value;
                        if (!_PDFPath.EndsWith("\\"))
                            _PDFPath += "\\";
                    }
                }
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

        private void MyCmdOpen_Click()
        {
            try
            {
                rpt.ReporteRechazadas objRp = new WpfEndososCandidatos.rpt.ReporteRechazadas();

                using (SqlExcuteCommand get = new SqlExcuteCommand
                {
                    DBCeeMasterCnnStr = DBMasterCeeCnnStr,
                    DBCnnStr = DBEndososCnnStr
                })
                {

                    DataTable T = get.MyGeRechazadasToInforme(cbLots_Item);
                    int totalDePaginas = T.Rows.Count;

                    int Pagina = 1;
                    string strPath = PDFPath + cbLots_Item;

                    if (Directory.Exists(strPath))
                        Directory.Delete(strPath,true);

                    Directory.CreateDirectory(strPath);



                    if (!strPath.EndsWith("\\"))
                        strPath += "\\";

                    foreach (DataRow R in T.Rows)
                    {
                        MyDataSet ds = new MyDataSet();
                        DataRow RR = ds.Tables[0].NewRow();

                        string lot = R["Lot"].ToString().Trim();
                        string NumElec = R["NumElec"].ToString().Trim();

                        RR["Lot"] = lot;
                        RR["Precinto"] = R["Precinto"];
                        RR["NumeroElec"] =      "Número Electoral / Precinto : "  +  NumElec ;
                        RR["Nombre"] =          "Nombre del Endosante (TF)   : " + string.Concat(R["Nombre"].ToString().Trim()," ", R["Paterno"].ToString().Trim()," ", R["Materno"].ToString().Trim()).Trim();
                        RR["NombreCee"] =       "Nombre del Endosante (CEE)  : " + get.MyCEENameToInforme(NumElec);
                        RR["FuncionarioElec"] = "Funcionario                 : " + R["notario"];
                        RR["CandidatoElec"] =   "Número Candidato            : " + R["Candidato"];
                        RR["CandidatoName"] =   "Nombre Candidato            : " + get.MyCandidatoNameToInforme(R["Candidato"].ToString().Trim());
                        RR["Batch"] =           "Batch                       : " + R["Batch"];
                        RR["Formulario"] =      "Formulario                  : " + R["Formulario"];
                        RR["Cargo"] =           "Cargo                       : " + R["Cargo"].ToString() + "-" + get.MyDecCargoToInforme(R["Cargo"].ToString().Trim()).Trim();
                        
                        RR["Razon"] = get.MyDecRechazoToInforme(  R["Errores"].ToString());
                        RR["totalDePaginas"] = string.Format("{0:#,#}", totalDePaginas);
                        RR["Pagina"] =string.Format("{0:#,#}", Pagina);

                        {// Para la Imagen
                            byte[] EndosoImage = (byte[])R["EndosoImage"];
                            RR["Img"] = EndosoImage;
                        }

                        ds.Rechazadas.Rows.Add(RR);

                        if (!Directory.Exists(strPath))
                             Directory.CreateDirectory(strPath);

                        string pdfName = strPath + Pagina.ToString().PadLeft(2, '0') +"_"+ RR["lot"] + "_Rechazadas.pdf";
                        objRp.SetDataSource(ds);

                        ExportOptions CrExportOptions;
                        DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                        PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                        CrDiskFileDestinationOptions.DiskFileName = pdfName;
                        CrExportOptions = objRp.ExportOptions;
                        {
                            CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                            CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                            CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                            CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        }
                        objRp.Export(); // Export PDF //      
                        Pagina++;
                    }
                    //Merge PDFs
                    {
                        MergeEx myMerge = new MergeEx();
                        myMerge.DestinationFile = strPath;
                        myMerge.setFileName = cbLots_Item + "_Informe de Rechazos"; //pdf name
                        myMerge.setSplit = 1000;
                        String[] FileList = Directory.GetFiles(strPath, "*.pdf"); //_pdfPathOut

                        foreach (string fileName in FileList)
                        {
                            myMerge.AddFile(fileName);
                        }
                        myMerge.Execute();

                        foreach (string fileName in FileList)
                        {
                            System.IO.File.Delete(fileName);
                        }

                        //open pdf
                        String fileNamepdf = strPath + myMerge.setFileName + "_1.pdf";
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = fileNamepdf;
                        process.Start();
                        process.WaitForExit();

                    }

                }
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
            get; private set;
        }
        public RelayCommand cmdOpen_Click
        {
            get; private set;
        }
        #endregion

        #region MyModules
        private void MyRefresh()
        {
            txtPDFPath = string.Empty;
            txtPDFPath = PDFPath;

            using (SqlExcuteCommand get = new SqlExcuteCommand()
            {
                DBCnnStr = DBEndososCnnStr
            })
            {
                _MyLotsTable = get.MyGetLot("0,1,2,3,4");
                cbLots.Clear();

                if (_MyLotsTable.Rows.Count == 0)
                    MessageBox.Show("No hay lotes ", "No Hay", MessageBoxButton.OK, MessageBoxImage.Information);

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
        
        #endregion

        #region Dispose
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmInforme()
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
