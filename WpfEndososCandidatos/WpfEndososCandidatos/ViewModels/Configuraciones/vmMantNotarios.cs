

namespace WpfEndososCandidatos.ViewModels.Configuraciones
{
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
    using Models;
    using WpfEndososCandidatos.View;
    using System.Data;

    class vmMantNotarios : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        private Brush _BorderBrush;
        private Brush _Background_txtNombre;
        private Brush _Background_txtApellido2;

        private string _txtNumElec;
        private string _DBEndososCnnStr;
        private Brush _Background_txtApellido1;
        private Brush _Background_txtNumElec;
        private bool _IsReadOnly_txtNumElec;
        private bool _IsReadOnly_txtApellido1;
        private bool _IsReadOnly_txtApellido2;
        private bool _IsReadOnly_txtNombrePartido;
        private Brush _Background_txtNombrePartido;
        private ObservableCollection<Partidos> _cbPartidos;
        private string _cbPartidos_Item;
        private int _cbPartidos_Item_Id;
        private DataTable _MyPartidosTable;
        private DataTable _MyNotarioTable;
        private string _txtNombrePartido;
        private Visibility _Visibility_txtNombrePartido;
        private bool _IsEnabled_txtNombrePartido;
        private bool _IsEnabled_cbPartidos;
        private ObservableCollection<Notarios> _cbNotario;
        private string _cbNotario_Item;
        private int _cbNotario_Item_Id;
        private string _txtNombre;
        private Visibility _Visibility_txtNombre;

        private string _txtApellido1;
        private string _txtApellido2;
        private Visibility _Visibility_cbPartidos;
        private Visibility _Visibility_cbNotario;
        private bool _IsReadOnly_txtNombre;
        private bool _IsEnabled_cmdAdd;
        private bool _IsEnabled_cmdDelete;
        private bool _IsEnabled_cmdSave;
        private bool _IsEnabled_cmdEdit;
        private bool _IsEnabled_CmdCancel;

