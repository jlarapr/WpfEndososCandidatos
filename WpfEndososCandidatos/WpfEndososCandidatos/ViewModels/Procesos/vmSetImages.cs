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
using System.Windows.Controls;
using System.Windows.Media;
using WpfEndososCandidatos.View.Procesos;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using jolcode;
using System.IO;

namespace WpfEndososCandidatos.ViewModels.Procesos
{
    class vmSetImages : ViewModelBase<IDialogView>, IDisposable
    {
        const string _REGPATH = "SOFTWARE\\CEE\\Endosos\\Partidos";
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderColor;
        private wpfSetImages _wpfSetImages;
        private ScrollViewer _sv;
        private Rectangle _SelectionRectangle;
        private int _MyX;
        private int _MyY;
        private double _MyH;
        private double _MyW;
        private string _lblCordenadas;
        private int _ViewboxWidth;
        private int _ViewboxHeight;
        private BitmapImage _srcEndoso = new BitmapImage();
        private ImageSource _Source_image;
        private System.Drawing.Image _imgEndoso;
        private RelayCommand _BtnCrop;
        private RelayCommand _BtnFullImages;
        private string _txtNombre_XY;
        private string _txtNumElec_XY;
        private string _txtPrecinto_XY;
        private string _txtSex_XY;
        private string _txtCargo_XY;
        private string _txtCandidato_XY;
        private string _txtNotarioElec_XY;
        private string _txtFirmaElec_XY;
        private string _txtNotarioFirma_XY;
        private string _txtFchEndoso_XY;
        private string _txtFchEndosoEntregada_XY;
        private string _txtFechaNac_XY;

        public vmSetImages() : base (new wpfSetImages())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdZoomInOut = new RelayCommand(param => MyCmdZoomInOut(param));

            cmdSetNumElec = new RelayCommand(param => MycmdSetNumElec());
            cmdSetPrecinto = new RelayCommand(param => MycmdSetPrecinto());
            cmdSetSexo = new RelayCommand(param => MycmdSetSexo());
            cmdSetFechaNac = new RelayCommand(param => MycmdSetFechaNac());
            cmdSetCargo = new RelayCommand(param => MycmdSetCargo());
            cmdSetCandidato = new RelayCommand(param => MycmdSetCandidato());
            cmdSetFuncionarioElec = new RelayCommand(param => MycmdSetFuncionarioElec());
            cmdSetfirmaElec = new RelayCommand(param => MycmdSetfirmaElec());
            cmdSetFirmaFuncionario = new RelayCommand(param => MycmdSetFirmaFuncionario());
            cmdSetFechaJuramento = new RelayCommand(param => MycmdSetFechaJuramento());
            cmdSetFechaEntrega = new RelayCommand(param => MycmdSetFechaEntrega());
            cmdSetNombre = new RelayCommand(param => MycmdSetNombre());
            cmdSave_Click = new RelayCommand(param => MycmdSave_Click());


        }

      



        /*Property*/
        #region MyProperty
        public string txtNombre_XY
        {
            get
            {
                return _txtNombre_XY;
            }
            set
            {
                _txtNombre_XY = value;
                this.RaisePropertychanged("txtNombre_XY");
            }
 
        }
        public string txtNumElec_XY
        {
            get
            {
                return _txtNumElec_XY;
            }
            set
            {
                _txtNumElec_XY = value;
                this.RaisePropertychanged("txtNumElec_XY");
            }
        }
        public string txtPrecinto_XY
        {
            get
            {
                return _txtPrecinto_XY;
            }
            set
            {
                _txtPrecinto_XY = value;
                this.RaisePropertychanged("txtPrecinto_XY");
            }
        }
        public string txtSex_XY
        {
            get
            {
                return _txtSex_XY;
            }
            set
            {
                _txtSex_XY = value;
                this.RaisePropertychanged("txtSex_XY");
            }
        }
        public string txtCargo_XY
        {
            get
            {
                return _txtCargo_XY;
            }
            set
            {
                _txtCargo_XY = value;
                this.RaisePropertychanged("txtCargo_XY");
            }
        }
        public string txtCandidato_XY
        {
            get
            {
                return _txtCandidato_XY;
            }
            set
            {
                _txtCandidato_XY = value;
                this.RaisePropertychanged("txtCandidato_XY");
            }
        }
        public string txtNotarioElec_XY
        {
            get
            {
                return _txtNotarioElec_XY;
            }
            set
            {
                _txtNotarioElec_XY = value;
                this.RaisePropertychanged("txtNotarioElec_XY");
            }
        }
        public string txtFirmaElec_XY
        {
            get
            {
                return _txtFirmaElec_XY;
            }
            set
            {
                _txtFirmaElec_XY = value;
                this.RaisePropertychanged("txtFirmaElec_XY");
            }
        }
        public string txtNotarioFirma_XY
        {
            get
            {
                return _txtNotarioFirma_XY;
            }
            set
            {
                _txtNotarioFirma_XY = value;
                this.RaisePropertychanged("txtNotarioFirma_XY");
            }
        }
        public string txtFchEndoso_XY
        {
            get
            {
                return _txtFchEndoso_XY;
            }
            set
            {
                _txtFchEndoso_XY = value;
                this.RaisePropertychanged("txtFchEndoso_XY");
            }
        }
        public string txtFchEndosoEntregada_XY
        {
            get
            {
                return _txtFchEndosoEntregada_XY;

            }
            set
            {
                _txtFchEndosoEntregada_XY = value;
                this.RaisePropertychanged("txtFchEndosoEntregada_XY");
            }
        }
        public string txtFechaNac_XY
        {
            get
            {
                return _txtFechaNac_XY;
            }
            set
            {
                _txtFechaNac_XY = value;
                this.RaisePropertychanged("txtFechaNac_XY");
            }
        }


