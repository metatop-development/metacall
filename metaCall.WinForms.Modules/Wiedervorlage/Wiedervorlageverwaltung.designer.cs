namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class Wiedervorlageverwaltung
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
            this.teamNodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.teamDeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingSourceUser = new System.Windows.Forms.BindingSource(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.toolStripTreeView = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonProject = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAgent = new System.Windows.Forms.ToolStripButton();
            this.treeViewFilter = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceUser)).BeginInit();
            this.panel3.SuspendLayout();
            this.toolStripTreeView.SuspendLayout();
            this.SuspendLayout();
            // 
            // teamNodeContextMenuStrip
            // 
            this.teamNodeContextMenuStrip.Name = "teamNodeContextMenuStrip";
            this.teamNodeContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            this.teamNodeContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.teamNodeContextMenuStrip_Opening);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Location = new System.Drawing.Point(281, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(285, 355);
            this.panel1.TabIndex = 14;
            // 
            // teamDeleteToolStripMenuItem
            // 
            this.teamDeleteToolStripMenuItem.Name = "teamDeleteToolStripMenuItem";
            this.teamDeleteToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.teamDeleteToolStripMenuItem.Text = "Team löschen";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.toolStripTreeView);
            this.panel3.Controls.Add(this.treeViewFilter);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(278, 355);
            this.panel3.TabIndex = 18;
            // 
            // toolStripTreeView
            // 
            this.toolStripTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonProject,
            this.toolStripButtonAgent});
            this.toolStripTreeView.Location = new System.Drawing.Point(0, 0);
            this.toolStripTreeView.Name = "toolStripTreeView";
            this.toolStripTreeView.Size = new System.Drawing.Size(278, 25);
            this.toolStripTreeView.TabIndex = 18;
            this.toolStripTreeView.Text = "toolStrip1";
            // 
            // toolStripButtonProject
            // 
            this.toolStripButtonProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonProject.Image = global::metatop.Applications.metaCall.WinForms.Modules.Properties.Resources.UserIcon;
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
            this.treeViewFilter.Location = new System.Drawing.Point(0, 28);
            this.treeViewFilter.Name = "treeViewFilter";
            this.treeViewFilter.Size = new System.Drawing.Size(278, 327);
            this.treeViewFilter.TabIndex = 8;
            this.treeViewFilter.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFilter_AfterSelect);
            this.treeViewFilter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewFilter_MouseDown);
            // 
            // Wiedervorlageverwaltung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "Wiedervorlageverwaltung";
            this.Size = new System.Drawing.Size(569, 355);
            this.Load += new System.EventHandler(this.Wiedervorlageverwaltung_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceUser)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.toolStripTreeView.ResumeLayout(false);
            this.toolStripTreeView.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip teamNodeContextMenuStrip;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem teamDeleteToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSourceUser;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolStrip toolStripTreeView;
        private System.Windows.Forms.ToolStripButton toolStripButtonProject;
        private System.Windows.Forms.ToolStripButton toolStripButtonAgent;
        private System.Windows.Forms.TreeView treeViewFilter;
    }
}
