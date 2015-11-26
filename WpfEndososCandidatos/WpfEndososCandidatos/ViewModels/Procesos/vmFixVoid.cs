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
                    DBCnnStr = DBEndososCnnStr
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




                    //txtRazonRechazo = _MyLotsTable.Rows[1]["Numelec"].ToString();












                    txtRazonRechazo = get.MyTipoDeRechazo(MyLotsTable.Rows[0]["TipoDeRechazo"].ToString());



                    //txtNumElec_Corregir = _MyLotsTable.Rows[1]["Numelec"].ToString();


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
