
namespace WpfEndososCandidatos.ViewModels.Configuraciones
{
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
    using Models;
    using WpfEndososCandidatos.View;

    class vmMantPartidos : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        private Brush _BorderBrush;
        private Brush _Background_txtNumPartido;
        private Brush _Background_txtEndoReq;

        private ObservableCollection<Partidos> _cbPartidos;
        private ObservableCollection<Area> _cbArea;

        private int _cbPartidos_Item_Id;
        private int _cbArea_Item_Id;

        private DataTable _MyAreasTable;
        private DataTable _MyPartidosTable;

        private string _cbArea_Item;
        private string _DBEndososCnnStr;
        private string _txtEndoReq;
        private string _cbPartidos_Item;
        private string _txtNombre;
        private string _txtNumPartido;
        private string _txtAreaGeografica;

        private bool _IsEnabled_cmdAdd;
        private bool _IsEnabled_cmdDelete;
        private bool _IsEnabled_CmdCancel;
        private bool _IsEnabled_cmdEdit;
        private bool _IsEnabled_cmdSave;
        private bool _IsEnabled_cmdSalir;
        private bool _IsReadOnly_txtNumPartido;
        private bool _IsReadOnly_txtEndoReq;
        private bool _IsReadOnly_txtAreaGeografica;

        private Visibility _Visibility_txtNombre;
        private Visibility _Visibility_cbPartidos;
        private Visibility _Visibility_txtAreaGeografica;
        private Visibility _Visibility_cbArea;

        public vmMantPartidos()
            : base(new wpfMantPartidos())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdCancel_Click = new RelayCommand(param => MyCmdCancel_Click());
            cmdEdit_Click = new RelayCommand(param => MyCmdEdit_Click());
            cmdSave_Click = new RelayCommand(param => MyCmdSave_Click());
            cmdDelete_Click = new RelayCommand(param => MyCmdDelete_Click());
            cmdAdd_Click = new RelayCommand(param => MyCmdAdd_Click());

