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
using WpfEndososCandidatos.View;
using System.Data;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Controls;
using WpfEndososCandidatos.Models;
using System.ComponentModel;
using WpfEndososCandidatos.ViewModels.Ver;
using System.Data.SqlClient;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Input;

namespace WpfEndososCandidatos.ViewModels.Procesos
{
    class vmFixVoid : ViewModelBase<IDialogView>, IDisposable
    {

        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private string _DBEndososCnnStr;
        private Brush _BorderColor;
        private string _Lot;
        private DataTable _MyLotsTable;
        private int _TotalRechazada;
        private string _txtRazonRechazo;
        private string _txtNumElec_Corregir;
        private int _i;
        private string _DBMasterCeeCnnStr;
        private string _txtNombre;
        private DataTable _tblCitizen;
        private string _txtNumElec;
        private string _txtPrecinto;
        private string _txtSex;
        private string _txtFechaNac;
        private string _txtNotarioNumElec;
        private string _txtNotarioFirstName;
        private string _txtPrecinto_Corregir;
        private string _txtSex_Corregir;
        private string _txtCargo_Corregir;
        private string _txtCandidato_Corregir;
        private string _txtNotarioElec_Corregir;
        private string _txtFirmaElec_Corregir;
        private string _txtNotarioFirma_Corregir;
        private bool _ckbFirma_Pet_Inv;
        private bool _ckbFirma_Not_Inv;
        private DateTime? _txtFchEndoso_Corregir;
        private DateTime? _txtFchEndosoEntregada_Corregir;
        private int _i_Display;
        private string _txtFormulario;
        private DateTime? _FechaNac_Corregir;
        private List<FixVoid> _DataToSave;
        private bool _CanGuardar;
        private string _SysUser;
        private string _DBCeeMasterImgCnnStr;
        private System.Windows.Media.ImageSource _Source_image;
        private int _ViewboxWidth;
        private int _ViewboxHeight;
        private ScrollViewer _sv;
        private wpfFixVoid _wpfFixVoid;
        private System.Windows.Shapes.Rectangle _SelectionRectangle;
        private int _MyX = 0;
        private int _MyY = 0;
        private double _MyH = 0;
        private double _MyW = 0;
        private BitmapImage _src = new BitmapImage();
        private System.Drawing.Image _img;

        private RelayCommand _BtnCrop;
        private RelayCommand _BtnFullImages;
        private string _lblCordenadas;
        private string _Nombre_Corregir;
        private BitmapImage _Source_image_Corregir;
        private RelayCommand _LostFocus;
        private RelayCommand _GotFocus;

        public vmFixVoid() :
            base(new wpfFixVoid())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdProximo_Click = new RelayCommand(param => MyCmdProximo_Click(),param=> CanProximo);
            cmdAnterior_Click = new RelayCommand(param => MyCmdAnterior_Click(),param=> CanAnterior);
            cmdVerElec_Click = new RelayCommand(param => MyCmdVerElec_Click());
            cmdGuardar_Click = new RelayCommand(param => MyCmdGuardar_Click(), param => CanGuardar);
            cmdZoomInOut = new RelayCommand(param => MyCmdZoomInOut(param));
            _DataToSave = new List<FixVoid>();



        }

        void View_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var mousePosition = e.GetPosition(sender as UIElement);
            Canvas.SetLeft(_SelectionRectangle, mousePosition.X);

            Canvas.SetTop(_SelectionRectangle, mousePosition.Y);

            _SelectionRectangle.Visibility = System.Windows.Visibility.Visible;

            //TituloFrmImg = string.Format("X:{0} Y:{1} H:{2} W:{3}", mousePosition.X.ToString(), mousePosition.Y.ToString(),_SelectionRectangle.Height,_SelectionRectangle.Width );

            //_MyX = (int)mousePosition.X;
            //_MyY = (int)mousePosition.Y;

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

