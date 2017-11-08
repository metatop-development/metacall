namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class Projektverwaltung
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Projektverwaltung));
            this.projectsTreeView = new System.Windows.Forms.TreeView();
            this.projectViewPanel = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newProjectToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editProjectToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveEditToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.cancelEditToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeProjectStateToolStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.neueAdressenHinzufügenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unusedTransferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unusedTransferToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.unsuitableAddressesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resestBrokenAddressTransferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addressTransferManager1 = new metatop.Applications.metaCall.WinForms.Modules.AddressTransferManager();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // projectsTreeView
            // 
            this.projectsTreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.projectsTreeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.projectsTreeView.HideSelection = false;
            this.projectsTreeView.Location = new System.Drawing.Point(0, 25);
            this.projectsTreeView.Name = "projectsTreeView";
            this.projectsTreeView.ShowNodeToolTips = true;
            this.projectsTreeView.Size = new System.Drawing.Size(397, 313);
            this.projectsTreeView.TabIndex = 11;
            this.projectsTreeView.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.projectsTreeView_DrawNode);
            this.projectsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.projectsTreeView_AfterSelect);
            this.projectsTreeView.DoubleClick += new System.EventHandler(this.projectsTreeView_DoubleClick);
            this.projectsTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.projectsTreeView_MouseDown);
            // 
            // projectViewPanel
            // 
            this.projectViewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.projectViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectViewPanel.Location = new System.Drawing.Point(397, 25);
            this.projectViewPanel.Name = "projectViewPanel";
            this.projectViewPanel.Size = new System.Drawing.Size(275, 313);
            this.projectViewPanel.TabIndex = 12;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripButton,
            this.editProjectToolStripButton,
            this.deleteToolStripButton,
            this.saveEditToolStripButton,
            this.cancelEditToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(672, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newProjectToolStripButton
            // 
            this.newProjectToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newProjectToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newProjectToolStripButton.Image")));
            this.newProjectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newProjectToolStripButton.Name = "newProjectToolStripButton";
            this.newProjectToolStripButton.Size = new System.Drawing.Size(42, 22);
            this.newProjectToolStripButton.Text = "Neu...";
            this.newProjectToolStripButton.Click += new System.EventHandler(this.NewProjectToolStripButton_Click);
            // 
            // editProjectToolStripButton
            // 
            this.editProjectToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.editProjectToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("editProjectToolStripButton.Image")));
            this.editProjectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editProjectToolStripButton.Name = "editProjectToolStripButton";
            this.editProjectToolStripButton.Size = new System.Drawing.Size(76, 22);
            this.editProjectToolStripButton.Text = "Bearbeiten...";
            this.editProjectToolStripButton.Click += new System.EventHandler(this.EditProjectToolStripButton_Click);
            // 
            // deleteToolStripButton
            // 
            this.deleteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.deleteToolStripButton.Enabled = false;
            this.deleteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripButton.Image")));
            this.deleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteToolStripButton.Name = "deleteToolStripButton";
            this.deleteToolStripButton.Size = new System.Drawing.Size(55, 22);
            this.deleteToolStripButton.Text = "Löschen";
            // 
            // saveEditToolStripButton
            // 
            this.saveEditToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.saveEditToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveEditToolStripButton.Image")));
            this.saveEditToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveEditToolStripButton.Name = "saveEditToolStripButton";
            this.saveEditToolStripButton.Size = new System.Drawing.Size(63, 22);
            this.saveEditToolStripButton.Text = "Speichern";
            this.saveEditToolStripButton.Click += new System.EventHandler(this.saveEditToolStripButton_Click);
            // 
            // cancelEditToolStripButton
            // 
            this.cancelEditToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cancelEditToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelEditToolStripButton.Image")));
            this.cancelEditToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cancelEditToolStripButton.Name = "cancelEditToolStripButton";
            this.cancelEditToolStripButton.Size = new System.Drawing.Size(69, 22);
            this.cancelEditToolStripButton.Text = "Abbrechen";
            this.cancelEditToolStripButton.Click += new System.EventHandler(this.cancelEditToolStripButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.changeProjectStateToolStripItem,
            this.neueAdressenHinzufügenToolStripMenuItem,
            this.unusedTransferToolStripMenuItem,
            this.unsuitableAddressesToolStripMenuItem,
            this.resestBrokenAddressTransferToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(338, 136);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(337, 22);
            this.editToolStripMenuItem.Text = "editToolStripMenuItem";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // changeProjectStateToolStripItem
            // 
            this.changeProjectStateToolStripItem.Name = "changeProjectStateToolStripItem";
            this.changeProjectStateToolStripItem.Size = new System.Drawing.Size(337, 22);
            this.changeProjectStateToolStripItem.Text = "ChangeProjectState";
            this.changeProjectStateToolStripItem.Click += new System.EventHandler(this.changeProjectStateToolStripItem_Click);
            // 
            // neueAdressenHinzufügenToolStripMenuItem
            // 
            this.neueAdressenHinzufügenToolStripMenuItem.Name = "neueAdressenHinzufügenToolStripMenuItem";
            this.neueAdressenHinzufügenToolStripMenuItem.Size = new System.Drawing.Size(337, 22);
            this.neueAdressenHinzufügenToolStripMenuItem.Text = "Adressen hinzufügen";
            this.neueAdressenHinzufügenToolStripMenuItem.Click += new System.EventHandler(this.neueAdressenHinzufügenToolStripMenuItem_Click);
            // 
            // unusedTransferToolStripMenuItem
            // 
            this.unusedTransferToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unusedTransferToolStripComboBox});
            this.unusedTransferToolStripMenuItem.Name = "unusedTransferToolStripMenuItem";
            this.unusedTransferToolStripMenuItem.Size = new System.Drawing.Size(337, 22);
            this.unusedTransferToolStripMenuItem.Text = "unbenutzte Anrufe übernehmen";
            // 
            // unusedTransferToolStripComboBox
            // 
            this.unusedTransferToolStripComboBox.DropDownWidth = 380;
            this.unusedTransferToolStripComboBox.Name = "unusedTransferToolStripComboBox";
            this.unusedTransferToolStripComboBox.Size = new System.Drawing.Size(300, 23);
            this.unusedTransferToolStripComboBox.DropDownClosed += new System.EventHandler(this.unusedTransferToolStripComboBox_DropDownClosed);
            // 
            // unsuitableAddressesToolStripMenuItem
            // 
            this.unsuitableAddressesToolStripMenuItem.Name = "unsuitableAddressesToolStripMenuItem";
            this.unsuitableAddressesToolStripMenuItem.Size = new System.Drawing.Size(337, 22);
            this.unsuitableAddressesToolStripMenuItem.Text = "ungeeignete Adressen bestätigen";
            this.unsuitableAddressesToolStripMenuItem.Click += new System.EventHandler(this.unsuitableAddressesToolStripMenuItem_Click);
            // 
            // resestBrokenAddressTransferToolStripMenuItem
            // 
            this.resestBrokenAddressTransferToolStripMenuItem.Name = "resestBrokenAddressTransferToolStripMenuItem";
            this.resestBrokenAddressTransferToolStripMenuItem.Size = new System.Drawing.Size(337, 22);
            this.resestBrokenAddressTransferToolStripMenuItem.Text = "fehlgeschlagenen Adressen-Transfer zurücksetzen";
            this.resestBrokenAddressTransferToolStripMenuItem.Click += new System.EventHandler(this.resestBrokenAddressTransferToolStripMenuItem_Click);
            // 
            // addressTransferManager1
            // 
            this.addressTransferManager1.ProgressChanged += new metatop.Applications.metaCall.WinForms.Modules.ProgressChangedEventHandler(this.addressTransferManager1_ProgressChanged);
            this.addressTransferManager1.TransferCompleted += new metatop.Applications.metaCall.WinForms.Modules.TransferCompletedEventHandler(this.addressTransferManager1_TransferCompleted);
            // 
            // Projektverwaltung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.projectViewPanel);
            this.Controls.Add(this.projectsTreeView);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Projektverwaltung";
            this.Size = new System.Drawing.Size(672, 338);
            this.Load += new System.EventHandler(this.ProjektUebernahme_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private new System.Windows.Forms.TreeView projectsTreeView;
        private System.Windows.Forms.Panel projectViewPanel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newProjectToolStripButton;
        private System.Windows.Forms.ToolStripButton editProjectToolStripButton;
        private System.Windows.Forms.ToolStripButton deleteToolStripButton;
        private System.Windows.Forms.ToolStripButton saveEditToolStripButton;
        private System.Windows.Forms.ToolStripButton cancelEditToolStripButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem changeProjectStateToolStripItem;
        private System.Windows.Forms.ToolStripMenuItem neueAdressenHinzufügenToolStripMenuItem;
        private AddressTransferManager addressTransferManager1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unusedTransferToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox unusedTransferToolStripComboBox;
        private System.Windows.Forms.ToolStripMenuItem unsuitableAddressesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resestBrokenAddressTransferToolStripMenuItem;
    }
}

