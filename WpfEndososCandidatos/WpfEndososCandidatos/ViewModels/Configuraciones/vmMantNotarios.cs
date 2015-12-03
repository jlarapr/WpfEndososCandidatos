

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
    using System.Diagnostics;
    using System.Windows.Input;

    class vmMantNotarios : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        private Logclass _LogClass;

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
        private bool _IsEnabled_cmdSalir;
        private bool _IsInsert;
        private bool _isEdit;
        private string _DBCeeMasterCnnStr;
        private bool _CanFind;

        public vmMantNotarios() : base(new wpfMantNotarios())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click(), param => CanSalir);
            cmdAdd_Click = new RelayCommand(param => MyCmdAdd_Click());
            cmdCancel_Click = new RelayCommand(param => MyCmdCancel_Click());
            cmdEdit_Click = new RelayCommand(param => MyCmdEdit_Click());
            cmdSave_Click = new RelayCommand(param => MyCmdSave_Click(),param => CanSave);
            cmdDelete_Click = new RelayCommand(param => MyCmdDelete_Click());
            cmdFind_Click = new RelayCommand(param => MyCmdFind_Click(),param => CanFind);

            cbPartidos = new ObservableCollection<Partidos>();
            cbNotario = new ObservableCollection<Notarios>();
            _LogClass = new Logclass();
        }

        #region MyPrperty
        public bool CanFind
        {
            get
            {
                if (string.IsNullOrEmpty(txtNumElec))
                {
                    txtNombre = string.Empty;
                    txtApellido1 = string.Empty;
                    txtApellido2 = string.Empty;
                    cbPartidos_Item_Id = -1;
                    //MySendTab();

                    return false;
                }

                if (txtNumElec.Trim().Length <= 0)
                {
                    txtNombre = string.Empty;
                    txtApellido1 = string.Empty;
                    txtApellido2 = string.Empty;
                    cbPartidos_Item_Id = -1;
                    //MySendTab();

                    return false;
                }

                int i;
                if (!int.TryParse(txtNumElec, out i))
                {
                    txtNombre = string.Empty;
                    txtApellido1 = string.Empty;
                    txtApellido2 = string.Empty;
                    cbPartidos_Item_Id = -1;
                    //MySendTab();

                    return false;
                }

                //if (_CanFind)
                //    return false;
                return true;
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
       
        private bool CanSave
        {
            get
            {
                if ( (txtNumElec ==null) || (txtNombre ==null) || (txtApellido1 ==null) || (_isEdit == false) || (txtNombrePartido ==null))
                    return false;

                if ((txtNumElec.Trim().Length != 7) || (txtNombre.Trim().Length == 0) || (txtApellido1.Trim().Length ==0 )|| (txtNombrePartido.Trim().Length ==0) )
                    return false;

                if (int.Parse(txtNumElec) == 0)
                    return false;

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
        public bool IsEnabled_cmdSalir
        {
            get
            {
                return _IsEnabled_cmdSalir;
            }set
            {
                _IsEnabled_cmdSalir = value;
                this.RaisePropertychanged("IsEnabled_cmdSalir");
            }
        }
        public string DBCeeMasterCnnStr
        {
            get
            {
                return _DBCeeMasterCnnStr;
            }
            set
            {
                _DBCeeMasterCnnStr = value;
            }
        }

        public string txtNumElec
        {
            get
            {
                return _txtNumElec;
            }set
            {
                if (value != null)
                {
                    if (value.Trim().Length > 0)
                    {
                        if(value != _txtNumElec)
                        {
                            txtApellido1 = string.Empty;
                            txtApellido2 = string.Empty;
                            txtNombre = string.Empty;
                            cbPartidos_Item_Id = -1;
                        }

                        _txtNumElec = value;


                        this.RaisePropertychanged("txtNumElec");
                    }
                    else
                    {
                        _txtNumElec = string.Empty;
                        txtApellido1 = string.Empty;
                        txtApellido2 = string.Empty;
                        txtNombre = string.Empty;
                        cbPartidos_Item_Id = -1;
                        this.RaisePropertychanged("txtNumElec");
                    }
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
                    txtNombrePartido = value;

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

                    txtNumElec = myData[0].Trim();
                    txtNombre =myData[1].Trim();
                    txtApellido1 = myData[2].Trim();
                    txtApellido2 = myData[3].Trim();

                    cbPartidos_Item_Id = FindByPartido(myData[4].Trim());
                    txtNombrePartido = cbPartidos_Item;

                    IsEnabled_cmdDelete = true;
                    IsEnabled_cmdEdit = true;
                    IsEnabled_cmdAdd = false;
                    IsEnabled_CmdCancel = true;
                    IsEnabled_cmdSalir = false;

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

                _LogClass.LogName = "Applica";
                _LogClass.SourceName = "Notario";
                _LogClass.MessageFile = string.Empty;
                _LogClass.CreateEvent();
                _LogClass.MYEventLog.WriteEntry("Notario Start:" + Dia + " " + Hora, EventLogEntryType.Information);

                MyRefresh();
                MyReset();
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                _LogClass.MYEventLog.WriteEntry(ex.ToString() + "\r\n" + site.Name, EventLogEntryType.Error, 9999);
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
                _LogClass.MYEventLog.WriteEntry(string.Concat(ex.Message, "\r\n", site.Name),EventLogEntryType.Error,9999);
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
                _isEdit = true;

            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                _LogClass.MYEventLog.WriteEntry(string.Concat(ex.Message, "\r\n", site.Name), EventLogEntryType.Error, 9999);
            }
        }
        private void MyCmdSave_Click()
        {
            try
            {
                bool myUpDate = false;
                string myWhere = string.Empty;


                if (!_IsInsert)
                {
                    myWhere = cbNotario_Item.Split('-')[0];
                }else
                {
                    myWhere = "";
                }



                using (SqlExcuteCommand mySqlExe = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    myUpDate = mySqlExe.MyChangeNotario(_IsInsert, txtNumElec, txtNombrePartido.Split('-')[0], txtNombre, txtApellido1, txtApellido2, myWhere);
                }

                if (!myUpDate)
                    throw new Exception("Error en la Base de Data");

                MessageBox.Show("Done...", "Save", MessageBoxButton.OK, MessageBoxImage.Information);

                _LogClass.MYEventLog.WriteEntry("Save..." + txtNumElec, EventLogEntryType.Information, 100);

            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.ToString(), site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                _LogClass.MYEventLog.WriteEntry(string.Concat(ex.Message, "\r\n", site.Name), EventLogEntryType.Error, 9999);
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
                string myWhere2 = string.Empty;
                myWhere = txtNumElec.Trim();
                myWhere2 = cbPartidos_Item.Split('-')[0];
                bool myDelete = false;

                using (SqlExcuteCommand mySqlExe = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    myDelete = mySqlExe.MyDeleteNotario(myWhere,myWhere2);


                }

                if (!myDelete)
                    throw new Exception("Error en la Base de Data");


                _LogClass.MYEventLog.WriteEntry("Delete...", EventLogEntryType.Information, 9999);

                MessageBox.Show("Done...", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                _LogClass.MYEventLog.WriteEntry(string.Concat(ex.Message, "\r\n", site.Name), EventLogEntryType.Error, 9999);
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
                IsEnabled_cmdSalir = false;

                _IsInsert = true;
                _isEdit = true;

            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                _LogClass.MYEventLog.WriteEntry(string.Concat(ex.Message, "\r\n", site.Name), EventLogEntryType.Error, 9999);
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
        public RelayCommand cmdFind_Click
        {
            get;private set;
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
            _CanFind = false;

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
            IsEnabled_cmdSalir = true;

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

            _IsInsert = false;
            _isEdit = false;
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

                    cbPartidos.Clear();

                    foreach (DataRow row in _MyPartidosTable.Rows)
                    {
                        Partidos mypartido = new Partidos();
                        mypartido.PartidoKey = row["Partido"].ToString();
                        mypartido.Desc = row["Desc"].ToString();
                        mypartido.EndoReq = (int)row["EndoReq"];
                        mypartido.Area = row["Area"].ToString();
                        cbPartidos.Add(mypartido);
                    }

                    cbNotario.Clear();

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


                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                _LogClass.MYEventLog.WriteEntry(string.Concat(ex.Message, "\r\n", site.Name), EventLogEntryType.Error, 9999);
            }
        }
        private void MyCmdFind_Click()
        {
            try
            {
                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCeeMasterCnnStr = DBCeeMasterCnnStr

                })
                {
                    _CanFind = true;

                    _txtNumElec = _txtNumElec.PadLeft(7, '0');

                    DataTable myTable = get.MyGetCitizen(_txtNumElec);
                    if (myTable.Rows.Count == 0)
                        throw new Exception("Número electoral invalido.");

                    txtNombre = myTable.Rows[0]["FirstName"].ToString();
                    txtApellido1 = myTable.Rows[0]["LastName1"].ToString();
                    txtApellido2 = myTable.Rows[0]["LastName2"].ToString();

                    switch (myTable.Rows[0]["Status"].ToString().Trim().ToUpper())
                    {
                        case "A":
                            break;
                        case "E":
                           //  MessageBox.Show("Excluido","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                            throw new Exception("Excluido");
                            //break;
                        case "I":
                            throw new Exception("Inactivo");

                            //MessageBox.Show("Inactivo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            //break;
                        default:
                            throw new Exception("Inactivo");
                            //MessageBox.Show("Inactivo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            //break;
                    }


                }
            }
            catch(Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                _LogClass.MYEventLog.WriteEntry(string.Concat(ex.Message, "\r\n", site.Name), EventLogEntryType.Error, 9999);
                MyReset();
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

                if (_LogClass != null)
                {
                    _LogClass.Dispose();
                    _LogClass = null;
                }

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