        public BitmapImage srcEndoso
        {
            get
            {
                return _srcEndoso;
            }
            set
            {
                _srcEndoso = value;
            }
        }
        public System.Drawing.Image imgEndoso
        {
            private get
            {
                return _imgEndoso;
            }
            set
            {
                _imgEndoso = value;

            }
        }
        public Brush BorderColor
        {
            get
            {
                return _BorderColor;
            }
            set
            {
                if (_BorderColor != value)
                {
                    _BorderColor = value;
                    this.RaisePropertychanged("BorderColor");
                }

            }
        }
        public ScrollViewer sv
        {
            get
            {
                return _sv;
            }
            set
            {
                _sv = value;
                this.RaisePropertychanged("sv");
            }
        }
        public string lblCordenadas
        {
            get
            {
                return _lblCordenadas;
            }
            set
            {
                _lblCordenadas = value;
                this.RaisePropertychanged("lblCordenadas");
            }
        }

        public int ViewboxWidth
        {
            get
            {
                return _ViewboxWidth;
            }
            set
            {
                _ViewboxWidth = value;
                this.RaisePropertychanged("ViewboxWidth");
            }
        }
        public int ViewboxHeight
        {
            get
            {
                return _ViewboxHeight;
            }
            set
            {
                _ViewboxHeight = value;
                this.RaisePropertychanged("ViewboxHeight");
            }
        }
        public System.Windows.Media.ImageSource Source_image
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

        #endregion

