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

        public vmMantAreas()
            : base(new wpfMantAreas())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => CmdSalir_Click(), param => CommandCan);
            cmdAddAllPrec_Click = new RelayCommand(param => MyCmdAddAllPrec_Click());
            cmdRemoveAllPrec_Click = new RelayCommand(param => MyCmdRemoveAllPrec_Click());
            cmdAddPrec_Click = new RelayCommand(param => MyCmdAddPrec_Click());
            cmdRemovePrec_Click = new RelayCommand(param => MyCmdRemovePrec_Click());

        }


        #region MyCmd
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
                        string myArea = this.cbArea_Item.Substring(0, 4);

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
                _cbArea_Item_Id = value;
                this.RaisePropertychanged("cbArea_Item_Id");
               
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
                              
                
               

                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {

                    DataTable myAreas = get.MyGetAreas();

                    foreach (DataRow row in myAreas.Rows)
                    {
                        cbArea.Add(row[0].ToString());
                    }

                    DataTable myPrecintos = get.MyGetPrecintos();

                    this.View.lsAllPrecints.ItemsSource = lsAllPrecints;
                    this.View.lsValidPrecints.ItemsSource = lsValidPrecints;

                    MySetAllPrecinto(myPrecintos);


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
