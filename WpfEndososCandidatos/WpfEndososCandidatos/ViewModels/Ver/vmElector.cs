namespace WpfEndososCandidatos.ViewModels.Ver
{
    using jolcode;
    using jolcode.Base;
    using jolcode.MyInterface;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using WpfEndososCandidatos.View;
    class vmElector : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private string _DBCeeMasterCnnStr;
        private Logclass _LogClass;
        private string _TxtElecNum;
        private string _TxtNombre;
        private string _TxtPaterno;
        private string _TxtMaterno;
        private string _TxtPadre;
        private string _TxtMadre;
        private string _TxtDia;
        private string _TxtMes;
        private string _TxtAno;
        private string _TxtPrecinto;
        private string _TxtUnidad;
        private bool _IsChecked_Activo;
        private bool _IsChecked_Inactivo;
        private bool _IsChecked_Excluido;
        private bool _IsChecked_SexF;
        private bool _IsChecked_SexM;
        private BitmapImage _Source_image;
        private BitmapImage _Source_imagePhoto;
        private bool _CanFind;
        private string _DBCeeMasterImgCnnStr;

        public vmElector()
            : base(new wpfElector())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click(), param => CommandCan);
            cmdFind_Click = new RelayCommand(param => MyCmdFind_Click(), param => CanFind);
            _LogClass = new Logclass();
        }


        #region MyProerty
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
        public string TxtElecNum
        {
            get
            {
                return _TxtElecNum;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (value != _TxtElecNum)
                        MyReset();

                    _TxtElecNum = value;

                    this.RaisePropertychanged("TxtElecNum");
                }
                else
                {
                    _TxtElecNum = null;
                    this.RaisePropertychanged("TxtElecNum");
                }

            }
        }
        public string TxtNombre
        {
            get
            {
                return _TxtNombre;
            }
            set
            {
                _TxtNombre = value;
                this.RaisePropertychanged("TxtNombre");
            }
        }
        public string TxtPaterno
        {
            get
            {
                return _TxtPaterno;
            }
            set
            {
                _TxtPaterno = value;
                this.RaisePropertychanged("TxtPaterno");
            }
        }
        public string TxtMaterno
        {
            get
            {
                return _TxtMaterno;
            }
            set
            {
                _TxtMaterno = value;
                this.RaisePropertychanged("TxtMaterno");
            }
        }
        public string TxtPadre
        {
            get
            {
                return _TxtPadre;
            }
            set
            {
                _TxtPadre = value;
                this.RaisePropertychanged("TxtPadre");
            }
        }
        public string TxtMadre
        {
            get
            {
                return _TxtMadre;
            }
            set
            {
                _TxtMadre = value;
                this.RaisePropertychanged("TxtMadre");
            }
        }
        public string TxtDia
        {
            get
            {
                return _TxtDia;
            }
            set
            {
                _TxtDia = value;
                this.RaisePropertychanged("TxtDia");
            }
        }
        public string TxtMes
        {
            get
            {
                return _TxtMes;
            }
            set
            {
                _TxtMes = value;
                this.RaisePropertychanged("TxtMes");
            }
        }
        public string TxtAno
        {
            get
            {
                return _TxtAno;
            }
            set
            {
                _TxtAno = value;
                this.RaisePropertychanged("TxtAno");
            }
        }
        public string TxtPrecinto
        {
            get
            {
                return _TxtPrecinto;
            }
            set
            {
                _TxtPrecinto = value;
                this.RaisePropertychanged("TxtPrecinto");
            }
        }
        public string TxtUnidad
        {
            get
            {
                return _TxtUnidad;
            }
            set
            {
                _TxtUnidad = value;
                this.RaisePropertychanged("TxtUnidad");
            }
        }
        public bool IsChecked_Activo
        {
            get
            {
                return _IsChecked_Activo;
            }
            set
            {
                _IsChecked_Activo = value;
                this.RaisePropertychanged("IsChecked_Activo");
            }
        }
        public bool IsChecked_Inactivo
        {
            get
            {
                return _IsChecked_Inactivo;
            }
            set
            {
                _IsChecked_Inactivo = value;
                this.RaisePropertychanged("IsChecked_Inactivo");
            }
        }
        public bool IsChecked_Excluido
        {
            get
            {
                return _IsChecked_Excluido;
            }
            set
            {
                _IsChecked_Excluido = value;
                this.RaisePropertychanged("IsChecked_Excluido");
            }
        }
        public bool IsChecked_SexM
        {
            get
            {
                return _IsChecked_SexM;
            }
            set
            {
                _IsChecked_SexM = value;
                this.RaisePropertychanged("IsChecked_SexM");
            }
        }
        public bool IsChecked_SexF
        {
            get
            {
                return _IsChecked_SexF;
            }
            set
            {
                _IsChecked_SexF = value;
                this.RaisePropertychanged("IsChecked_SexF");
            }
        }
        public BitmapImage Source_image
        {
            get
            {
                return _Source_image;
            }
            set
            {
                _Source_image = value;
                this.RaisePropertychanged("Source_image");
            }
        }
        public BitmapImage Source_imagePhoto
        {
            get
            {
                return _Source_imagePhoto;
            }
            set
            {
                _Source_imagePhoto = value;
                this.RaisePropertychanged("Source_imagePhoto");
            }
        }

        public bool CanFind
        {
            get
            {

                if (string.IsNullOrEmpty(TxtElecNum))
                {
                    MyReset();
                    return false;
                }

                int i;
                if (!int.TryParse(TxtElecNum, out i))
                {
                    MyReset();
                    return false;
                }

                if (_CanFind)
                    return false;

                return true;// !string.IsNullOrEmpty(TxtElecNum);
            }

        }
        private bool CommandCan
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region MyCmd

        private void MyInitWindow()
        {
            bool mIsWriteLog = false;
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
                _LogClass.SourceName = "VerElector";
                _LogClass.MessageFile = string.Empty;
                _LogClass.CreateEvent();
                _LogClass.MYEventLog.WriteEntry("Var Elector Start:" + Dia + " " + Hora, EventLogEntryType.Information);

                MyReset();
                if (!string.IsNullOrEmpty(TxtElecNum))
                {
                    if (TxtElecNum.Trim().Length > 0)
                        MyCmdFind_Click();
                    else
                        TxtElecNum = string.Empty;
                }
                mIsWriteLog = true;

            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                if (mIsWriteLog)
                    _LogClass.MYEventLog.WriteEntry(ex.ToString() + "\r\n" + site.Name, EventLogEntryType.Error, 9999);

            }
        }
        public bool? MyOnShow()
        {
            return this.View.ShowDialog();
        }
        public void MyCmdFind_Click()
        {
            try
            {
                MyReset();

                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCeeMasterCnnStr = DBCeeMasterCnnStr,
                    DBImagenesCnnStr = DBCeeMasterImgCnnStr
                })
                {
                    DataTable myTable;
                    DataTable myTableImg;

                    if (TxtElecNum.Trim().Length < 7)
                        TxtElecNum = TxtElecNum.Trim().PadLeft(7, '0');

                    _CanFind = true;

                    myTable = get.MyGetCitizen(TxtElecNum);
                    myTableImg = get.MyGetCitizenImg(TxtElecNum);

                    if (myTable.Rows.Count == 0)
                        throw new Exception("Número electoral invalido.");

                    TxtNombre = myTable.Rows[0]["FirstName"].ToString();
                    TxtPaterno = myTable.Rows[0]["LastName1"].ToString();
                    TxtMaterno = myTable.Rows[0]["LastName2"].ToString();

                    TxtPadre = myTable.Rows[0]["FatherName"].ToString();
                    TxtMadre = myTable.Rows[0]["MotherName"].ToString();
                    TxtUnidad = myTable.Rows[0]["SecondGeoCode"].ToString().PadLeft(2, '0');
                    TxtPrecinto = myTable.Rows[0]["FirstGeoCode"].ToString().PadLeft(3, '0');

                    string Fechanac = myTable.Rows[0]["DateOfBirth"].ToString();

                    if (!string.IsNullOrEmpty(Fechanac))
                    {
                        TxtMes = Fechanac.Split('/')[0].Trim().PadLeft(2, '0');
                        TxtDia = Fechanac.Split('/')[1].Trim().PadLeft(2, '0');
                        TxtAno = Fechanac.Split('/')[2].Trim().Substring(0, 4);
                    }

                    switch (myTable.Rows[0]["Status"].ToString().Trim().ToUpper())
                    {
                        case "A":
                            IsChecked_Activo = true;
                            break;
                        case "E":
                            IsChecked_Excluido = true;
                            break;
                        case "I":
                            IsChecked_Inactivo = true;
                            break;
                        default:
                            IsChecked_Activo = false;
                            IsChecked_Excluido = false;
                            IsChecked_Inactivo = false;
                            break;
                    }
                    switch (myTable.Rows[0]["Gender"].ToString().Trim().ToUpper())
                    {
                        case "F":
                            IsChecked_SexF = true;
                            break;
                        case "M":
                            IsChecked_SexM = true;
                            break;
                        default:
                            IsChecked_SexF = false;
                            IsChecked_SexM = false;
                            break;

                    }
                    // 'DESPLIEGA LA FIRMA DEL ELECTOR

                    if (myTableImg.Rows.Count > 0)
                    {
                        byte[] dataSignature = (byte[])myTableImg.Rows[0]["SignatureImage"];
                        MemoryStream strmSignature = new MemoryStream();
                        strmSignature.Write(dataSignature, 0, dataSignature.Length);
                        strmSignature.Position = 0;
                        System.Drawing.Image img = System.Drawing.Image.FromStream(strmSignature);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        Source_image = bi;
                    }
                    else
                        Source_image = null;
                    // 'DESPLIEGA LA Photo DEL ELECTOR
                    //byte[] dataPhoto = (byte[])myTable.Rows[0]["PhotoImage"];

                    //MemoryStream strmPhoto = new MemoryStream();
                    //strmPhoto.Write(dataPhoto, 0, dataPhoto.Length);
                    //strmPhoto.Position = 0;
                    //System.Drawing.Image imgPhoto = System.Drawing.Image.FromStream(strmPhoto);
                    //BitmapImage biPhoto = new BitmapImage();
                    //biPhoto.BeginInit();
                    //MemoryStream msPhoto = new MemoryStream();
                    //imgPhoto.Save(msPhoto, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //msPhoto.Seek(0, SeekOrigin.Begin);
                    //biPhoto.StreamSource = msPhoto;
                    //biPhoto.EndInit();
                    //Source_imagePhoto = biPhoto;

                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);

                try { _LogClass.MYEventLog.WriteEntry(ex.ToString() + "\r\n" + site.Name, EventLogEntryType.Error, 9999); } catch   {      }
            }
            finally
            {

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

        public RelayCommand initWindow
        {
            get;
            private set;
        }
        public RelayCommand cmdFind_Click
        {
            get; private set;
        }
        public RelayCommand cmdSalir_Click
        {
            get;
            private set;
        }
        #endregion


        #region MyModulos
        private void MyReset()
        {
            _CanFind = false;

            TxtNombre = string.Empty;
            TxtPadre = string.Empty;
            TxtPaterno = string.Empty;
            TxtUnidad = string.Empty;
            TxtMaterno = string.Empty;
            TxtMadre = string.Empty;
            TxtDia = string.Empty;
            TxtAno = string.Empty;
            TxtMes = string.Empty;
            TxtPrecinto = string.Empty;

            IsChecked_Activo = false;
            IsChecked_Excluido = false;
            IsChecked_Inactivo = false;
            IsChecked_SexF = false;
            IsChecked_SexM = false;

            Source_imagePhoto = null;
            Source_image = null;

        }
        #endregion



        #region Dispose



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmElector()
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

    }
}