        public vmMantNotarios() : base(new wpfMantNotarios())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click(), param => CanSalir);
            cmdAdd_Click = new RelayCommand(param => MyCmdAdd_Click());
            cmdCancel_Click = new RelayCommand(param => MyCmdCancel_Click());
            cmdEdit_Click = new RelayCommand(param => MyCmdEdit_Click());
            cmdSave_Click = new RelayCommand(param => MyCmdSave_Click());
            cmdDelete_Click = new RelayCommand(param => MyCmdDelete_Click());

            cbPartidos = new ObservableCollection<Partidos>();
            cbNotario = new ObservableCollection<Notarios>();
        }

        #region MyPrperty

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
        public Brush Background_txtNombre
        {
            get
            {
                return _Background_txtNombre;
            }
            set
            {
                _Background_txtNombre = value;
                this.RaisePropertychanged("Background_txtNombre");
            }
        }
        public Brush Background_txtApellido2
        {
            get
            {
                return _Background_txtApellido2;
            }set
            {
                _Background_txtApellido2 = value;
                this.RaisePropertychanged("Background_txtApellido2");
            }
        }
        public Brush Background_txtApellido1
        {
            get
            {
                return _Background_txtApellido1;
            }set
            {
                _Background_txtApellido1 = value;
                this.RaisePropertychanged("Background_txtApellido1");
            }
        }
        public Brush Background_txtNumElec
        {
            get
            {
                return _Background_txtNumElec;
            }set
            {
                _Background_txtNumElec = value;
                this.RaisePropertychanged("Background_txtNumElec");
            }
        }
        public Brush Background_txtNombrePartido
        {
            get
            {
                return _Background_txtNombrePartido;
            }set
            {
                _Background_txtNombrePartido = value;
                this.RaisePropertychanged("Background_txtNombrePartido");
            }
        }

        private bool CanSalir
        {
            get
            {
                return true;
            }
        }
       
        public bool IsReadOnly_txtNumElec
        {
            get
            {
                return _IsReadOnly_txtNumElec;
            }set
            {
                _IsReadOnly_txtNumElec = value;
                this.RaisePropertychanged("IsReadOnly_txtNumElec");
            }
        }
        public bool IsReadOnly_txtApellido1
        {
            get
            {
                return _IsReadOnly_txtApellido1;
            }
            set
            {
                _IsReadOnly_txtApellido1 = value;
                this.RaisePropertychanged("IsReadOnly_txtApellido1");
            }
        }
        public bool IsReadOnly_txtApellido2
        {
            get
            {
                return _IsReadOnly_txtApellido2;
            }set
            {
                _IsReadOnly_txtApellido2 = value;
                this.RaisePropertychanged("IsReadOnly_txtApellido2");
            }
        }
        public bool IsReadOnly_txtNombre
        {
            get
            {
                return _IsReadOnly_txtNombre;
            }set
            {
                _IsReadOnly_txtNombre = value;
                this.RaisePropertychanged("IsReadOnly_txtNombre");
            }
        }
        public bool IsReadOnly_txtNombrePartido
        {
            get
            {
                return _IsReadOnly_txtNombrePartido;
            }set
            {
                _IsReadOnly_txtNombrePartido = value;
                this.RaisePropertychanged("IsReadOnly_txtNombrePartido");
            }
                 
        }
        public bool IsEnabled_txtNombrePartido
        {
            get
            {
                return _IsEnabled_txtNombrePartido;
            }set
            {
                _IsEnabled_txtNombrePartido = value;
                this.RaisePropertychanged("IsEnabled_txtNombrePartido");
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
        public string txtNombrePartido
        {
            get
            {
                return _txtNombrePartido;
            }set
            {
                _txtNombrePartido = value;
                this.RaisePropertychanged("txtNombrePartido");
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
        public string txtApellido1
        {
            get
            {
                return _txtApellido1;
            }set
            {
                _txtApellido1 = value;
                this.RaisePropertychanged("txtApellido1");
            }
        }
        public string txtApellido2
        {
            get
            {
                return _txtApellido2;
            }
            set
            {
                _txtApellido2 = value;
                this.RaisePropertychanged("txtApellido2");
            }
        }

        public Visibility Visibility_txtNombrePartido
        {
            get
            {
                return _Visibility_txtNombrePartido;
            }set
            {
                _Visibility_txtNombrePartido = value;
                this.RaisePropertychanged("Visibility_txtNombrePartido");
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
        public Visibility Visibility_cbNotario
        {
            get
            {
                return _Visibility_cbNotario;
            }set
            {
                _Visibility_cbNotario = value;
                this.RaisePropertychanged("Visibility_cbNotario");
            }
        }

        public ObservableCollection<Partidos> cbPartidos
        {
            get
            {
                return _cbPartidos;
            }
            set
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

        public ObservableCollection<Notarios> cbNotario
        {
            get
            {
                return _cbNotario;
            }
            set
            {
                _cbNotario = value;
                this.RaisePropertychanged("cbNotario");
            }
        }
        public string cbNotario_Item
        {
            get
            {
                return _cbNotario_Item;
            }
            set
            {
                if (value != null)
                {
                    string[] myData = value.Split('-');
                    _cbNotario_Item = value;

                    txtNumElec = myData[0];
                    txtNombre =myData[1];
                    txtApellido1 = myData[2];
                    txtApellido2 = myData[3];

                    cbPartidos_Item_Id = FindByPartido(myData[4].Trim());
                    txtNombrePartido = cbPartidos_Item;

                    IsEnabled_cmdDelete = true;
                    IsEnabled_cmdEdit = true;
                    IsEnabled_cmdAdd = false;
                    IsEnabled_CmdCancel = true;

                    this.RaisePropertychanged("cbNotario_Item");
                }
            }
        }
        public int cbNotario_Item_Id
        {
            get
            {
                return _cbNotario_Item_Id;
            }
            set
            {
                _cbNotario_Item_Id = value;
              

                this.RaisePropertychanged("cbNotario_Item_Id");
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
        private void MyCmdCancel_Click()
        {
            MyReset();
        }
        private void MyCmdEdit_Click()
        {
            try
            {
                Visibility_txtNombre = Visibility.Visible;
                Visibility_cbNotario = Visibility.Hidden;
                Visibility_cbPartidos = Visibility.Visible;
                Visibility_txtNombrePartido = Visibility.Hidden;

                IsEnabled_cbPartidos = true;
                IsReadOnly_txtApellido1 = false;
                IsReadOnly_txtApellido2 = false;
                IsReadOnly_txtNumElec = false;
                IsReadOnly_txtNombre = false;

                Background_txtNumElec = Brushes.Beige;
                Background_txtNombre = Brushes.Beige;
                Background_txtApellido1   = Brushes.Beige;
                Background_txtApellido2 = Brushes.Beige;

                IsEnabled_cmdAdd = false;
                IsEnabled_cmdDelete = false;
                IsEnabled_cmdSave = true;
                IsEnabled_cmdEdit = false;
                IsEnabled_CmdCancel = true;


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
                bool myUpDate = false;

                using (SqlExcuteCommand mySqlExe = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                 //   myUpDate = mySqlExe.MyChangePartidos(_IsInsert, txtNumPartido, txtNombre, txtEndoReq, txtAreaGeografica, myWhere);
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
    
        private void MyCmdDelete_Click()
        {
            try
            {
                var response = MessageBox.Show("!!!Do you really want to Delete this Partido?\r\n" , "Deleting...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (response == MessageBoxResult.No)
                    return;

                string myWhere = string.Empty;
             //   myWhere = txtNumPartido;
                bool myDelete = false;

                using (SqlExcuteCommand mySqlExe = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                 //   myDelete = mySqlExe.MyDeletePartidos(myWhere);


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
                Visibility_txtNombre = Visibility.Visible;
                Visibility_cbNotario = Visibility.Hidden;
                Visibility_cbPartidos = Visibility.Visible;
                Visibility_txtNombrePartido = Visibility.Hidden;

                IsEnabled_cbPartidos = true;
                IsReadOnly_txtApellido1 = false;
                IsReadOnly_txtApellido2 = false;
                IsReadOnly_txtNumElec = false;
                IsReadOnly_txtNombre = false;

                Background_txtNumElec = Brushes.Beige;
                Background_txtNombre = Brushes.Beige;
                Background_txtApellido1 = Brushes.Beige;
                Background_txtApellido2 = Brushes.Beige;

                IsEnabled_cmdAdd = false;
                IsEnabled_cmdSave = true;
                IsEnabled_CmdCancel = true;

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
        public RelayCommand cmdAdd_Click
        {
            get;private set;
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

        #endregion

        #region MyMetodos

        private void MyReset()
        {
            IsReadOnly_txtNumElec = true;
            IsReadOnly_txtApellido1 = true;
            IsReadOnly_txtApellido2 = true;
            IsReadOnly_txtNombrePartido = true;
            IsReadOnly_txtNombre = true;

            IsEnabled_txtNombrePartido = true;
            IsEnabled_cbPartidos = false;

            IsEnabled_cmdAdd = true;
            IsEnabled_cmdDelete = false;
            IsEnabled_cmdSave = false;
            IsEnabled_cmdEdit = false;
            IsEnabled_CmdCancel = false;

            txtNombrePartido = string.Empty;
            txtNombre = string.Empty;
            txtNumElec = string.Empty;
            txtNombre = string.Empty;
            txtApellido1 = string.Empty;
            txtApellido2 = string.Empty;
           
            Background_txtNombre = Brushes.Yellow;
            Background_txtApellido2 = Brushes.Yellow;
            Background_txtApellido1 = Brushes.Yellow;
            Background_txtNumElec = Brushes.Yellow;
            Background_txtNombrePartido = Brushes.Yellow;

            Visibility_txtNombrePartido = Visibility.Visible;
            Visibility_txtNombre = Visibility.Hidden;
            Visibility_cbPartidos = Visibility.Hidden;
            Visibility_cbNotario = Visibility.Visible;

            cbPartidos_Item_Id = -1;
            cbNotario_Item_Id = -1;
        }

        private void MyRefresh()
        {
            try
            {
                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    _MyNotarioTable = get.MyGetNotarios();

                    _MyPartidosTable = get.MyGetPartidos();

                    foreach (DataRow row in _MyNotarioTable.Rows)
                    {
                        Notarios myNotario = new Notarios();

                        myNotario.NumElec = row["NumElec"].ToString();
                        myNotario.Partido = row["Partido"].ToString();
                        myNotario.Nombre = row["Nombre"].ToString();
                        myNotario.Apellido1 = row["Apellido1"].ToString();
                        myNotario.Apellido2 = row["Apellido2"].ToString();
                        myNotario.AllColumn = false;
                        cbNotario.Add(myNotario);
                    }

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int FindByPartido(string partidoKey)
        {
            Partidos myInfoPAartido = cbPartidos.Where(x => x.PartidoKey == partidoKey).Single<Partidos>();

            int myIndex = cbPartidos.IndexOf(myInfoPAartido);

            return myIndex;
        }

        #endregion

        #region Dispose



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmMantNotarios()
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
