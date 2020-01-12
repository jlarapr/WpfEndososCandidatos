using jolcode;
using jolcode.Base;
using jolcode.MyInterface;
using System;
using System.Windows.Media;
using System.Runtime.InteropServices;
using WpfEndososCandidatos.View.Informes;

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfEndososCandidatos.Models;
using System.Configuration;
using System.Reflection;
using System.Windows;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace WpfEndososCandidatos.ViewModels.Informes
{
    public class vmDuplicadoPorNumElec : ViewModelBase<IDialogView>, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private Brush _BorderBrush;
        private string _DBEndososCnnStr;
        private string _DBMasterCeeCnnStr;
        private string _DBCeeMasterImgCnnStr;
        private string _txtTotal;
        private ObservableCollection<LotsEndo> _ItemsSource;
        private string _TxtElecNum;
        private int _dgSelectedIndex;
        private infoEndososRechazados _dgSelectedItem;
        private ObservableCollection<String> fileTmp;

        public vmDuplicadoPorNumElec() : base(new wpfDuplicadoPorNumElec())
        {
            initWindow = new RelayCommand(param => MyInitWindow());
            cmdSalir_Click = new RelayCommand(param => MyCmdSalir_Click());
            cmdExecute_Click = new RelayCommand(param => MycmdExecute_Click());
            cmdToPdf_Click = new RelayCommand(param => MycmdToPdf_Click());

            ItemsSource = new ObservableCollection<LotsEndo>();
            fileTmp = new ObservableCollection<string>();

        }


        #region MyProperty
        public int WhatIsModo { get; set; }
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

        public string DBMasterCeeCnnStr
        {
            get
            {
                return _DBMasterCeeCnnStr;
            }
            set
            {
                _DBMasterCeeCnnStr = value;
            }
        }

        public string DBCeeMasterImgCnnStr
        {
            get
            {
                return _DBCeeMasterImgCnnStr;
            }
            set
            {
                _DBCeeMasterImgCnnStr = value;
            }
        }

        public string txtTotal
        {
            get
            {
                return _txtTotal;
            }
            set
            {

                _txtTotal = value;
                this.RaisePropertychanged("txtTotal");
            }
        }


        public ObservableCollection<LotsEndo> ItemsSource
        {
            get
            {
                return _ItemsSource;
            }
            set
            {
                _ItemsSource = value;
                this.RaisePropertychanged("ItemsSource");
            }
        }
        public int dgSelectedIndex
        {
            get
            {
                return _dgSelectedIndex;
            }
            set
            {
                _dgSelectedIndex = value;
                this.RaisePropertychanged("dgSelectedIndex");
            }
        }
        public infoEndososRechazados dgSelectedItem
        {
            get
            {
                return _dgSelectedItem;
            }
            set
            {
                _dgSelectedItem = value;
                this.RaisePropertychanged("dgSelectedItem");
            }
        }

        public string TxtElecNum
        {
            get
            {
                return _TxtElecNum;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (value != _TxtElecNum)
                        MyReset();

                    _TxtElecNum = value;

                    this.RaisePropertychanged("TxtElecNum");
                }
                else
                {
                    _TxtElecNum = null;
                    this.RaisePropertychanged("TxtElecNum");
                }

            }
        }

        public RelayCommand initWindow { get; private set; }
        public RelayCommand cmdSalir_Click { get; private set; }
        public RelayCommand cmdExecute_Click { get; private set; }
        public RelayCommand cmdToPdf_Click { get; private set; }
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
        private void MyCmdSalir_Click()
        {
            try
            {
                try
                {
                    foreach (String fileName in fileTmp)
                    {
                        System.IO.File.Delete(fileName);
                    }
                    System.IO.Directory.Delete("C:\\ApplicaTmp\\", true);

                }
                catch { }

                this.View.Close();
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.Message, site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MycmdExecute_Click()
        {
            try
            {

                ItemsSource.Clear();

                if (string.IsNullOrEmpty(TxtElecNum))
                    return;



                using (SqlExcuteCommand get = new SqlExcuteCommand
                {
                    DBCeeMasterCnnStr = DBMasterCeeCnnStr,
                    DBCnnStr = DBEndososCnnStr
                })
                {
                    txtTotal = "0";
                    DataTable T = new DataTable();
                    T = get.MyGetDuplicadoPorNumeroElectoral(TxtElecNum,WhatIsModo);
                    txtTotal = string.Format("{0:N0}", T.Rows.Count);

                    if (T.Rows.Count <= 0)
                    {
                        MessageBox.Show("No Hay Datos", TxtElecNum, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }

                    foreach (DataRow row in T.Rows)
                    {
                        LotsEndo datos = new LotsEndo();

                        //datos.GoodNumElec       = row["GoodNumElec"].ToString();
                        //datos.BadNumElec        = row["BadNumElec"].ToString();
                        //datos.GoodPartido       = row["GoodPartido"].ToString();
                        //datos.BadPartido        = row["BadPartido"].ToString();
                        //datos.GoodLot           = row["GoodLot"].ToString();
                        //datos.BadLot            = row["BadLot"].ToString();
                        //datos.GoodBatch         = row["GoodBatch"].ToString();
                        //datos.BadBatch          = row["BadBatch"].ToString();
                        //datos.GoodFormulario    = row["GoodFormulario"].ToString();
                        //datos.BadFormulario     = row["BadFormulario"].ToString();
                        //datos.GoodEndosoImage   = (byte[])row["GoodEndosoImage"];
                        //datos.BadEndosoImage    = (byte[])row["BadEndosoImage"];
                        //datos.GoodPathImage     = row["GoodPathImage"].ToString();
                        //datos.BadPathImage      = row["BadPathImage"].ToString();
                        //datos.GoodErrores       = row["GoodErrores"].ToString();
                        //datos.BadErrores        = row["BadErrores"].ToString();

                        datos.NumElec = row["NumElec"].ToString();
                        datos.Partido = row["Partido"].ToString();
                        datos.Lot = row["Lot"].ToString();
                        datos.Batch = row["Batch"].ToString();
                        datos.Formulario = row["Formulario"].ToString();
                        datos.Image = row["Image"].ToString();
                        datos.Errores = row["Errores"].ToString();
                        datos.Status = row["Status"].ToString();
                        datos.EndosoImage = get.MyGetImgInByte(datos.NumElec, datos.Partido, datos.Lot, datos.Batch, datos.Formulario, datos.Image, datos.Errores, datos.Status);
                        datos.Modo = int.Parse(row["Modo"].ToString());

                        if (WhatIsModo == 1)
                        {
                            if (datos.Modo == 1)
                                ItemsSource.Add(datos);
                        }
                        else
                        {
                            if (datos.Modo == 2)
                                ItemsSource.Add(datos);
                        }



                    }
                }
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.ToString(), site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MycmdToPdf_Click()
        {
            try
            {
                if (ItemsSource.Count <= 0)
                    return;

                jolcode.Code.AplicarEfecto(View as Window);
                jolcode.Code.DoEvents();

                // SaveFileDialog sfd = new SaveFileDialog();
                // sfd.Filter = "Pdfs Files|*.Pdf|All Files|*.*";

                // if (sfd.ShowDialog() == true)
                {
                    MergeEx mergeEx = new MergeEx();
                    String path = @"C:\\ApplicaTmp\\";// System.IO.Path.GetDirectoryName(sfd.FileName);

                    if (!path.EndsWith("\\"))
                        path += "\\";

                    path += Guid.NewGuid().ToString() + "\\";

                    if (System.IO.Directory.Exists(path))
                    {
                        String[] fileList1 = System.IO.Directory.GetFiles(path, "*.pdf");
                        foreach (String fileName in fileList1)
                        {
                            System.IO.File.Delete(fileName);
                        }
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }

                    int count = 0;

                    foreach (LotsEndo item in ItemsSource)
                    {
                        count++;
                        mergeEx.DestinationFile = path;
                        mergeEx.setFileName = item.NumElec;  //System.IO.Path.GetFileName(sfd.FileName);
                        mergeEx.setSplit = 1000;

                        byte[] Tif = item.EndosoImage;

                        String fileImg = "Formulario-" + item.Formulario + "-" + Guid.NewGuid().ToString() + ".tif";

                        System.IO.File.WriteAllBytes(path + fileImg, Tif);

                        mergeEx.ConvertImageToPdf(path + fileImg, path + fileImg + ".pdf");

                        System.Drawing.Point point = new System.Drawing.Point(500, 310);

                        String valido = "(Válido Lote:";
                        if (item.Status != "0")
                            valido = "(Inválido Lote:";

                        //                        mergeEx.AddTextToPdf(path + fileImg + ".pdf" , path + count.ToString("00#") + ".pdf", valido + item.Lot + " Formulario:" + item.Formulario + " " + item.Status +")",point );
                        mergeEx.AddTextToPdf(path + fileImg + ".pdf", path + count.ToString("00#") + ".pdf", valido + item.Lot + " Formulario:" + item.Formulario + ")", point);

                        System.IO.File.Delete(path + fileImg);
                        System.IO.File.Delete(path + fileImg + ".pdf");

                    }

                    String[] fileList = System.IO.Directory.GetFiles(path, "*.pdf");

                    foreach (String fileName in fileList)
                    {
                        mergeEx.AddFile(fileName);
                    }

                    mergeEx.Execute();
                    foreach (String fileName in fileList)
                    {
                        System.IO.File.Delete(fileName);
                    }


                    //open pdf
                    String fileNamepdf = path + mergeEx.setFileName + "_1.pdf";

                    fileTmp.Add(fileNamepdf);

                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = fileNamepdf;
                    process.Start();
                    //if (process != null)
                    //    process.WaitForExit();




                }

                MessageBox.Show("Done...", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MethodBase site = ex.TargetSite;
                MessageBox.Show(ex.ToString(), site.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                jolcode.Code.QuitarEfecto(View as Window);
            }
        }


        #endregion

        #region MyModules
        private void MyRefresh()
        {
            MyReset();
        }

        private void MyReset()
        {
            TxtElecNum = String.Empty;
            ItemsSource.Clear();
            txtTotal = "0";
            try
            {
                foreach (String fileName in fileTmp)
                {
                    System.IO.File.Delete(fileName);
                }
            }
            catch { }

        }
        #endregion



        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~vmDuplicadoPorNumElec()
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
