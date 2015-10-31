
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
        private ObservableCollection<string> _cbArea;
        private string _cbArea_Item;
        private int _cbArea_Item_Id;
        private DataTable _MyAreasTable;
        private DataTable _MyPartidosTable;
        private string _DBEndososCnnStr;
        private string _txtEndoReq;
        private ObservableCollection<Partidos> _cbPartidos;
        private string _cbPartidos_Item;
        private int _cbPartidos_Item_Id;

        public vmMantPartidos()
            : base(new wpfMantPartidos())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdCancel_Click = new RelayCommand(param => MyCmdCancel_Click());

            cbArea = new ObservableCollection<string>();
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
                _cbPartidos_Item = value;
                this.RaisePropertychanged("cbPartidos_Item");
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
        public string txtEndoReq
        {
            get
            {
                return _txtEndoReq;
            }set
            {
                _txtEndoReq = value;
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
                    _MyAreasTable = get.MyGetAreas();

                    foreach (DataRow row in _MyAreasTable.Rows)
                    {
                        cbArea.Add(row[0].ToString());
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
        #endregion

        #region MyMeodos
        private void MyReset()
        {
            cbArea_Item_Id = -1;
            cbPartidos_Item_Id = -1;
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

    }
}
