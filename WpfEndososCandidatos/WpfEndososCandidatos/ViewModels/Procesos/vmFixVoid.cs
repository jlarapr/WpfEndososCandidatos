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

        public vmFixVoid() :
            base(new wpfFixVoid())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            
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
                this.RaisePropertychanged("i");
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
        public string txtNumElec_Corregir
        {
            get
            {
                return _txtNumElec_Corregir;
            }set
            {
                _txtNumElec_Corregir = value;
                this.RaisePropertychanged("txtNumElec_Corregir");
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
                _Lot = value;
                this.RaisePropertychanged("Lot");
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



                MyRefresh();

                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr,
                    DBCeeMasterCnnStr = DBMasterCeeCnnStr

                })
                {
                    TextBlock obTextBlock = new TextBlock();

                    //Binding TipoDeRechazo = new Binding();
                    //TipoDeRechazo.Source = this;
                    ////TipoDeRechazo.Path = new PropertyPath("Rows[0][TipoDeRechazo]");
                    //TipoDeRechazo.Path = new PropertyPath("txtRazonRechazo");
                    //TipoDeRechazo.Mode = BindingMode.TwoWay;
                    //TipoDeRechazo.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    //BindingOperations.SetBinding(obTextBlock, TextBlock.TextProperty, TipoDeRechazo);


                    ////   txtRazonRechazo.SetBinding(TextBox.TextProperty,TipoDeRechazo);

                    //TextBlock obTextBlock1 = new TextBlock();

                    //Binding g = new Binding();
                    //g.Source = _MyLotsTable;
                    //g.Path = new PropertyPath("Rows[0][TipoDeRechazo]");
                    //obTextBlock1.SetBinding(TextBlock.TextProperty, g);

                    //obTextBlock.Text = obTextBlock1.Text;


                    txtNumElec_Corregir = _MyLotsTable.Rows[0]["Numelec"].ToString();
                    txtNotarioNumElec = _MyLotsTable.Rows[0]["Notario"].ToString();

                    txtPrecinto_Corregir = _MyLotsTable.Rows[0]["Precinto"].ToString().Trim().PadLeft(3,'0');
                    txtSex_Corregir = _MyLotsTable.Rows[0]["Sexo"].ToString().Trim();
                    txtCargo_Corregir = _MyLotsTable.Rows[0]["Cargo"].ToString().Trim();
                    txtCandidato_Corregir = _MyLotsTable.Rows[0]["Candidato"].ToString().Trim();
                    txtNotarioElec_Corregir = txtNotarioNumElec;
                    txtFirmaElec_Corregir = _MyLotsTable.Rows[0]["Candidato"].ToString().Trim();


                    txtRazonRechazo = get.MyTipoDeRechazo(MyLotsTable.Rows[0]["TipoDeRechazo"].ToString());

                    _tblCitizen = get.MyGetCitizen(txtNumElec_Corregir);
                    DataTable notarioInfo = get.MyGetCitizen(txtNotarioNumElec);

                    if (notarioInfo.Rows.Count >0)
                    {
                        string[] notario ={

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
                            txtFechaNac = Mes + "//" + Dia + "//" + Ano;
                        }
                    }else
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

        #endregion



        #region MyModule
        private void MyRefresh()
        {
            TotalRechazada = 0;
            txtRazonRechazo = string.Empty;

            txtNumElec_Corregir = string.Empty;
            txtPrecinto_Corregir = string.Empty;
            txtSex_Corregir = string.Empty;
            txtCargo_Corregir = string.Empty;
            txtCandidato_Corregir = string.Empty;
            txtNotarioElec_Corregir = string.Empty;
            txtFirmaElec_Corregir = string.Empty;


            txtNumElec = string.Empty;
            txtNombre = string.Empty;
            txtPrecinto = string.Empty;
            txtSex = string.Empty;
            txtFechaNac = string.Empty;
            txtNotarioNumElec = string.Empty;
            txtNotarioFirstName = string.Empty;
            i = 0;

            using (SqlExcuteCommand get = new SqlExcuteCommand()
            {
                DBCnnStr = DBEndososCnnStr
            })
            {
                MyLotsTable = get.MyGetLotToFixVoid(Lot);


                if (_MyLotsTable.Rows.Count == 0)
                    MessageBox.Show("No hay rechazadas para procesar", "No Hay", MessageBoxButton.OK, MessageBoxImage.Information);


                TotalRechazada = _MyLotsTable.Rows.Count;


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
