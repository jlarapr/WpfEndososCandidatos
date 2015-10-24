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
    using WpfEndososCandidatos.View;

    class vmMantAreas : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private ObservableCollection<string> _cbArea;
        private string _cbArea_Item;
        private int _cbArea_Item_Id;
        private string _DBEndososCnnStr;
        private string _DBCeeMasterCnnStr;
        private string _DBImagenesCnnStr;
        private ObservableCollection<string> _lsAllPrecints;
        private string _lsAllPrecints_Item;
        private int _lsAllPrecints_Item_Id;
        private ObservableCollection<string> _lsValidPrecints;
        private string _lsValidPrecints_Item;

        public vmMantAreas()
            : base(new wpfMantAreas())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => CmdSalir_Click(), param => CommandCan);
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
        public ObservableCollection<string> lsAllPrecints
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
        public string lsAllPrecints_Item
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
        public ObservableCollection<string> lsValidPrecints
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
        public string lsValidPrecints_Item
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

                        foreach (DataRow row in t.Rows)
                        {
                            string[] mypresitos = row[1].ToString().Split('|');
                            this.lsValidPrecints.Add(row[1].ToString());
                        }
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
                this.lsValidPrecints.Clear();
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
                lsAllPrecints = new ObservableCollection<string>();
                lsValidPrecints = new ObservableCollection<string>();
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

                    foreach(DataRow row in myPrecintos.Rows)
                    {
                        lsAllPrecints.Add(row[0].ToString());
                    }
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
