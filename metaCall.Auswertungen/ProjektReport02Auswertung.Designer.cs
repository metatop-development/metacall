namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class ProjektReport02Auswertung
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
            this.treeViewFilter = new System.Windows.Forms.TreeView();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.btnNachtlauf = new System.Windows.Forms.RadioButton();
            this.btnAktuell = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // treeViewFilter
            // 
            this.treeViewFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewFilter.HideSelection = false;
            this.treeViewFilter.HotTracking = true;
            this.treeViewFilter.Location = new System.Drawing.Point(3, 44);
            this.treeViewFilter.Name = "treeViewFilter";
            this.treeViewFilter.Size = new System.Drawing.Size(231, 386);
            this.treeViewFilter.TabIndex = 9;
            this.treeViewFilter.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFilter_AfterSelect);
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.DisplayGroupTree = false;
            this.crystalReportViewer1.EnableToolTips = false;
            this.crystalReportViewer1.Location = new System.Drawing.Point(240, 4);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.SelectionFormula = "";
            this.crystalReportViewer1.ShowCloseButton = false;
            this.crystalReportViewer1.ShowExportButton = false;
            this.crystalReportViewer1.ShowGroupTreeButton = false;
            this.crystalReportViewer1.ShowRefreshButton = false;
            this.crystalReportViewer1.ShowTextSearchButton = false;
            this.crystalReportViewer1.Size = new System.Drawing.Size(337, 423);
            this.crystalReportViewer1.TabIndex = 14;
            this.crystalReportViewer1.ViewTimeSelectionFormula = "";
            // 
            // btnNachtlauf
            // 
            this.btnNachtlauf.AutoSize = true;
            this.btnNachtlauf.Checked = true;
            this.btnNachtlauf.Location = new System.Drawing.Point(3, 23);
            this.btnNachtlauf.Margin = new System.Windows.Forms.Padding(1);
            this.btnNachtlauf.Name = "btnNachtlauf";
            this.btnNachtlauf.Size = new System.Drawing.Size(159, 17);
            this.btnNachtlauf.TabIndex = 16;
            this.btnNachtlauf.TabStop = true;
            this.btnNachtlauf.Text = "Daten aus letztem Nachtlauf";
            this.btnNachtlauf.UseVisualStyleBackColor = true;
            // 
            // btnAktuell
            // 
            this.btnAktuell.AutoSize = true;
            this.btnAktuell.Location = new System.Drawing.Point(3, 4);
            this.btnAktuell.Margin = new System.Windows.Forms.Padding(1);
            this.btnAktuell.Name = "btnAktuell";
            this.btnAktuell.Size = new System.Drawing.Size(230, 17);
            this.btnAktuell.TabIndex = 15;
            this.btnAktuell.Text = "aktuelle Daten (Berechnungsdauer > 1 min)";
            this.btnAktuell.UseVisualStyleBackColor = true;
            this.btnAktuell.CheckedChanged += new System.EventHandler(this.btnAktuell_CheckedChanged);
            // 
            // ProjektReport02Auswertung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnNachtlauf);
            this.Controls.Add(this.btnAktuell);
            this.Controls.Add(this.crystalReportViewer1);
            this.Controls.Add(this.treeViewFilter);
            this.Name = "ProjektReport02Auswertung";
            this.Size = new System.Drawing.Size(577, 430);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewFilter;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.RadioButton btnNachtlauf;
        private System.Windows.Forms.RadioButton btnAktuell;
    }
}
