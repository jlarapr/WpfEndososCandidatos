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
using System.Globalization;
using System.Collections.ObjectModel;

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
        private string _txtFchJuramento_Corregir;
        private string _txtFchEndosoEntregada_Corregir;
        private int _i_Display;
        private string _txtFormulario;
        private string _FechaNac_Corregir;
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
        private BitmapImage _srcEndoso = new BitmapImage();
        private System.Drawing.Image _imgEndoso;

        private RelayCommand _BtnCrop;
        private RelayCommand _BtnFullImages;
        private string _lblCordenadas;
        private string _Nombre_Corregir;
        private System.Windows.Media.ImageSource _Source_image_Corregir;
        private RelayCommand _LostFocus;
        private RelayCommand _GotFocus;
        private int _ViewboxWidthSing;
        private int _ViewboxHeightSing;
        private DataTable _myTableImgFirmaElector;
        private DataTable _myTableImgNotario;
        private bool _isFirmaNotario;
        private string _batchTF;
        private string _txtStatusElec;
        private string _txtStatusNotario;
        private ObservableCollection<Brush> _txtColor;
        Brush tmpBrushes;
        private bool _isAll;
        private bool _chkOtraRazonDeRechazo;
        private string _txtOtraRazonDeRechazo;
        private int? _txtFchEndosoEntregada_Corregir_dias = null;
        private int _GoIdx;
        private string _txtNumElec_Go;
        private string _txtNombre_Corregir;
        private string _txtFchFirmaElector_Corregir;
        string[] _formatsDate = { "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/M/yy", "ddMMyyyy", "dMyyyy", "ddMMyy", "d/M/yy" };

        public vmFixVoid() :
            base(new wpfFixVoid())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdProximo_Click = new RelayCommand(param => MyCmdProximo_Click(), param => CanProximo);
            cmdAnterior_Click = new RelayCommand(param => MyCmdAnterior_Click(), param => CanAnterior);
            cmdVerElec_Click = new RelayCommand(param => MyCmdVerElec_Click());
            cmdGuardar_Click = new RelayCommand(param => MyCmdGuardar_Click(), param => CanGuardar);
            cmdLast = new RelayCommand(param => MycmdLast());
            cmdFirst = new RelayCommand(param => MycmdFirst());
            BtnGo_Elec = new RelayCommand(param => MyBtnGo_Elec(), param => BtnGo_Elec_Can);
            cmdZoomInOut = new RelayCommand(param => MyCmdZoomInOut(param));
            BtnGo = new RelayCommand(param => MyBtnGo());
            cmdSetImg = new RelayCommand(param => MyCmdSetImg());
            SendTab = new RelayCommand(param => MySendTab());
            _DataToSave = new List<FixVoid>();
            txtColor = new ObservableCollection<Brush>();


        }


        /*Property*/
        #region MyProperty



        private bool BtnGo_Elec_Can
        {
            get
            {
                if (string.IsNullOrEmpty(txtNumElec_Go))
                    return false;

                int elec;
                if (!int.TryParse(txtNumElec_Go, out elec))
                    return false;

                return true;
            }
        }

        public bool isAll
        {
            get
            {
                return _isAll;
            }
            set
            {
                _isAll = value;
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
            get
            {
                return _imgEndoso;
            }
            private set
            {
                _imgEndoso = value;

            }
        }

        public string batchTF
        {
            get
            {
                return _batchTF;
            }
            set
            {
                _batchTF = value;
                this.RaisePropertychanged("batchTF");
            }
        }
        public string Nombre_Corregir
        {
            get
            {
                return _Nombre_Corregir;
            }
            set
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
            }
            set
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
        public string txtStatusElec
        {
            get
            {
                return _txtStatusElec;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _txtStatusElec = value;
                    this.RaisePropertychanged("txtStatusElec");
                }
            }
        }
        public string txtStatusNotario
        {
            get
            {
                return _txtStatusNotario;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _txtStatusNotario = value;
                    this.RaisePropertychanged("txtStatusNotario");
                }
            }
        }

        public int ViewboxWidthSing
        {
            get
            {
                return _ViewboxWidthSing;
            }
            set
            {
                _ViewboxWidthSing = value;
                this.RaisePropertychanged("ViewboxWidthSing");
            }
        }
        public int ViewboxHeightSing
        {
            get
            {
                return _ViewboxHeightSing;
            }
            set
            {
                _ViewboxHeightSing = value;
                this.RaisePropertychanged("ViewboxHeightSing");
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
        public System.Windows.Media.ImageSource Source_image_Corregir
        {
            get
            {
                return _Source_image_Corregir;
            }
            set
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
            }
            set
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
            }
            set
            {
                _i = value;
                this.RaisePropertychanged("i");

            }
        }
        public int GoIdx
        {
            get
            {
                return _GoIdx;
            }
            set
            {
                _GoIdx = value;
                this.RaisePropertychanged("GoIdx");

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
            }
            set
            {
                _SysUser = value;
            }
        }

        public string txtRazonRechazo
        {
            get
            {
                return _txtRazonRechazo;
            }
            set
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
            }
            set
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
            }
            set
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
            }
            set
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
            }
            set
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
            }
            set
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
            }
            set
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
            }
            set
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

        public string txtNombre_Corregir
        {
            get
            {
                return _txtNombre_Corregir;
            }
            set
            {
                if (_txtNombre_Corregir != value)
                {
                    _txtNombre_Corregir = value.ToUpper();

                    if (!string.IsNullOrEmpty(value))
                    {
                        if (_DataToSave.Count > 0)
                        {
                            CanGuardar = true;
                        }
                    }

                    this.RaisePropertychanged("txtNombre_Corregir");
                }
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
            }
            set
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
            }
            set
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
            }
            set
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
            }
            set
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
            }
            set
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
            }
            set
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
            }
            set
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
            }
            set
            {
                _ckbFirma_Pet_Inv = value;

                if (_DataToSave.Count > 0)
                {
                    CanGuardar = true;
                }

                this.RaisePropertychanged("ckbFirma_Pet_Inv");
            }
        }
        public string txtOtraRazonDeRechazo
        {
            get
            {
                return _txtOtraRazonDeRechazo;
            }
            set
            {
                _txtOtraRazonDeRechazo = value;
                this.RaisePropertychanged("txtOtraRazonDeRechazo");
            }
        }
        public bool chkOtraRazonDeRechazo
        {
            get
            {
                return _chkOtraRazonDeRechazo;
            }
            set
            {
                _chkOtraRazonDeRechazo = value;
                if (_DataToSave.Count > 0)
                {
                    CanGuardar = true;
                }
                if (!value)
                    txtOtraRazonDeRechazo = string.Empty;

                this.RaisePropertychanged("chkOtraRazonDeRechazo");
            }
        }
        public bool ckbFirma_Not_Inv
        {
            get
            {
                return _ckbFirma_Not_Inv;
            }
            set
            {
                _ckbFirma_Not_Inv = value;

                if (_DataToSave.Count > 0)
                {
                    CanGuardar = true;
                }

                this.RaisePropertychanged("ckbFirma_Not_Inv");
            }
        }
        public string txtFchJuramento_Corregir
        {//Fecha de la Firma del Notario
            get
            {
                return _txtFchJuramento_Corregir;
            }
            set
            {
                _txtFchJuramento_Corregir = value;

                if (value != null)
                {
                    if (_DataToSave.Count > 0)
                    {
                        CanGuardar = true;
                    }
                    _txtFchJuramento_Corregir = jolcode.DateTimeUtil.MyValidarFecha2(value) == null ? value : jolcode.DateTimeUtil.MyValidarFecha2(value);

                    if (_txtFchJuramento_Corregir == null)
                    {
                        ;
                    }

                }

                this.RaisePropertychanged("txtFchJuramento_Corregir");

            }
        }
        public string txtFchFirmaElector_Corregir
        {//Fecha de la firma del elector
            get
            {
                return _txtFchFirmaElector_Corregir;
            }
            set
            {
                _txtFchFirmaElector_Corregir = value;
                if (value != null)
                {
                    if (_DataToSave.Count > 0)
                    {
                        CanGuardar = true;
                    }
                    _txtFchFirmaElector_Corregir = jolcode.DateTimeUtil.MyValidarFecha2(value) == null ? value : jolcode.DateTimeUtil.MyValidarFecha2(value);
                }
                this.RaisePropertychanged("txtFchFirmaElector_Corregir");
            }
        }
        public string txtFchEndosoEntregada_Corregir
        {//Fecha de Recibo del Endoso a la CEE
            get
            {
                return _txtFchEndosoEntregada_Corregir;
            }
            set
            {
                _txtFchEndosoEntregada_Corregir = value;

                if (value != null)
                {
                    if (_DataToSave.Count > 0)
                    {
                        CanGuardar = true;
                    }
                    _txtFchEndosoEntregada_Corregir = jolcode.DateTimeUtil.MyValidarFecha2(value) == null ? value : jolcode.DateTimeUtil.MyValidarFecha2(value);

                }
                this.RaisePropertychanged("txtFchEndosoEntregada_Corregir");

            }
        }
        public string txtNumElec_Go
        {
            get
            {
                return _txtNumElec_Go;
            }
            set
            {
                _txtNumElec_Go = value;
                this.RaisePropertychanged("txtNumElec_Go");
            }
        }
        public int? txtFchEndosoEntregada_Corregir_dias
        {
            get
            {
                return _txtFchEndosoEntregada_Corregir_dias;
            }
            set
            {
                if (value != null)
                {
                    _txtFchEndosoEntregada_Corregir_dias = value;
                    this.RaisePropertychanged("txtFchEndosoEntregada_Corregir_dias");
                }
            }
        }

        public string FechaNac_Corregir
        {
            get
            {
                return _FechaNac_Corregir;
            }
            set
            {
                _FechaNac_Corregir = value;
                if (value != null)
                {
                    if (_DataToSave.Count > 0)
                    {
                        CanGuardar = true;
                    }
                    _FechaNac_Corregir = jolcode.DateTimeUtil.MyValidarFecha2(value) == null ? value : jolcode.DateTimeUtil.MyValidarFecha2(value);
                }

                this.RaisePropertychanged("FechaNac_Corregir");
            }
        }


        public ObservableCollection<Brush> txtColor
        {
            get
            {
                return _txtColor;
            }
            set
            {
                _txtColor = value;
                this.RaisePropertychanged("txtColor");
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

        public string Lot
        {
            get
            {
                return _Lot;
            }
            set
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
            }
            set
            {
                _TotalRechazada = value;
                this.RaisePropertychanged("TotalRechazada");
            }

        }

        private bool CanGuardar
        {
            get
            {
                return _CanGuardar;
            }
            set
            {
                _CanGuardar = value;
            }
        }
        private bool CanProximo
        {
            get
            {
                if (i >= (TotalRechazada) - 1)
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



        /*Command*/
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

                for (int idx = 0; idx <= 23; idx++)
                {
                    txtColor.Add(Brushes.White);
                }

                MyRefresh(true);

                MyFillField(i);


                TextBox tet = new TextBox();
                tet.Name = "Nombre_Corregir";
                MyGotFocus(tet);

                //MySendTab();

            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool? MyOnShow()
        {
            _wpfFixVoid = this.View as wpfFixVoid;

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

        private void MyBtnGo_Elec()
        {
            int find = 0;
            bool masdeuno = false;
            bool loencontre = false;
            string strIDX = string.Empty;

            for (int f = 0; f < TotalRechazada; f++)
            {

                if (_DataToSave[f].Numelec == txtNumElec_Go)
                {
                    if (loencontre)
                        masdeuno = true;

                    find = f;
                    strIDX += f.ToString() + "|";
                    loencontre = true;
                }

            }

            if (masdeuno)
                MessageBox.Show("Este numero electoral esta varias veces en esta radicación. Números de Página:" + strIDX, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            if (!loencontre)
                MessageBox.Show("Este numero electoral no esta en esta radicación. ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            SaveTmp();
            MyFillField(find);
            i = find;
            GoIdx = i;
            i_Display = i + 1;
            CanGuardar = true;
            txtNumElec_Go = string.Empty;
        }
        public RelayCommand BtnGo_Elec
        {
            get; private set;
        }
        private void MyCmdProximo_Click()
        {
            SaveTmp();

            i++;
            int TotalRechazada_tmp = TotalRechazada - 1;

            if (i >= (TotalRechazada_tmp))
                i = TotalRechazada_tmp;

            i_Display = i + 1;
            MyFillField(i);

            GoIdx = i;

            CanGuardar = true;
        }
        private void MycmdLast()
        {
            SaveTmp();
            MyFillField(TotalRechazada - 1);

            i = TotalRechazada - 1;
            GoIdx = i;
            i_Display = i + 1;
            CanGuardar = true;


        }
        public RelayCommand cmdLast
        {
            get; private set;
        }
        public RelayCommand cmdFirst { get; private set; }
        private void MycmdFirst()
        {
            SaveTmp();
            MyFillField(0);
            i = 0;
            GoIdx = 0;
            i_Display = 1;
            CanGuardar = true;

        }
        private void MyBtnGo()
        {
            SaveTmp();

            if (GoIdx < 0)
                GoIdx = 0;

            int TotalRechazada_tmp = TotalRechazada - 1;

            if (GoIdx >= (TotalRechazada_tmp))
                GoIdx = TotalRechazada_tmp;

            MyFillField(GoIdx);
            CanGuardar = true;
            i = GoIdx;


            i_Display = i + 1;


        }
        public RelayCommand BtnGo
        {
            get; private set;
        }

        private void MyCmdAnterior_Click()
        {
            SaveTmp();

            i--;

            if (i < 0)
                i = 0;

            i_Display = i + 1;

            MyFillField(i);

            GoIdx = i;

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
                                string FechaNac = null;

                                string Fecha_Recibo_CEE = null;
                                string Fecha_Firma_Elector = null;
                                string Fecha_Firma_Notario = null;

                                if (data.FechaNac != null)
                                    FechaNac = data.FechaNac.Value.ToString("MMddyyyy");

                                if (data.Firma_Fecha_Notario != null)
                                    Fecha_Firma_Notario = data.Firma_Fecha_Notario.Value.ToString("MM/dd/yy");

                                if (data.Fecha_Recibo_CEE != null)
                                    Fecha_Recibo_CEE = data.Fecha_Recibo_CEE.Value.ToString("MM/dd/yyyy");

                                if (data.Firma_Fecha_Elector != null)
                                    Fecha_Firma_Elector = data.Firma_Fecha_Elector.Value.ToString("MM/dd/yyyy");

                                Exe.MyUpdateTFTable(
                                    data.FirstName,
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
                                   data.chkOtraRazonDeRechazo == true ? "1" : "0",
                                   data.txtOtraRazonDeRechazo,
                                   Fecha_Firma_Elector,
                                   Fecha_Firma_Notario,
                                   Fecha_Recibo_CEE,
                                   data.Leer_Inv,
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
        private void MyCmdSetImg()
        {
            try
            {
                using (vmSetImages frmSetImg = new vmSetImages())
                {
                    frmSetImg.View.Owner = this.View as Window;
                    frmSetImg.Source_image = Source_image;
                    frmSetImg.ViewboxHeight = ViewboxHeight;
                    frmSetImg.ViewboxWidth = ViewboxWidth;
                    frmSetImg.imgEndoso = imgEndoso;
                    frmSetImg.srcEndoso = srcEndoso;
                    frmSetImg.MyOnShow();
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                    lblCordenadas = string.Format("int X = {0};int Y = {1};int H = {2};int W = {3};int V = {4};int Ho = {5};string fieldContents  = X.ToString() + \"|\" + Y.ToString() + \"|\" + H.ToString() + \"|\" + W.ToString()+ \"|\" + V.ToString()+\"|\" + Ho.ToString() ;return fieldContents;", _MyX, _MyY, _SelectionRectangle.Height, _SelectionRectangle.Width, _sv.VerticalOffset, _sv.HorizontalOffset);

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
            BitmapSource displayImage = new CroppedBitmap(srcEndoso, new Int32Rect(0, 0, 0, 0));  // new CroppedBitmap(original, crop);
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

                txt.Background = tmpBrushes;
                switch (txt.Name)
                {
                    case "txtFchEndoso_Corregir":
                    case "txtFchEndosoEntregada_Corregir":
                    case "txtFchFirmaElector_Corregir":
                        {
                            DateTime dt;
                            DateTime? dtNull = null;
                            if (DateTime.TryParseExact(txt.Text, _formatsDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                            {
                                dtNull = dt;
                            }
                            try
                            {
                                txt.Text = dtNull.Value.ToString("dd/MM/yyyy");
                            }
                            catch
                            {
                                MessageBox.Show("Error con el Formato de la Fecha dd/MM/yyyy", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        break;
                }
            }
            if (param is Label)
            {
                var txt = param as Label;
                txt.Background = tmpBrushes;

            }
            if (param is DatePicker)
            {
                var txt = param as DatePicker;
                txt.Background = tmpBrushes;

            }
            tmpBrushes = Brushes.White;

            if (_isFirmaNotario)
            {
                _isFirmaNotario = false;
                if (_myTableImgFirmaElector.Rows.Count > 0)
                {
                    byte[] dataSignature = (byte[])_myTableImgFirmaElector.Rows[0]["SignatureImage"];
                    MemoryStream strmSignature = new MemoryStream();
                    strmSignature.Write(dataSignature, 0, dataSignature.Length);
                    strmSignature.Position = 0;
                    System.Drawing.Image img = System.Drawing.Image.FromStream(strmSignature);
                    //System.Drawing.Image img = System.Drawing.Image.FromFile(_DataToSave[i].image);


                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    ms.Seek(0, SeekOrigin.Begin);
                    bi.StreamSource = ms;
                    bi.EndInit();

                    ViewboxHeightSing = img.Height;
                    ViewboxWidthSing = img.Width;

                    BitmapSource displayImage = bi;//new CroppedBitmap(bi, new Int32Rect(0, 0,img.Width, img.Width));  // new CroppedBitmap(original, crop);

                    Source_image_Corregir = displayImage;

                }
                else
                    Source_image_Corregir = null;
            }

            return true;
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
                strReturn = "int X = 0;int Y = 0;int H = 0;int W = 0;int V = 0;int Ho = 0;string fieldContents  = X.ToString() + \"|\" + Y.ToString() + \"|\" + H.ToString() + \"|\" + W.ToString()+ \"|\" + V.ToString()+\"|\" + Ho.ToString() ;return fieldContents;";
                jolcode.Registry.write(REGPATH, param, strReturn);
            }
            return strReturn;
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

                txt.SelectAll();

                tmpBrushes = txt.Background;

                txt.Background = Brushes.Red;
            }
            if (name is Label)
            {
                var txt = name as Label;
                contenido = txt.Name;
                tmpBrushes = txt.Background;
                txt.Background = Brushes.Red;

            }
            if (name is DatePicker)
            {
                var txt = name as DatePicker;
                contenido = txt.Name;
                tmpBrushes = txt.Background;
                txt.Background = Brushes.Red;

            }


            try
            {
                /*
                Nombre : X = 6; Y = 321; H = 218; W = 1096; V = 183; Ho = 0;
                ElecNum : X = 344; Y = 1270; H = 184; W = 752; V = 1218; Ho = 241;
                Precinto : X = 598; Y = 1256; H = 162; W = 842; V = 1167; Ho = 385;
                Sexo : X = 723; Y = 1044; H = 239; W = 901; V = 923; Ho = 554;
                FechaNac : X = 1052; Y = 1052; H = 204; W = 642; V = 860; Ho = 554;
                Cargo: X = 49; Y = 1340; H = 176; W = 1103; V = 1097; Ho = 6;
                Candidato : X = 48; Y = 1273; H = 226; W = 1028; V = 1137; Ho = 0;
                Funcionario: X = 60; Y = 1667; H = 243; W = 804; V = 1305; Ho = 0;
                FirmaElec : X = 742; Y = 734; H = 179; W = 921; V = 507; Ho = 554;
                FirmaFuncionario: X = 581; Y = 1714; H = 280; W = 979; V = 1557; Ho = 554;
                FechaFirma : X = 230; Y = 1586; H = 244; W = 970; V = 1557; Ho = 110;
                FechaRadi : X = 1021; Y = 1454; H = 580; W = 630; V = 1404; Ho = 554;
                */
                string[] strError = new string[0];
                switch (contenido)
                {
                    case "txtNombre_Corregir":
                    case "Nombre_Corregir":
                        {
                            string[] xy = jolcode.MyNombreXY.DynamicCode().Split('|');

                            X = int.Parse(xy[0]);
                            Y = int.Parse(xy[1]);
                            H = int.Parse(xy[2]);
                            W = int.Parse(xy[3]);

                            double dV = double.Parse(xy[4]);
                            V = (int)dV;
                            double dHo = double.Parse(xy[5]);
                            Ho = (int)dHo;
                        }

                        break;
                    case "txtNumElec_Corregir":
                        {
                            string[] xy = jolcode.MyNumElecXY.DynamicCode().Split('|');

                            X = int.Parse(xy[0]);
                            Y = int.Parse(xy[1]);
                            H = int.Parse(xy[2]);
                            W = int.Parse(xy[3]);

                            double dV = double.Parse(xy[4]);
                            V = (int)dV;
                            double dHo = double.Parse(xy[5]);
                            Ho = (int)dHo;
                        }
                        break;

                    case "txtPrecinto_Corregir":
                        {
                            string[] xy = jolcode.MyPrecintoXY.DynamicCode().Split('|');

                            X = int.Parse(xy[0]);
                            Y = int.Parse(xy[1]);
                            H = int.Parse(xy[2]);
                            W = int.Parse(xy[3]);

                            double dV = double.Parse(xy[4]);
                            V = (int)dV;
                            double dHo = double.Parse(xy[5]);
                            Ho = (int)dHo;
                        }
                        break;

                    case "txtSex_Corregir":
                        {
                            string[] xy = jolcode.MySexoXY.DynamicCode().Split('|');

                            X = int.Parse(xy[0]);
                            Y = int.Parse(xy[1]);
                            H = int.Parse(xy[2]);
                            W = int.Parse(xy[3]);

                            double dV = double.Parse(xy[4]);
                            V = (int)dV;
                            double dHo = double.Parse(xy[5]);
                            Ho = (int)dHo;
                        }
                        break;
                    case "FechaNac_Corregir":

                        {
                            string[] xy = jolcode.MyFechaNacXY.DynamicCode().Split('|');

                            X = int.Parse(xy[0]);
                            Y = int.Parse(xy[1]);
                            H = int.Parse(xy[2]);
                            W = int.Parse(xy[3]);

                            double dV = double.Parse(xy[4]);
                            V = (int)dV;
                            double dHo = double.Parse(xy[5]);
                            Ho = (int)dHo;
                        }
                        break;
                    case "txtCargo_Corregir":
                        {
                            string[] xy = jolcode.MyCargoXY.DynamicCode().Split('|');

                            X = int.Parse(xy[0]);
                            Y = int.Parse(xy[1]);
                            H = int.Parse(xy[2]);
                            W = int.Parse(xy[3]);

                            double dV = double.Parse(xy[4]);
                            V = (int)dV;
                            double dHo = double.Parse(xy[5]);
                            Ho = (int)dHo;
                        }
                        break;
                    case "txtCandidato_Corregir":
                        {
                            string[] xy = jolcode.MyCandidatoXY.DynamicCode().Split('|');

                            X = int.Parse(xy[0]);
                            Y = int.Parse(xy[1]);
                            H = int.Parse(xy[2]);
                            W = int.Parse(xy[3]);

                            double dV = double.Parse(xy[4]);
                            V = (int)dV;
                            double dHo = double.Parse(xy[5]);
                            Ho = (int)dHo;
                        }
                        break;
                    case "txtNotarioElec_Corregir":
                        {
                            string[] xy = jolcode.MyNotarioElecXY.DynamicCode().Split('|');

                            X = int.Parse(xy[0]);
                            Y = int.Parse(xy[1]);
                            H = int.Parse(xy[2]);
                            W = int.Parse(xy[3]);

                            double dV = double.Parse(xy[4]);
                            V = (int)dV;
                            double dHo = double.Parse(xy[5]);
                            Ho = (int)dHo;
                        }
                        break;
                    case "txtFchFirmaElector_Corregir":
                        {
                            string[] xy = jolcode.MyFirmaElecXY.DynamicCode().Split('|');

                            X = int.Parse(xy[0]);
                            Y = int.Parse(xy[1]);
                            H = int.Parse(xy[2]);
                            W = int.Parse(xy[3]);

                            double dV = double.Parse(xy[4]);
                            V = (int)dV;
                            double dHo = double.Parse(xy[5]);
                            Ho = (int)dHo;

                        }
                        break;
                    case "txtFirmaElec_Corregir":
                        {
                            string[] xy = jolcode.MyFirmaElecXY.DynamicCode().Split('|');

                            X = int.Parse(xy[0]);
                            Y = int.Parse(xy[1]);
                            H = int.Parse(xy[2]);
                            W = int.Parse(xy[3]);

                            double dV = double.Parse(xy[4]);
                            V = (int)dV;
                            double dHo = double.Parse(xy[5]);
                            Ho = (int)dHo;

                            if (_isFirmaNotario)
                            {
                                _isFirmaNotario = false;
                                if (_myTableImgFirmaElector.Rows.Count > 0)
                                {
                                    byte[] dataSignature = (byte[])_myTableImgFirmaElector.Rows[0]["SignatureImage"];
                                    MemoryStream strmSignature = new MemoryStream();
                                    strmSignature.Write(dataSignature, 0, dataSignature.Length);
                                    strmSignature.Position = 0;
                                    System.Drawing.Image img = System.Drawing.Image.FromStream(strmSignature);
                                    // System.Drawing.Image img = System.Drawing.Image.FromFile(_DataToSave[i].image);

                                    BitmapImage bi = new BitmapImage();
                                    bi.BeginInit();
                                    MemoryStream ms = new MemoryStream();
                                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    ms.Seek(0, SeekOrigin.Begin);
                                    bi.StreamSource = ms;
                                    bi.EndInit();

                                    ViewboxHeightSing = img.Height;
                                    ViewboxWidthSing = img.Width;

                                    BitmapSource displayImage = bi;//new CroppedBitmap(bi, new Int32Rect(0, 0,img.Width, img.Width));  // new CroppedBitmap(original, crop);

                                    Source_image_Corregir = displayImage;

                                }
                                else
                                    Source_image_Corregir = null;
                            }
                        }

                        break;
                    case "txtNotarioFirma_Corregir":
                        {
                            string[] xy = jolcode.MyNotarioFirmaXY.DynamicCode().Split('|');

                            X = int.Parse(xy[0]);
                            Y = int.Parse(xy[1]);
                            H = int.Parse(xy[2]);
                            W = int.Parse(xy[3]);

                            double dV = double.Parse(xy[4]);
                            V = (int)dV;
                            double dHo = double.Parse(xy[5]);
                            Ho = (int)dHo;

                            if (!_isFirmaNotario)
                            {
                                if (_myTableImgNotario.Rows.Count > 0)
                                {
                                    byte[] dataSignature = (byte[])_myTableImgNotario.Rows[0]["SignatureImage"];
                                    MemoryStream strmSignature = new MemoryStream();
                                    strmSignature.Write(dataSignature, 0, dataSignature.Length);
                                    strmSignature.Position = 0;
                                    System.Drawing.Image img = System.Drawing.Image.FromStream(strmSignature);
                                    //System.Drawing.Image img = System.Drawing.Image.FromFile(_DataToSave[i].image);

                                    BitmapImage bi = new BitmapImage();
                                    bi.BeginInit();
                                    MemoryStream ms = new MemoryStream();
                                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    ms.Seek(0, SeekOrigin.Begin);
                                    bi.StreamSource = ms;
                                    bi.EndInit();


                                    ViewboxHeightSing = img.Height;
                                    ViewboxWidthSing = img.Width;

                                    BitmapSource displayImage = bi;//new CroppedBitmap(bi, new Int32Rect(0, 0,img.Width, img.Width));  // new CroppedBitmap(original, crop);

                                    Source_image_Corregir = displayImage;


                                }
                                else
                                    Source_image_Corregir = null;

                                _isFirmaNotario = true;
                            }
                        }

                        break;
                    case "txtFchEndoso_Corregir":
                        {
                            string[] xy = jolcode.MyFchEndosoXY.DynamicCode().Split('|');

                            X = int.Parse(xy[0]);
                            Y = int.Parse(xy[1]);
                            H = int.Parse(xy[2]);
                            W = int.Parse(xy[3]);

                            double dV = double.Parse(xy[4]);
                            V = (int)dV;
                            double dHo = double.Parse(xy[5]);
                            Ho = (int)dHo;
                        }
                        break;
                    case "txtFchEndosoEntregada_Corregir":
                        {
                            string[] xy = jolcode.MyFchEndosoEntregadaXY.DynamicCode().Split('|');

                            X = int.Parse(xy[0]);
                            Y = int.Parse(xy[1]);
                            H = int.Parse(xy[2]);
                            W = int.Parse(xy[3]);

                            double dV = double.Parse(xy[4]);
                            V = (int)dV;
                            double dHo = double.Parse(xy[5]);
                            Ho = (int)dHo;
                        }
                        break;


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ->" + ex.ToString());
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

        public RelayCommand cmdSetImg
        {
            get; private set;
        }
        public RelayCommand cmdZoomInOut
        {
            get; private set;
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
            get; private set;
        }
        public RelayCommand cmdAnterior_Click
        {
            get; private set;
        }
        public RelayCommand cmdVerElec_Click
        {
            get; private set;
        }
        public RelayCommand cmdGuardar_Click
        {
            get; private set;
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
        public RelayCommand SendTab { get; private set; }
        #endregion

        /*Modulos*/
        #region MyModule
        private void MyFillField(int idx)
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
                    Nombre_Corregir = _DataToSave[idx].Nombre;
                    txtNombre_Corregir = _DataToSave[idx].FirstName;
                    txtNumElec_Corregir = _DataToSave[idx].Numelec;
                    txtNotarioNumElec = _DataToSave[idx].NotarioElec;
                    txtPrecinto_Corregir = _DataToSave[idx].Precinto;
                    txtSex_Corregir = _DataToSave[idx].Sexo;
                    txtCargo_Corregir = _DataToSave[idx].Cargo;
                    txtCandidato_Corregir = _DataToSave[idx].Candidato;
                    txtNotarioElec_Corregir = _DataToSave[idx].NotarioElec;

                    txtFirmaElec_Corregir = _DataToSave[idx].FirmaElec;
                    txtNotarioFirma_Corregir = _DataToSave[idx].NotarioFirma;

                    ckbFirma_Pet_Inv = _DataToSave[idx].Firma_Pet_Inv;
                    ckbFirma_Not_Inv = _DataToSave[idx].Firma_Not_Inv;
                    chkOtraRazonDeRechazo = _DataToSave[idx].chkOtraRazonDeRechazo;
                    txtOtraRazonDeRechazo = _DataToSave[idx].txtOtraRazonDeRechazo;

                    txtFormulario = _DataToSave[idx].Formulario;

                    batchTF = _DataToSave[idx].Batch;

                    txtFchEndosoEntregada_Corregir_dias = _DataToSave[idx].Leer_Inv;

                    if (_DataToSave[idx].FechaNac != null)
                        FechaNac_Corregir = _DataToSave[idx].FechaNac.Value.ToString("dd/MM/yyyy");

                    if (_DataToSave[idx].Fecha_Recibo_CEE != null) // Fecha del endosos entregado en la CEE 
                        txtFchEndosoEntregada_Corregir = _DataToSave[idx].Fecha_Recibo_CEE.Value.ToString("dd/MM/yyyy");

                    if (_DataToSave[idx].Firma_Fecha_Elector != null) // Fecha de la Firma del Elector
                        txtFchFirmaElector_Corregir = _DataToSave[idx].Firma_Fecha_Elector.Value.ToString("dd/MM/yyyy");

                    if (_DataToSave[idx].Firma_Fecha_Notario != null) // Nota : firma del Notario 
                        txtFchJuramento_Corregir = _DataToSave[idx].Firma_Fecha_Notario.Value.ToString("dd/MM/yyyy");

                    byte[] EndosoImage = null;
                    Source_image = null;

                    string rechazoNum = string.Empty;
                    txtRazonRechazo = get.MyTipoDeRechazo(_DataToSave[idx].TipoDeRechazo, txtFormulario, Lot, _DataToSave[idx].Batch, ref EndosoImage, ref rechazoNum);

                    if (!string.IsNullOrEmpty(rechazoNum))
                    {
                        SetColor(rechazoNum);
                    }
                    else
                    {
                        EndosoImage = _DataToSave[idx].EndosoImage;
                    }
                    srcEndoso = new BitmapImage();

                    // 'DESPLIEGA la imagen del Endoso
                    if (EndosoImage != null)
                    {
                        MemoryStream strmEndosoImage = new MemoryStream();
                        strmEndosoImage.Write(EndosoImage, 0, EndosoImage.Length);
                        strmEndosoImage.Position = 0;
                        srcEndoso.BeginInit();
                        imgEndoso = System.Drawing.Image.FromStream(strmEndosoImage);
                        //  imgEndoso = System.Drawing.Image.FromFile(_DataToSave[idx].image);

                        MemoryStream ms = new MemoryStream();
                        imgEndoso.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
                        ms.Seek(0, SeekOrigin.Begin);
                        srcEndoso.StreamSource = ms;
                        srcEndoso.EndInit();

                        ViewboxHeight = imgEndoso.Height;
                        ViewboxWidth = imgEndoso.Width;

                        BitmapSource displayImage = new CroppedBitmap(srcEndoso, new Int32Rect(0, 0, imgEndoso.Width, imgEndoso.Width));  // new CroppedBitmap(original, crop);

                        Source_image = displayImage;

                    }
                    else
                        Source_image = null;

                    _myTableImgFirmaElector = new DataTable();
                    _myTableImgNotario = new DataTable();


                    if (txtNumElec_Corregir.Trim().Length > 0)
                    {
                        _tblCitizen = get.MyGetCitizen(txtNumElec_Corregir);

                        _myTableImgFirmaElector = get.MyGetCitizenImg(txtNumElec_Corregir);
                    }
                    else
                    {
                        _tblCitizen = new DataTable();
                        _myTableImgFirmaElector = new DataTable();

                    }

                    DataTable notarioInfo = new DataTable();

                    if (txtNotarioNumElec.Trim().Length > 0)
                    {
                        notarioInfo = get.MyGetCitizen(txtNotarioNumElec);
                        _myTableImgNotario = get.MyGetCitizenImg(txtNotarioNumElec);
                    }
                    if (notarioInfo.Rows.Count > 0)
                    {
                        string[] notario =
                            {
                                        notarioInfo.Rows[0]["FirstName"].ToString()," ",
                                        notarioInfo.Rows[0]["LastName1"].ToString()," ",
                                        notarioInfo.Rows[0]["LastName2"].ToString()
                                };

                        txtNotarioFirstName = string.Concat(notario);

                        switch (notarioInfo.Rows[0]["Status"].ToString().Trim().ToUpper())
                        {
                            case "A":
                                txtStatusNotario = "Activo";
                                break;
                            case "E":
                                txtStatusNotario = "Excluido";
                                break;
                            case "I":
                                txtStatusNotario = "Inactivo";
                                break;
                            default:
                                txtStatusNotario = "?";
                                break;
                        }
                    }
                    else
                    {
                        txtNotarioFirstName = "No hay Datos en el Master";
                        txtStatusNotario = "?";
                    }
                    Source_image_Corregir = null;

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

                        switch (_tblCitizen.Rows[0]["Status"].ToString().Trim().ToUpper())
                        {
                            case "A":
                                txtStatusElec = "Activo";
                                break;
                            case "E":
                                txtStatusElec = "Excluido";
                                break;
                            case "I":
                                txtStatusElec = "Inactivo";
                                break;
                            default:
                                txtStatusElec = "?";
                                break;
                        }


                        txtFechaNac = _tblCitizen.Rows[0]["DateOfBirth"].ToString().Trim();
                        if (!string.IsNullOrEmpty(txtFechaNac))
                        {
                            string Mes = txtFechaNac.Split('/')[0].Trim().PadLeft(2, '0');
                            string Dia = txtFechaNac.Split('/')[1].Trim().PadLeft(2, '0');
                            string Ano = txtFechaNac.Split('/')[2].Trim().Substring(0, 4);
                            txtFechaNac = Dia + "/" + Mes + "/" + Ano;
                        }

                        if (_myTableImgFirmaElector.Rows.Count > 0)
                        {//Del Master de la CEE
                            byte[] dataSignature = (byte[])_myTableImgFirmaElector.Rows[0]["SignatureImage"];
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

                            ViewboxHeightSing = img.Height;
                            ViewboxWidthSing = img.Width;

                            BitmapSource displayImage = bi;//new CroppedBitmap(bi, new Int32Rect(0, 0,img.Width, img.Width));  // new CroppedBitmap(original, crop);

                            Source_image_Corregir = displayImage;

                        }
                        else
                            Source_image_Corregir = null;

                    }
                    else
                    {
                        txtNumElec = txtNumElec_Corregir;

                        txtNombre = "No Hay Datos";
                        txtPrecinto = "???";
                        txtSex = "?";
                        txtFechaNac = "No Hay Datos";
                        txtStatusElec = "?";

                    }
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.ToString(), site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SetColor(string param)
        {
            param = param.Substring(0, param.Length - 1);
            string[] cargos = param.Split('|');

            for (int idx = 0; idx <= 23; idx++)
            {
                txtColor[idx] = Brushes.White;
            }

            foreach (string s in cargos)
            {
                int idx = int.Parse(s);

                txtColor[idx] = Brushes.Yellow;

                if (idx== 23)
                    txtColor[0] = Brushes.GreenYellow;

                if ((idx == 11) || (idx == 20) || idx == 21)
                    txtColor[6] = Brushes.GreenYellow;

                if (idx == 19)
                    txtColor[15] = Brushes.GreenYellow;

                if (idx == 8)
                    txtColor[4] = Brushes.GreenYellow;
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
            txtFchJuramento_Corregir = null;
            txtFchEndosoEntregada_Corregir = null;
            txtFchFirmaElector_Corregir = null;
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
            batchTF = string.Empty;
            txtStatusElec = string.Empty;
            txtStatusNotario = string.Empty;
            txtOtraRazonDeRechazo = string.Empty;
            chkOtraRazonDeRechazo = false;

            for (int idx = 0; idx <= 21; idx++)
            {
                txtColor[idx] = Brushes.White;
            }

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

                    MyLotsTable = get.MyGetLotToFixVoid(Lot, isAll);

                    if (MyLotsTable.Rows.Count == 0)
                        MessageBox.Show("No hay rechazadas para procesar", "No Hay", MessageBoxButton.OK, MessageBoxImage.Information);

                    int k = 0;
                    TotalRechazada = MyLotsTable.Rows.Count;
                    foreach (DataRow row in MyLotsTable.Rows)
                    {//12/01/0015
                     //string FechaNac_Mes = row["FechaNac_Mes"].ToString().Trim().PadLeft(2,'0');
                     //string FechaNac_Dia = row["FechaNac_Dia"].ToString().Trim().PadLeft(2,'0');
                     //string FechaNac_Ano = row["FechaNac_Ano"].ToString().Trim().PadLeft(4,'0');
                     //[FechaNac]
                     // string FechaNac = FechaNac_Mes + FechaNac_Dia + FechaNac_Ano;
                        string FechaNac = row["FechaNac"].ToString().Trim();

                        //string FechaFirm_Mes = row["FechaFirm_Mes"].ToString().Trim().PadLeft(2, '0');
                        //string FechaFirm_Dia = row["FechaFirm_Dia"].ToString().Trim().PadLeft(2, '0');
                        //string FechaFirm_Ano = row["FechaFirm_Ano"].ToString().Trim().PadLeft(4, '0');

                        //  string Fecha_Endoso = FechaFirm_Mes + FechaFirm_Dia + FechaFirm_Ano;

                        string Fecha_Endoso_Notario = row["Fecha_Endoso"].ToString().Trim(); //Fecha Firma notario
                        string Firma_Fecha_Elector = row["Firma_Fecha"].ToString().Trim();   //Fecha Firma elector
                        string Fecha_Recibo_CEE = row["Fecha_Recibo"].ToString().Trim();     //Fecha Entragado el endosos a la CEE

                        DateTime? FirmaFechaElector = DateTimeUtil.MyValidarFecha(Firma_Fecha_Elector);
                        DateTime? FechaEndosoNotario = DateTimeUtil.MyValidarFecha(Fecha_Endoso_Notario);
                        DateTime? FechaReciboCEE = DateTimeUtil.MyValidarFecha(Fecha_Recibo_CEE);

                        if (FechaReciboCEE == null)
                            FechaReciboCEE = DateTimeUtil.MyValidarFechaMMddyy(Fecha_Recibo_CEE);

                        if (FirmaFechaElector == null)
                            FirmaFechaElector = DateTimeUtil.MyValidarFechaMMddyy(Firma_Fecha_Elector);

                        if (FechaEndosoNotario == null)
                            FechaEndosoNotario = DateTimeUtil.MyValidarFechaMMddyy(Fecha_Endoso_Notario);



                        int Leer_Inv = 0;

                        int.TryParse(row["Leer_Inv"].ToString(), out Leer_Inv);

                        _DataToSave.Add(new FixVoid
                        {
                            i = k,
                            CurrElect = row["NumElec"].ToString(),
                            Nombre = row["Nombre"].ToString().Trim() + " " + row["Paterno"].ToString().Trim() + " " + row["Materno"].ToString(),
                            FirstName = row["Nombre"].ToString().Trim(),
                            Lot = Lot,
                            Formulario = row["Formulario"].ToString().Trim(),
                            TipoDeRechazo = "",
                            Numelec = row["NumElec"].ToString(),
                            NotarioElec = row["Notario"].ToString(),
                            Precinto = row["Precinto"].ToString().Trim().PadLeft(3, '0'),
                            FechaNac = DateTimeUtil.MyValidarFecha(FechaNac),
                            Sexo = row["Sexo"].ToString().Trim(),
                            Candidato = row["Candidato"].ToString().Trim(),
                            Cargo = row["Cargo"].ToString().Trim(),
                            FirmaElec = row["Firma_Peticionario"].ToString().Trim(),
                            NotarioFirma = row["Firma_Notario"].ToString().Trim(),
                            Firma_Pet_Inv = row["Firma_Pet_Inv"].ToString().Trim() == "1" ? true : false,
                            Firma_Not_Inv = row["Firma_Not_Inv"].ToString().Trim() == "1" ? true : false,
                            chkOtraRazonDeRechazo = row["Alteracion"].ToString().Trim() == "1" ? true : false,
                            txtOtraRazonDeRechazo = row["LeerMSG"].ToString(),
                            Firma_Fecha_Notario = FechaEndosoNotario,
                            Firma_Fecha_Elector = FirmaFechaElector,
                            Fecha_Recibo_CEE = FechaReciboCEE,
                            Batch = row["Batch"].ToString(),
                            image = row["Image"].ToString(),
                            EndosoImage = (byte[])row["EndosoImage"],
                            Leer_Inv = Leer_Inv

                        });
                        k++;
                    }
                }
            }
        }

        private void SaveTmp()
        {
            DateTime dtNac;
            DateTime? dtNacNull = null;

            DateTime dtFecha_Recibo_CEE;
            DateTime? dtFecha_Recibo_CEEnull = null;

            DateTime dtFecha_Firma_Notario_Corregir;
            DateTime? dtFecha_Firma_Notario_Corregirnull = null;

            DateTime dtFirma_Fecha_Elector;
            DateTime? dtFirma_Fecha_Electornull = null;

            if (DateTime.TryParseExact(txtFchFirmaElector_Corregir, _formatsDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtFirma_Fecha_Elector))
            {//Fecha de la Firma del Elector
                dtFirma_Fecha_Electornull = dtFirma_Fecha_Elector;
            }


            if (DateTime.TryParseExact(txtFchJuramento_Corregir, _formatsDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtFecha_Firma_Notario_Corregir))
            {//Fecha de la Firma del Notario
                dtFecha_Firma_Notario_Corregirnull = dtFecha_Firma_Notario_Corregir;
            }

            if (DateTime.TryParseExact(txtFchEndosoEntregada_Corregir, _formatsDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtFecha_Recibo_CEE))
            {//Fecha de Recibo del en endoso a la cee
                dtFecha_Recibo_CEEnull = dtFecha_Recibo_CEE;
            }

            if (DateTime.TryParseExact(FechaNac_Corregir, _formatsDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtNac))
            {//Fecha Nacimiento del Elector
                dtNacNull = dtNac;
            }

            _DataToSave[i].i = i;
            _DataToSave[i].Lot = Lot;
            _DataToSave[i].Formulario = txtFormulario;
            // _DataToSave[i].TipoDeRechazo = row["TipoDeRechazo"].ToString(),

            //for (int tmp=0;tmp < TotalRechazada;tmp++)
            //        _DataToSave[tmp].Leer_Inv = txtFchEndosoEntregada_Corregir_dias; // esto es para cambiar a todos los endosos la cantidad de dedias 

            _DataToSave[i].Leer_Inv = txtFchEndosoEntregada_Corregir_dias;
            _DataToSave[i].FirstName = txtNombre_Corregir;
            _DataToSave[i].Numelec = txtNumElec_Corregir;
            _DataToSave[i].NotarioElec = txtNotarioElec_Corregir;
            _DataToSave[i].Precinto = txtPrecinto_Corregir;
            _DataToSave[i].FechaNac = dtNacNull;
            _DataToSave[i].Sexo = txtSex_Corregir;
            _DataToSave[i].Candidato = txtCandidato_Corregir;
            _DataToSave[i].Cargo = txtCargo_Corregir;
            _DataToSave[i].FirmaElec = txtFirmaElec_Corregir;
            _DataToSave[i].NotarioFirma = txtNotarioFirma_Corregir;
            _DataToSave[i].Firma_Pet_Inv = ckbFirma_Pet_Inv;
            _DataToSave[i].Firma_Not_Inv = ckbFirma_Not_Inv;
            _DataToSave[i].chkOtraRazonDeRechazo = chkOtraRazonDeRechazo;
            _DataToSave[i].txtOtraRazonDeRechazo = txtOtraRazonDeRechazo;
            _DataToSave[i].Firma_Fecha_Notario = dtFecha_Firma_Notario_Corregirnull;
            _DataToSave[i].Firma_Fecha_Elector = dtFirma_Fecha_Electornull;
            _DataToSave[i].Fecha_Recibo_CEE = dtFecha_Recibo_CEEnull;

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

        /*Dispose*/
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
