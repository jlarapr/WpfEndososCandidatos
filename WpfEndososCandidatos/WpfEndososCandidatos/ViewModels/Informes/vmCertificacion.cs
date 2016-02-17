using jolcode;
using jolcode.Base;
using jolcode.MyInterface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using System.Data;
using System.IO;
using CrystalDecisions.Shared;
using System.Globalization;

namespace WpfEndososCandidatos.ViewModels.Informes
{
    class vmCertificacion : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private string _DBMasterCeeCnnStr;
        private string _DBEndososCnnStr;
        private string _DBCeeMasterImgCnnStr;
        private string _PDFPath;
        private string _txtHUpDerecha;
        private string _txtFecha;
        private string _txtInfoSecretario;
        private string _txtInfoComisionado;
        private string _txtP1Body;
        private string _txtP2Body;
        private string _txtInfoDirectorValidaciones;
        private string _txtDireccionPostal;
        private string _txtTelefono;
        private string _txtLogo;
        private string _Nombre;
        private string _Cargo;
        private int _Total;
        private string _DBRadicacionesCEECnnStr;
        private string _NumCandidato;
        private string _Partido;
        private string _txtNombre;
        private string _Area;
        private string _txtCargo;
        private string _txtColor;
        private string _PageHeader;
        private byte _Template;
        private string _txtFirma;

