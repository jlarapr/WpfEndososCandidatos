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
    using System.Collections;
    using MVVM.Client.Infrastructure.Behaviors;
    using System.Windows.Interactivity;
    using System.Windows.Controls;
    using MVVM.Client.Infrastructure;
    using System.Collections.Specialized;

    class vmMantAreas : ViewModelBase<IDialogViewAreas>, IDisposable 
    {

        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private ObservableCollection<string> _cbArea;
        private string _cbArea_Item;
        private int _cbArea_Item_Id;
        private string _DBEndososCnnStr;
        private string _DBCeeMasterCnnStr;
        private string _DBImagenesCnnStr;
        private ObservableCollection<Precintos> _lsAllPrecints;
        private Precintos _lsAllPrecints_Item;
        private int _lsAllPrecints_Item_Id;
        private ObservableCollection<Precintos> _lsValidPrecints;
        private Precintos _lsValidPrecints_Item;
        private bool _IsEnabled_CmdAddAllPrec;
        private bool _IsEnabled_CmdAddPrec;
        private bool _IsEnabled_CmdRemovePrec;
        private bool _IsEnabled_CmdRemoveAllPrec;
        private bool _IsEnabled_CmdEditar;
        private bool _IsEnabled_CmdGuardar;
        private bool _IsEnabled_cbArea;
        private bool _IsEnabled_CmdCancel;
        private DataTable _MyAreasTable;
        private DataTable _MyPrecintosTable;
        private bool _IsEditable_cbArea;
        private bool _IsEnabled_CmdSalir;
        private bool _IsEnabled_txtArea1;
        private bool _IsEnabled_txtArea2;
        private string _txtArea1;
        private string _txtArea2;
        private string _txtArea0;
        private Visibility _Visibility_cbArea;

        public vmMantAreas()
            : base(new wpfMantAreas())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => CmdSalir_Click(), param => CommandCan);
            cmdAddAllPrec_Click = new RelayCommand(param => MyCmdAddAllPrec_Click());
            cmdRemoveAllPrec_Click = new RelayCommand(param => MyCmdRemoveAllPrec_Click());
            cmdAddPrec_Click = new RelayCommand(param => MyCmdAddPrec_Click());
            cmdRemovePrec_Click = new RelayCommand(param => MyCmdRemovePrec_Click());
            cmdEditar_Click = new RelayCommand(param => MyCmdEditar_Click());
            cmdCancel_Click = new RelayCommand(param => MyCmdCancel_Click());
            cmdGuardar_Click = new RelayCommand(param => MyCmdGuardar_Click());
        }


        #region MyCmd
        private void MyCmdGuardar_Click ()
        {
            try
            {
                string myArea=string.Empty;
                string myDesc = string.Empty;
                string myPrecintos = string.Empty;
                string myElectivePositionID = "0";
                string myDemarcationID = "0";
                string myWhere = string.Empty;
               

                myWhere = cbArea_Item.Substring(0, 4);
                myArea = string.Concat(txtArea0, txtArea1);
                myDesc = txtArea2;

                foreach (Precintos p in lsValidPrecints.ToList())
                {
                    myPrecintos += p.ToString().Split('-')[0].Trim() + "|";
                }

                bool myUpDate = false;

                using (SqlExcuteCommand mySqlExe = new SqlExcuteCommand()
                {
                    DBCnnStr = _DBEndososCnnStr
                })
                {
                    myUpDate = mySqlExe.MyChangeArea(false, myArea, myDesc, myPrecintos, myElectivePositionID, myDemarcationID, myWhere);

                    if (myUpDate)
                    {
                        _MyAreasTable = mySqlExe.MyGetAreas();
                        cbArea.Clear();
                        foreach (DataRow row in _MyAreasTable.Rows)
                        {
                            cbArea.Add(row[0].ToString());
                        }
                    }
                }

                if (!myUpDateError)
                    throw new Exception("Error en la Base de Data");

                MyCmdCancel_Click();
                MessageBox.Show("Done...", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"Error",MessageBoxButton.OK,MessageBoxImage.Error); 
            }
        }
        private void MyCmdRemovePrec_Click()
        {
            try
            {
                while (this.View.lsValidPrecints.SelectedItems.Count > 0)
                {
                    lsAllPrecints.Add(lsValidPrecints_Item);
                    lsValidPrecints.Remove(lsValidPrecints_Item);
                }
                lsAllPrecints.Sort();
                //lsValidPrecints.Sort();
            }
            catch
            {
                throw;
            }
        }
        private void MyCmdAddAllPrec_Click()
        {
            try
            {
                var myCopy = new ObservableCollection<Precintos>(this.lsAllPrecints);
                foreach(var item in myCopy)
                {
                    this.lsValidPrecints.Add(item);
                    this.lsAllPrecints.Remove(item);
                }
                //lsAllPrecints.Sort();
                lsValidPrecints.Sort();

            }
            catch (Exception)
            {
                throw;
            }
        }
        private void MyCmdRemoveAllPrec_Click()
        {
            try
            {
                var myCopy = new ObservableCollection<Precintos>(this.lsValidPrecints);
                foreach (var item in myCopy)
                {
                    this.lsAllPrecints.Add(item);
                    this.lsValidPrecints.Remove(item);
                }
                lsAllPrecints.Sort();
                //lsValidPrecints.Sort();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void MyCmdAddPrec_Click()
        {
            try
            {
                if (this.View.lsAllPrecints != null)
                {
                    while (this.View.lsAllPrecints.SelectedItems.Count > 0)
                    {
                        this.lsValidPrecints.Add(lsAllPrecints_Item);
                        this.lsAllPrecints.Remove(lsAllPrecints_Item);
                    }
                    //lsAllPrecints.Sort();
                    lsValidPrecints.Sort();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void MyCmdEditar_Click()
        {
            try
            {
                IsEnabled_cbArea = false;
                IsEnabled_CmdEditar = false;
                IsEnabled_CmdSalir = false;

                Visibility_cbArea = Visibility.Hidden;

                IsEnabled_CmdGuardar = true;
                IsEnabled_CmdAddAllPrec = true;
                IsEnabled_CmdAddPrec = true;
                IsEnabled_CmdRemoveAllPrec = true;
                IsEnabled_CmdRemovePrec = true;
                IsEnabled_CmdCancel = true;
                IsEnabled_txtArea1 = true;
                IsEnabled_txtArea2 = true;
            }
            catch (Exception)
            {
               
                throw;
            }
        }
        private void MyCmdCancel_Click()
        {
            try
            {
                IsEnabled_cbArea = true;
                IsEnabled_CmdSalir = true;

                IsEnabled_CmdGuardar = false;
                IsEnabled_CmdAddAllPrec = false;
                IsEnabled_CmdAddPrec = false;
                IsEnabled_CmdRemoveAllPrec = false;
                IsEnabled_CmdRemovePrec = false;
                IsEnabled_CmdCancel = false;

                IsEnabled_txtArea1 = false;
                IsEnabled_txtArea2 = false;
                txtArea0 = string.Empty;
                txtArea1 = string.Empty;
                txtArea2 = string.Empty;
                Visibility_cbArea = Visibility.Visible;

                cbArea_Item_Id = -1;
                lsAllPrecints.Clear();
                lsValidPrecints.Clear();
                MySetAllPrecinto(_MyPrecintosTable);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public RelayCommand cmdAddAllPrec_Click
        {
            get;private set;
        }
        public RelayCommand cmdRemoveAllPrec_Click
        {
            get;private set;
        }
        public RelayCommand cmdAddPrec_Click
        {
            get;private set;
        }
        public RelayCommand cmdRemovePrec_Click
        {
            get;private set;
        }
        public RelayCommand cmdEditar_Click
        {
            get;private set;
        }
        public RelayCommand cmdCancel_Click
        {
            get;private set;
        }
        public RelayCommand cmdGuardar_Click
        {
            get;private set;
        }
        #endregion

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
            }set
            {
                _DBEndososCnnStr = value;
            }
        }
        public string DBCeeMasterCnnStr
        {
            get
            {
                return _DBCeeMasterCnnStr;
            }set
            {
                _DBCeeMasterCnnStr = value;
            }
        }
        public string DBImagenesCnnStr
        {
            get
            {
                return _DBImagenesCnnStr;
            }set
            {
                _DBImagenesCnnStr = value;
            }
        }
        public ObservableCollection<Precintos> lsAllPrecints
        {
            get
            {
                return _lsAllPrecints;
            }set
            {
                _lsAllPrecints = value;
                this.RaisePropertychanged("lsAllPrecints");
            }
        }
        public Precintos lsAllPrecints_Item
        {
            get
            {
                return _lsAllPrecints_Item;
            }set
            {
                _lsAllPrecints_Item = value;
                this.RaisePropertychanged("lsAllPrecints_Item");
            }
        }
        public int lsAllPrecints_Item_Id
        {
            get
            {
                return _lsAllPrecints_Item_Id;
            }set
            {
                _lsAllPrecints_Item_Id = value;
                this.RaisePropertychanged("lsAllPrecints_Item_Id");
            }
        }
        public ObservableCollection<Precintos> lsValidPrecints
        {
            get
            {
                return _lsValidPrecints;
            }set
            {
                _lsValidPrecints = value;
                this.RaisePropertychanged("lsValidPrecints");           
            }
        }
        public Precintos lsValidPrecints_Item
        {
            get
            {
                return _lsValidPrecints_Item; 
            }set
            {
                _lsValidPrecints_Item = value;
                this.RaisePropertychanged("lsValidPrecints_Item");
            }
        }
        public ObservableCollection<string> cbArea
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

                if (value != null && cbArea_Item_Id > -1)
                {
                    using (SqlExcuteCommand mySqlExcuteCommand = new SqlExcuteCommand()
                    {
                        DBCnnStr = _DBEndososCnnStr
                    })
                    {
                        string myArea = value.Substring(0, 4);
                        txtArea0 = value.Substring(0, 1);
                        txtArea1 = value.Substring(1, 3);
                        txtArea2 = value.Split('-')[1];
                        DataTable t = mySqlExcuteCommand.MyGetPrecintosValidos(myArea);

                        MySetValidPrecintos(t);
                    }
                }


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
                if (value >-1)
                {
                    IsEnabled_CmdGuardar = false;
                    IsEnabled_CmdEditar = true;
                }else
                {
                    IsEnabled_CmdGuardar = false;
                    IsEnabled_CmdEditar = false;
                }
                _cbArea_Item_Id = value;
                this.RaisePropertychanged("cbArea_Item_Id");               
            }
        }

        public bool IsEnabled_CmdAddAllPrec
        {
            get
            {
                return _IsEnabled_CmdAddAllPrec;
            }set
            {
                _IsEnabled_CmdAddAllPrec = value;
                this.RaisePropertychanged("IsEnabled_CmdAddAllPrec");
            }
        }
        public bool IsEnabled_CmdAddPrec
        {
            get
            {
                return _IsEnabled_CmdAddPrec;
            }set
            {
                _IsEnabled_CmdAddPrec = value;
                this.RaisePropertychanged("IsEnabled_CmdAddPrec");
            }
        }
        public bool IsEnabled_CmdRemovePrec
        {
            get
            {
                return _IsEnabled_CmdRemovePrec;
            }
            set
            {
                _IsEnabled_CmdRemovePrec = value;
                this.RaisePropertychanged("IsEnabled_CmdRemovePrec");
            }
        }
        public bool IsEnabled_CmdRemoveAllPrec
        {
            get
            {
                return _IsEnabled_CmdRemoveAllPrec;
            }set
            {
                _IsEnabled_CmdRemoveAllPrec = value;
                this.RaisePropertychanged("IsEnabled_CmdRemoveAllPrec");
            }
        }
        public bool IsEnabled_CmdEditar
        {
            get
            {
                return _IsEnabled_CmdEditar;
            }
            set
            {
                _IsEnabled_CmdEditar = value;
                this.RaisePropertychanged("IsEnabled_CmdEditar");
            }
        }
        public bool IsEnabled_CmdGuardar
        {
            get
            {
                return _IsEnabled_CmdGuardar;
            }
            set
            {
                _IsEnabled_CmdGuardar = value;
                this.RaisePropertychanged("IsEnabled_CmdGuardar");
            }
        }
        public bool IsEnabled_cbArea
        {
            get
            {
                return _IsEnabled_cbArea;
            }
            set
            {
                _IsEnabled_cbArea = value;
                this.RaisePropertychanged("IsEnabled_cbArea");
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
        public bool IsEditable_cbArea
        {
            get
            {
                return _IsEditable_cbArea;
            }
            set
            {
                _IsEditable_cbArea = value;
                this.RaisePropertychanged("IsEditable_cbArea");
            }
        }
        public bool IsEnabled_CmdSalir
        {
            get
             {
                return _IsEnabled_CmdSalir;
            }
            set
            {
                _IsEnabled_CmdSalir = value;
                this.RaisePropertychanged("IsEnabled_CmdSalir");
            }
        }
        public bool IsEnabled_txtArea1
        {
            get
            {
                return _IsEnabled_txtArea1;
            }
            set
            {
                _IsEnabled_txtArea1 = value;
                this.RaisePropertychanged("IsEnabled_txtArea1");
            }
        }
        public bool IsEnabled_txtArea2
        {
            get
            {
                return _IsEnabled_txtArea2;
            }
            set
            {
                _IsEnabled_txtArea2 = value;
                this.RaisePropertychanged("IsEnabled_txtArea2");
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

        public string txtArea1
        {
            get
            {
                return _txtArea1;
            }
            set
            {
                _txtArea1 = value;
                this.RaisePropertychanged("txtArea1");
            }
        }
        public string txtArea2
        {
            get
            {
                return _txtArea2;
            }
            set
            {
                if (value != null)
                {
                    _txtArea2 = value.Trim().ToUpper();
                    this.RaisePropertychanged("txtArea2");
                }
            }
        }
        public string txtArea0
        {
            get
            {
                return _txtArea0;
            }
            set
            {
                if (value != null)
                {
                    _txtArea0 = value.Trim();
                    this.RaisePropertychanged("txtArea0");
                }
            }
        }


        #region initWindow OnShow
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

                cbArea = new ObservableCollection<string>();
                lsAllPrecints = new ObservableCollection<Precintos>();
                lsValidPrecints = new ObservableCollection<Precintos>();
                cbArea_Item_Id = -1;

                txtArea0 = string.Empty;
                txtArea1 = string.Empty;
                txtArea2 = string.Empty;

                IsEnabled_CmdAddAllPrec = false;
                IsEnabled_CmdAddPrec = false;
                IsEnabled_CmdRemovePrec = false;
                IsEnabled_CmdRemoveAllPrec = false;

                IsEnabled_CmdEditar = false;
                IsEnabled_CmdGuardar = false;
                IsEnabled_CmdCancel = false;
                IsEditable_cbArea = false;

                IsEnabled_txtArea1 = false;
                IsEnabled_txtArea2 = false;

                IsEnabled_cbArea = true;
                IsEnabled_CmdSalir = true;

                Visibility_cbArea = Visibility.Visible;

                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {

                    _MyAreasTable = get.MyGetAreas();

                    foreach (DataRow row in _MyAreasTable.Rows)
                    {
                        cbArea.Add(row[0].ToString());
                    }

                    _MyPrecintosTable = get.MyGetPrecintos();

                    this.View.lsAllPrecints.ItemsSource = lsAllPrecints;
                    this.View.lsValidPrecints.ItemsSource = lsValidPrecints;

                    MySetAllPrecinto(_MyPrecintosTable);


                }
            }
            catch (Exception ex)
            {

                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

   

        public bool? OnShow()
        {
            return this.View.ShowDialog();
        }
        public RelayCommand initWindow
        {
            get;
            private set;
        }
        #endregion
        #region Metodos
        private bool MySetValidPrecintos(DataTable Table)
        {
            bool myBoolOut = false;
            try
            {
                if (lsValidPrecints.Count > 0)
                {
                    foreach (Precintos p in this.lsValidPrecints.ToList())
                    {
                        lsAllPrecints.Add(p);
                        lsValidPrecints.Remove(p);
                    }
                    lsAllPrecints.Sort();
                }

                string[] myPrecitosBuff = Table.Rows[0][1].ToString().Split('|');

                foreach (string precinto in myPrecitosBuff)
                {
                    foreach (Precintos itemprecinto in this.lsAllPrecints.ToList())
                    {
                        string myPrecinto = itemprecinto.PrecintoKey;
                        if (myPrecinto == precinto)
                        {
                            this.lsValidPrecints.Add(itemprecinto);
                            this.lsAllPrecints.Remove(itemprecinto);
                        }
                    }

                }
                if (lsValidPrecints.Count>0)
                    lsValidPrecints.Sort();
                myBoolOut = true;
            }
            catch (Exception)
            {

                throw;
            }
            return myBoolOut;
        }
       private bool MySetAllPrecinto(DataTable Table)
        {
            bool myBoolOut = false;
            try
            {
                lsAllPrecints.Clear();
                foreach (DataRow row in Table.Rows)
                {
                    string myPrecinto = row["Precinto"].ToString();
                    string myDesc = row["Desc"].ToString();
                    Precintos item = new Precintos();
                    item.PrecintoKey = myPrecinto;
                    item.Desc = myDesc;
                    lsAllPrecints.Add(item);
                }
                lsAllPrecints.Sort();
                myBoolOut = true;
            }
            catch
            {
                throw;
            }
            return myBoolOut;
        }
        
        
        #endregion



        #region Exit
        public RelayCommand cmdSalir_Click
        {
            get;
            private set;
        }
        public void CmdSalir_Click()
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
        private bool CommandCan
        {
            get
            {
                return true;
            }
        }


        #endregion
        #region Dispose

       

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmMantAreas()
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
