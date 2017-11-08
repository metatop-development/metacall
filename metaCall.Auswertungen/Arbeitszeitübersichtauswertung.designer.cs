namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class Arbeitszeitübersicht
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
            this.components = new System.ComponentModel.Container();
            this.treeViewFilter = new System.Windows.Forms.TreeView();
            this.teamNodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.teamAddMenueToolStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.teamEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.teamDeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerNodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.centerAddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerDeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.centerTeamAddtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelWorkTimeInfo = new System.Windows.Forms.Panel();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.selectionPeriod1 = new metatop.Applications.metaCall.WinForms.Modules.SelectionPeriod();
            this.teamNodeContextMenuStrip.SuspendLayout();
            this.centerNodeContextMenuStrip.SuspendLayout();
            this.panelWorkTimeInfo.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewFilter
            // 
            this.treeViewFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeViewFilter.HideSelection = false;
            this.treeViewFilter.HotTracking = true;
            this.treeViewFilter.Location = new System.Drawing.Point(0, 0);
            this.treeViewFilter.Name = "treeViewFilter";
            this.treeViewFilter.Size = new System.Drawing.Size(165, 355);
            this.treeViewFilter.TabIndex = 7;
            this.treeViewFilter.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFilter_AfterSelect);
            this.treeViewFilter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewFilter_MouseDown);
            // 
            // teamNodeContextMenuStrip
            // 
            this.teamNodeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.teamAddMenueToolStripItem,
            this.teamEditToolStripMenuItem,
            this.teamDeleteToolStripMenuItem});
            this.teamNodeContextMenuStrip.Name = "teamNodeContextMenuStrip";
            this.teamNodeContextMenuStrip.Size = new System.Drawing.Size(168, 70);
            // 
            // teamAddMenueToolStripItem
            // 
            this.teamAddMenueToolStripItem.Name = "teamAddMenueToolStripItem";
            this.teamAddMenueToolStripItem.Size = new System.Drawing.Size(167, 22);
            this.teamAddMenueToolStripItem.Text = "Team hinzufügen";
            // 
            // teamEditToolStripMenuItem
            // 
            this.teamEditToolStripMenuItem.Name = "teamEditToolStripMenuItem";
            this.teamEditToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.teamEditToolStripMenuItem.Text = "Team bearbeiten";
            // 
            // teamDeleteToolStripMenuItem
            // 
            this.teamDeleteToolStripMenuItem.Name = "teamDeleteToolStripMenuItem";
            this.teamDeleteToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.teamDeleteToolStripMenuItem.Text = "Team löschen";
            // 
            // centerNodeContextMenuStrip
            // 
            this.centerNodeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.centerAddToolStripMenuItem,
            this.centerEditToolStripMenuItem,
            this.centerDeleteToolStripMenuItem,
            this.toolStripSeparator1,
            this.centerTeamAddtoolStripMenuItem});
            this.centerNodeContextMenuStrip.Name = "centerNodeContextMenuStrip";
            this.centerNodeContextMenuStrip.Size = new System.Drawing.Size(175, 98);
            // 
            // centerAddToolStripMenuItem
            // 
            this.centerAddToolStripMenuItem.Name = "centerAddToolStripMenuItem";
            this.centerAddToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.centerAddToolStripMenuItem.Text = "Center hinzufügen";
            // 
            // centerEditToolStripMenuItem
            // 
            this.centerEditToolStripMenuItem.Name = "centerEditToolStripMenuItem";
            this.centerEditToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.centerEditToolStripMenuItem.Text = "Center bearbeiten";
            // 
            // centerDeleteToolStripMenuItem
            // 
            this.centerDeleteToolStripMenuItem.Name = "centerDeleteToolStripMenuItem";
            this.centerDeleteToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.centerDeleteToolStripMenuItem.Text = "Center löschen";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(171, 6);
            // 
            // centerTeamAddtoolStripMenuItem
            // 
            this.centerTeamAddtoolStripMenuItem.Name = "centerTeamAddtoolStripMenuItem";
            this.centerTeamAddtoolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.centerTeamAddtoolStripMenuItem.Text = "Team hinzufügen";
            // 
            // panelWorkTimeInfo
            // 
            this.panelWorkTimeInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelWorkTimeInfo.BackColor = System.Drawing.SystemColors.Control;
            this.panelWorkTimeInfo.Controls.Add(this.crystalReportViewer1);
            this.panelWorkTimeInfo.Location = new System.Drawing.Point(165, 68);
            this.panelWorkTimeInfo.Name = "panelWorkTimeInfo";
            this.panelWorkTimeInfo.Size = new System.Drawing.Size(664, 287);
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
            this.crystalReportViewer1.Size = new System.Drawing.Size(664, 287);
            this.crystalReportViewer1.TabIndex = 13;
            this.crystalReportViewer1.ViewTimeSelectionFormula = "";
            // 
            // panelSearch
            // 
            this.panelSearch.Controls.Add(this.selectionPeriod1);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(165, 0);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(664, 69);
            this.panelSearch.TabIndex = 9;
            // 
            // selectionPeriod1
            // 
            this.selectionPeriod1.Location = new System.Drawing.Point(0, 3);
            this.selectionPeriod1.Name = "selectionPeriod1";
            this.selectionPeriod1.SelectionFrom = new System.DateTime(2007, 5, 6, 0, 0, 0, 0);
            this.selectionPeriod1.SelectionTo = new System.DateTime(2007, 5, 31, 0, 0, 0, 0);
            this.selectionPeriod1.Size = new System.Drawing.Size(610, 68);
            this.selectionPeriod1.TabIndex = 0;
            // 
            // Arbeitszeitübersicht
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelWorkTimeInfo);
            this.Controls.Add(this.treeViewFilter);
            this.Name = "Arbeitszeitübersicht";
            this.Size = new System.Drawing.Size(829, 355);
            this.Load += new System.EventHandler(this.Arbeitseitübersicht_Load);
            this.teamNodeContextMenuStrip.ResumeLayout(false);
            this.centerNodeContextMenuStrip.ResumeLayout(false);
            this.panelWorkTimeInfo.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewFilter;
        private System.Windows.Forms.ContextMenuStrip teamNodeContextMenuStrip;
        private System.Windows.Forms.ContextMenuStrip centerNodeContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem centerAddToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centerEditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centerDeleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem centerTeamAddtoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem teamAddMenueToolStripItem;
        private System.Windows.Forms.ToolStripMenuItem teamEditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem teamDeleteToolStripMenuItem;
        private System.Windows.Forms.Panel panelWorkTimeInfo;
        private System.Windows.Forms.Panel panelSearch;
        private SelectionPeriod selectionPeriod1;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
    }
}
