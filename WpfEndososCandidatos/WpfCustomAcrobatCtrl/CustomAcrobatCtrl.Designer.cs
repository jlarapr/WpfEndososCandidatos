namespace WpfCustomAcrobatCtrl
{
    partial class CustomAcrobatCtrl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomAcrobatCtrl));
            this.axAcroPdf = new AxAcroPDFLib.AxAcroPDF();
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPdf)).BeginInit();
            this.SuspendLayout();
            // 
            // axAcroPdf
            // 
            this.axAcroPdf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axAcroPdf.Enabled = true;
            this.axAcroPdf.Location = new System.Drawing.Point(0, 0);
            this.axAcroPdf.Name = "axAcroPdf";
            this.axAcroPdf.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAcroPdf.OcxState")));
            this.axAcroPdf.Size = new System.Drawing.Size(703, 600);
            this.axAcroPdf.TabIndex = 0;
            // 
            // CustomAcrobatCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axAcroPdf);
            this.Name = "CustomAcrobatCtrl";
            this.Size = new System.Drawing.Size(703, 600);
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPdf)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxAcroPDFLib.AxAcroPDF axAcroPdf;
    }
}
