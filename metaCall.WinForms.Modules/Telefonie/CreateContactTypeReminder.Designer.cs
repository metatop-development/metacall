namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class CreateContactTypeReminder
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
            this.btnTelefon = new System.Windows.Forms.Button();
            this.btnMobil = new System.Windows.Forms.Button();
            this.btnAlternativ = new System.Windows.Forms.Button();
            this.txtCurrentNumber = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnTelefon
            // 
            this.btnTelefon.Location = new System.Drawing.Point(220, 16);
            this.btnTelefon.Name = "btnTelefon";
            this.btnTelefon.Size = new System.Drawing.Size(70, 22);
            this.btnTelefon.TabIndex = 0;
            this.btnTelefon.Text = "Telefon";
            this.btnTelefon.UseVisualStyleBackColor = true;
            this.btnTelefon.Click += new System.EventHandler(this.btnTelefon_Click);
            // 
            // btnMobil
            // 
            this.btnMobil.Location = new System.Drawing.Point(300, 15);
            this.btnMobil.Name = "btnMobil";
            this.btnMobil.Size = new System.Drawing.Size(70, 22);
            this.btnMobil.TabIndex = 1;
            this.btnMobil.Text = "Mobil";
            this.btnMobil.UseVisualStyleBackColor = true;
            this.btnMobil.Click += new System.EventHandler(this.btnMobil_Click);
            // 
            // btnAlternativ
            // 
            this.btnAlternativ.Location = new System.Drawing.Point(380, 15);
            this.btnAlternativ.Name = "btnAlternativ";
            this.btnAlternativ.Size = new System.Drawing.Size(70, 22);
            this.btnAlternativ.TabIndex = 2;
            this.btnAlternativ.Text = "Alternativ";
            this.btnAlternativ.UseVisualStyleBackColor = true;
            this.btnAlternativ.Click += new System.EventHandler(this.btnAlternativ_Click);
            // 
            // txtCurrentNumber
            // 
            this.txtCurrentNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.txtCurrentNumber.Location = new System.Drawing.Point(20, 15);
            this.txtCurrentNumber.Name = "txtCurrentNumber";
            this.txtCurrentNumber.Size = new System.Drawing.Size(190, 24);
            this.txtCurrentNumber.TabIndex = 4;
            this.txtCurrentNumber.WordWrap = false;
            this.txtCurrentNumber.Leave += new System.EventHandler(this.txtCurrentNumber_Leave);
            // 
            // CreateContactTypeReminder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.Controls.Add(this.txtCurrentNumber);
            this.Controls.Add(this.btnAlternativ);
            this.Controls.Add(this.btnMobil);
            this.Controls.Add(this.btnTelefon);
            this.Name = "CreateContactTypeReminder";
            this.Size = new System.Drawing.Size(480, 150);
            this.Load += new System.EventHandler(this.CreateContactTypeReminder_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTelefon;
        private System.Windows.Forms.Button btnMobil;
        private System.Windows.Forms.Button btnAlternativ;
        private System.Windows.Forms.TextBox txtCurrentNumber;
    }
}
