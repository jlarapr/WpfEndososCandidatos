using jolcode;
using jolcode.Base;
using jolcode.MyInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfEndososCandidatos.Models;
using WpfEndososCandidatos.View;

namespace WpfEndososCandidatos.ViewModels.Configuraciones
{
    class vmMantCandidatos : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private string _DBEndososCnnStr;
        private Brush _BorderBrush;
        private bool _IsEnabled_cmdSalir;
        private bool _IsEnabled_cmdSave;
        private bool _IsEnabled_cmdEdit;
        private bool _IsEnabled_CmdCancel;
        private bool _IsEnabled_cmdDelete;
        private bool _IsEnabled_cmdAdd;
        private bool _IsEdit;
        private Visibility _Visibility_txtNombre;
        private string _txtNombre;
        private ObservableCollection<string> _cbNombre;
        private string _cbNombre_Item;
        private int _cbNombre_Item_Id;
        private string _txtNumCandidato;
        private bool _IsEnabled_gbCargo;
        private bool _IsReadOnly_txtNumCandidato;
        private Brush _Background_txtNumCandidato;
        private Visibility _Visibility_cbNombre;
        private ObservableCollection<bool> _IsChecked_rbCargos;
        private ObservableCollection<Partidos> _cbPartidos;
        private string _cbPartidos_Item;
        private int _cbPartidos_Item_Id;
        DataTable _MyPartidosTable;
        DataTable _MyAreasTable;
        private ObservableCollection<Area> _cbArea;
        private string _cbArea_Item;
        private int _cbArea_Item_Id;
        private bool _IsEnabled_cbArea;
        private bool _IsEnabled_cbPartidos;
        private bool _IsReadOnly_txtEndoReq;
        private Brush _Background_txtEndoReq;
        private string _txtEndoReq;
        private DataTable _MyCandidatosTable;
        private bool _IsInsert;

        public vmMantCandidatos() 
            : base (new wpfMantCadidatos() )
        {
            InitWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdCancel_Click = new RelayCommand(param => MyCmdCancel_Click());
            cmdEdit_Click = new RelayCommand(param => MyCmdEdit_Click());
            cmdSave_Click = new RelayCommand(param => MyCmdSave_Click(), param => MyCanSave);
            cmdDelete_Click = new RelayCommand(param => MyCmdDelete_Click());
            cmdAdd_Click = new RelayCommand(param => MyCmdAdd_Click());

            cbNombre = new ObservableCollection<string>();
            IsChecked_rbCargos = new ObservableCollection<bool>();
            cbPartidos = new ObservableCollection<Partidos>();
            cbArea = new ObservableCollection<Area>();

        }

