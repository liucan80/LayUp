namespace LayUp
{
    partial class FrmPLC
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPLC));
            this.axActUtlType1 = new AxActUtlTypeLib.AxActUtlType();
            ((System.ComponentModel.ISupportInitialize)(this.axActUtlType1)).BeginInit();
            this.SuspendLayout();
            // 
            // axActUtlType1
            // 
            this.axActUtlType1.Enabled = true;
            this.axActUtlType1.Location = new System.Drawing.Point(53, 393);
            this.axActUtlType1.Name = "axActUtlType1";
            this.axActUtlType1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axActUtlType1.OcxState")));
            this.axActUtlType1.Size = new System.Drawing.Size(32, 32);
            this.axActUtlType1.TabIndex = 0;
            // 
            // FrmPLC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.axActUtlType1);
            this.Name = "FrmPLC";
            this.Text = "FrmPLC";
            ((System.ComponentModel.ISupportInitialize)(this.axActUtlType1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public AxActUtlTypeLib.AxActUtlType axActUtlType1;
    }
}