                      lblCordenadas = string.Format("X = {0}; Y = {1}; H = {2}; W = {3}; V = {4}; Ho = {5};", _MyX, _MyY, _SelectionRectangle.Height, _SelectionRectangle.Width, _sv.VerticalOffset, _sv.HorizontalOffset);

                }
                catch
                {
                    //   MessageBox.Show(ex.ToString());
                }
            }
        }


        void BtnCrop_Click()
        {
            double factorX, factorY;

            factorX = _src.PixelWidth / _src.Width;
            factorY = _src.PixelHeight / _src.Height;

            BitmapSource displayImage = new CroppedBitmap(_src, new Int32Rect(_MyX, _MyY, (int)(_MyW), (int)(_MyH)));  // new CroppedBitmap(original, crop);

            ViewboxWidth = (int)_MyW + 10;
            ViewboxHeight = (int)_MyH + 10;

            Source_image = displayImage;

            _sv.ScrollToVerticalOffset(0);
            _sv.ScrollToHorizontalOffset(0);

        }
        private void BtnFullImages_Click(ref int v, ref int Ho)
        {

            ViewboxWidth = _img.Width;
            ViewboxHeight = _img.Height;
            BitmapSource displayImage = new CroppedBitmap(_src, new Int32Rect(0, 0, 0, 0));  // new CroppedBitmap(original, crop);
            Source_image = displayImage;

            _sv.ScrollToVerticalOffset(v);
            _sv.ScrollToHorizontalOffset(Ho);

            _sv.UpdateLayout();

        }
        private bool MyLostFocus(object param)
        {
            if (param is TextBox)
            {
                var txt = param as TextBox;

                txt.Background = Brushes.White;
            }
            if (param is Label)
            {
                var txt = param as Label;
                txt.Background = Brushes.White;

            }
            if (param is DatePicker)
            {
                var txt = param as DatePicker;
                txt.Background = Brushes.White;

            }
            return true;
        }
        public void MyGotFocus(object name)
        {

            int X = 0;
            int Y = 0;
            int H = 0;
            int W = 0;
            int V = 0;
            int Ho = 0;


            string contenido = string.Empty;
         

            if (name is TextBox)
            {
                var txt = name as TextBox;
                contenido = txt.Name;

                //if (CurrentIDX > txt.TabIndex)
                //{
                //    _ChangeTap = true;
                //}
                //else
                //{
                //    _ChangeTap = false;
                //}

                //CurrentIDX = txt.TabIndex;

                txt.SelectAll();

                txt.Background = Brushes.Red;
            }
            if (name is Label)
            {
                var txt = name as Label;
                contenido = txt.Name;
                txt.Background = Brushes.Red;

            }
            if (name is DatePicker)
            {
                var txt = name as DatePicker;
                contenido = txt.Name;
                txt.Background = Brushes.Red;

            }


            try
            {
                switch (contenido)
                {
                    case "txtNumElec_Corregir":
                        {//X:638 Y:1968 H:148 W:692 V:1919.81300691275 H:528
                            X = 638;
                            Y = 1968;
                            H = 148;
                            W = 692;
                            V = 1919;
                            Ho = 528;
                            break;
                        }
                    case "txtPrecinto_Corregir":
                        {
                            X = 1059; Y = 1940; H = 170; W = 845; V = 1848; Ho = 946;
                            break;
                        }
                    case "txtSex_Corregir":
                        X = 1274; Y = 1652; H = 180; W = 544; V = 1544; Ho = 1127;
                        break;
                    case "FechaNac_Corregir":
                       // X = 1672; Y = 1597; H = 271; W = 833; V = 1515; Ho = 1595;
                       X = 1697; Y = 1660; H = 194; W = 771; V = 1434; Ho = 1508;

                        break;
                    case "txtCargo_Corregir":
                        X = 429; Y = 2106; H = 151; W = 940; V = 1919; Ho = 409;
                        break;
                    case "txtCandidato_Corregir":
                        X = 28; Y = 1946; H = 204; W = 870; V = 1836; Ho = 0;
                        break;
                    case "txtNotarioElec_Corregir":
                        X = 109; Y = 2639; H = 175; W = 723; V = 2456; Ho = 0;
                        break;
                    case "txtFirmaElec_Corregir":
                        X = 1259; Y = 1180; H = 174; W = 940; V = 1022; Ho = 1249;
                        break;
                    case "txtNotarioFirma_Corregir":
                        X = 1145; Y = 2638; H = 180; W = 898; V = 2400; Ho = 1106;
                        break;
                    case "txtFchEndoso_Corregir":
                        X = 577; Y = 2462; H = 154; W = 797; V = 2407; Ho = 470;
                        break;
                    case "txtFchEndosoEntregada_Corregir":
                        X = 2143; Y = 2303; H = 477; W = 360; V = 2276; Ho = 1595;
                        break;
                    case "Nombre_Corregir":
                        X = 75; Y = 483; H = 307; W = 877; V = 446; Ho = 0;
                        break;
                   




                }
            }
            catch
            {
                MessageBox.Show("Error");
            }

            try
            {


                BtnFullImages_Click(ref V, ref Ho);

                //_inMoveUpDw = V;
                //_inMoveLfRh = Ho;
                //_sv.ScrollToVerticalOffset(V);
                //_sv.ScrollToHorizontalOffset(Ho);



            }
            catch
            {
                MessageBox.Show("Error en el Croop");
            }
            finally
            {
            }
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
        public RelayCommand miLostFocus
        {
            get
            {
                if (_LostFocus == null)
                {
                    _LostFocus = new RelayCommand(param => MyLostFocus(param));
                }
                return _LostFocus;
            }
        }
        public RelayCommand miGotFocus
        {
            get
            {
                if (_GotFocus == null)
                {
                    _GotFocus = new RelayCommand(param => MyGotFocus(param));
                }
                return _GotFocus;
            }
        }
        #region MyProperty

        public string Nombre_Corregir
        {
            get
            {
                return _Nombre_Corregir;
            }set
            {
                _Nombre_Corregir = value;
                this.RaisePropertychanged("Nombre_Corregir");
            }
        }

        public string lblCordenadas
        {
            get
            {
                return _lblCordenadas;
            }set
            {
                _lblCordenadas = value;
                this.RaisePropertychanged("lblCordenadas");
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
        public BitmapImage Source_image_Corregir
        {
            get
            {
                return _Source_image_Corregir;
            }set
            {
                _Source_image_Corregir = value;
                this.RaisePropertychanged("Source_image_Corregir");
            }
        }


        public DataTable MyLotsTable
        {
            get
            {
                return _MyLotsTable;
            }set
            {
                _MyLotsTable = value;
                this.RaisePropertychanged("MyLotsTable");
            }
        }
        public int i
        {
            get
            {
                return _i;
            }set
            {
                _i = value;
                this.RaisePropertychanged("i");
               
            }
        }
        public int i_Display
        {
            get
            {
                return _i_Display;
            }
            set
            {
                _i_Display = value;
                this.RaisePropertychanged("i_Display");
            }
        }
        public string SysUser
        {
            get
            {
                return _SysUser;
            }set
            {
                _SysUser = value;
            }
        }

        public string txtRazonRechazo
        {
            get
            {
                return _txtRazonRechazo;
            }set
            {
                _txtRazonRechazo = value;
                this.RaisePropertychanged("txtRazonRechazo");
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
        public string txtNumElec
        {
            get
            {
                return _txtNumElec;
            }set
            {
                _txtNumElec = value;
                this.RaisePropertychanged("txtNumElec");
            }
        }
        public string txtPrecinto
        {
            get
            {
                return _txtPrecinto;
            }set
            {
                _txtPrecinto = value;
                this.RaisePropertychanged("txtPrecinto");
            }
        }
        public string txtSex
        {
            get
            {
                return _txtSex;
            }set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int trash = 0;
                    if (int.TryParse(value, out trash))
                        _txtSex = "E";
                    else
                    _txtSex = value;

                    this.RaisePropertychanged("txtSex");
                }
            }
        }
        public string txtFechaNac
        {
            get
            {
                return _txtFechaNac;
            }set
            {
                _txtFechaNac = value;
                this.RaisePropertychanged("txtFechaNac");

            }
        }
        public string txtNotarioNumElec
        {
            get
            {
                return _txtNotarioNumElec;
            }set
            {
                _txtNotarioNumElec = value;
                this.RaisePropertychanged("txtNotarioNumElec");
            }
        }
        public string txtNotarioFirstName
        {
            get
            {
                return _txtNotarioFirstName;
            }set
            {
                _txtNotarioFirstName = value;
                this.RaisePropertychanged("txtNotarioFirstName");
            }
        }
        public string txtFormulario
        {
            get
            {
                return _txtFormulario;
            }
            set
            {
                _txtFormulario = value;

                this.RaisePropertychanged("txtFormulario");
            }
        }

        public string txtNumElec_Corregir
        {
            get
            {
                return _txtNumElec_Corregir;
            }
            set
            {

                if (_txtNumElec_Corregir != value)
                {
                    _txtNumElec_Corregir = value;

                    if (!string.IsNullOrEmpty(value))
                    {
                        if (_DataToSave.Count > 0)
                        {
                            CanGuardar = true;
                        }
                    }
                    this.RaisePropertychanged("txtNumElec_Corregir");
                }
            }
        }

        public string txtPrecinto_Corregir
        {
            get
            {
                return _txtPrecinto_Corregir;
            }set
            {
                if (_txtPrecinto_Corregir != value)
                {
                    _txtPrecinto_Corregir = value;

                    if (!string.IsNullOrEmpty(value))
                    {
                        if (_DataToSave.Count > 0)
                        {
                            CanGuardar = true;
                        }
                    }

                    this.RaisePropertychanged("txtPrecinto_Corregir");
                }
            }
        }
        public string txtSex_Corregir
        {
            get
            {
                return _txtSex_Corregir;
            }set
            {
                if (_txtSex_Corregir != value)
                {
                    _txtSex_Corregir = value;

                    if (!string.IsNullOrEmpty(value))
                    {
                        if (_DataToSave.Count > 0)
                        {
                            CanGuardar = true;
                        }
                    }

                    this.RaisePropertychanged("txtSex_Corregir");
                }
            }
        }
        public string txtCargo_Corregir
        {
            get
            {
                return _txtCargo_Corregir;
            }set
            {
                if (_txtCargo_Corregir != value)
                {
                    _txtCargo_Corregir = value;

                    if (!string.IsNullOrEmpty(value))
                    {
                        if (_DataToSave.Count > 0)
                        {
                            CanGuardar = true;
                        }
                    }

                    this.RaisePropertychanged("txtCargo_Corregir");

                }
            }
        }
        public string txtCandidato_Corregir
        {
            get
            {
                return _txtCandidato_Corregir;
            }set
            {

                if (_txtCandidato_Corregir != value)
                {
                    _txtCandidato_Corregir = value;

                    if (!string.IsNullOrEmpty(value))
                    {
                        if (_DataToSave.Count > 0)
                        {
                            CanGuardar = true;
                        }
                    }

                    this.RaisePropertychanged("txtCandidato_Corregir");
                }
            }
        }
        public string txtNotarioElec_Corregir
        {
            get
            {
                return _txtNotarioElec_Corregir;
            }set
            {
                if (_txtNotarioElec_Corregir != value)
                {
                    _txtNotarioElec_Corregir = value;

                    if (!string.IsNullOrEmpty(value))
                    {
                        if (_DataToSave.Count > 0)
                        {
                            CanGuardar = true;
                        }
                    }

                    this.RaisePropertychanged("txtNotarioElec_Corregir");
                }
            }
        }
        public string txtFirmaElec_Corregir
        {
            get
            {
                return _txtFirmaElec_Corregir;
            }set
            {

                if (_txtFirmaElec_Corregir != value)
                {
                    _txtFirmaElec_Corregir = value;

                    if (!string.IsNullOrEmpty(value))
                    {
                        if (_DataToSave.Count > 0)
                        {
                            CanGuardar = true;
                        }
                    }
                   
                    this.RaisePropertychanged("txtFirmaElec_Corregir");
                }
            }
        }
        public string txtNotarioFirma_Corregir
        {
            get
            {
                return _txtNotarioFirma_Corregir;
            }set
            {
                if (_txtNotarioFirma_Corregir != value)
                {
                    _txtNotarioFirma_Corregir = value;

                    if (!string.IsNullOrEmpty(value))
                    {
                        if (_DataToSave.Count > 0)
                        {
                            CanGuardar = true;
                        }
                    }

                    this.RaisePropertychanged("txtNotarioFirma_Corregir");
                }
            }
        }
        public bool ckbFirma_Pet_Inv
        {
            get
            {
                return _ckbFirma_Pet_Inv;
            }set
            {
                _ckbFirma_Pet_Inv = value;

                if (_DataToSave.Count > 0)
                {
                    CanGuardar = true;
                }

                this.RaisePropertychanged("ckbFirma_Pet_Inv");
            }
        }
        public bool ckbFirma_Not_Inv
        {
            get
            {
                return _ckbFirma_Not_Inv;
            }set
            {
                _ckbFirma_Not_Inv = value;

                if (_DataToSave.Count > 0)
                {
                    CanGuardar = true;
                }

                this.RaisePropertychanged("ckbFirma_Not_Inv");
            }
        }
        public DateTime? txtFchEndoso_Corregir
        {
            get
            {
                return _txtFchEndoso_Corregir;
            }set
            {
                _txtFchEndoso_Corregir = value;

                if (value != null)
                {
                    if (_DataToSave.Count > 0)
                    {
                        CanGuardar = true;
                    }
                }

                this.RaisePropertychanged("txtFchEndoso_Corregir");
            }
        }
        public DateTime? txtFchEndosoEntregada_Corregir
        {
            get
            {
                return _txtFchEndosoEntregada_Corregir;
            }set
            {
                _txtFchEndosoEntregada_Corregir = value;

                if (value !=null)
                {
                    if (_DataToSave.Count > 0)
                    {
                        CanGuardar = true;
                    }
                }

                this.RaisePropertychanged("txtFchEndosoEntregada_Corregir");
            }
        }
       
        public DateTime? FechaNac_Corregir
        {
            get
            {
                return _FechaNac_Corregir;
            }set
            {
                _FechaNac_Corregir = value;
                if (value!=null)
                {
                    if (_DataToSave.Count > 0)
                    {
                        CanGuardar = true;
                    }
                }
                this.RaisePropertychanged("FechaNac_Corregir");
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
        public string DBEndososCnnStr
        {
            get
            {
                return _DBEndososCnnStr;
            }set
            {
                _DBEndososCnnStr = value;
            }
        }
        public string DBMasterCeeCnnStr
        {
            get
            {
                return _DBMasterCeeCnnStr;
            }set
            {
                _DBMasterCeeCnnStr = value;
            }
        }
        public string DBCeeMasterImgCnnStr
        {
            get
            {
                return _DBCeeMasterImgCnnStr;
            }set
            {
                _DBCeeMasterImgCnnStr = value;
            }
        }

        public string Lot
        {
            get
            {
                return _Lot;
            } set
            {
                if (_Lot != value)
                {
                    _Lot = value;
                  
                    this.RaisePropertychanged("Lot");
                }
            }
        }
        public int TotalRechazada
        {
            get
            {
                return _TotalRechazada;
            }set
            {
                _TotalRechazada = value;
                this.RaisePropertychanged("TotalRechazada");
            }

        }
        
        private bool CanGuardar
        {
            get
            {
                return _CanGuardar ;
            }set
            {
                _CanGuardar = value;
            }
        }
        private bool CanProximo
        {
            get
            {
                if (i >= (TotalRechazada)-1)
                    return false;
                else
                    return true;
            }
        }
        
        private bool CanAnterior
        {
            get
            {
                if (i > 0)
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #region MyCmd
        private void MyInitWindow()
        {
            try
            {
                CanGuardar = false;

                string myBorderBrush = ConfigurationManager.AppSettings["BorderBrush"];

                if (myBorderBrush != null && myBorderBrush.Trim().Length > 0)
                {
                    Type t = typeof(Brushes);
                    Brush b = (Brush)t.GetProperty(myBorderBrush).GetValue(null, null);
                    BorderColor = b;
                }
                else
                    BorderColor = Brushes.Black;


                MyRefresh(true);

                MyFillField();

                MySendTab();
                TextBox tet = new TextBox();
                tet.Name = "txtNumElec_Corregir";
                MyGotFocus(tet);
                
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool? MyOnShow()
        {
            _wpfFixVoid =this.View as wpfFixVoid;

            _wpfFixVoid.miCanvas.MouseMove += View_MouseMove;
            _wpfFixVoid.miCanvas.MouseLeftButtonDown += View_MouseLeftButtonDown;
            sv = _wpfFixVoid.scrollViewer;
            _SelectionRectangle = _wpfFixVoid.selectionRectangle;

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
        private void MyCmdProximo_Click ()
        {
            SaveTmp();

            i++;
            int TotalRechazada_tmp = TotalRechazada - 1;

            if (i >= (TotalRechazada_tmp))
                i = TotalRechazada_tmp;

            i_Display = i + 1;
             MyFillField();

            CanGuardar = true;
        }
        private void MyCmdAnterior_Click()
        {
            SaveTmp();

            i--;

            if (i < 0)
                i = 0;

            i_Display = i + 1;

            MyFillField();
        }
        public void MyCmdVerElec_Click()
        {
            try
            {
                using (vmElector frmElector = new vmElector())
                {
                    frmElector.View.Owner = this.View as Window;
                    frmElector.DBCeeMasterCnnStr = DBMasterCeeCnnStr;
                    frmElector.DBCeeMasterImgCnnStr = DBCeeMasterImgCnnStr;
                    frmElector.TxtElecNum = txtNumElec;
                    frmElector.MyOnShow();
                }
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void MyCmdGuardar_Click()
        {
            SaveTmp();

            SqlTransaction transaction = null;

            try
            {
                using (SqlExcuteCommand Exe = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr,
                    DBCeeMasterCnnStr = DBMasterCeeCnnStr

                })
                {
                    using (SqlConnection cnn = new SqlConnection()
                    {
                        ConnectionString = DBEndososCnnStr
                    })
                    {
                        cnn.Open();
                        using (SqlCommand cmd = new SqlCommand()
                        {
                             Connection = cnn,
                             CommandType = CommandType.Text
                        })
                        {
                            // Start a local transaction.
                            transaction = cnn.BeginTransaction(IsolationLevel.ReadCommitted);
                            cmd.Transaction = transaction;

                            foreach (FixVoid data in _DataToSave)
                            {
                                string FchEndoso = null;
                                string FechaNac = null;

                                if (data.FechaNac !=null)
                                    FechaNac = data.FechaNac.Value.ToString("MMddyyyy");

                                if (data.FchEndoso != null)
                                    FchEndoso = data.FchEndoso.Value.ToString("MMddyyyy");

                                Exe.MyUpdateTFTable(
                                   data.Numelec,
                                   data.Precinto,
                                   data.Sexo,
                                   FechaNac,
                                   data.Cargo,
                                   data.NotarioElec,
                                   data.Candidato,
                                   data.FirmaElec,
                                   data.NotarioFirma,
                                   data.Firma_Pet_Inv == true ? "1" : "0",
                                   data.Firma_Not_Inv == true ? "1" : "0",
                                   FchEndoso,
                                   data.Lot,
                                   data.Batch,
                                   data.Formulario,
                                   data.CurrElect,
                                   SysUser,
                                   cmd
                                    );
                            }
                            transaction.Commit();

                            Exe.MyReverseLots(Lot, SysUser);

                            MessageBox.Show("Este Lote Fue Enviado a Reprocesar", "Done.", MessageBoxButton.OK, MessageBoxImage.Information);

                            MyCmdSalir_Click();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                        if (transaction.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                                " was encountered while attempting to roll back the transaction.");
                        }
                    }
                }

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
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


        public RelayCommand cmdZoomInOut
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
        public RelayCommand cmdProximo_Click
        {
            get;private set;
        }
        public RelayCommand cmdAnterior_Click
        {
            get;private set;
        }
        public RelayCommand cmdVerElec_Click
        {
            get;private set;
        }
        public RelayCommand cmdGuardar_Click
        {
            get;private set;
        }

        #endregion

        #region MyModule
        private void MyFillField()
        {
            try
            {
                MyRefresh(false);
                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr,
                    DBCeeMasterCnnStr = DBMasterCeeCnnStr,
                    DBImagenesCnnStr = DBCeeMasterImgCnnStr

                })
                {
                    Nombre_Corregir = _DataToSave[i].Nombre;
                    txtNumElec_Corregir = _DataToSave[i].Numelec;
                    txtNotarioNumElec = _DataToSave[i].NotarioElec;
                    txtPrecinto_Corregir = _DataToSave[i].Precinto;
                    txtSex_Corregir = _DataToSave[i].Sexo;
                    txtCargo_Corregir = _DataToSave[i].Cargo;
                    txtCandidato_Corregir = _DataToSave[i].Candidato;
                    txtNotarioElec_Corregir = _DataToSave[i].NotarioElec;

                    txtFirmaElec_Corregir = _DataToSave[i].FirmaElec;
                    txtNotarioFirma_Corregir = _DataToSave[i].NotarioFirma;

                    ckbFirma_Pet_Inv = _DataToSave[i].Firma_Pet_Inv;
                    ckbFirma_Not_Inv = _DataToSave[i].Firma_Not_Inv;

                    txtFchEndoso_Corregir = _DataToSave[i].FchEndoso;
                    txtFormulario = _DataToSave[i].Formulario;
                    FechaNac_Corregir = _DataToSave[i].FechaNac;

                    txtFchEndosoEntregada_Corregir = _DataToSave[i].FchEndosoEntregada;

                    if (txtFchEndoso_Corregir == null) // Nota : Hay que preguntar cual es la diferencia de la Fecha_Endoso y Firma_Fecha
                        txtFchEndoso_Corregir = _DataToSave[i].Firma_Fecha;

                    byte[] EndosoImage = null;

                    txtRazonRechazo = get.MyTipoDeRechazo(_DataToSave[i].TipoDeRechazo, txtFormulario, Lot,ref EndosoImage);
                    _src = new BitmapImage();
                  
                        // 'DESPLIEGA la imagen
                    if (EndosoImage != null)
                    {
                        MemoryStream strmEndosoImage = new MemoryStream();
                        strmEndosoImage.Write(EndosoImage, 0, EndosoImage.Length);
                        strmEndosoImage.Position = 0;
                        _src.BeginInit();
                        _img = System.Drawing.Image.FromStream(strmEndosoImage);

                        MemoryStream ms = new MemoryStream();
                        _img.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
                        ms.Seek(0, SeekOrigin.Begin);
                        _src.StreamSource = ms;
                        _src.EndInit();

                        ViewboxHeight =  _img.Height;
                        ViewboxWidth =  _img.Width;

                        BitmapSource displayImage = new CroppedBitmap(_src, new Int32Rect(0, 0, _img.Width, _img.Width));  // new CroppedBitmap(original, crop);


                        Source_image = displayImage;

                    }
                    else
                        Source_image = null;




                    DataTable myTableImg = new DataTable();

                    if (txtNumElec_Corregir.Trim().Length > 0)
                    {
                        _tblCitizen = get.MyGetCitizen(txtNumElec_Corregir);

                        myTableImg = get.MyGetCitizenImg(txtNumElec_Corregir);
                    }
                    DataTable notarioInfo = new DataTable();

                    if (txtNotarioNumElec.Trim().Length >0)
                        notarioInfo = get.MyGetCitizen(txtNotarioNumElec);

                    if (notarioInfo.Rows.Count > 0)
                    {
                        string[] notario =
                            {
                                        notarioInfo.Rows[0]["FirstName"].ToString()," ",
                                        notarioInfo.Rows[0]["LastName1"].ToString()," ",
                                        notarioInfo.Rows[0]["LastName2"].ToString()
                                };

                        txtNotarioFirstName = string.Concat(notario);
                    }
                    else
                        txtNotarioFirstName = "Error No hay Datos en el Master";


                    if (_tblCitizen.Rows.Count > 0)
                    {
                        string[] name = {
                                        _tblCitizen.Rows[0]["FirstName"].ToString().Trim()," ",
                                        _tblCitizen.Rows[0]["LastName1"].ToString().Trim()," ",
                                        _tblCitizen.Rows[0]["LastName2"].ToString().Trim()
                                    };

                        txtNombre = string.Concat(name);
                        txtNumElec = txtNumElec_Corregir;
                        txtPrecinto = _tblCitizen.Rows[0]["FirstGeoCode"].ToString().Trim().PadLeft(3, '0');
                        txtSex = _tblCitizen.Rows[0]["Gender"].ToString().Trim();

                        txtFechaNac = _tblCitizen.Rows[0]["DateOfBirth"].ToString().Trim();
                        if (!string.IsNullOrEmpty(txtFechaNac))
                        {
                            string Mes = txtFechaNac.Split('/')[0].Trim().PadLeft(2, '0');
                            string Dia = txtFechaNac.Split('/')[1].Trim().PadLeft(2, '0');
                            string Ano = txtFechaNac.Split('/')[2].Trim().Substring(0, 4);
                            txtFechaNac = Mes + "/" + Dia + "/" + Ano;
                        }

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
                            Source_image_Corregir = bi;
                        }
                        else
                            Source_image_Corregir = null;

                    }
                    else
                    {
                        txtNumElec = txtNumElec_Corregir;

                        txtNombre = "No Hay Datos";
                        txtPrecinto = "No Hay Datos";
                        txtSex = "No Hay Datos";
                        txtFechaNac = "No Hay Datos";
                    }
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MyRefresh(bool resetAllData)
        {
            txtRazonRechazo = string.Empty;

            txtNumElec_Corregir = string.Empty;
            txtPrecinto_Corregir = string.Empty;
            txtSex_Corregir = string.Empty;
            txtCargo_Corregir = string.Empty;
            txtCandidato_Corregir = string.Empty;
            txtNotarioElec_Corregir = string.Empty;
            txtFirmaElec_Corregir = string.Empty;
            txtNotarioFirma_Corregir = string.Empty;
            ckbFirma_Pet_Inv = false;
            ckbFirma_Not_Inv = false;
            txtFchEndoso_Corregir = null;
            txtFchEndosoEntregada_Corregir = null;
            FechaNac_Corregir = null;

            txtFormulario = string.Empty;
            txtNumElec = string.Empty;
            txtNombre = string.Empty;
            txtPrecinto = string.Empty;
            txtSex = string.Empty;
            txtFechaNac = string.Empty;
            txtNotarioNumElec = string.Empty;
            txtNotarioFirstName = string.Empty;
            Nombre_Corregir = string.Empty;


            if (resetAllData)
            {
                i = 0;
                i_Display = 1;
                TotalRechazada = 0;

                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    

                    MyLotsTable = get.MyGetLotToFixVoid(Lot);


                    if (_MyLotsTable.Rows.Count == 0)
                        MessageBox.Show("No hay rechazadas para procesar", "No Hay", MessageBoxButton.OK, MessageBoxImage.Information);

                    int k = 0;
                    TotalRechazada = _MyLotsTable.Rows.Count;
                    foreach (DataRow row in _MyLotsTable.Rows)
                    {
                        string FechaNac_Mes = row["FechaNac_Mes"].ToString().Trim().PadLeft(2,'0');
                        string FechaNac_Dia = row["FechaNac_Dia"].ToString().Trim().PadLeft(2,'0');
                        string FechaNac_Ano = row["FechaNac_Ano"].ToString().Trim().PadLeft(4,'0');

                        string FechaNac= FechaNac_Mes + FechaNac_Dia + FechaNac_Ano;

                        string FechaFirm_Mes = row["FechaFirm_Mes"].ToString().Trim().PadLeft(2, '0');
                        string FechaFirm_Dia = row["FechaFirm_Dia"].ToString().Trim().PadLeft(2, '0');
                        string FechaFirm_Ano = row["FechaFirm_Ano"].ToString().Trim().PadLeft(4, '0');

                        string Fecha_Endoso = FechaFirm_Mes + FechaFirm_Dia + FechaFirm_Ano;

                        _DataToSave.Add(new FixVoid
                        {
                            i = k,
                            CurrElect = row["NumElec"].ToString(),
                            Nombre =  row["Nombre"].ToString().Trim() + " " + row["Paterno"].ToString().Trim() + " " + row["Materno"].ToString(),
                            Lot = Lot,
                            Formulario = row["BatchPgNo"].ToString().Trim(),
                            TipoDeRechazo = "",
                            Numelec = row["NumElec"].ToString(),
                            NotarioElec = row["Notario_Funcionario"].ToString(),
                            Precinto = row["Precinto"].ToString().Trim().PadLeft(3, '0'),
                            FechaNac = DateTimeUtil.MyValidarFecha(FechaNac),                            
                            Sexo = row["Sexo"].ToString().Trim(),
                            Candidato = row["Num_Candidato"].ToString().Trim(),
                            Cargo = row["Funcionario"].ToString().Trim(),
                            FirmaElec = row["FirmaElector"].ToString().Trim(),
                            NotarioFirma = row["FirmaNotario"].ToString().Trim(),
                            Firma_Pet_Inv = row["FirmaElec_Inv"].ToString().Trim() == "1" ? true : false,
                            Firma_Not_Inv = row["FirmaNot_Inv"].ToString().Trim() == "1" ? true : false,
                            FchEndoso =null,
                            Firma_Fecha = DateTimeUtil.MyValidarFecha(Fecha_Endoso),
                            FchEndosoEntregada = null,
                            Batch = row["BatchNo"].ToString(),
                            image = row["Nombre_Image"].ToString(),
                            
                        });
                        k++;
                    }

                }
            }
        }

        private void SaveTmp()
        {
                _DataToSave[i].i = i;
                _DataToSave[i].Lot = Lot;
                _DataToSave[i].Formulario = txtFormulario;
                // _DataToSave[i].TipoDeRechazo = row["TipoDeRechazo"].ToString(),
                _DataToSave[i].Numelec = txtNumElec_Corregir;
                _DataToSave[i].NotarioElec = txtNotarioElec_Corregir;
                _DataToSave[i].Precinto = txtPrecinto_Corregir;
                _DataToSave[i].FechaNac = FechaNac_Corregir;
                _DataToSave[i].Sexo = txtSex_Corregir;
                _DataToSave[i].Candidato = txtCandidato_Corregir;
                _DataToSave[i].Cargo = txtCargo_Corregir;
                _DataToSave[i].FirmaElec = txtFirmaElec_Corregir;
                _DataToSave[i].NotarioFirma = txtNotarioFirma_Corregir;
                _DataToSave[i].Firma_Pet_Inv = ckbFirma_Pet_Inv;
                _DataToSave[i].Firma_Not_Inv = ckbFirma_Not_Inv;
                _DataToSave[i].FchEndoso = txtFchEndoso_Corregir;
                _DataToSave[i].FchEndosoEntregada = txtFchEndosoEntregada_Corregir;
                      
            // _DataToSave[i].FchEndosoEntregada = null,
            
            //_DataToSave[i].Batch = row["Batch"].ToString(),
            
            // _DataToSave[i].image = row["image"].ToString(),
        }
        public void MySendTab()
        {
            try
            {
                KeyEventArgs e1 = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
                e1.RoutedEvent = Keyboard.KeyDownEvent;
                InputManager.Current.ProcessInput(e1);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion




        #region Dispose



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmFixVoid()
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