        /*CMD*/
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
                    BorderColor = b;
                }
                else
                    BorderColor = Brushes.Black;


                txtNombre_XY= MyGetRegister("NombreXY", _REGPATH);
                txtNumElec_XY= MyGetRegister("ElecNumXY", _REGPATH);
                txtPrecinto_XY= MyGetRegister("PrecintoXY", _REGPATH);
                txtSex_XY= MyGetRegister("SexoXY", _REGPATH );
                txtCargo_XY=MyGetRegister("CargoXY", _REGPATH);
                txtCandidato_XY= MyGetRegister("CandidatoXY", _REGPATH);
                txtNotarioElec_XY= MyGetRegister("FuncionarioXY", _REGPATH );
                txtFirmaElec_XY= MyGetRegister("FirmaElecXY", _REGPATH);
                txtNotarioFirma_XY= MyGetRegister("FirmaFuncionarioXY", _REGPATH);
                txtFchEndoso_XY= MyGetRegister("FechaFirmaXY", _REGPATH);
                txtFchEndosoEntregada_XY= MyGetRegister("FechaRadiXY", _REGPATH);
                txtFechaNac_XY= MyGetRegister("FechaNacXY", _REGPATH);

            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool? MyOnShow()
        {
            _wpfSetImages = this.View as wpfSetImages;

            _wpfSetImages.miCanvas.MouseMove += View_MouseMove;
            _wpfSetImages.miCanvas.MouseLeftButtonDown += View_MouseLeftButtonDown;
            sv = _wpfSetImages.scrollViewer;
            _SelectionRectangle = _wpfSetImages.selectionRectangle;

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
        void BtnCrop_Click()
        {
            double factorX, factorY;

            factorX = srcEndoso.PixelWidth / srcEndoso.Width;
            factorY = srcEndoso.PixelHeight / srcEndoso.Height;

            BitmapSource displayImage = new CroppedBitmap(srcEndoso, new Int32Rect(_MyX, _MyY, (int)(_MyW), (int)(_MyH)));  // new CroppedBitmap(original, crop);

            ViewboxWidth = (int)_MyW + 10;
            ViewboxHeight = (int)_MyH + 10;

            Source_image = displayImage;

            _sv.ScrollToVerticalOffset(0);
            _sv.ScrollToHorizontalOffset(0);

        }
        private void BtnFullImages_Click(ref int v, ref int Ho)
        {

            ViewboxWidth = imgEndoso.Width;
            ViewboxHeight = imgEndoso.Height;
            BitmapSource displayImage = new CroppedBitmap(srcEndoso, new Int32Rect(0, 0, 0, 0));  
            Source_image = displayImage;

            _sv.ScrollToVerticalOffset(v);
            _sv.ScrollToHorizontalOffset(Ho);

            _sv.UpdateLayout();

        }
        private bool MyCmdZoomInOut(object param)
        {
            switch (param as string)
            {
                case "IN":
                    {
                        if (ViewboxWidth <= 2000)
                        {
                            ViewboxHeight += 100;
                            ViewboxWidth += 100;
                        }
                        break;
                    }
                case "OUT":
                    {
                        if (ViewboxWidth >= 150)
                        {
                            ViewboxHeight -= 100;
                            ViewboxWidth -= 100;
                        }
                        break;
                    }
            }
            return true;
        }

        private void MycmdSetNombre()
        {
            try
            {
                txtNombre_XY = lblCordenadas;
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MycmdSetFechaEntrega()
        {
            try
            {
                txtFchEndosoEntregada_XY = lblCordenadas;
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MycmdSetFechaJuramento()
        {
            try
            {
                txtFchEndoso_XY = lblCordenadas;
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MycmdSetFirmaFuncionario()
        {
            try
            {
                txtNotarioFirma_XY = lblCordenadas;
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MycmdSetfirmaElec()
        {
            try
            {
                txtFirmaElec_XY = lblCordenadas;
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MycmdSetFuncionarioElec()
        {
            try
            {
                txtNotarioElec_XY = lblCordenadas;
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MycmdSetCandidato()
        {
            try
            {
                txtCandidato_XY = lblCordenadas;
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MycmdSetCargo()
        {
            try
            {
                txtCargo_XY = lblCordenadas;
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MycmdSetFechaNac()
        {
            try
            {
                txtFechaNac_XY = lblCordenadas;
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MycmdSetSexo()
        {
            try
            {
                txtSex_XY = lblCordenadas;
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MycmdSetPrecinto()
        {
            try
            {
                txtPrecinto_XY = lblCordenadas;
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MycmdSetNumElec()
        {
            try
            {
                txtNumElec_XY = lblCordenadas;
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MycmdSave_Click()
        {
            try
            {
                
                string[] strError = new string[0];

                //str = MyGetRegister("NombreXY", REGPATH);
                MyWriteRegister("NombreXY", _REGPATH, txtNombre_XY);
                MyWriteRegister("ElecNumXY", _REGPATH, txtNumElec_XY);
                MyWriteRegister("PrecintoXY", _REGPATH, txtPrecinto_XY);
                MyWriteRegister("SexoXY", _REGPATH, txtSex_XY);
                MyWriteRegister("CargoXY", _REGPATH, txtCargo_XY);
                MyWriteRegister("CandidatoXY", _REGPATH, txtCandidato_XY);
                MyWriteRegister("FuncionarioXY", _REGPATH, txtNotarioElec_XY);
                MyWriteRegister("FirmaElecXY", _REGPATH, txtFirmaElec_XY);
                MyWriteRegister("FirmaFuncionarioXY", _REGPATH, txtNotarioFirma_XY);
                MyWriteRegister("FechaFirmaXY", _REGPATH, txtFchEndoso_XY);
                MyWriteRegister("FechaRadiXY", _REGPATH, txtFchEndosoEntregada_XY);
                MyWriteRegister("FechaNacXY", _REGPATH, txtFechaNac_XY);


                using (ValidateItDynamicCode objValidateItDynamicCode = new ValidateItDynamicCode())
                {
                    //FileStream fileStream = new FileStream(ValidateItDynamicCode.ValidateThis("txtNombre_XY.dll", txtNombre_XY,  ref strError).Location, FileMode.Open, FileAccess.Read) ;
                  

                    if (!objValidateItDynamicCode.ValidateThis("newdll\\txtNombre_XY.dll", txtNombre_XY, "MyNombreXY", ref strError))
                    {
                        string s = string.Empty;

                        foreach (string ss in strError)
                            s += ss + "\r\n";
                        throw new Exception(s);
                    }
                    if (!objValidateItDynamicCode.ValidateThis("newdll\\txtNumElec_XY.dll",  txtNumElec_XY, "MyNumElecXY", ref strError))
                    {
                        string s = string.Empty;

                        foreach (string ss in strError)
                            s += ss + "\r\n";
                        throw new Exception(s);
                    }
                    if (!objValidateItDynamicCode.ValidateThis("newdll\\txtPrecinto_XY.dll", txtPrecinto_XY, "MyPrecintoXY", ref strError))
                    {
                        string s = string.Empty;

                        foreach (string ss in strError)
                            s += ss + "\r\n";
                        throw new Exception(s);
                    }
                    if (!objValidateItDynamicCode.ValidateThis("newdll\\txtSex_XY.dll", txtSex_XY, "MySexoXY", ref strError))
                    {
                        string s = string.Empty;

                        foreach (string ss in strError)
                            s += ss + "\r\n";
                        throw new Exception(s);
                    }
                    if (!objValidateItDynamicCode.ValidateThis("newdll\\txtCargo_XY.dll", txtCargo_XY, "MyCargoXY", ref strError))
                    {
                        string s = string.Empty;

                        foreach (string ss in strError)
                            s += ss + "\r\n";
                        throw new Exception(s);
                    }
                    if (!objValidateItDynamicCode.ValidateThis("newdll\\txtCandidato_XY.dll", txtCandidato_XY, "MyCandidatoXY", ref strError))
                    {
                        string s = string.Empty;

                        foreach (string ss in strError)
                            s += ss + "\r\n";
                        throw new Exception(s);
                    }
                    if (!objValidateItDynamicCode.ValidateThis("newdll\\txtNotarioElec_XY.dll",  txtNotarioElec_XY, "MyNotarioElecXY", ref strError))
                    {
                        string s = string.Empty;

                        foreach (string ss in strError)
                            s += ss + "\r\n";
                        throw new Exception(s);
                    }
                    if (!objValidateItDynamicCode.ValidateThis("newdll\\txtFirmaElec_XY.dll",  txtFirmaElec_XY, "MyFirmaElecXY", ref strError))
                    {
                        string s = string.Empty;

                        foreach (string ss in strError)
                            s += ss + "\r\n";
                        throw new Exception(s);
                    }
                    if (!objValidateItDynamicCode.ValidateThis("newdll\\txtNotarioFirma_XY.dll", txtNotarioFirma_XY, "MyNotarioFirmaXY", ref strError))
                    {
                        string s = string.Empty;

                        foreach (string ss in strError)
                            s += ss + "\r\n";
                        throw new Exception(s);
                    }
                    if (!objValidateItDynamicCode.ValidateThis("newdll\\txtFchEndoso_XY.dll", txtFchEndoso_XY, "MyFchEndosoXY",ref strError))
                    {
                        string s = string.Empty;

                        foreach (string ss in strError)
                            s += ss + "\r\n";
                        throw new Exception(s);
                    }
                    if (!objValidateItDynamicCode.ValidateThis("newdll\\txtFchEndosoEntregada_XY.dll", txtFchEndosoEntregada_XY, "MyFchEndosoEntregadaXY", ref strError))
                    {
                        string s = string.Empty;

                        foreach (string ss in strError)
                            s += ss + "\r\n";
                        throw new Exception(s);
                    }
                    if (!objValidateItDynamicCode.ValidateThis("newdll\\txtFechaNac_XY.dll", txtFechaNac_XY, "MyFechaNacXY", ref strError))
                    {
                        string s = string.Empty;

                        foreach (string ss in strError)
                            s += ss + "\r\n";
                        throw new Exception(s);
                    }
                }

                //{
                //    MemoryStream memoryStream = new MemoryStream(2048);
                //    ValidateItDynamicCode.CopyStream((Stream)fileStream, (Stream)memoryStream, 2048);
                //    fileStream.Close();
                //    memoryStream.Position = 0L;

                //    //FileStream file = new FileStream("NewDll\\txtNombre_XY2.dll", FileMode.Create, FileAccess.Write);

                //    //memoryStream.WriteTo(file);
                //    //file.Close();
                //    memoryStream.Close();
                ////    byte[] kaka = Encoding.ASCII.GetString(memoryStream);
                //}
                // dataRow["CompiledCode"] = (object) memoryStream.ToArray();
                //if (kaka == null)
                //    throw new Exception();



                MessageBox.Show("Done");


            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //RelayCommand
        public RelayCommand initWindow
        {
            get;
            private set;
        }
        public RelayCommand BtnFullImages
        {
            get
            {
                int v = 0;
                int ho = 0;
                if (_BtnFullImages == null)
                {
                    _BtnFullImages = new RelayCommand(param => BtnFullImages_Click(ref v, ref ho));
                }
                return _BtnFullImages;
            }
        }
        public RelayCommand BtnCrop
        {
            get
            {
                if (_BtnCrop == null)
                {
                    _BtnCrop = new RelayCommand(param => BtnCrop_Click());
                }
                return _BtnCrop;
            }
        }
        public RelayCommand cmdSalir_Click
        {
            get;
            private set;
        }
        public RelayCommand cmdZoomInOut
        {
            get; private set;
        }

        public RelayCommand cmdSetNumElec
        {
            get;
            private set;
        }
        public RelayCommand cmdSetPrecinto
        {
            get;
            private set;
        }
        public RelayCommand cmdSetSexo
        {
            get;
            private set;
        }
        public RelayCommand cmdSetFechaNac
        {
            get;
            private set;
        }
        public RelayCommand cmdSetCargo
        {
            get;
            private set;
        }
        public RelayCommand cmdSetCandidato
        {
            get;
            private set;
        }
        public RelayCommand cmdSetFuncionarioElec
        {
            get;
            private set;
        }
        public RelayCommand cmdSetfirmaElec
        {
            get;
            private set;
        }
        public RelayCommand cmdSetFirmaFuncionario
        {
            get;
            private set;
        }
        public RelayCommand cmdSetFechaJuramento
        {
            get;
            private set;
        }
        public RelayCommand cmdSetFechaEntrega
        {
            get;
            private set;
        }
        public RelayCommand cmdSetNombre
        {
            get;
            private set;
        }
        public RelayCommand cmdSave_Click { get; private set; }




    #endregion

    /*Metodos*/
    #region MyMetodos
    void View_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var mousePosition = e.GetPosition(sender as UIElement);
            Canvas.SetLeft(_SelectionRectangle, mousePosition.X);

            Canvas.SetTop(_SelectionRectangle, mousePosition.Y);

            _SelectionRectangle.Visibility = System.Windows.Visibility.Visible;
        }
        void View_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    var mousePosition = e.GetPosition(sender as UIElement);

                    _SelectionRectangle.Width = mousePosition.X - Canvas.GetLeft(_SelectionRectangle);
                    _SelectionRectangle.Height = mousePosition.Y - Canvas.GetTop(_SelectionRectangle);

                    _MyX = (int)Canvas.GetLeft(_SelectionRectangle);
                    _MyY = (int)Canvas.GetTop(_SelectionRectangle);
                    _MyH = _SelectionRectangle.Height;
                    _MyW = _SelectionRectangle.Width;
                    lblCordenadas = string.Format("int X = {0};int Y = {1};int H = {2};int W = {3};double V = {4};double Ho = {5};string fieldContents  = X.ToString() + \"|\" + Y.ToString() + \"|\" + H.ToString() + \"|\" + W.ToString()+ \"|\" + V.ToString()+\"|\" + Ho.ToString() ;return fieldContents;", _MyX, _MyY, _SelectionRectangle.Height, _SelectionRectangle.Width, _sv.VerticalOffset, _sv.HorizontalOffset);

                }
                catch
                {
                    //   MessageBox.Show(ex.ToString());
                }
            }
        }
        private string MyGetRegister(string param, string REGPATH)
        {
            string strReturn = null;
            try
            {
                string str = jolcode.Registry.read(REGPATH, param);
                if (string.IsNullOrEmpty(str))
                {
                    throw new Exception();
                }
                else
                {
                    strReturn = str;
                }

            }
            catch
            {
                strReturn = "int X = 0;int Y = 0;int H = 0;int W = 0;double V = 0;double Ho = 0;string fieldContents  = X.ToString() + \"|\" + Y.ToString() + \"|\" + H.ToString() + \"|\" + W.ToString()+ \"|\" + V.ToString()+\"|\" + Ho.ToString() ;return fieldContents;";
                jolcode.Registry.write(REGPATH, param, strReturn);
            }
            return strReturn;
        }


        private void MyWriteRegister(string param,string REGPATH, string str)
        {
             
            if (string.IsNullOrEmpty(str))
                str = "int X = 0;int Y = 0;int H = 0;int W = 0;double V = 0;double Ho = 0;string fieldContents  = X.ToString() + \"|\" + Y.ToString() + \"|\" + H.ToString() + \"|\" + W.ToString()+ \"|\" + V.ToString()+\"|\" + Ho.ToString() ;return fieldContents;";
                 

            jolcode.Registry.write(REGPATH, param, str);
        }
        #endregion



        /*Dispose*/
        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmSetImages()
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
