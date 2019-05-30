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
using System.Windows.Media;
using WpfEndososCandidatos.View.Procesos;
using WpfEndososCandidatos.Models;

namespace WpfEndososCandidatos.ViewModels.Procesos
{
   public class vmFixLot : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private DataTable _Onlots;
        private System.Data.SqlClient.SqlDataAdapter _DA;
        private System.Data.SqlClient.SqlConnection _cnn;
       // private string _cnnString = "Integrated Security=true;Data Source=.;Initial Catalog=TF-Endosos;";
        private string _DBEndososCnnStr;
        private string _DBMasterCeeCnnStr;
        private string _DBCeeMasterImgCnnStr;
        private string _SysUser;
        private string _DBRadicacionesCEECnnStr;
        private string _txtPartido;
        private ObservableCollection<modelCandidato> _cbInfoCandidato;
        private string _cbInfoCandidato_Item;
        private int _cbInfoCandidato_Item_Id;
        private string _txtFindCandidato;
        private bool _isFrmLoadFull = false;

        public vmFixLot () : base(new wpfFixLot())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            OnSave = new RelayCommand(param => MyOnSave());
            OnBtnGo = new RelayCommand(param => MyOnBtnGo());
            _cnn = new System.Data.SqlClient.SqlConnection();
            Onlots = new DataTable("lots");
        }


        #region MyProperty
        public string txtFindCandidato
        {
            get
            {
                return _txtFindCandidato;
            }set
            {
                _txtFindCandidato = value;
                this.RaisePropertychanged("txtFindCandidato");
            }
        }
        public ObservableCollection<modelCandidato> cbInfoCandidato
        {
            get
            {
                return _cbInfoCandidato;
            }
            set
            {
                _cbInfoCandidato = value;
                this.RaisePropertychanged("cbInfoCandidato");
            }
        }
        public string cbInfoCandidato_Item
        {
            get
            {
                return _cbInfoCandidato_Item;
            }set
            {
                if (_cbInfoCandidato_Item != value)
                {
                    _cbInfoCandidato_Item = value;
                   if (_isFrmLoadFull) MyReset();

                    string[] datos = value.Split('|');


                    txtPartido = datos[datos.Length - 1];

                    this.RaisePropertychanged("cbInfoCandidato_Item");
                }
            }
        }
        public int cbInfoCandidato_Item_Id
        {
            get
            {
                return _cbInfoCandidato_Item_Id;
            }set
            {
                if (_cbInfoCandidato_Item_Id != value)
                {
                    _cbInfoCandidato_Item_Id = value;
                    this.RaisePropertychanged("cbInfoCandidato_Item_Id");
                   
                }
            }
        }
        public DataTable Onlots
        {
            get
            {
                return _Onlots;
            }
            set
            {
                _Onlots = value;
                this.RaisePropertychanged("Onlots");
            }
        }
        public string txtPartido
        {
            get
            {
                return _txtPartido;
            }set
            {
                _txtPartido = value;
                this.RaisePropertychanged("txtPartido");
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
        public string tableReydi { get; private set; }
        public string DBRadicacionesCEECnnStr
        {
            get
            {
                return _DBRadicacionesCEECnnStr;
            }
            set
            {
                _DBRadicacionesCEECnnStr = value;
            }
        }
        #endregion

        #region Mycmd
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




                MyRefresh();
                MyReset();
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
        private void MyOnSave()
        {
            try
            {
                //SqlCommandBuilder com = new SqlCommandBuilder(_DA);
                //_DA.Update(Onlots);
                //MessageBox.Show("Done...");

                using (vmSubsanar frm = new vmSubsanar())
                {
                    frm.View.Owner = this.View as Window;
                    frm.tableReydi = tableReydi;
                    frm.DBRadicacionesCEECnnStr = DBRadicacionesCEECnnStr;

                    foreach (DataRow row in Onlots.Rows)
                    {
                        frm.NumeroDeRadicacion.Add(new ListaNumeroEndosos
                        {
                             Numero = row["lot"].ToString(),
                             Amount = row["Amount"].ToString()

                        });
                    }


                    frm.View.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MyOnBtnGo()
        {
            try
            {
               int idx = cbInfoCandidato.IndexOf( cbInfoCandidato.Where(x => x.numCandidato == txtFindCandidato).FirstOrDefault());

                //cbInfoCandidato.Sort();
                cbInfoCandidato_Item_Id = idx;
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
       public RelayCommand OnSave
        {
            get;private set;
        }
        public RelayCommand OnBtnGo
        {
            get;private set;
        }
        #endregion

        #region MyMetodos 
        private  void MyRefresh()
        {
            try
            {
                tableReydi = string.Empty;
                object partido = null;
                cbInfoCandidato = new ObservableCollection<modelCandidato>();

                string query = "select top 1 [Partido] from [Partidos] ";

                if (_cnn.State == System.Data.ConnectionState.Closed)
                {
                    _cnn.ConnectionString = DBEndososCnnStr;
                    _cnn.Open();
                }
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand()
                {
                    Connection = _cnn,
                    CommandType = System.Data.CommandType.Text,
                    CommandText = query
                })
                {
                    partido = cmd.ExecuteScalar();
                  //  txtPartido = partido.ToString();

                    switch (partido as string)
                    { 
                        case "PNP":
                            tableReydi = "EndososPNP";
                            break;
                        case "PPD":
                            tableReydi = "EndososPPD";
                            break;
                        case "SEC":
                            tableReydi = "EndososCEE";
                            break;
                        default:
                            tableReydi = "EndososCEE";
                            break;
                    }

                    //query = "select   CONCAT( [NumCand],' | ',[Nombre]) as Candidato from [Candidatos] order by nombre ";
                    query = "select   [NumCand],[Nombre],[Partido] from [Candidatos] order by nombre ";
                    cmd.CommandText = query;

                    System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();
                    
                    while(dr.Read())
                    {
                        cbInfoCandidato.Add(new modelCandidato
                        {
                            numCandidato = dr[0] as string,
                            Nombre = dr[1].ToString(),
                            partido = dr[2].ToString()
                        });

                    }
                   
                    dr.Close();
                    dr = null;
                   // _cnn.Close();

                    _isFrmLoadFull = true;

                }

            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MyReset()
        {
            try
            {
                string[] candidato = cbInfoCandidato_Item.Split('|');
                                             
                txtFindCandidato = candidato[0].Trim();
                int icandidato;
                int.TryParse(candidato[0].Trim() ,out icandidato);

                 //string query = "select [Partido], Lot,[Num_Candidato],Amount,[ValidatedEndorsements],[RejectedEndorsements],[StatusReydi] from lots where [Num_Candidato] = " + icandidato.ToString() + " and StatusReydi = 1";
                string query = "select [Partido], Lot,[Num_Candidato],Amount,[ValidatedEndorsements],[RejectedEndorsements] from lots where Partido='" + candidato[candidato.Length-1].Trim() + "'";



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
                    Onlots = dt;
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

        ~vmFixLot()
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

   public class modelCandidato : IEquatable<modelCandidato>, IComparable
    {

        public string numCandidato { get; set; }

        public string Nombre { get; set; }

        public string partido { get; set; }

        public override string ToString()
        {
            List<string> myOut = new List<string>()
            {
                numCandidato,
                Nombre,
                partido
            };
            string myJoined = string.Join(" | ", myOut);
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

        public bool Equals(modelCandidato other)
        {
            if (other == null) return false;
            return (this.numCandidato.Equals(other.numCandidato));
        }

        public int CompareTo(object obj)
        {
            modelCandidato a = this;
            modelCandidato b = (modelCandidato)obj;
            return string.Compare(a.numCandidato, b.numCandidato);

        }
    }

}