        public vmCertificacion() : base (new View.Informes.wpfCertificacion())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdRefresh_Click = new RelayCommand(param => MyCmdRefresh_Click());
            btnSave = new RelayCommand(param => MybtnSave());
            btnLogo = new RelayCommand(param => MybtnLogo());
            btnPrint = new RelayCommand(param => MybtnPrint());
            btnFirma = new RelayCommand(para => MybtnFirma());
        }
        public RelayCommand cmdSalir_Click { get; private set; }
        public RelayCommand initWindow { get; private set; }
        public RelayCommand cmdRefresh_Click { get; private set; }
        public RelayCommand btnSave { get; private set; }
        public RelayCommand btnLogo { get; private set; }
        public RelayCommand btnPrint { get; private set; }
        public RelayCommand btnFirma { get; private set; }

        public byte Template
        {
            get
            {
                return _Template;
            }set
            {
                if (value > 0)
                    _Template = value;
                else
                    _Template = 1;

                this.RaisePropertychanged("Template");
            }
        }
        public string Partido
        {
            get
            {
               return _Partido;
            }set
            {
                _Partido = value;
            }
        }
        public string NumCandidato
        {
            get
            {
                return _NumCandidato;
            }set
            {
                _NumCandidato = value;
            }
        }
        public string txtCargo
        {
            get
            {
                return _txtCargo;
            }set
            {
                _txtCargo = value;
                this.RaisePropertychanged("txtCargo");
            }
        }
        public string txtPageHeader
        {
            get
            {
                return _PageHeader;
            }set
            {
                _PageHeader = value;
                this.RaisePropertychanged("txtPageHeader");
            }
        }
        public string txtNombre
        {
            get
            {
                return _txtNombre;
            }set
            {
                _txtNombre = value;
                this.RaisePropertychanged("txtNombre");
            }
        }
        public string Nombre
        {
            get
            {
                return _Nombre;
            }
            set
            {
                _Nombre = value;
            }
        }
        public string Area
        {
            get
            {
                return _Area;
            }set
            {
                _Area = value;
            }
        }
        public string Cargo
        {
            get
            {
                return _Cargo;
            }set
            {
                _Cargo = value;
            }
        }
        public int Total
        {
            get
            {
                return _Total;
            }set
            {
                _Total = value;
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
        public string PDFPath
        {
            get
            {
                return _PDFPath;
            }
            set
            {
                _PDFPath = value;
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
        public string txtHUpDerecha
        {
            get
            {
                return _txtHUpDerecha;
            }set
            {
                _txtHUpDerecha = value;
                this.RaisePropertychanged("txtHUpDerecha");
            }
        }
        public string txtFecha
        {
            get
            {
                return _txtFecha;
            }set
            {
                _txtFecha = value;
                this.RaisePropertychanged("txtFecha");
                
            }
        }
        public string txtInfoSecretario
        {
            get
            {
                return _txtInfoSecretario;
            }set
            {
                _txtInfoSecretario = value;
                this.RaisePropertychanged("txtInfoSecretario");
            }
        }
        public string txtInfoComisionado
        {
            get
            {
                return _txtInfoComisionado;
            }set
            {
                _txtInfoComisionado = value;
                this.RaisePropertychanged("txtInfoComisionado");
            }
        }
        public string txtP1Body
        {
            get
            {
                return _txtP1Body;
            }set
            {
                _txtP1Body = value;
                this.RaisePropertychanged("txtP1Body");
            }
        }
        public string txtP2Body
        {
            get
            {
                return _txtP2Body;
            }
            set
            {
                _txtP2Body = value;
                this.RaisePropertychanged("txtP2Body");
            }
        }
        public string txtInfoDirectorValidaciones
        {
            get
            {
                return _txtInfoDirectorValidaciones;
            }set
            {
                _txtInfoDirectorValidaciones = value;
                this.RaisePropertychanged("txtInfoDirectorValidaciones");
            }
        }
        public string txtDireccionPostal
        {
            get
            {
                return _txtDireccionPostal;
            }set
            {
                _txtDireccionPostal = value;
                this.RaisePropertychanged("txtDireccionPostal");
            }
        }
        public string txtTelefono
        {
            get
            {
                return _txtTelefono;
            }set
            {
                _txtTelefono = value;
                this.RaisePropertychanged("txtTelefono");
            }
        }
        public string txtColor
        {
            get
            {
                return _txtColor;
            }set
            {
                _txtColor = value;
                this.RaisePropertychanged("txtColor");
            }
        }
        public string txtLogo
        {
            get
            {
                return _txtLogo;                      
            }set
            {
                _txtLogo = value;
                this.RaisePropertychanged("txtLogo");
            }
        }
        public string txtFirma
        {
            get
            {
                return _txtFirma;
            }set
            {
                _txtFirma = value;
                this.RaisePropertychanged("txtFirma");
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
        private void MybtnLogo()
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "Logo |*.jpg|All Files|*.*";
                bool? result;

                result = op.ShowDialog();

                if ((bool)result)
                {
                    txtLogo = op.FileName;
                }

            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MybtnFirma()
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "Firma |*.jpg|All Files|*.*";
                bool? result;

                result = op.ShowDialog();

                if ((bool)result)
                {
                    txtFirma = op.FileName;
                }

            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void MybtnSave()
        {
            try
            {
                using(jolcode.SqlExcuteCommand insert = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    insert.MyInsertBodyCertificacion
                        (
                           txtHUpDerecha,
                           txtFecha,
                           txtInfoSecretario,
                           txtInfoComisionado,
                           txtP1Body,
                           txtP2Body,
                           txtInfoDirectorValidaciones,
                           txtDireccionPostal,
                           txtTelefono,txtLogo,txtColor,txtPageHeader, Template,txtFirma
                        );
                }
                MessageBox.Show("Done...", "done", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MyRefresh()
        {
            DateTime dateNow = System.DateTime.Now;
            string mes = string.Empty;

            switch (dateNow.Month)
            {
                case 1:
                    mes = "enero";
                    break;
                case 2:
                    mes = "febrero";
                    break;
                case 3:
                    mes = "marzo";
                    break;
                case 4:
                    mes = "abril";
                    break;
                case 5:
                    mes = "mayo";
                    break;
                case 6:
                    mes = "junio";
                    break;
                case 7:
                    mes = "julio";
                    break;
                case 8:
                    mes = "agosto";
                    break;
                case 9:
                    mes = "septiembre";
                    break;
                case 10:
                    mes = "octubre";
                    break;
                case 11:
                    mes = "noviembre";
                    break;
                case 12:
                    mes = "diciembre";
                    break;
            }

            txtFecha = dateNow.Day.ToString() + " de " + mes + " de " + dateNow.Year.ToString("####");

            string[] data = Nombre.Split('-');
            Partido = data[0].Trim();
            NumCandidato = data[1].Trim();

            string[] dataReydi; 
            using (SqlExcuteCommand get = new SqlExcuteCommand()
            {
                DBRadicacionesCEECnnStr = DBRadicacionesCEECnnStr
            })
            {
                dataReydi =
                get.MyGetTotalReq(NumCandidato);

            }

            int total = 0;

            int.TryParse(dataReydi[0].Trim(), out total);
            Total = total;

            Nombre = dataReydi[1].Trim();                // data[2].Trim();

            if (dataReydi[2].Trim().Length > 0)
                Nombre += " " + dataReydi[2].Trim();

            if (dataReydi[3].Trim().Length > 0)
            {
                if (dataReydi[3].Trim().Length ==1) 
                    Nombre += " " + dataReydi[3].Trim() + ".";
                else
                    Nombre += " " + dataReydi[3].Trim();

            }

            if (dataReydi[4].Trim().Length > 0)
                Nombre += " " + dataReydi[4].Trim();



            Cargo = data[4].Trim();
            txtNombre = Nombre.ToUpper(); // ConvertTo_ProperCase(Nombre);



            switch (Cargo)
            {
                case "1": //isla
                    Cargo = "Gobernador";
                    break;
                case "2"://isla
                    Cargo = "Comisionado Residente";
                    break;
                case "3":
                    {
                        string[] temp = Area.Split('-');
                        //SENADOR DISTRITO 1 SAN JUAN
                        temp[1] = temp[1].Trim().Substring(19);
                        Cargo = "Senador Distrito ";
                        Cargo += temp[1].Trim();
                        break;
                    }
                case "4"://isla
                    Cargo = "Senador por Acumulación ";
                    //Cargo = Area;

                    break;
                case "5":
                    {
                        Cargo = "Representante Distrito ";
                        //5011 - REP. DIST. 11
                        string[] temp = Area.Split('-');
                        temp[1] = temp[1].Replace("REP. DIST. ", "Representante Distrito ");
                        Cargo = temp[1].Trim();
                        break;
                    }
                case "6"://isla
                    Cargo = "Representante por Acumulación ";
                    //Cargo = Area;
                    break;
                case "7":
                    {
                        Cargo = "Alcalde ";
                        string[] temp = Area.Split('-');
                        Cargo += temp[1].Trim();
                        break;
                    }
                case "8":
                    {
                        string[] temp = Area.Split('-');
                        Cargo = "Asambleista ";
                        Cargo += temp[1].Trim();
                        break;
                    }
            }

            txtCargo = Cargo.ToUpper();


            using (SqlExcuteCommand get = new SqlExcuteCommand()
            {
                DBCnnStr = DBEndososCnnStr,
                DBRadicacionesCEECnnStr = DBRadicacionesCEECnnStr
            })
            {
                DataTable t = get.MyGetBodyCertificacion();

                foreach (DataRow row in t.Rows)
                {
                    txtHUpDerecha = row["HupDerecho"].ToString();
                    txtFecha = txtFecha;
                    txtInfoSecretario = row["InfoSecretario"].ToString();
                    txtInfoComisionado = row["InfoComisionado"].ToString();
                    txtP1Body = row["P1Body"].ToString();
                    txtP2Body = row["P2Body"].ToString();
                    txtInfoDirectorValidaciones = row["InfoDirectorValidaciones"].ToString();
                    txtDireccionPostal = row["DireccionPostal"].ToString();
                    txtTelefono = row["Telefono"].ToString();
                    txtLogo = row["LogoPath"].ToString();
                    txtColor = row["Color"].ToString();
                    txtPageHeader = row["PageHeader"].ToString();
                    Template =byte.Parse( row["Template"].ToString());
                    txtFirma = row["FirmaPath"].ToString();
                }
              
            }
        }
        public static string ConvertTo_ProperCase(string text)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            return myTI.ToTitleCase(text.ToLower());
        }
        private void MybtnPrint()
        {
            try
            {
                string saveName = string.Empty;
                SaveFileDialog sd = new SaveFileDialog();
                sd.Filter = "Pdf File|*.pdf";
                bool? respuesta;

                respuesta = sd.ShowDialog();

                if (respuesta.Value)
                {

                    string P1Body = txtP1Body.Replace("<Nombre>", "<b>" +  txtNombre.ToUpper()  + "</b>");
                    P1Body = P1Body.Replace("<Cargo>", "<b>" + txtCargo.ToUpper() + "</b>");
                    P1Body = P1Body.Replace("<fecha>", "<b>" + txtFecha + "</b>");
                    P1Body = P1Body.Replace("<Total>", "<b>" + Total.ToString("###,###,##0") + "</b>");

                    string P2Body = txtP2Body.Replace("<Nombre>", "<b>" + txtNombre.ToUpper() + "</b>");
                    P2Body = P2Body.Replace("<Cargo>", "<b>" + txtCargo.ToUpper() + "</b>");
                    P2Body = P2Body.Replace("<fecha>", "<b>" + txtFecha + "</b>");
                    P2Body = P2Body.Replace("<Total>", "<b>" + Total.ToString("###,###,##0") + "</b>");


                    saveName = sd.FileName;

                    //rpt.mycertificacion objRpt = new rpt.mycertificacion();
                    dynamic objRpt = new rpt.mycertificacion();


                    if (Template == 2)
                        objRpt = new rpt.mycertificacion_ppd();

                    if (System.IO.File.Exists(saveName))
                        System.IO.File.Delete(saveName);

                    rpt.Certificacion ds = new rpt.Certificacion();
                    DataRow RR = ds.Tables[0].NewRow();

                    FileStream stream = new FileStream(txtLogo, FileMode.Open, FileAccess.Read);
                    BinaryReader reader = new BinaryReader(stream);
                    byte[] img = reader.ReadBytes((int)stream.Length);
                    reader.Close();
                    stream.Close();
                    RR["LogoPartido"] = img;

                    stream = new FileStream(txtFirma, FileMode.Open, FileAccess.Read);
                    reader = new BinaryReader(stream);
                    img = reader.ReadBytes((int)stream.Length);
                    reader.Close();
                    stream.Close();

                    RR["FirmaComisionado"] = img;

                    RR["HupDerecha"] = "<span style=color:" + txtColor + ">"+ txtHUpDerecha + "</span>";
                    RR["Fecha"] = txtFecha;
                    RR["InfoSecretario"] = txtInfoSecretario;
                    RR["InfoComisionado"] = txtInfoComisionado;
                    RR["P1Body"] = P1Body ;
                    RR["P2Body"] = P2Body;
                    RR["infoDirectorValidaciones"] = txtInfoDirectorValidaciones;
                    RR["direccionPostal"] = txtDireccionPostal;
                    RR["Telefono"] = txtTelefono;
                    RR["PageHeader"] =  txtPageHeader ;

                    ds.tblCertificacion.Rows.Add(RR);

                    objRpt.SetDataSource(ds);

                    ExportOptions CrExportOptions;
                    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                    PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                    CrDiskFileDestinationOptions.DiskFileName = saveName;
                    CrExportOptions = objRpt.ExportOptions;
                    {
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                    }
                    objRpt.Export();

                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = saveName;
                    process.Start();

                }



            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmCertificacion()
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
