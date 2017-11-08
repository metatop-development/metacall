namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class PhoneTimesAuswertung
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
            this.selectionPeriodStartStop = new metatop.Applications.metaCall.WinForms.Modules.SelectionPeriod();
            this.SuspendLayout();
            // 
            // treeViewFilter
            // 
            this.treeViewFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewFilter.HideSelection = false;
            this.treeViewFilter.HotTracking = true;
            this.treeViewFilter.Location = new System.Drawing.Point(3, 78);
            this.treeViewFilter.Name = "treeViewFilter";
            this.treeViewFilter.Size = new System.Drawing.Size(231, 352);
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
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.EnableToolTips = false;
            this.crystalReportViewer1.Location = new System.Drawing.Point(240, 78);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.SelectionFormula = "";
            this.crystalReportViewer1.ShowCloseButton = false;
            this.crystalReportViewer1.ShowExportButton = false;
            this.crystalReportViewer1.ShowGroupTreeButton = false;
            this.crystalReportViewer1.ShowRefreshButton = false;
            this.crystalReportViewer1.ShowTextSearchButton = false;
            this.crystalReportViewer1.Size = new System.Drawing.Size(612, 349);
            this.crystalReportViewer1.TabIndex = 14;
            this.crystalReportViewer1.ViewTimeSelectionFormula = "";
            // 
            // btnNachtlauf
            // 
            this.btnNachtlauf.AutoSize = true;
            this.btnNachtlauf.Enabled = false;
            this.btnNachtlauf.Location = new System.Drawing.Point(6, 42);
            this.btnNachtlauf.Margin = new System.Windows.Forms.Padding(1);
            this.btnNachtlauf.Name = "btnNachtlauf";
            this.btnNachtlauf.Size = new System.Drawing.Size(159, 17);
            this.btnNachtlauf.TabIndex = 16;
            this.btnNachtlauf.Text = "Daten aus letztem Nachtlauf";
            this.btnNachtlauf.UseVisualStyleBackColor = true;
            // 
            // btnAktuell
            // 
            this.btnAktuell.AutoSize = true;
            this.btnAktuell.Checked = true;
            this.btnAktuell.Location = new System.Drawing.Point(6, 23);
            this.btnAktuell.Margin = new System.Windows.Forms.Padding(1);
            this.btnAktuell.Name = "btnAktuell";
            this.btnAktuell.Size = new System.Drawing.Size(230, 17);
            this.btnAktuell.TabIndex = 15;
            this.btnAktuell.TabStop = true;
            this.btnAktuell.Text = "aktuelle Daten (Berechnungsdauer > 1 min)";
            this.btnAktuell.UseVisualStyleBackColor = true;
            this.btnAktuell.CheckedChanged += new System.EventHandler(this.btnAktuell_CheckedChanged);
            // 
            // selectionPeriodStartStop
            // 
            this.selectionPeriodStartStop.Location = new System.Drawing.Point(239, 4);
            this.selectionPeriodStartStop.Name = "selectionPeriodStartStop";
            this.selectionPeriodStartStop.SelectionFrom = new System.DateTime(2016, 7, 1, 0, 0, 0, 0);
            this.selectionPeriodStartStop.SelectionTo = new System.DateTime(2016, 7, 31, 0, 0, 0, 0);
            this.selectionPeriodStartStop.Size = new System.Drawing.Size(610, 68);
            this.selectionPeriodStartStop.TabIndex = 19;
            // 
            // PhoneTimesAuswertung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.selectionPeriodStartStop);
            this.Controls.Add(this.btnNachtlauf);
            this.Controls.Add(this.btnAktuell);
            this.Controls.Add(this.crystalReportViewer1);
            this.Controls.Add(this.treeViewFilter);
            this.Name = "PhoneTimesAuswertung";
            this.Size = new System.Drawing.Size(852, 430);
            this.Load += new System.EventHandler(this.PhoneTimesAuswertung_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewFilter;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.RadioButton btnNachtlauf;
        private System.Windows.Forms.RadioButton btnAktuell;
        private SelectionPeriod selectionPeriodStartStop;
    }
}
