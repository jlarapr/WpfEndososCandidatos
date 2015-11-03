

namespace WpfEndososCandidatos.ViewModels.Configuraciones
{
    using jolcode;
    using jolcode.Base;
    using jolcode.MyInterface;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using WpfEndososCandidatos.View;

    class vmMantNotarios : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        private Brush _BorderBrush;
        private Brush _Background_txtNombre;
        private Brush _Background_txtApellido2;

        private string _txtNumElec;

        public vmMantNotarios() : base(new wpfMantNotarios())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click(), param => CanSalir);
            cmdAdd_Click = new RelayCommand(param => MyCmdAdd_Click());
            cmdCancel_Click = new RelayCommand(param => MyCmdCancel_Click());
            cmdEdit_Click = new RelayCommand(param => MyCmdEdit_Click());
            cmdSave_Click = new RelayCommand(param => MyCmdSave_Click());
            cmdDelete_Click = new RelayCommand(param => MyCmdDelete_Click());
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
        private bool CanSalir
        {
            get
            {
                return true;
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
                IsEnabled_cmdSave = true;
                IsEnabled_cmdEdit = false;

                IsEnabled_cmdSalir = false;
                IsReadOnly_txtNumPartido = false;
                IsReadOnly_txtEndoReq = false;
                IsReadOnly_txtAreaGeografica = false;

                _IsEdit = true;

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
                if ((txtNumPartido.Trim().Length == 0) || (txtNombre.Trim().Length == 0) || (txtEndoReq.Trim().Length == 0) || (txtAreaGeografica.Trim().Length == 0))
                    return;

                bool myUpDate = false;

                string myWhere = string.Empty;

                myWhere = _IsInsert == false ? _PartidoTmp : "";

                if (myWhere.Trim().Length == 0)
                    myWhere = txtNumPartido;

                using (SqlExcuteCommand mySqlExe = new SqlExcuteCommand()
                {
                    DBCnnStr = _DBEndososCnnStr
                })
                {
                    myUpDate = mySqlExe.MyChangePartidos(_IsInsert, txtNumPartido, txtNombre, txtEndoReq, txtAreaGeografica, myWhere);
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
                if ((txtNumPartido == null) || (txtNombre == null) || (txtEndoReq == null) || (txtAreaGeografica == null) || (_IsEdit == false))
                    return false;

                if ((txtNumPartido.Trim().Length == 0) || (txtNombre.Trim().Length == 0) || (txtEndoReq.Trim().Length == 0) || (txtAreaGeografica.Trim().Length == 0))
                    return false;

                return true;
            }
        }
        private void MyCmdDelete_Click()
        {
            try
            {
                var response = MessageBox.Show("!!!Do you really want to Delete this Partido?\r\n" + txtNumPartido, "Deleting...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (response == MessageBoxResult.No)
                    return;

                string myWhere = string.Empty;
                myWhere = txtNumPartido;
                bool myDelete = false;

                using (SqlExcuteCommand mySqlExe = new SqlExcuteCommand()
                {
                    DBCnnStr = _DBEndososCnnStr
                })
                {
                    myDelete = mySqlExe.MyDeletePartidos(myWhere);


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
                Visibility_txtAreaGeografica = Visibility.Hidden;

                Visibility_cbPartidos = Visibility.Hidden;
                Visibility_cbArea = Visibility.Visible;

                IsEnabled_CmdCancel = true;
                IsEnabled_cmdDelete = false;
                IsEnabled_cmdSave = true;
                IsEnabled_cmdEdit = false;

                IsEnabled_cmdSalir = false;
                IsReadOnly_txtNumPartido = false;
                IsReadOnly_txtEndoReq = false;
                IsReadOnly_txtAreaGeografica = false;

                Background_txtEndoReq = Brushes.Beige;
                Background_txtNumPartido = Brushes.Beige;

                IsEnabled_cmdAdd = false;

                _IsInsert = true;
                _IsEdit = true;

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
            cbArea_Item_Id = -1;
            cbPartidos_Item_Id = -1;

            _IsEdit = false;
            _IsInsert = false;
            _PartidoTmp = string.Empty;

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
            txtNumElec = string.Empty; 

            Visibility_txtNombre = Visibility.Hidden;
            Visibility_cbPartidos = Visibility.Visible;
            Visibility_txtAreaGeografica = Visibility.Visible;
            Visibility_cbArea = Visibility.Hidden;

            IsReadOnly_txtNumPartido = true;
            IsReadOnly_txtEndoReq = true;
            IsReadOnly_txtAreaGeografica = true;

            Background_txtNombre = Brushes.Yellow;
            Background_txtApellido2 = Brushes.Yellow;
        }

        private void MyRefresh()
        {
            try
            {
                cbArea.Clear();
                cbPartidos.Clear();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
