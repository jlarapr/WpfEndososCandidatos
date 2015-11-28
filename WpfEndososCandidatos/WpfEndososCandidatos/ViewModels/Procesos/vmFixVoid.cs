﻿using jolcode;
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

        public vmFixVoid() :
            base(new wpfFixVoid())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdProximo_Click = new RelayCommand(param => MyCmdProximo_Click());
            cmdAnterior_Click = new RelayCommand(param => MyCmdAnterior_Click());
            cmdVerElec_Click = new RelayCommand(param => MyCmdVerElec_Click());
            cmdGuardar_Click = new RelayCommand(param => MyCmdGuardar_Click(), param => CanGuardar);
        }

        #region MyProperty
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
                _txtSex = value;
                this.RaisePropertychanged("txtSex");
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

        public string txtNumElec_Corregir
        {
            get
            {
                return _txtNumElec_Corregir;
            }
            set
            {
                _txtNumElec_Corregir = value;
                this.RaisePropertychanged("txtNumElec_Corregir");
            }
        }

        public string txtPrecinto_Corregir
        {
            get
            {
                return _txtPrecinto_Corregir;
            }set
            {
                _txtPrecinto_Corregir = value;
                this.RaisePropertychanged("txtPrecinto_Corregir");
            }
        }
        public string txtSex_Corregir
        {
            get
            {
                return _txtSex_Corregir;
            }set
            {
                _txtSex_Corregir = value;
                this.RaisePropertychanged("txtSex_Corregir");
            }
        }
        public string txtCargo_Corregir
        {
            get
            {
                return _txtCargo_Corregir;
            }set
            {
                _txtCargo_Corregir = value;
                this.RaisePropertychanged("txtCargo_Corregir");
            }
        }
        public string txtCandidato_Corregir
        {
            get
            {
                return _txtCandidato_Corregir;
            }set
            {
                _txtCandidato_Corregir = value;
                this.RaisePropertychanged("txtCandidato_Corregir");
            }
        }
        public string txtNotarioElec_Corregir
        {
            get
            {
                return _txtNotarioElec_Corregir;
            }set
            {
                _txtNotarioElec_Corregir = value;
                this.RaisePropertychanged("txtNotarioElec_Corregir");
            }
        }
        public string txtFirmaElec_Corregir
        {
            get
            {
                return _txtFirmaElec_Corregir;
            }set
            {
                _txtFirmaElec_Corregir = value;
                this.RaisePropertychanged("txtFirmaElec_Corregir");
            }
        }
        public string txtNotarioFirma_Corregir
        {
            get
            {
                return _txtNotarioFirma_Corregir;
            }set
            {
                _txtNotarioFirma_Corregir = value;
                this.RaisePropertychanged("txtNotarioFirma_Corregir");
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
                this.RaisePropertychanged("txtFchEndosoEntregada_Corregir");
            }
        }
        public string txtFormulario
        {
            get
            {
                return _txtFormulario;
            }set
            {
                _txtFormulario = value;
                this.RaisePropertychanged("txtFormulario");
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
                    BorderColor = b;
                }
                else
                    BorderColor = Brushes.Black;


                MyRefresh(true);

                MyFillField();

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
            try
            {

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
                    DBCeeMasterCnnStr = DBMasterCeeCnnStr

                })
                {

                    txtNumElec_Corregir = _DataToSave[i].Numelec;
                    txtNotarioNumElec = _DataToSave[i].NotarioElec;
                    txtPrecinto_Corregir = _DataToSave[i].Precinto;
                    txtSex_Corregir =_DataToSave[i].Sexo;
                    txtCargo_Corregir =_DataToSave[i].Cargo ;
                    txtCandidato_Corregir =_DataToSave[i].Candidato ;
                    txtNotarioElec_Corregir = _DataToSave[i].NotarioElec;

                    txtFirmaElec_Corregir =_DataToSave[i].FirmaElec;
                    txtNotarioFirma_Corregir = _DataToSave[i].NotarioFirma;

                    ckbFirma_Pet_Inv =_DataToSave[i].Firma_Pet_Inv;
                    ckbFirma_Not_Inv = _DataToSave[i].Firma_Not_Inv;

                    txtFchEndoso_Corregir = _DataToSave[i].FchEndoso;
                    txtFormulario = _DataToSave[i].Formulario;
                    FechaNac_Corregir = _DataToSave[i].FechaNac;

                    txtFchEndosoEntregada_Corregir = _DataToSave[i].FchEndosoEntregada;

                    if (txtFchEndoso_Corregir == null) // Nota : Hay que preguntar cual es la diferencia de la Fecha_Endoso y Firma_Fecha
                        txtFchEndoso_Corregir = _DataToSave[i].Firma_Fecha;


                    txtRazonRechazo = get.MyTipoDeRechazo(_DataToSave[i].TipoDeRechazo);

                    _tblCitizen = get.MyGetCitizen(txtNumElec_Corregir);

                    DataTable notarioInfo = get.MyGetCitizen(txtNotarioNumElec);

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
                                        _tblCitizen.Rows[0]["FirstName"].ToString()," ",
                                        _tblCitizen.Rows[0]["LastName1"].ToString()," ",
                                        _tblCitizen.Rows[0]["LastName2"].ToString()
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
                    }
                    else
                    {
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
            CanGuardar = false;

            if (resetAllData)
            {
                i = 0;
                i_Display = 1;
                TotalRechazada = 0;
                _DataToSave = new List<FixVoid>();

                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    MyLotsTable = get.MyGetLotToFixVoid(Lot);


                    if (_MyLotsTable.Rows.Count == 0)
                        MessageBox.Show("No hay rechazadas para procesar", "No Hay", MessageBoxButton.OK, MessageBoxImage.Information);


                    TotalRechazada = _MyLotsTable.Rows.Count;
                    foreach (DataRow row in _MyLotsTable.Rows)
                    {
                        _DataToSave.Add(new FixVoid
                        {
                            Lot = Lot,
                            Formulario = row["Formulario"].ToString().Trim(),
                            TipoDeRechazo = row["TipoDeRechazo"].ToString(),
                            Numelec = row["Numelec"].ToString(),
                            NotarioElec = row["Notario"].ToString(),
                            Precinto = row["Precinto"].ToString().Trim().PadLeft(3, '0'),
                            FechaNac = DateTimeUtil.MyValidarFecha(row["FechaNac"].ToString().Trim()),
                            Sexo = row["Sexo"].ToString().Trim(),
                            Candidato = row["Candidato"].ToString().Trim(),
                            Cargo = row["Cargo"].ToString().Trim(),
                            FirmaElec = row["Firma_Peticionario"].ToString().Trim(),
                            NotarioFirma = row["Firma_Notario"].ToString().Trim(),
                            Firma_Pet_Inv = row["Firma_Pet_Inv"].ToString().Trim() == "1" ? true : false,
                            Firma_Not_Inv = row["Firma_Not_Inv"].ToString().Trim() == "1" ? true : false,
                            FchEndoso = DateTimeUtil.MyValidarFecha(row["Fecha_Endoso"].ToString().Trim()),
                            Firma_Fecha = DateTimeUtil.MyValidarFecha(row["Firma_Fecha"].ToString().Trim()),
                            FchEndosoEntregada = null,
                            Batch =row["Batch"].ToString(),
                            image = row["image"].ToString(),
                        });

                    }

                }
            }
        }

        private void SaveTmp()
        {

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
