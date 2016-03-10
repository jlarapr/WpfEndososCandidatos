using jolcode;
using jolcode.Base;
using jolcode.MyInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WpfEndososCandidatos.View.Procesos;

namespace WpfEndososCandidatos.ViewModels.Procesos
{
    class vmSubsanar: ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private System.Data.SqlClient.SqlDataAdapter _DA;
        private System.Data.SqlClient.SqlConnection _cnn;
        private string _txtTotalDeRadicaciones;
        private string _EndorsementGroupCode;
        private int _ValidatedEndorsements;
       
        private DataView _ReymiTable;
        private string _txtTotalEndososEndososEnLaRadicacion;
        private int _RejectedEndorsements;
        private int _SelectedValuePath;
        private bool _isDGEnable;

        public vmSubsanar() : base (new wpfSubsanar() )
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click(),param => MyCmdSalir_Click_Can);
            OnBtnBack = new RelayCommand(param => MyOnBtnBack(),param=> MyCmdSalir_Click_Can);
            OnBtnNext = new RelayCommand(param => MyOnBtnNext(),param=> MyCmdSalir_Click_Can);
            OnSelectionChanged = new RelayCommand(param => MyOnSelectionChanged());
            onCancel = new RelayCommand(param => MyonCancel());
            NumeroDeRadicacion = new ObservableCollection<ListaNumeroEndosos>();
        }

      

        public string tableReydi
        {
            get;set;
        }
        public string DBRadicacionesCEECnnStr { get; set; }
        public ObservableCollection<ListaNumeroEndosos> NumeroDeRadicacion { get; set; }

        public bool isDGEnable
        {
            get
            {
                return _isDGEnable;
            }set
            {
                _isDGEnable = value;
                this.RaisePropertychanged("isDGEnable");
            }
        }
        public string txtTotalEndososEndososEnLaRadicacion
        {
            get
            {
                return _txtTotalEndososEndososEnLaRadicacion;
            }set
            {
                _txtTotalEndososEndososEnLaRadicacion = value;
                this.RaisePropertychanged("txtTotalEndososEndososEnLaRadicacion");
            }
        }
        public string txtTotalDeRadicaciones
        {
            get
            {
                return _txtTotalDeRadicaciones;
            }set
            {
                _txtTotalDeRadicaciones = value;
                this.RaisePropertychanged("txtTotalDeRadicaciones");
            }
        }
        public string EndorsementGroupCode
        {
            get
            {
                return _EndorsementGroupCode;
            }set
            {
                _EndorsementGroupCode = value;
                this.RaisePropertychanged("EndorsementGroupCode");
            }
        }
        public int ValidatedEndorsements
        {
            get
            {
                return _ValidatedEndorsements;
            }set
            {
                _ValidatedEndorsements = value;
               
                this.RaisePropertychanged("ValidatedEndorsements");
            }
        }
      public int RejectedEndorsements
        {
            get
            {
                return _RejectedEndorsements;
            }set
            {
                _RejectedEndorsements = value;
                this.RaisePropertychanged("RejectedEndorsements");
            }
        }
        public DataView ReymiTable
        {
            get
            {
                return _ReymiTable;
            }set
            {
                _ReymiTable = value;
                this.RaisePropertychanged("ReymiTable");
            }
        }
        public int SelectedValuePath
        {
            get
            {
                return _SelectedValuePath;
            }set
            {
                _SelectedValuePath = value;
                this.RaisePropertychanged("SelectedValuePath");
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
        public RelayCommand OnBtnBack
        {
            get;private set;
        }
        public RelayCommand OnBtnNext
        {
            get;private set;
        }
        public RelayCommand OnSelectionChanged
        {
            get;private set;
        }
        public RelayCommand onCancel
        {
            get;private set;
        }
        public bool MyCmdSalir_Click_Can { get; private set; }

        private void MyInitWindow()
        {
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

                _cnn = new System.Data.SqlClient.SqlConnection();
                _DA = new System.Data.SqlClient.SqlDataAdapter();

                _cnn.ConnectionString = DBRadicacionesCEECnnStr;
                if (_cnn.State == System.Data.ConnectionState.Closed)
                    _cnn.Open();

                txtTotalDeRadicaciones = NumeroDeRadicacion.Count.ToString("Total de Radicaciones : ###,###,##0");
                MyRefresh();
                //MyReset();
               // txtTotalEndososEndososEnLaRadicacion = (ValidatedEndorsements + RejectedEndorsements).ToString("###,###,##0");
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MyRefresh()
        {

            try
            {


                ReymiTable = new DataView();

                string quereWhere = string.Empty;
                string query = "select [EndorsementGroupCode],[ValidatedEndorsements],[RejectedEndorsements],[EndorsementValidationDate],[ValidatedEndorsements]+[RejectedEndorsements] as Amount from " + tableReydi + " where EndorsementGroupCode in (";

                foreach (ListaNumeroEndosos s in NumeroDeRadicacion)
                {
                    quereWhere += s + ",";
                }
                quereWhere = quereWhere.Substring(0, quereWhere.Length - 1);

                query += quereWhere + ")";

             //   MessageBox.Show(query);
                if (_cnn.State == ConnectionState.Closed)
                    _cnn.Open();

                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand()
                {
                    Connection = _cnn,
                    CommandType = System.Data.CommandType.Text,
                    CommandText = query
                })
                {
                    cmd.CommandText = query;
                    _DA = new System.Data.SqlClient.SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    _DA.Fill(dt);
                    ReymiTable = dt.DefaultView;
                    SelectedValuePath = 0;

                    string v = ReymiTable.Table.Rows[SelectedValuePath]["ValidatedEndorsements"].ToString();
                    string r = ReymiTable.Table.Rows[SelectedValuePath]["RejectedEndorsements"].ToString();

                    int t = int.Parse(v) + int.Parse(r);

                    txtTotalEndososEndososEnLaRadicacion = t.ToString("###,###,##0");

                    // MyCmdSalir_Click_Can = true;

                   

                    //DataView dataView = ReymiTable.DefaultView;

                    //Binding bindMyColumn = new Binding();

                    //bindMyColumn.Source = dataView;

                    //bindMyColumn.Path = new PropertyPath("[0][EndorsementGroupCode]");
                    //EndorsementGroupCode.SetBinding(TextBlock.TextProperty, bindMyColumn);

                }

            }
            catch (Exception ex)
            {
                MyCmdSalir_Click_Can = false;
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MyOnBtnNext()
        {
            try
            {
                if (SelectedValuePath < ReymiTable.Count - 1)
                {
                    SelectedValuePath++;
                    string v = ReymiTable.Table.Rows[SelectedValuePath]["ValidatedEndorsements"].ToString();
                    string r = ReymiTable.Table.Rows[SelectedValuePath]["RejectedEndorsements"].ToString();

                   int t = int.Parse(v) + int.Parse(r);
                    txtTotalEndososEndososEnLaRadicacion = t.ToString("###,###,##0");



                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MyOnBtnBack()
        {
            try
            {
                if (SelectedValuePath <= ReymiTable.Count && SelectedValuePath > 0)
                {
                    SelectedValuePath--;
                    string v = ReymiTable.Table.Rows[SelectedValuePath]["ValidatedEndorsements"].ToString();
                    string r = ReymiTable.Table.Rows[SelectedValuePath]["RejectedEndorsements"].ToString();

                    int t = int.Parse(v) + int.Parse(r);
                    txtTotalEndososEndososEnLaRadicacion = t.ToString("###,###,##0");
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MyOnSelectionChanged()
        {
            MyCmdSalir_Click_Can = false;

            string v = ReymiTable.Table.Rows[SelectedValuePath]["ValidatedEndorsements"].ToString();
            string r = ReymiTable.Table.Rows[SelectedValuePath]["RejectedEndorsements"].ToString();

            int t = int.Parse(v) + int.Parse(r);
            
            txtTotalEndososEndososEnLaRadicacion = t.ToString("###,###,##0");

            int totaldeendososenrydi = int.Parse(ReymiTable.Table.Rows[SelectedValuePath]["Amount"].ToString());

            if (t == totaldeendososenrydi)
                MyCmdSalir_Click_Can = true;



            isDGEnable = MyCmdSalir_Click_Can;


        }
        public void MyCmdSalir_Click()
        {
            try
            {
                SqlCommandBuilder com = new SqlCommandBuilder(_DA);
                _DA.Update(ReymiTable.Table);
                MessageBox.Show("Done...");
                this.View.Close();
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MyonCancel()
        {
            this.View.Close();
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmSubsanar()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources AnotherResource 

                if (_cnn != null)
                {
                    if (_cnn.State == ConnectionState.Open)
                        _cnn.Close();

                    _cnn.Dispose();
                    _cnn = null;
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

    public class ListaNumeroEndosos : IEquatable<ListaNumeroEndosos>
    {
      public  string Numero;
        public string Amount;

        public override string ToString()
        {
            List<string> myOut = new List<string>()
            {
               "'" + Numero + "'"
            };

            string myJoined = string.Join(",", myOut);
            return myJoined;

        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public bool Equals(ListaNumeroEndosos other)
        {
            if (other == null) return false;
            return (this.Numero.Equals(other.Numero));
        }
    }

}
