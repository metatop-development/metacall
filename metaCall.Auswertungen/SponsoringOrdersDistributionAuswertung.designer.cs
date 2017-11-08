namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class SponsoringOrdersDistributionAuswertung
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
            this.panelWorkTimeInfo = new System.Windows.Forms.Panel();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.selectionPeriod1 = new metatop.Applications.metaCall.WinForms.Modules.SelectionPeriod();
            this.ContactReport1 = new metatop.Applications.metaCall.WinForms.Modules.Auswertungen.ContactReport();
            this.panelTreeView = new System.Windows.Forms.Panel();
            this.toolStripTreeView = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonProject = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAgent = new System.Windows.Forms.ToolStripButton();
            this.treeViewFilter = new System.Windows.Forms.TreeView();
            this.panelWorkTimeInfo.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelTreeView.SuspendLayout();
            this.toolStripTreeView.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelWorkTimeInfo
            // 
            this.panelWorkTimeInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelWorkTimeInfo.BackColor = System.Drawing.SystemColors.Control;
            this.panelWorkTimeInfo.Controls.Add(this.crystalReportViewer1);
            this.panelWorkTimeInfo.Location = new System.Drawing.Point(321, 65);
            this.panelWorkTimeInfo.Name = "panelWorkTimeInfo";
            this.panelWorkTimeInfo.Size = new System.Drawing.Size(618, 290);
            this.panelWorkTimeInfo.TabIndex = 8;
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.DisplayGroupTree = false;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.EnableToolTips = false;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.SelectionFormula = "";
            this.crystalReportViewer1.ShowCloseButton = false;
            this.crystalReportViewer1.ShowExportButton = false;
            this.crystalReportViewer1.ShowGroupTreeButton = false;
            this.crystalReportViewer1.ShowRefreshButton = false;
            this.crystalReportViewer1.ShowTextSearchButton = false;
            this.crystalReportViewer1.Size = new System.Drawing.Size(618, 290);
            this.crystalReportViewer1.TabIndex = 13;
            this.crystalReportViewer1.ViewTimeSelectionFormula = "";
            // 
            // panelSearch
            // 
            this.panelSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSearch.Controls.Add(this.selectionPeriod1);
            this.panelSearch.Location = new System.Drawing.Point(321, 0);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(618, 69);
            this.panelSearch.TabIndex = 9;
            // 
            // selectionPeriod1
            // 
            this.selectionPeriod1.Location = new System.Drawing.Point(0, 3);
            this.selectionPeriod1.Name = "selectionPeriod1";
            this.selectionPeriod1.SelectionFrom = new System.DateTime(2007, 7, 1, 0, 0, 0, 0);
            this.selectionPeriod1.SelectionTo = new System.DateTime(2007, 7, 31, 0, 0, 0, 0);
            this.selectionPeriod1.Size = new System.Drawing.Size(610, 68);
            this.selectionPeriod1.TabIndex = 0;
            // 
            // panelTreeView
            // 
            this.panelTreeView.Controls.Add(this.toolStripTreeView);
            this.panelTreeView.Controls.Add(this.treeViewFilter);
            this.panelTreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTreeView.Location = new System.Drawing.Point(0, 0);
            this.panelTreeView.Name = "panelTreeView";
            this.panelTreeView.Size = new System.Drawing.Size(315, 355);
            this.panelTreeView.TabIndex = 10;
            // 
            // toolStripTreeView
            // 
            this.toolStripTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonProject,
            this.toolStripButtonAgent});
            this.toolStripTreeView.Location = new System.Drawing.Point(0, 0);
            this.toolStripTreeView.Name = "toolStripTreeView";
            this.toolStripTreeView.Size = new System.Drawing.Size(315, 25);
            this.toolStripTreeView.TabIndex = 19;
            this.toolStripTreeView.Text = "toolStrip1";
            // 
            // toolStripButtonProject
            // 
            this.toolStripButtonProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProject.Name = "toolStripButtonProject";
            this.toolStripButtonProject.Size = new System.Drawing.Size(45, 22);
            this.toolStripButtonProject.Text = "Projekt";
            this.toolStripButtonProject.Click += new System.EventHandler(this.toolStripButtonProject_Click);
            // 
            // toolStripButtonAgent
            // 
            this.toolStripButtonAgent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonAgent.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.toolStripButtonAgent.Name = "toolStripButtonAgent";
            this.toolStripButtonAgent.Size = new System.Drawing.Size(54, 22);
            this.toolStripButtonAgent.Text = "Benutzer";
            this.toolStripButtonAgent.Click += new System.EventHandler(this.toolStripButtonAgent_Click);
            // 
            // treeViewFilter
            // 
            this.treeViewFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewFilter.HideSelection = false;
            this.treeViewFilter.HotTracking = true;
            this.treeViewFilter.Location = new System.Drawing.Point(-3, 28);
            this.treeViewFilter.Name = "treeViewFilter";
            this.treeViewFilter.Size = new System.Drawing.Size(315, 324);
            this.treeViewFilter.TabIndex = 8;
            this.treeViewFilter.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFilter_AfterSelect);
            this.treeViewFilter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewFilter_MouseDown);
            // 
            // SponsoringOrdersDistributionAuswertung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelTreeView);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelWorkTimeInfo);
            this.Name = "SponsoringOrdersDistributionAuswertung";
            this.Size = new System.Drawing.Size(939, 355);
            this.Load += new System.EventHandler(this.ProjectReportAuswertung_Load);
            this.panelWorkTimeInfo.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelTreeView.ResumeLayout(false);
            this.panelTreeView.PerformLayout();
            this.toolStripTreeView.ResumeLayout(false);
            this.toolStripTreeView.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelWorkTimeInfo;
        private System.Windows.Forms.Panel panelSearch;
        private SelectionPeriod selectionPeriod1;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private metatop.Applications.metaCall.WinForms.Modules.Auswertungen.ContactReport ContactReport1;
        private System.Windows.Forms.Panel panelTreeView;
        private System.Windows.Forms.TreeView treeViewFilter;
        private System.Windows.Forms.ToolStrip toolStripTreeView;
        private System.Windows.Forms.ToolStripButton toolStripButtonProject;
        private System.Windows.Forms.ToolStripButton toolStripButtonAgent;
    }
}
