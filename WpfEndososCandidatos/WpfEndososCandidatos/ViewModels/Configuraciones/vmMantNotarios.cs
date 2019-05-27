

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
        private bool _IsReadOnly_txtNombreAspirante;
        private Brush _Background_txtNombreAspirante;
        private ObservableCollection<Candidatos> _cbAspirante;
        //private ObservableCollection<Partidos> _cbPartidos;
        //private string _cbPartidos_Item;
        //private int _cbPartidos_Item_Id;
        private bool _IsEnabled_cbPartidos;
        private string _cbAspirante_Item;
        private int _cbAspirante_Item_Id;
        //DataTable _MyPartidosTable;
        private DataTable _MyAspiranteTable;
        private DataTable _MyNotarioTable;
        private string _txtNombreAspirante;
        private Visibility _Visibility_txtNombreAspirante;
        private bool _IsEnabled_txtNombreAspirante;
        private bool _IsEnabled_cbAspirante;
        private ObservableCollection<Notarios> _cbNotario;
        private string _cbNotario_Item;
        private int _cbNotario_Item_Id;
        private string _txtNombre;
        private Visibility _Visibility_txtNombre;

        private string _txtApellido1;
        private string _txtApellido2;
        private Visibility _Visibility_cbAspirante;
        private Visibility _Visibility_cbNotario;
        private Visibility _Visibility_cbPartidos;
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
        // private bool _CanFind;
        private string _txtStatusElec;
        private string _txtNumElecAspirante;

        public vmMantNotarios() : base(new wpfMantNotarios())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click(), param => CanSalir);
            cmdAdd_Click = new RelayCommand(param => MyCmdAdd_Click());
            cmdCancel_Click = new RelayCommand(param => MyCmdCancel_Click());
            cmdEdit_Click = new RelayCommand(param => MyCmdEdit_Click());
            cmdSave_Click = new RelayCommand(param => MyCmdSave_Click(), param => CanSave);
            cmdDelete_Click = new RelayCommand(param => MyCmdDelete_Click());
            cmdFind_Click = new RelayCommand(param => MyCmdFind_Click(), param => CanFind);

            cmdFindAspirante_Click = new RelayCommand(param => MyCmdFindAspirante_Click(), param => CanFindAspirante);

            cbAspirante = new ObservableCollection<Candidatos>();
            cbNotario = new ObservableCollection<Notarios>();

            _LogClass = new Logclass();
        }

        #region MyPrperty
        public string txtStatusElec
        {
            get
            {
                return _txtStatusElec;
            }
            set
            {
                _txtStatusElec = value;
                this.RaisePropertychanged("txtStatusElec");
            }
        }
        public bool CanFind
        {
            get
            {
                if (string.IsNullOrEmpty(txtNumElec))
                {
                    txtNombre = string.Empty;
                    txtApellido1 = string.Empty;
                    txtApellido2 = string.Empty;
                    //txtNombreAspirante = string.Empty;
                    //cbAspirante_Item_Id = -1;
                    //MySendTab();

                    return false;
                }

                if (txtNumElec.Trim().Length <= 0)
                {
                    txtNombre = string.Empty;
                    txtApellido1 = string.Empty;
                    txtApellido2 = string.Empty;
                    //txtNombreAspirante = string.Empty;

                    //cbAspirante_Item_Id = -1;
                    //MySendTab();

                    return false;
                }

                int i;
                if (!int.TryParse(txtNumElec, out i))
                {
                    txtNombre = string.Empty;
                    txtApellido1 = string.Empty;
                    txtApellido2 = string.Empty;
                    //txtNombreAspirante = string.Empty;
                    //cbAspirante_Item_Id = -1;
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
            }
            set
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
            }
            set
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
            }
            set
            {
                _Background_txtNumElec = value;
                this.RaisePropertychanged("Background_txtNumElec");
            }
        }
        public Brush Background_txtNombreAspirante
        {
            get
            {
                return _Background_txtNombreAspirante;
            }
            set
            {
                _Background_txtNombreAspirante = value;
                this.RaisePropertychanged("Background_txtNombreAspirante");
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
                if ((txtNumElec == null) || (txtNombre == null) || (_isEdit == false) || (txtNombreAspirante == null))
                    return false;

                if ((txtNumElec.Trim().Length != 7) || (txtNombre.Trim().Length == 0) || (txtNombreAspirante.Trim().Length == 0))
                    return false;

                if (int.Parse(txtNumElec) == 0)
                    return false;

                return true;
            }
        }

        public bool CanFindAspirante
        {
            get
            {
                if (string.IsNullOrEmpty(txtNumElecAspirante))
                {
                    return false;
                }

                if (txtNumElecAspirante.Trim().Length <= 0)
                {
                    return false;
                }

                int i;
                if (!int.TryParse(txtNumElecAspirante, out i))
                {
                    return false;
                }

                return true;
            }
        }
        public bool IsReadOnly_txtNumElec
        {
            get
            {
                return _IsReadOnly_txtNumElec;
            }
            set
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
            }
            set
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
            }
            set
            {
                _IsReadOnly_txtNombre = value;
                this.RaisePropertychanged("IsReadOnly_txtNombre");
            }
        }
        public bool IsReadOnly_txtNombreAspirante
        {
            get
            {
                return _IsReadOnly_txtNombreAspirante;
            }
            set
            {
                _IsReadOnly_txtNombreAspirante = value;
                this.RaisePropertychanged("IsReadOnly_txtNombreAspirante");
            }

        }
        public bool IsEnabled_txtNombreAspirante
        {
            get
            {
                return _IsEnabled_txtNombreAspirante;
            }
            set
            {
                _IsEnabled_txtNombreAspirante = value;
                this.RaisePropertychanged("IsEnabled_txtNombreAspirante");
            }
        }
        public bool IsEnabled_cbAspirante
        {
            get
            {
                return _IsEnabled_cbAspirante;
            }
            set
            {
                _IsEnabled_cbAspirante = value;
                this.RaisePropertychanged("IsEnabled_cbAspirante");
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
            }
            set
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
            }
            set
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

        public string txtNumElecAspirante
        {
            get
            {
                return _txtNumElecAspirante;
            }
            set
            {

                if (_txtNumElecAspirante != value)
                {
                    cbAspirante_Item_Id = -1;
                    txtNombreAspirante = string.Empty;


                    _txtNumElecAspirante = value;
                    this.RaisePropertychanged("txtNumElecAspirante");
                }
            }
        }

        public string txtNumElec
        {
            get
            {
                return _txtNumElec;
            }
            set
            {
                if (value != null)
                {
                    if (value.Trim().Length > 0)
                    {
                        if (value != _txtNumElec)
                        {
                            txtApellido1 = string.Empty;
                            txtApellido2 = string.Empty;
                            txtNombre = string.Empty;
                            cbAspirante_Item_Id = -1;

                            txtNombreAspirante = string.Empty;
                            txtStatusElec = string.Empty;

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
                        cbAspirante_Item_Id = -1;

                        txtNombreAspirante = string.Empty;
                        txtStatusElec = string.Empty;
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
        public string txtNombreAspirante
        {
            get
            {
                return _txtNombreAspirante;
            }
            set
            {
                _txtNombreAspirante = value;
                this.RaisePropertychanged("txtNombreAspirante");
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
        public string txtApellido1
        {
            get
            {
                return _txtApellido1;
            }
            set
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

        public Visibility Visibility_txtNombreAspirante
        {
            get
            {
                return _Visibility_txtNombreAspirante;
            }
            set
            {
                _Visibility_txtNombreAspirante = value;
                this.RaisePropertychanged("Visibility_txtNombreAspirante");
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
        public Visibility Visibility_cbAspirante
        {
            get
            {
                return _Visibility_cbAspirante;
            }
            set
            {
                _Visibility_cbAspirante = value;
                this.RaisePropertychanged("Visibility_cbAspirante");
            }
        }
        public Visibility Visibility_cbNotario
        {
            get
            {
                return _Visibility_cbNotario;
            }
            set
            {
                _Visibility_cbNotario = value;
                this.RaisePropertychanged("Visibility_cbNotario");
            }
        }






        public Visibility Visibility_cbPartidos
        {
            get
            {
                return _Visibility_cbPartidos;
            }
            set
            {
                _Visibility_cbPartidos = value;
                this.RaisePropertychanged("Visibility_cbPartidos");
            }
        }

        public bool IsEnabled_cbPartidos
        {
            get
            {
                return _IsEnabled_cbPartidos;
            }
            set
            {
                _IsEnabled_cbPartidos = value;
                this.RaisePropertychanged("IsEnabled_cbPartidos");
            }

        }
        public ObservableCollection<Candidatos> cbAspirante
        {
            get
            {
                return _cbAspirante;
            }
            set
            {
                _cbAspirante = value;
                this.RaisePropertychanged("cbAspirante");
            }
        }
        public string cbAspirante_Item
        {
            get
            {
                return _cbAspirante_Item;
            }
            set
            {
                if (value != null)
                {
                    //txtNumElecAspirante = value.Split('-')[1];
                    _cbAspirante_Item = value;
                    txtNombreAspirante = value;


                    this.RaisePropertychanged("cbAspirante_Item");
                }
            }
        }
        public int cbAspirante_Item_Id
        {
            get
            {
                return _cbAspirante_Item_Id;
            }
            set
            {
                if (_cbAspirante_Item_Id != value)
                {
                    if (!string.IsNullOrEmpty(txtNumElecAspirante))
                        txtNumElecAspirante = string.Empty;



                    _cbAspirante_Item_Id = value;

                    this.RaisePropertychanged("cbAspirante_Item_Id");
                }
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
                    txtNombre = myData[1].Trim();
                    txtApellido1 = myData[2].Trim();
                    txtApellido2 = myData[3].Trim();

                    cbAspirante_Item_Id = FindByAspirante(myData[4].Trim());
                    txtNombreAspirante = cbAspirante_Item;
                    txtStatusElec = myData[5].Trim();

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
                _LogClass.MYEventLog.WriteEntry(string.Concat(ex.Message, "\r\n", site.Name), EventLogEntryType.Error, 9999);
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
                Visibility_cbAspirante = Visibility.Visible;
                Visibility_cbPartidos = Visibility.Visible;
                Visibility_txtNombreAspirante = Visibility.Hidden;

                IsEnabled_cbAspirante = true;
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
                }
                else
                {
                    myWhere = "";
                }



                using (SqlExcuteCommand mySqlExe = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    myUpDate = mySqlExe.MyChangeNotario(_IsInsert, txtNumElec, txtNombreAspirante.Split('-')[0], txtNombreAspirante.Split('-')[1], txtNombre, txtApellido1, txtApellido2, txtStatusElec, myWhere);
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
                var response = MessageBox.Show("!!!Do you really want to Delete this ?\r\n", "Deleting...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (response == MessageBoxResult.No)
                    return;

                string myWhere = string.Empty;
                string myWhere2 = string.Empty;
                string NumCand = string.Empty;

                myWhere = txtNumElec.Trim();
                myWhere2 = cbAspirante_Item.Split('-')[0];
                NumCand = cbAspirante_Item.Split('-')[1];

                bool myDelete = false;

                using (SqlExcuteCommand mySqlExe = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    myDelete = mySqlExe.MyDeleteNotario(myWhere, myWhere2, NumCand);


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
                MyReset();

                Visibility_txtNombre = Visibility.Visible;
                Visibility_cbNotario = Visibility.Hidden;

                Visibility_cbAspirante = Visibility.Visible;
                Visibility_cbPartidos = Visibility.Visible;

                Visibility_txtNombreAspirante = Visibility.Hidden;

                IsEnabled_cbAspirante = true;
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
                cbAspirante_Item_Id = -1;



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

        private void MyCmdFindAspirante_Click()
        {
            try
            {
                cbAspirante_Item_Id = FindByAspirante(txtNumElecAspirante);
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                _LogClass.MYEventLog.WriteEntry(string.Concat(ex.Message, "\r\n", site.Name), EventLogEntryType.Error, 9999);
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
            get; private set;
        }
        public RelayCommand cmdAdd_Click
        {
            get; private set;
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
        public RelayCommand cmdFindAspirante_Click
        {
            get; private set;
        }
        #endregion

        #region MyMetodos

        private void MyReset()
        {
            //_CanFind = false;

            IsReadOnly_txtNumElec = true;
            IsReadOnly_txtApellido1 = true;
            IsReadOnly_txtApellido2 = true;
            IsReadOnly_txtNombreAspirante = true;
            IsReadOnly_txtNombre = true;

            IsEnabled_txtNombreAspirante = true;

            IsEnabled_cbAspirante = false;
            IsEnabled_cbPartidos = false;

            IsEnabled_cmdAdd = true;
            IsEnabled_cmdDelete = false;
            IsEnabled_cmdSave = false;
            IsEnabled_cmdEdit = false;
            IsEnabled_CmdCancel = false;
            IsEnabled_cmdSalir = true;

            txtNombreAspirante = string.Empty;
            txtNombre = string.Empty;
            txtNumElec = string.Empty;
            txtNombre = string.Empty;
            txtApellido1 = string.Empty;
            txtApellido2 = string.Empty;
            txtStatusElec = string.Empty;
            txtNumElecAspirante = string.Empty;

            Background_txtNombre = Brushes.Yellow;
            Background_txtApellido2 = Brushes.Yellow;
            Background_txtApellido1 = Brushes.Yellow;
            Background_txtNumElec = Brushes.Yellow;
            Background_txtNombreAspirante = Brushes.Yellow;

            Visibility_txtNombreAspirante = Visibility.Visible;
            Visibility_txtNombre = Visibility.Hidden;
            Visibility_cbAspirante = Visibility.Hidden;
            Visibility_cbPartidos = Visibility.Hidden;
            Visibility_cbNotario = Visibility.Visible;

            _IsInsert = false;
            _isEdit = false;
            cbAspirante_Item_Id = -1;
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
                    _MyAspiranteTable = get.MyGetCandidatos();

                    cbAspirante.Clear();
                    foreach (DataRow row in _MyAspiranteTable.Rows)
                    {
                        Candidatos myCand = new Candidatos();
                        myCand.Partido = row["Partido"].ToString();
                        myCand.NumCand = row["NumCand"].ToString();
                        myCand.Nombre = row["Nombre"].ToString();
                        myCand.Area = row["Area"].ToString();
                        myCand.Cargo = row["Cargo"].ToString();
                        myCand.EndoReq = row["EndoReq"].ToString();
                        cbAspirante.Add(myCand);
                    }
                    cbAspirante_Item_Id = -1;

                    cbNotario.Clear();
                    foreach (DataRow row in _MyNotarioTable.Rows)
                    {
                        Notarios myNotario = new Notarios();

                        myNotario.NumElec = row["NumElec"].ToString();
                        myNotario.NumCand = row["NumCand"].ToString();
                        myNotario.Nombre = row["Nombre"].ToString();
                        myNotario.Apellido1 = row["Apellido1"].ToString();
                        myNotario.Apellido2 = row["Apellido2"].ToString();
                        myNotario.Status = row["Status"].ToString();
                        myNotario.AllColumn = false;
                        cbNotario.Add(myNotario);
                    }
                    cbNotario_Item_Id = -1;
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
                cbNotario_Item_Id = FindByNotario(txtNumElec);

                if (cbNotario_Item_Id < 0)
                {
                    using (SqlExcuteCommand get = new SqlExcuteCommand()
                    {
                        DBCeeMasterCnnStr = DBCeeMasterCnnStr

                    })
                    {
                        // _CanFind = true;

                        txtNumElec = txtNumElec.PadLeft(7, '0');

                        DataTable myTable = get.MyGetCitizen(_txtNumElec);
                        if (myTable.Rows.Count == 0)
                            throw new Exception("Número electoral invalido.");

                        txtNombre = myTable.Rows[0]["FirstName"].ToString();
                        txtApellido1 = myTable.Rows[0]["LastName1"].ToString();
                        txtApellido2 = myTable.Rows[0]["LastName2"].ToString();

                        switch (myTable.Rows[0]["Status"].ToString().Trim().ToUpper())
                        {
                            case "A":
                                txtStatusElec = "A";
                                break;
                            case "E":
                                //  MessageBox.Show("Excluido","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                                txtStatusElec = "E";
                                break;
                            case "I":
                                txtStatusElec = "I";
                                //MessageBox.Show("Inactivo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;
                            default:
                                txtStatusElec = "?";
                                break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                _LogClass.MYEventLog.WriteEntry(string.Concat(ex.Message, "\r\n", site.Name), EventLogEntryType.Error, 9999);
                MyReset();
            }
        }
        private int FindByNotario(string NotarioKey)
        {
            int myIndex = -1;
            try
            {

                Notarios myInfoNotario = cbNotario.Where(x => x.NumElec == NotarioKey).Single<Notarios>();
                myIndex = cbNotario.IndexOf(myInfoNotario);
            }
            //catch (System.InvalidOperationException exInOp)
            //{
            //    //MethodBase site = exInOp.TargetSite;

            //    //if (exInOp.Message == "Sequence contains no elements")
            //    //    MessageBox.Show("No Exite en la Tabla!!!!", site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            catch
            {
                //MethodBase site = ex.TargetSite;
                //MessageBox.Show(ex.ToString(), site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return myIndex;
        }


        private int FindByAspirante(string AspiranteKey)
        {
            int myIndex = -1;
            try
            {

                Candidatos myInfoAspirante = cbAspirante.Where(x => x.NumCand == AspiranteKey).Single<Candidatos>();
                myIndex = cbAspirante.IndexOf(myInfoAspirante);
            }
            catch (System.InvalidOperationException exInOp)
            {
                MethodBase site = exInOp.TargetSite;

                if (exInOp.Message == "Sequence contains no elements")
                    MessageBox.Show("Aspirante No Exite en la Tabla!!!!", site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.ToString(), site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
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

                // throw;
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
