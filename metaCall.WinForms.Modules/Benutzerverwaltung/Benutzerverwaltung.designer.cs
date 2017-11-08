namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class Benutzerverwaltung
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
            this.centerNodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.centerAddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerDeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.centerTeamAddtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelUserInfo = new System.Windows.Forms.Panel();
            this.teamNodeContextMenuStrip.SuspendLayout();
            this.centerNodeContextMenuStrip.SuspendLayout();
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
            this.teamEditToolStripMenuItem});
            this.teamNodeContextMenuStrip.Name = "teamNodeContextMenuStrip";
            this.teamNodeContextMenuStrip.Size = new System.Drawing.Size(168, 70);
            this.teamNodeContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.teamNodeContextMenuStrip_Opening);
            // 
            // teamAddMenueToolStripItem
            // 
            this.teamAddMenueToolStripItem.Name = "teamAddMenueToolStripItem";
            this.teamAddMenueToolStripItem.Size = new System.Drawing.Size(167, 22);
            this.teamAddMenueToolStripItem.Text = "Team hinzufügen";
            this.teamAddMenueToolStripItem.Click += new System.EventHandler(this.teamAddMenueToolStripItem_Click);
            // 
            // teamEditToolStripMenuItem
            // 
            this.teamEditToolStripMenuItem.Name = "teamEditToolStripMenuItem";
            this.teamEditToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.teamEditToolStripMenuItem.Text = "Team bearbeiten";
            this.teamEditToolStripMenuItem.Click += new System.EventHandler(this.teamEditToolStripMenuItem_Click);
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
            this.centerNodeContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.centerNodeContextMenuStrip_Opening);
            // 
            // centerAddToolStripMenuItem
            // 
            this.centerAddToolStripMenuItem.Name = "centerAddToolStripMenuItem";
            this.centerAddToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.centerAddToolStripMenuItem.Text = "Center hinzufügen";
            this.centerAddToolStripMenuItem.Click += new System.EventHandler(this.centerAddToolStripMenuItem_Click);
            // 
            // centerEditToolStripMenuItem
            // 
            this.centerEditToolStripMenuItem.Name = "centerEditToolStripMenuItem";
            this.centerEditToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.centerEditToolStripMenuItem.Text = "Center bearbeiten";
            this.centerEditToolStripMenuItem.Click += new System.EventHandler(this.centerEditToolStripMenuItem_Click);
            // 
            // centerDeleteToolStripMenuItem
            // 
            this.centerDeleteToolStripMenuItem.Name = "centerDeleteToolStripMenuItem";
            this.centerDeleteToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.centerDeleteToolStripMenuItem.Text = "Center löschen";
            this.centerDeleteToolStripMenuItem.Click += new System.EventHandler(this.centerDeleteToolStripMenuItem_Click);
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
            this.centerTeamAddtoolStripMenuItem.Click += new System.EventHandler(this.teamAddMenueToolStripItem_Click);
            // 
            // panelUserInfo
            // 
            this.panelUserInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUserInfo.Location = new System.Drawing.Point(171, 0);
            this.panelUserInfo.Name = "panelUserInfo";
            this.panelUserInfo.Size = new System.Drawing.Size(398, 355);
            this.panelUserInfo.TabIndex = 8;
            // 
            // Benutzerverwaltung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelUserInfo);
            this.Controls.Add(this.treeViewFilter);
            this.Name = "Benutzerverwaltung";
            this.Size = new System.Drawing.Size(569, 355);
            this.Load += new System.EventHandler(this.Benutzerverwaltung_Load);
            this.teamNodeContextMenuStrip.ResumeLayout(false);
            this.centerNodeContextMenuStrip.ResumeLayout(false);
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
        private System.Windows.Forms.Panel panelUserInfo;
    }
}
