namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class InvoiceInfo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBoxInvoice = new System.Windows.Forms.GroupBox();
            this.dGVInvioceItems = new System.Windows.Forms.DataGridView();
            this.labelVerkaeufer = new System.Windows.Forms.Label();
            this.labelFaellig_am = new System.Windows.Forms.Label();
            this.labelMahnstufe = new System.Windows.Forms.Label();
            this.labelAuftragsnummer = new System.Windows.Forms.Label();
            this.labelAuftragsdatum = new System.Windows.Forms.Label();
            this.labelRechnungsdatum = new System.Windows.Forms.Label();
            this.labelV = new System.Windows.Forms.Label();
            this.labelF_am = new System.Windows.Forms.Label();
            this.labelMS = new System.Windows.Forms.Label();
            this.labelAufNr = new System.Windows.Forms.Label();
            this.labelAufDat = new System.Windows.Forms.Label();
            this.labelReDat = new System.Windows.Forms.Label();
            this.labelRechnungsnummer = new System.Windows.Forms.Label();
            this.labelReNr = new System.Windows.Forms.Label();
            this.groupBoxInvoice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVInvioceItems)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxInvoice
            // 
            this.groupBoxInvoice.Controls.Add(this.dGVInvioceItems);
            this.groupBoxInvoice.Controls.Add(this.labelVerkaeufer);
            this.groupBoxInvoice.Controls.Add(this.labelFaellig_am);
            this.groupBoxInvoice.Controls.Add(this.labelMahnstufe);
            this.groupBoxInvoice.Controls.Add(this.labelAuftragsnummer);
            this.groupBoxInvoice.Controls.Add(this.labelAuftragsdatum);
            this.groupBoxInvoice.Controls.Add(this.labelRechnungsdatum);
            this.groupBoxInvoice.Controls.Add(this.labelV);
            this.groupBoxInvoice.Controls.Add(this.labelF_am);
            this.groupBoxInvoice.Controls.Add(this.labelMS);
            this.groupBoxInvoice.Controls.Add(this.labelAufNr);
            this.groupBoxInvoice.Controls.Add(this.labelAufDat);
            this.groupBoxInvoice.Controls.Add(this.labelReDat);
            this.groupBoxInvoice.Controls.Add(this.labelRechnungsnummer);
            this.groupBoxInvoice.Controls.Add(this.labelReNr);
            this.groupBoxInvoice.Location = new System.Drawing.Point(10, 10);
            this.groupBoxInvoice.Name = "groupBoxInvoice";
            this.groupBoxInvoice.Size = new System.Drawing.Size(494, 205);
            this.groupBoxInvoice.TabIndex = 2;
            this.groupBoxInvoice.TabStop = false;
            this.groupBoxInvoice.Text = "Rechnungsinformationen";
            // 
            // dGVInvioceItems
            // 
            this.dGVInvioceItems.AllowUserToAddRows = false;
            this.dGVInvioceItems.AllowUserToDeleteRows = false;
            this.dGVInvioceItems.AllowUserToResizeColumns = false;
            this.dGVInvioceItems.AllowUserToResizeRows = false;
            this.dGVInvioceItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dGVInvioceItems.BackgroundColor = System.Drawing.Color.MistyRose;
            this.dGVInvioceItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dGVInvioceItems.DefaultCellStyle = dataGridViewCellStyle1;
            this.dGVInvioceItems.Location = new System.Drawing.Point(23, 120);
            this.dGVInvioceItems.Margin = new System.Windows.Forms.Padding(0);
            this.dGVInvioceItems.MultiSelect = false;
            this.dGVInvioceItems.Name = "dGVInvioceItems";
            this.dGVInvioceItems.ReadOnly = true;
            this.dGVInvioceItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGVInvioceItems.Size = new System.Drawing.Size(450, 70);
            this.dGVInvioceItems.TabIndex = 51;
            // 
            // labelVerkaeufer
            // 
            this.labelVerkaeufer.AutoSize = true;
            this.labelVerkaeufer.Location = new System.Drawing.Point(368, 67);
            this.labelVerkaeufer.Name = "labelVerkaeufer";
            this.labelVerkaeufer.Size = new System.Drawing.Size(56, 13);
            this.labelVerkaeufer.TabIndex = 15;
            this.labelVerkaeufer.Text = "Verkäufer:";
            // 
            // labelFaellig_am
            // 
            this.labelFaellig_am.AutoSize = true;
            this.labelFaellig_am.Location = new System.Drawing.Point(368, 43);
            this.labelFaellig_am.Name = "labelFaellig_am";
            this.labelFaellig_am.Size = new System.Drawing.Size(48, 13);
            this.labelFaellig_am.TabIndex = 14;
            this.labelFaellig_am.Text = "fällig am:";
            // 
            // labelMahnstufe
            // 
            this.labelMahnstufe.AutoSize = true;
            this.labelMahnstufe.Location = new System.Drawing.Point(368, 20);
            this.labelMahnstufe.Name = "labelMahnstufe";
            this.labelMahnstufe.Size = new System.Drawing.Size(60, 13);
            this.labelMahnstufe.TabIndex = 13;
            this.labelMahnstufe.Text = "Mahnstufe:";
            // 
            // labelAuftragsnummer
            // 
            this.labelAuftragsnummer.AutoSize = true;
            this.labelAuftragsnummer.Location = new System.Drawing.Point(160, 68);
            this.labelAuftragsnummer.Name = "labelAuftragsnummer";
            this.labelAuftragsnummer.Size = new System.Drawing.Size(86, 13);
            this.labelAuftragsnummer.TabIndex = 12;
            this.labelAuftragsnummer.Text = "Auftragsnummer:";
            // 
            // labelAuftragsdatum
            // 
            this.labelAuftragsdatum.AutoSize = true;
            this.labelAuftragsdatum.Location = new System.Drawing.Point(160, 90);
            this.labelAuftragsdatum.Name = "labelAuftragsdatum";
            this.labelAuftragsdatum.Size = new System.Drawing.Size(78, 13);
            this.labelAuftragsdatum.TabIndex = 11;
            this.labelAuftragsdatum.Text = "Auftragsdatum:";
            // 
            // labelRechnungsdatum
            // 
            this.labelRechnungsdatum.AutoSize = true;
            this.labelRechnungsdatum.Location = new System.Drawing.Point(160, 43);
            this.labelRechnungsdatum.Name = "labelRechnungsdatum";
            this.labelRechnungsdatum.Size = new System.Drawing.Size(94, 13);
            this.labelRechnungsdatum.TabIndex = 10;
            this.labelRechnungsdatum.Text = "Rechnungsdatum:";
            // 
            // labelV
            // 
            this.labelV.AutoSize = true;
            this.labelV.Location = new System.Drawing.Point(266, 67);
            this.labelV.Name = "labelV";
            this.labelV.Size = new System.Drawing.Size(56, 13);
            this.labelV.TabIndex = 9;
            this.labelV.Text = "Verkäufer:";
            // 
            // labelF_am
            // 
            this.labelF_am.AutoSize = true;
            this.labelF_am.Location = new System.Drawing.Point(266, 43);
            this.labelF_am.Name = "labelF_am";
            this.labelF_am.Size = new System.Drawing.Size(48, 13);
            this.labelF_am.TabIndex = 8;
            this.labelF_am.Text = "fällig am:";
            // 
            // labelMS
            // 
            this.labelMS.AutoSize = true;
            this.labelMS.Location = new System.Drawing.Point(266, 20);
            this.labelMS.Name = "labelMS";
            this.labelMS.Size = new System.Drawing.Size(60, 13);
            this.labelMS.TabIndex = 7;
            this.labelMS.Text = "Mahnstufe:";
            // 
            // labelAufNr
            // 
            this.labelAufNr.AutoSize = true;
            this.labelAufNr.Location = new System.Drawing.Point(20, 68);
            this.labelAufNr.Name = "labelAufNr";
            this.labelAufNr.Size = new System.Drawing.Size(86, 13);
            this.labelAufNr.TabIndex = 6;
            this.labelAufNr.Text = "Auftragsnummer:";
            // 
            // labelAufDat
            // 
            this.labelAufDat.AutoSize = true;
            this.labelAufDat.Location = new System.Drawing.Point(20, 90);
            this.labelAufDat.Name = "labelAufDat";
            this.labelAufDat.Size = new System.Drawing.Size(78, 13);
            this.labelAufDat.TabIndex = 5;
            this.labelAufDat.Text = "Auftragsdatum:";
            // 
            // labelReDat
            // 
            this.labelReDat.AutoSize = true;
            this.labelReDat.Location = new System.Drawing.Point(20, 43);
            this.labelReDat.Name = "labelReDat";
            this.labelReDat.Size = new System.Drawing.Size(94, 13);
            this.labelReDat.TabIndex = 4;
            this.labelReDat.Text = "Rechnungsdatum:";
            // 
            // labelRechnungsnummer
            // 
            this.labelRechnungsnummer.AutoSize = true;
            this.labelRechnungsnummer.Location = new System.Drawing.Point(160, 20);
            this.labelRechnungsnummer.Name = "labelRechnungsnummer";
            this.labelRechnungsnummer.Size = new System.Drawing.Size(102, 13);
            this.labelRechnungsnummer.TabIndex = 3;
            this.labelRechnungsnummer.Text = "Rechnungsnummer:";
            // 
            // labelReNr
            // 
            this.labelReNr.AutoSize = true;
            this.labelReNr.Location = new System.Drawing.Point(20, 20);
            this.labelReNr.Name = "labelReNr";
            this.labelReNr.Size = new System.Drawing.Size(102, 13);
            this.labelReNr.TabIndex = 2;
            this.labelReNr.Text = "Rechnungsnummer:";
            // 
            // InvoiceInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MistyRose;
            this.Controls.Add(this.groupBoxInvoice);
            this.MinimumSize = new System.Drawing.Size(521, 240);
            this.Name = "InvoiceInfo";
            this.Size = new System.Drawing.Size(521, 240);
            this.Load += new System.EventHandler(this.InvoiceInfo_Load);
            this.groupBoxInvoice.ResumeLayout(false);
            this.groupBoxInvoice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVInvioceItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxInvoice;
        private System.Windows.Forms.Label labelRechnungsnummer;
        private System.Windows.Forms.Label labelReNr;
        private System.Windows.Forms.Label labelV;
        private System.Windows.Forms.Label labelF_am;
        private System.Windows.Forms.Label labelMS;
        private System.Windows.Forms.Label labelAufNr;
        private System.Windows.Forms.Label labelAufDat;
        private System.Windows.Forms.Label labelReDat;
        private System.Windows.Forms.Label labelVerkaeufer;
        private System.Windows.Forms.Label labelFaellig_am;
        private System.Windows.Forms.Label labelMahnstufe;
        private System.Windows.Forms.Label labelAuftragsnummer;
        private System.Windows.Forms.Label labelAuftragsdatum;
        private System.Windows.Forms.Label labelRechnungsdatum;
        private System.Windows.Forms.DataGridView dGVInvioceItems;

    }
}