        #region MyProperty
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
        public Brush Background_txtNumCandidato
        {
            get
            {
                return _Background_txtNumCandidato;
            }
            set
            {
                _Background_txtNumCandidato = value;
                this.RaisePropertychanged("Background_txtNumCandidato");
            }
        }
        public Brush Background_txtEndoReq
        {
            get
            {
                return _Background_txtEndoReq;
            }set
            {
                _Background_txtEndoReq = value;
                this.RaisePropertychanged("Background_txtEndoReq");
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
        public string txtNumCandidato
        {
            get
            {
                return _txtNumCandidato;
            }
            set
            {
                _txtNumCandidato = value.Trim();
                this.RaisePropertychanged("txtNumCandidato");
                //if (!string.IsNullOrEmpty(value))
                //{
                //    long myLongValue = 0;

                //    if (long.TryParse(value, out myLongValue))
                //    {
                //        _txtNumCandidato = value.Trim();
                //        this.RaisePropertychanged("txtNumCandidato");
                //    }else
                //    {
                //        _txtNumCandidato = string.Empty;
                //        this.RaisePropertychanged("txtNumCandidato");
                //    }
                //}
                //else
                //{
                //    _txtNumCandidato = string.Empty;
                //    this.RaisePropertychanged("txtNumCandidato");
                //}
            }
        }
        public string txtEndoReq
        {
            get
            {
                return _txtEndoReq;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    long myLongValue = 0;

                    if (long.TryParse(value, out myLongValue))
                    {
                        _txtEndoReq = value.Trim();
                        this.RaisePropertychanged("txtEndoReq");
                    }else
                    {
                        _txtEndoReq = string.Empty;
                        this.RaisePropertychanged("txtEndoReq");
                    }
                }
                else
                {
                    _txtEndoReq = string.Empty;
                    this.RaisePropertychanged("txtEndoReq");
                }
            }
        }

        public ObservableCollection<bool> IsChecked_rbCargos
        {
            get
            {
                return _IsChecked_rbCargos;
            }set
            { 
                _IsChecked_rbCargos = value;
                this.RaisePropertychanged("IsChecked_rbCargos");
            }

        }

        public bool IsEnabled_cmdAdd
        {
            get
            {
                return _IsEnabled_cmdAdd;
            }
            set
            {
                _IsEnabled_cmdAdd = value;
                this.RaisePropertychanged("IsEnabled_cmdAdd");
            }
        }
        public bool IsEnabled_cmdDelete
        {
            get
            {
                return _IsEnabled_cmdDelete;
            }
            set
            {
                _IsEnabled_cmdDelete = value;
                this.RaisePropertychanged("IsEnabled_cmdDelete");
            }
        }
        public bool IsEnabled_CmdCancel
        {
            get
            {
                return _IsEnabled_CmdCancel;
            }
            set
            {
                _IsEnabled_CmdCancel = value;
                this.RaisePropertychanged("IsEnabled_CmdCancel");
            }
        }
        public bool IsEnabled_cmdEdit
        {
            get
            {
                return _IsEnabled_cmdEdit;
            }
            set
            {
                _IsEnabled_cmdEdit = value;
                this.RaisePropertychanged("IsEnabled_cmdEdit");
            }
        }
        public bool IsEnabled_cmdSave
        {
            get
            {
                return _IsEnabled_cmdSave;
            }
            set
            {
                _IsEnabled_cmdSave = value;
                this.RaisePropertychanged("IsEnabled_cmdSave");
            }
        }
        public bool IsEnabled_cmdSalir
        {
            get
            {
                return _IsEnabled_cmdSalir;
            }
            set
            {
                _IsEnabled_cmdSalir = value;
                this.RaisePropertychanged("IsEnabled_cmdSalir");
            }
        }        
        public bool IsEnabled_gbCargo
        {
            get
            {
                return _IsEnabled_gbCargo;
            }set
            {
                _IsEnabled_gbCargo = value;
                this.RaisePropertychanged("IsEnabled_gbCargo");
            }
        }
        public bool IsReadOnly_txtNumCandidato
        {
            get
            {
                return _IsReadOnly_txtNumCandidato;
            }
            set
            {
                _IsReadOnly_txtNumCandidato = value;
                this.RaisePropertychanged("IsReadOnly_txtNumCandidato");
            }
        }
        public bool IsReadOnly_txtEndoReq
        {
            get
            {
                return _IsReadOnly_txtEndoReq;
            }set
            {
                _IsReadOnly_txtEndoReq = value;
                this.RaisePropertychanged("IsReadOnly_txtEndoReq");

            }
        }
        public bool IsEnabled_cbArea
        {
            get
            {
                return _IsEnabled_cbArea;
            }set
            {
                _IsEnabled_cbArea = value;
                this.RaisePropertychanged("IsEnabled_cbArea");
            }
        }
        public bool IsEnabled_cbPartidos
        {
            get
            {
                return _IsEnabled_cbPartidos;
            }set
            {
                _IsEnabled_cbPartidos = value;
                this.RaisePropertychanged("IsEnabled_cbPartidos");
            }

        }


        public Visibility Visibility_txtNombre
        {
            get
            {
                return _Visibility_txtNombre;
            }
            set
            {
                _Visibility_txtNombre = value;
                this.RaisePropertychanged("Visibility_txtNombre");
            }
        }
        public Visibility Visibility_cbNombre
        {
            get
            {
                return _Visibility_cbNombre;
            }set
            {
                _Visibility_cbNombre = value;
                this.RaisePropertychanged("Visibility_cbNombre");
            }
        }

        public ObservableCollection<string> cbNombre
        {
            get
            {
                return _cbNombre;
            }
            set
            {
                _cbNombre = value;
                this.RaisePropertychanged("cbNombre");
            }
        }
        public string cbNombre_Item
        {
            get
            {
                return _cbNombre_Item;
            }
            set
            {
                if (value != null)
                {
                    string[] myData = value.Split('-');
                    cbArea_Item_Id = FindByArea(myData[3].Trim());
                    cbPartidos_Item_Id = FindByPartido(myData[0].Trim());
                    txtNumCandidato = myData[1].ToString();
                    txtEndoReq = myData[5].ToString();
                    int i;
                    if (int.TryParse(myData[4].ToString(), out i)) 
                        IsChecked_rbCargos[i] = true;

                    _cbNombre_Item = value;
                    txtNombre = myData[2].ToString();
                    IsEnabled_cmdEdit = true;
                    IsEnabled_cmdDelete = true;
                    IsEnabled_cmdAdd = false;
                    IsEnabled_CmdCancel = true;
                    IsEnabled_cmdSalir = false;
                    this.RaisePropertychanged("cbNombre_Item");


                }
            }
        }
        public int cbNombre_Item_Id
        {
            get
            {
                return _cbNombre_Item_Id;
            }
            set
            {
                _cbNombre_Item_Id = value;
                this.RaisePropertychanged("cbNombre_Item_Id");
            }
        }
      
        public ObservableCollection<Partidos> cbPartidos
        {
            get
            {
                return _cbPartidos;
            }set
            {
                _cbPartidos = value;
                this.RaisePropertychanged("cbPartidos");
            }
        }
        public string cbPartidos_Item
        {
            get
            {
                return _cbPartidos_Item;
            }
            set
            {
                if (value != null)
                {
                    _cbPartidos_Item = value;
                    this.RaisePropertychanged("cbPartidos_Item");
                }
            }
        }
        public int cbPartidos_Item_Id
        {
            get
            {
                return _cbPartidos_Item_Id;
            }
            set
            {
                _cbPartidos_Item_Id = value;
                this.RaisePropertychanged("cbPartidos_Item_Id");
            }
        }

        public ObservableCollection<Area> cbArea
        {
            get
            {
                return _cbArea;
            }
            set
            {
                _cbArea = value;
                this.RaisePropertychanged("cbArea");
            }
        }
        public string cbArea_Item
        {
            get
            {
                return _cbArea_Item;
            }
            set
            {
                _cbArea_Item = value;
                this.RaisePropertychanged("cbArea_Item");
            }
        }
        public int cbArea_Item_Id
        {
            get
            {
                return _cbArea_Item_Id;
            }
            set
            {
              
                _cbArea_Item_Id = value;

               
                this.RaisePropertychanged("cbArea_Item_Id");
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
                    BorderBrush = b;
                }
                else
                    BorderBrush = Brushes.Black;


                cbPartidos.Clear();
                cbArea.Clear();

                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    _MyAreasTable = get.MyGetAreas(true);

                    foreach (DataRow row in _MyAreasTable.Rows)
                    {
                        Area myArea = new Area();

                        myArea.AreaKey = row["Area"].ToString();
                        myArea.Precintos = row["Precintos"].ToString();
                        myArea.Desc = row["Desc"].ToString();
                        myArea.ElectivePositionID = row["ElectivePositionID"].ToString();
                        myArea.DemarcationID = row["DemarcationID"].ToString();

                        cbArea.Add(myArea);
                    }

                    _MyPartidosTable = get.MyGetPartidos();

                    foreach (DataRow row in _MyPartidosTable.Rows)
                    {
                        Partidos mypartido = new Partidos();

                        mypartido.PartidoKey = row["Partido"].ToString();
                        mypartido.Desc = row["Desc"].ToString();
                        mypartido.EndoReq = (int)row["EndoReq"];
                        mypartido.Area = row["Area"].ToString();

                        cbPartidos.Add(mypartido);
                    }
                }
                cbPartidos.Sort();




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
        private void MyCmdCancel_Click()
        {
            MyReset();
        }
        private void MyCmdEdit_Click()
        {
            try
            {
                IsEnabled_cmdAdd = false;
                IsEnabled_cmdSalir = false;
                IsEnabled_cmdDelete = false;
                IsEnabled_cmdEdit = false;

                IsEnabled_gbCargo = true;
                IsEnabled_CmdCancel = true;
                IsEnabled_cmdSave = true;
                IsEnabled_cbArea = true;
                IsEnabled_cbPartidos = true;


                IsReadOnly_txtNumCandidato = false;
                IsReadOnly_txtEndoReq = false;

                _IsEdit = true;
                Visibility_txtNombre = Visibility.Visible;
                Visibility_cbNombre = Visibility.Hidden;

                Background_txtNumCandidato = Brushes.Beige;
                Background_txtEndoReq = Brushes.Beige;


            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MyCmdSave_Click()
        {
            try
            {
                if ((txtNumCandidato.Trim().Length == 0) || (txtNombre.Trim().Length == 0) || (txtEndoReq.Trim().Length == 0) )
                    return;

                if (cbArea_Item_Id < 0 || cbPartidos_Item_Id <0 )
                    return;
               

                bool myUpDate = false;

                string myWhere = string.Empty;



                if (!_IsInsert)
                    myWhere = cbNombre_Item.Split('-')[1].Trim();
                else
                    myWhere = "";

                if (myWhere.Trim().Length == 0)
                    myWhere = txtNumCandidato.Trim();

                using (SqlExcuteCommand mySqlExe = new SqlExcuteCommand()
                {
                    DBCnnStr = _DBEndososCnnStr
                })
                {
                    string myPartido= cbPartidos_Item.Split('-')[0].Trim();
                    string myNumCand= txtNumCandidato.Trim();
                    string myNombre= txtNombre.Trim();
                    string myArea = cbArea_Item.Split('-')[0].Trim();
                    int myCargo=0;
                    string myEndoReq= txtEndoReq.Trim();


                    for (int rb =0; rb <8;rb++)
                        if (IsChecked_rbCargos[rb] == true)
                        {
                            myCargo =rb;
                            break;
                        }

                    
                    myUpDate = mySqlExe.MyChangeCandidatos(_IsInsert, myPartido, myNumCand, myNombre, myArea, myCargo.ToString(), myEndoReq, myWhere);
                }

                if (!myUpDate)
                    throw new Exception("Error en la Base de Data");

                MessageBox.Show("Done...", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                MyRefresh();
                MyReset();

            }
        }
        private bool MyCanSave
        {
            get
            {
                if ((txtNumCandidato == null) || (txtNombre == null) || (txtEndoReq == null) || (_IsEdit == false))
                    return false;

                if ((txtNumCandidato.Trim().Length == 0) || (txtNombre.Trim().Length == 0) ||(txtEndoReq.Trim().Length ==0) )
                    return false;

                if (cbArea_Item_Id <0)
                    return false;

                if (cbPartidos_Item_Id <0)
                    return false;

                foreach (bool rb in IsChecked_rbCargos)
                    if (rb == true)
                        return true; 

                return false;
            }
        }
        private void MyCmdDelete_Click()
        {
            try
            {
                var response = MessageBox.Show("!!!Do you really want to Delete this Candidato?\r\n" + txtNumCandidato, "Deleting...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (response == MessageBoxResult.No)
                    return;

                string myWhere = string.Empty;
                myWhere = txtNumCandidato;
                bool myDelete = false;

                using (SqlExcuteCommand mySqlExe = new SqlExcuteCommand()
                {
                    DBCnnStr = _DBEndososCnnStr
                })
                {
                    myDelete = mySqlExe.MyDeleteCandidato(myWhere);
                }

                if (!myDelete)
                    throw new Exception("Error en la Base de Data");

                MessageBox.Show("Done...", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                MyRefresh();
                MyReset();
            }
        }
        private void MyCmdAdd_Click()
        {
            try
            {
                IsEnabled_cmdAdd = false;
                IsEnabled_cmdSalir = false;
                IsEnabled_cmdDelete = false;
                IsEnabled_cmdEdit = false;

                IsEnabled_gbCargo = true;
                IsEnabled_CmdCancel = true;
                IsEnabled_cmdSave = true;
                IsEnabled_cbArea = true;
                IsEnabled_cbPartidos = true;

                IsReadOnly_txtNumCandidato = false;
                IsReadOnly_txtEndoReq = false;

                Background_txtNumCandidato = Brushes.Beige;
                Background_txtEndoReq = Brushes.Beige;

                Visibility_txtNombre = Visibility.Visible;
                Visibility_cbNombre = Visibility.Hidden;

                _IsEdit = true;

                _IsInsert = true;

            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {

            }
        }




        public RelayCommand InitWindow
        {
            get;private set;
        }
        public RelayCommand cmdSalir_Click
        {
            get;
            private set;
        }
        public RelayCommand cmdCancel_Click
        {
            get; private set;
        }
        public RelayCommand cmdEdit_Click
        {
            get; private set;
        }
        public RelayCommand cmdSave_Click
        {
            get; private set;
        }
        public RelayCommand cmdDelete_Click
        {
            get; private set;
        }
        public RelayCommand cmdAdd_Click
        {
            get; private set;
        }


        #endregion

        #region MyMetodos
        private void MyReset()
        {
            IsChecked_rbCargos.Clear();
            for (int i = 0; i<9; i++)
                IsChecked_rbCargos.Add(false);

            IsEnabled_cmdAdd = true;
            IsEnabled_cmdDelete = false;
            IsEnabled_CmdCancel = false;
            IsEnabled_cmdEdit = false;
            IsEnabled_cmdSave = false;
            IsEnabled_cmdSalir = true;

            IsEnabled_gbCargo = false;
            IsReadOnly_txtNumCandidato = true;
            IsReadOnly_txtEndoReq = true;

            _IsEdit = false;
            _IsInsert = false;
            Visibility_txtNombre = Visibility.Hidden;
            Visibility_cbNombre = Visibility.Visible;

            Background_txtNumCandidato = Brushes.Yellow;
            Background_txtEndoReq = Brushes.Yellow;

            txtNombre = string.Empty;
            txtNumCandidato = string.Empty;
            txtEndoReq = string.Empty;

            cbArea_Item_Id = -1;
            cbPartidos_Item_Id = -1;
            cbNombre_Item_Id = -1;
            IsEnabled_cbArea = false;
            IsEnabled_cbPartidos = false;

            MyRefresh();
        }
        private void MyRefresh()
        {
            cbNombre.Clear();

            using (SqlExcuteCommand get = new SqlExcuteCommand()
            {
                DBCnnStr = DBEndososCnnStr
            })
            {
                _MyCandidatosTable = get.MyGetCandidatos();

                foreach (DataRow row in _MyCandidatosTable.Rows)
                {
                    Candidatos myCand = new Candidatos();
                            
                    myCand.Partido = row["Partido"].ToString();
                    myCand.NumCand = row["NumCand"].ToString();
                    myCand.Nombre = row["Nombre"].ToString();
                    myCand.Area = row["Area"].ToString();
                    myCand.Cargo = row["Cargo"].ToString();
                    myCand.EndoReq = row["EndoReq"].ToString();

                    cbNombre.Add(myCand.ToString());
                }

            }

        }
        private int FindByPartido(string partidoKey)
        {
            int myIndex = -1;
            try
            {

                Partidos myInfoPAartido = cbPartidos.Where(x => x.PartidoKey == partidoKey).Single<Partidos>();
                myIndex = cbPartidos.IndexOf(myInfoPAartido);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return myIndex;
        }
        private int FindByArea(string Area)
        {
            Area myOut = cbArea.Where(x => x.AreaKey.Substring(0, 4) == Area).Single<Area>();

            int myIndex = cbArea.IndexOf(myOut);



            //foreach(Area area in coll)
            //{
            //    if (area.AreaKey.Substring(0,4) == Area)
            //    {

            //        myOut= area;
            //        break;
            //    }
            //}
            return myIndex;
        }
        #endregion

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmMantCandidatos()
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

    }//end
}//end
