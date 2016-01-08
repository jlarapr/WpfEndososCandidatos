using jolcode;
using jolcode.Base;
using jolcode.MyInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfEndososCandidatos.View.Informes;
using WpfEndososCandidatos.Models;
using System.Data;

namespace WpfEndososCandidatos.ViewModels.Informes
{

    class vmInformeReydi : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private string _DBMasterCeeCnnStr;
        private string _DBEndososCnnStr;
        private string _DBCeeMasterImgCnnStr;
        private string _PDFPath;
        private Brush _BorderBrush;
        private ObservableCollection<InfoReydi> _ItemsSource;
        private DataTable _T;
        private string _DBRadicacionesCEECnnStr;
        private string _txtTotal;

        public vmInformeReydi() : base(new wpfInformeReydi())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdRefresh_Click = new RelayCommand(param => MyCmdRefresh_Click(param));
            cmdToExcel_Click = new RelayCommand(param => MycmdToExcel_Click());
            ItemsSource = new ObservableCollection<Models.InfoReydi>();
        }

        #region Property
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
        public ObservableCollection<Models.InfoReydi> ItemsSource
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
        public string txtTotal
        {
            get
            {
                return _txtTotal;
            }set
            {
               
                _txtTotal = value;
                this.RaisePropertychanged("txtTotal");
            }
        }
        #endregion

        #region MyCmd
        private void MycmdToExcel_Click()
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


        private void MyCmdRefresh_Click(object param)
        {
            try
            {
                MyRefresh(param.ToString());
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
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

                MyRefresh("1");

            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
        public bool? MyOnShow()
        {
            return this.View.ShowDialog();
        }
        public RelayCommand cmdRefresh_Click
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
        public RelayCommand cmdToExcel_Click
        {
            get;private set;
        }

        #endregion

        #region Metodos
        private void MyRefresh(string siEnReydi)
        {
            try
            {
                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr,
                    DBCeeMasterCnnStr = DBCeeMasterImgCnnStr,
                    DBRadicacionesCEECnnStr = DBRadicacionesCEECnnStr
                })
                {
                    txtTotal = "0";
                    _T = get.MyGetLotInReydi(siEnReydi);
                    txtTotal =  string.Format("{0:0,0}" ,_T.Rows.Count);
                    ItemsSource = new ObservableCollection<InfoReydi>();

                    foreach (DataRow row in _T.Rows)
                    {
                        int Amount=0;
                        int ValidatedEndorsements = 0;
                        int RejectedEndorsements = 0;
                        int Num_Candidato = 0;

                        int.TryParse(row["Amount"].ToString(),out Amount);
                        int.TryParse(row["ValidatedEndorsements"].ToString(),out ValidatedEndorsements);
                        int.TryParse(row["RejectedEndorsements"].ToString(), out RejectedEndorsements);
                        int.TryParse(row["Num_Candidato"].ToString(), out Num_Candidato);

                        ItemsSource.Add(new InfoReydi()
                        {
                            Lot = row["Lot"].ToString(),
                            Num_Candidato = Num_Candidato,
                            Nombre = get.MyCandidatoNameToInforme(row["Num_Candidato"].ToString()),
                            TotalDeEndosos = Amount,
                            ValidatedEndorsements = ValidatedEndorsements,
                            RejectedEndorsements = RejectedEndorsements,
                            FinUser = row["FinUser"].ToString(),
                            FinDate = row["FinDate"].ToString(),
                            StatusReydi = row["StatusReydi"].ToString() == "1"?"SI":"NO"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion


        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmInformeReydi()
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
