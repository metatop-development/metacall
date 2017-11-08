namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class CustomerInfo
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblContactDisplay = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblDisplayResidence = new System.Windows.Forms.Label();
            this.lblStrasse = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblContactDisplay
            // 
            this.lblContactDisplay.AutoSize = true;
            this.lblContactDisplay.Location = new System.Drawing.Point(94, 52);
            this.lblContactDisplay.Name = "lblContactDisplay";
            this.lblContactDisplay.Size = new System.Drawing.Size(41, 13);
            this.lblContactDisplay.TabIndex = 27;
            this.lblContactDisplay.Text = "label21";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label14.Location = new System.Drawing.Point(9, 52);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 13);
            this.label14.TabIndex = 28;
            this.label14.Text = "Ansprechpartner";
            // 
            // lblDisplayResidence
            // 
            this.lblDisplayResidence.AutoSize = true;
            this.lblDisplayResidence.Location = new System.Drawing.Point(94, 26);
            this.lblDisplayResidence.Name = "lblDisplayResidence";
            this.lblDisplayResidence.Size = new System.Drawing.Size(41, 13);
            this.lblDisplayResidence.TabIndex = 46;
            this.lblDisplayResidence.Text = "label28";
            // 
            // lblStrasse
            // 
            this.lblStrasse.AutoSize = true;
            this.lblStrasse.Location = new System.Drawing.Point(94, 10);
            this.lblStrasse.Name = "lblStrasse";
            this.lblStrasse.Size = new System.Drawing.Size(41, 13);
            this.lblStrasse.TabIndex = 45;
            this.lblStrasse.Text = "label27";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label7.Location = new System.Drawing.Point(9, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Vereinssitz";
            // 
            // CustomerInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.Controls.Add(this.lblDisplayResidence);
            this.Controls.Add(this.lblStrasse);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lblContactDisplay);
            this.Name = "CustomerInfo";
            this.Size = new System.Drawing.Size(350, 80);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblContactDisplay;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblDisplayResidence;
        private System.Windows.Forms.Label lblStrasse;
        private System.Windows.Forms.Label label7;
    }
}
