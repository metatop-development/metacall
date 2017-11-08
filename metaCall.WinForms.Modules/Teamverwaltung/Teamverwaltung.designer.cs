namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class Teamverwaltung
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Teamverwaltung));
            this.treeViewFilter = new System.Windows.Forms.TreeView();
            this.dataGridViewTeams = new System.Windows.Forms.DataGridView();
            this.bindingSourceTeams = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.centerNodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.centerAddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerDeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.centerTeamAddtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTeams)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.centerNodeContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewFilter
            // 
            this.treeViewFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewFilter.HideSelection = false;
            this.treeViewFilter.HotTracking = true;
            this.treeViewFilter.Location = new System.Drawing.Point(0, 0);
            this.treeViewFilter.Name = "treeViewFilter";
            this.treeViewFilter.Size = new System.Drawing.Size(165, 355);
            this.treeViewFilter.TabIndex = 7;
            this.treeViewFilter.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFilter_AfterSelect);
            this.treeViewFilter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewFilter_MouseDown);
            // 
            // dataGridViewTeams
            // 
            this.dataGridViewTeams.AllowUserToAddRows = false;
            this.dataGridViewTeams.AllowUserToDeleteRows = false;
            this.dataGridViewTeams.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridViewTeams.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTeams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTeams.AutoGenerateColumns = false;
            this.dataGridViewTeams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTeams.DataSource = this.bindingSourceTeams;
            this.dataGridViewTeams.Location = new System.Drawing.Point(165, 25);
            this.dataGridViewTeams.MultiSelect = false;
            this.dataGridViewTeams.Name = "dataGridViewTeams";
            this.dataGridViewTeams.ReadOnly = true;
            this.dataGridViewTeams.RowHeadersWidth = 32;
            this.dataGridViewTeams.RowTemplate.Height = 18;
            this.dataGridViewTeams.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewTeams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTeams.Size = new System.Drawing.Size(404, 330);
            this.dataGridViewTeams.TabIndex = 8;
            this.dataGridViewTeams.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewUsers_CellMouseDoubleClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.editToolStripButton,
            this.deleteToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(165, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(105, 25);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newToolStripButton.Image = global::metatop.Applications.metaCall.WinForms.Modules.Properties.Resources.UserIcon;
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(30, 22);
            this.newToolStripButton.Text = "Neu";
            this.newToolStripButton.Click += new System.EventHandler(this.newToolStripButton_Click);
            // 
            // editToolStripButton
            // 
            this.editToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.editToolStripButton.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.editToolStripButton.Name = "editToolStripButton";
            this.editToolStripButton.Size = new System.Drawing.Size(63, 22);
            this.editToolStripButton.Text = "Bearbeiten";
            this.editToolStripButton.Click += new System.EventHandler(this.editToolStripButton_Click);
            // 
            // deleteToolStripButton
            // 
            this.deleteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.deleteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripButton.Image")));
            this.deleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteToolStripButton.Name = "deleteToolStripButton";
            this.deleteToolStripButton.Size = new System.Drawing.Size(50, 17);
            this.deleteToolStripButton.Text = "Löschen";
            this.deleteToolStripButton.Visible = false;
            // 
            // centerNodeContextMenuStrip
            // 
            this.centerNodeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.centerAddToolStripMenuItem,
            this.centerEditToolStripMenuItem,
            this.centerDeleteToolStripMenuItem,
            this.toolStripSeparator1});
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
            // 
            // Teamverwaltung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridViewTeams);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.treeViewFilter);
            this.Name = "Teamverwaltung";
            this.Size = new System.Drawing.Size(569, 355);
            this.Load += new System.EventHandler(this.Teamverwaltung_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTeams)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.centerNodeContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewFilter;
        private System.Windows.Forms.DataGridView dataGridViewTeams;
        private System.Windows.Forms.BindingSource bindingSourceTeams;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton editToolStripButton;
        private System.Windows.Forms.ToolStripButton deleteToolStripButton;
        private System.Windows.Forms.ContextMenuStrip centerNodeContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem centerAddToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centerEditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centerDeleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem centerTeamAddtoolStripMenuItem;
    }
}