            cbArea = new ObservableCollection<Area>();
            cbPartidos = new ObservableCollection<Partidos>();
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
        public Brush Background_txtNumPartido
        {
            get
            {
                return _Background_txtNumPartido;
            }set
            {
                _Background_txtNumPartido = value;
                this.RaisePropertychanged("Background_txtNumPartido");
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

        public bool IsReadOnly_txtNumPartido
        {
            get
            {
                return _IsReadOnly_txtNumPartido;
            }set
            {
                _IsReadOnly_txtNumPartido = value;
                this.RaisePropertychanged("IsReadOnly_txtNumPartido");
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
        public bool IsReadOnly_txtAreaGeografica
        {
            get
            {
                return _IsReadOnly_txtAreaGeografica;
            }set
            {
                _IsReadOnly_txtAreaGeografica = value;
                this.RaisePropertychanged("IsReadOnly_txtAreaGeografica");
            }
        }

        public bool IsEnabled_cmdAdd
        {
            get
            {
                return _IsEnabled_cmdAdd;
            }set
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
            }set
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
            }set
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
            }set
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
            }set
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
        public string txtAreaGeografica
        {
            get
            {
                return _txtAreaGeografica;
            }set
            {
                _txtAreaGeografica = value;
                this.RaisePropertychanged("txtAreaGeografica");
            }
        }
        public string txtNumPartido
        {
            get
            {
                return _txtNumPartido;
            }
            set
            {
                _txtNumPartido = value;
                this.RaisePropertychanged("txtNumPartido");
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
                _txtEndoReq = value;
                this.RaisePropertychanged("txtEndoReq");
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

        public Visibility Visibility_txtNombre
        {
            get
            {
                return _Visibility_txtNombre;
            }set
            {
                _Visibility_txtNombre = value;
                this.RaisePropertychanged("Visibility_txtNombre");
            }
        }
        public Visibility Visibility_cbPartidos
        {
            get
            {
                return _Visibility_cbPartidos;
            }set
            {
                _Visibility_cbPartidos = value;
                this.RaisePropertychanged("Visibility_cbPartidos");
            }
        }
        public Visibility Visibility_txtAreaGeografica
        {
            get
            {
                return _Visibility_txtAreaGeografica;
            }set
            {
                _Visibility_txtAreaGeografica = value;
                this.RaisePropertychanged("Visibility_txtAreaGeografica");
            }
        }
        public Visibility Visibility_cbArea
        {
            get
            {
                return _Visibility_cbArea;
            }set
            {
                _Visibility_cbArea = value;
                this.RaisePropertychanged("Visibility_cbArea");
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
            }set
            {
                if (value != null)
                {
                    string[] myData = value.Split('-');

                    _cbPartidos_Item = value;
                    txtNumPartido = myData[0].Trim();
                    txtNombre = myData[1].Trim();
                    txtEndoReq = myData[2].Trim();

                    IsEnabled_cmdEdit = true;
                    IsEnabled_cmdDelete = true;
                    IsEnabled_cmdAdd = false;
                    IsEnabled_CmdCancel = true;
                    

                    cbArea_Item_Id = FindByArea(myData[3].Trim());
                    txtAreaGeografica = cbArea_Item;

                    this.RaisePropertychanged("cbPartidos_Item");
                }
            }
        }
        public int cbPartidos_Item_Id
        {
            get
            {
                return _cbPartidos_Item_Id;
            }set
            {
                _cbPartidos_Item_Id = value;
                this.RaisePropertychanged("cbPartidos_Item_Id");
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
                        mypartido.Desc  = row["Desc"].ToString();
                        mypartido.EndoReq =(int) row["EndoReq"];
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
                Visibility_txtNombre = Visibility.Visible;
                Visibility_txtAreaGeografica = Visibility.Hidden;

                Visibility_cbPartidos = Visibility.Hidden;
                Visibility_cbArea = Visibility.Visible;

                IsEnabled_CmdCancel = true;
                IsEnabled_cmdDelete = false;
                IsEnabled_cmdSave= true;

                IsEnabled_cmdSalir = false;
                IsReadOnly_txtNumPartido = false;
                IsReadOnly_txtEndoReq = false;
                IsReadOnly_txtAreaGeografica = false;

                Background_txtEndoReq = Brushes.Beige;
                Background_txtNumPartido = Brushes.Beige;
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
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }finally
            {
                MyReset();
            }
        }
        private void MyCmdDelete_Click()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }finally
            {
                MyReset();
            }
        }
        private void MyCmdAdd_Click()
        {
            try
            {
                throw new NotImplementedException();
         
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }finally
            {
                MyReset();
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
        public RelayCommand cmdCancel_Click
        {
            get; private set;
        }
        public RelayCommand cmdEdit_Click
        {
            get;private set;
        }
        public RelayCommand cmdSave_Click
        {
            get;private set;
        }
        public RelayCommand cmdDelete_Click
        {
            get;private set;
        }
        public RelayCommand cmdAdd_Click
        {
            get;private set;
        }
        
        #endregion

        #region MyMeodos

        private void MyReset()
        {
            cbArea_Item_Id = -1;
            cbPartidos_Item_Id = -1;

            IsEnabled_cmdAdd = true;
            IsEnabled_cmdDelete = false;
            IsEnabled_CmdCancel = false;
            IsEnabled_cmdEdit = false;
            IsEnabled_cmdSave = false;
            IsEnabled_cmdSalir = true;

            txtNumPartido = string.Empty;
            txtNombre = string.Empty;
            txtEndoReq = string.Empty;
            txtAreaGeografica = string.Empty;

            Visibility_txtNombre = Visibility.Hidden;
            Visibility_cbPartidos = Visibility.Visible;
            Visibility_txtAreaGeografica = Visibility.Visible;
            Visibility_cbArea = Visibility.Hidden;

            IsReadOnly_txtNumPartido = true;
            IsReadOnly_txtEndoReq = true;
            IsReadOnly_txtAreaGeografica = true;

            Background_txtEndoReq = Brushes.Yellow;
            Background_txtNumPartido = Brushes.Yellow;
        }

        private int FindByArea(string Area )
        {
            Area myOut = cbArea.Where(x => x.AreaKey.Substring(0,4) == Area).Single<Area>();

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

        ~vmMantPartidos()
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
