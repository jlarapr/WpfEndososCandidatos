using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsHost
{
    public partial class AcrobatCtrl : UserControl
    {
        private AxAcroPDFLib.AxAcroPDF AdobeAcrobatPDfControl
        {
            get
            {
                return this.axAcroPDF;
            }
        }

        public void LoadFile(string pdfFilePath)
        {
            AdobeAcrobatPDfControl.LoadFile(pdfFilePath);
        }
        public AcrobatCtrl()
        {
            InitializeComponent();
        }
    }
}
