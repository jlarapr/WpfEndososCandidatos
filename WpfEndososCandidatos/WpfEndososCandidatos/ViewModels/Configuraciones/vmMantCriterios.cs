﻿

namespace WpfEndososCandidatos.ViewModels.Configuraciones
{
    using jolcode;
    using jolcode.Base;
    using jolcode.MyInterface;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using Models;
    using WpfEndososCandidatos.View.Configuraciones;
    using System.Collections.ObjectModel;

    class vmMantCriterios : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private Logclass _LogClass;
        private DataTable _MyCriteriosTable;
        private List<Criterios> _Criterios;
        private string _DBEndososCnnStr;
        private ObservableCollection<Criterios> _chk;

        public vmMantCriterios()
            : base(new wpfMantCriterios())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => CmdSalir_Click(), param => CommandCan);

            _LogClass = new Logclass();
            _Criterios = new List<Criterios>();

            chk = new ObservableCollection<Criterios>();

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

        public ObservableCollection<Criterios> chk
        {
            get
            {
                return _chk;
            }set
            {
                _chk = value;
                this.RaisePropertychanged("chk");
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
                _LogClass.SourceName = "Criterios";
                _LogClass.MessageFile = string.Empty;
                _LogClass.CreateEvent();
                _LogClass.MYEventLog.WriteEntry("Criterios Start:" + Dia + " " + Hora, EventLogEntryType.Information);

              
             
              

                MyRefresh();
                //MyReset();

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

        public RelayCommand initWindow
        {
            get;
            private set;
        }
        #endregion

        #region MyMetodos

        private void MyRefresh()
        {
            try
            {
                using (SqlExcuteCommand get = new SqlExcuteCommand()
                {
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    _MyCriteriosTable = get.MyGetCriterios();

                    foreach (DataRow row in _MyCriteriosTable.Rows)
                    {
                        chk.Add(new Models.Criterios
                        {
                            Campo = row["Campo"].ToString(),
                            Editar = row["Editar"] as bool?,
                            Desc = row["Desc"].ToString(),
                            Warning = row["Warning"].ToString()
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void MyReset()
        {

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

        ~vmMantCriterios()
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
