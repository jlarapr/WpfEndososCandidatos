using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace jolcode
{
    public class ToExcel: IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public ToExcel()
        {

        }

        public void TableToExcel(string excelFileName,DataTable data)
        {
            try
            {
                if (System.IO.File.Exists(excelFileName))
                    System.IO.File.Delete(excelFileName);

                string worksheetsName = "CEE " + DateTime.Now.ToString();
                bool firstRowIsHeader = true;

               // var format = new ExcelTextFormat();

                using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFileName)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);

                    worksheet.Cells["A:XFD"].Style.Font.Bold = true;

                    //worksheet.Cells["A:XFD"].LoadFromText(new FileInfo(csvFileName), format, OfficeOpenXml.Table.TableStyles.Medium27, firstRowIsHeader);
                    worksheet.Cells["A:XFD"].LoadFromDataTable(data, firstRowIsHeader, OfficeOpenXml.Table.TableStyles.Medium27);

                    package.Save();
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        public void TableToExcel(string excelFileName, IEnumerable<InfoReydi> data)
        {
            try
            {
                if (System.IO.File.Exists(excelFileName))
                    System.IO.File.Delete(excelFileName);

                string worksheetsName = "CEE " + DateTime.Now.ToString();
                bool firstRowIsHeader = true;

                // var format = new ExcelTextFormat();

                using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFileName)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);

                    worksheet.Cells["A:XFD"].Style.Font.Bold = true;

                    //worksheet.Cells["A:XFD"].LoadFromText(new FileInfo(csvFileName), format, OfficeOpenXml.Table.TableStyles.Medium27, firstRowIsHeader);
                    worksheet.Cells["A:XFD"].LoadFromCollection<InfoReydi> (data, firstRowIsHeader, OfficeOpenXml.Table.TableStyles.Medium27);

                    package.Save();
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Dispose



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ToExcel()
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
