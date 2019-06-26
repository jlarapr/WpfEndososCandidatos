using System;
using System.Collections;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using System.Drawing;

namespace jolcode
{
    public class MergeEx
    {
        #region Fields
        private String sourcefolder;
        private String destinationfile;
        private ArrayList fileList = new ArrayList();
        //  private Label labelFilesCount = null;
        //  private Label labelStatus = null;
        private String s_fileName = null;
        private Int32 i_Split = 1;
        #endregion

        #region Properties
        ///
        /// Gets or Sets the SourceFolder
        ///
        public string SourceFolder
        {
            get { return sourcefolder; }
            set { sourcefolder = value; }
        }

        ///
        /// Gets or Sets the DestinationFile
        ///
        public string DestinationFile
        {
            get { return destinationfile; }
            set
            {
                if (System.IO.File.Exists(value))
                    System.IO.File.Delete(value);

                destinationfile = value;
            }
        }

        //public Label setLabelFilesCount
        //{
        //    set { labelFilesCount = value; }
        //}

        //public Label setLabelStatus
        //{
        //    set { labelStatus = value; }
        //}

        public String setFileName
        {
            set { s_fileName = value; }
            get { return s_fileName; }
        }

        public Int32 setSplit
        {
            set { i_Split = value; }
        }
        #endregion

        #region Private Methods

        public void ConvertImageToPdf(string srcFilename, string dstFilename)
        {
            iTextSharp.text.Rectangle pageSize = null;

            using (var srcImage = new System.Drawing.Bitmap(srcFilename))
            {
                pageSize = new iTextSharp.text.Rectangle(0, 0, srcImage.Width, srcImage.Height);
            }
            using (var ms = new MemoryStream())
            {
                var document = new iTextSharp.text.Document(pageSize, 0, 0, 0, 0);
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms).SetFullCompression();
                document.Open();
                var image = iTextSharp.text.Image.GetInstance(srcFilename);
                document.Add(image);
                document.Close();
                File.WriteAllBytes(dstFilename, ms.ToArray());
            }
        }
        public void AddTextToPdf(string inputPdfPath, string outputPdfPath, string textToAdd, System.Drawing.Point point)
        {
            
            //variables
            string pathin = inputPdfPath;
            string pathout = outputPdfPath;

            //create PdfReader object to read from the existing document
            PdfReader reader = new PdfReader(pathin);
            {
                //create PdfStamper object to write to get the pages from reader 
                PdfStamper stamper = new PdfStamper(reader, new FileStream(pathout, FileMode.Create));
                {
                    //select two pages from the original document
                    reader.SelectPages("1-2");

                    //gettins the page size in order to substract from the iTextSharp coordinates
                    var pageSize = reader.GetPageSize(1);

                    // PdfContentByte from stamper to add content to the pages over the original content
                    PdfContentByte pbover = stamper.GetOverContent(1);

                    //add content to the page using ColumnText
                    //iTextSharp.text.Font font = new iTextSharp.text.Font();
                    iTextSharp.text.Font font = FontFactory.GetFont("Arial", 30, Font.BOLD, new BaseColor(125, 88, 15));
                   // font.Size = 25;
                   
                    

                    //setting up the X and Y coordinates of the document
                    int x = point.X;
                    int y = point.Y;

                    x = (int)(pageSize.Width - x); 
                    y = (int)(pageSize.Height - y);

                    ColumnText.ShowTextAligned(pbover, Element.ALIGN_CENTER, new Phrase(textToAdd, font), x, y, 0);
                }stamper.Close();
            }
            reader.Close();
        }
       

        ///
        /// Merges the Docs and renders the destinationFile
        ///
        private void MergeDocs()
        {
            try
            {
                Boolean isStart = true;
                Int32 countFiles = 1;
                Int32 countProcessedFiles = 1;
                Int32 countMerge = 1;
                Int32 n = 0;
                Int32 rotation = 0;
                Document document = null;
                PdfContentByte cb = null;
                PdfImportedPage page;
                PdfWriter writer = null;

                //Sort file list
                fileList.Sort();

                //Loops for each file that has been listed
                foreach (String filename in fileList)
                {
                    if (isStart)
                    {
                        //Create a Docuement-Object
                        document = new Document();

                        //We create a writer that listens to the document
                        writer = PdfWriter.GetInstance(document, new FileStream(destinationfile + "\\" + s_fileName + "_"
                                                       + countMerge + ".pdf", FileMode.Create));
                        ////Reset Status
                        //labelStatus.Text = "";
                        //Application.DoEvents();

                        //Open the document
                        document.Open();

                        cb = writer.DirectContent;

                        n = 0;
                        rotation = 0;

                        countFiles = 1;
                        countMerge++;
                        isStart = false;
                    }

                    //We create a reader for the document
                    PdfReader reader = new PdfReader(filename);

                    //Gets the number of pages to process
                    n = reader.NumberOfPages;

                    Int32 i = 0;
                    while (i < n)
                    {
                        i++;
                        document.SetPageSize(reader.GetPageSizeWithRotation(1));
                        document.NewPage();

                        //Insert to Destination on the first page
                        if (i == 1)
                        {
                            Chunk fileRef = new Chunk(" ");
                            fileRef.SetLocalDestination(filename);
                            document.Add(fileRef);
                        }

                        page = writer.GetImportedPage(reader, i);
                        rotation = reader.GetPageRotation(i);
                        if (rotation == 90 || rotation == 270)
                        {
                            cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                        }
                        else
                        {
                            cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                        }
                    }
                    //labelFilesCount.Text = "Files merged: " + String.Format("{0:0,0}", countProcessedFiles);
                    countFiles++;
                    countProcessedFiles++;


                    if (countFiles > i_Split || fileList.IndexOf(filename) == fileList.Count - 1)
                    {
                        //                labelStatus.Text = "Closing document...";
                        //              Application.DoEvents();
                        document.Close();
                        isStart = true;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "-->MergeDocs");
            }
        }

        private void CreatePS()
        {

        }
        #endregion

        #region Public Methods
        ///
        /// Add a new file, together with a given docname to the fileList and namelist collection
        ///

        public void AddFile(string pathname)
        {
            try
            {
                fileList.Add(pathname);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "-->AddFile");
            }
        }

        ///
        /// Generate the merged PDF
        ///
        public void Execute()
        {
            MergeDocs();
        }
        #endregion
    }
}
