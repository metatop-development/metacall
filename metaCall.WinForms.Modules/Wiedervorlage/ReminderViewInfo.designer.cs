namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class ReminderViewInfo
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
            this.bindingSourceCallJobReminder = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripMenue = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStripCallJobReminder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CallJobReminderGridSelectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CallJobReminderGridSelectNothingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxCountReminders = new System.Windows.Forms.TextBox();
            this.dataGridViewReminders = new System.Windows.Forms.DataGridView();
            this.teamDeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingSourceUser = new System.Windows.Forms.BindingSource(this.components);
            this.CallJobReminderGetSetDoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCallJobReminder)).BeginInit();
            this.toolStripMenue.SuspendLayout();
            this.contextMenuStripCallJobReminder.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReminders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceUser)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMenue
            // 
            this.toolStripMenue.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.editToolStripButton});
            this.toolStripMenue.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenue.Name = "toolStripMenue";
            this.toolStripMenue.Size = new System.Drawing.Size(569, 25);
            this.toolStripMenue.TabIndex = 9;
            this.toolStripMenue.Text = "toolStrip1";
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
            // contextMenuStripCallJobReminder
            // 
            this.contextMenuStripCallJobReminder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CallJobReminderGridSelectAllToolStripMenuItem,
            this.CallJobReminderGridSelectNothingToolStripMenuItem,
            this.CallJobReminderGetSetDoneToolStripMenuItem});
            this.contextMenuStripCallJobReminder.Name = "contextMenuStripCallJobReminder";
            this.contextMenuStripCallJobReminder.Size = new System.Drawing.Size(175, 92);
            this.contextMenuStripCallJobReminder.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripCallJobReminder_Opening);
            // 
            // CallJobReminderGridSelectAllToolStripMenuItem
            // 
            this.CallJobReminderGridSelectAllToolStripMenuItem.Name = "CallJobReminderGridSelectAllToolStripMenuItem";
            this.CallJobReminderGridSelectAllToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.CallJobReminderGridSelectAllToolStripMenuItem.Text = "Alle Auswählen";
            this.CallJobReminderGridSelectAllToolStripMenuItem.Click += new System.EventHandler(this.CallJobReminderGridSelectAllToolStripMenuItem_Click);
            // 
            // CallJobReminderGridSelectNothingToolStripMenuItem
            // 
            this.CallJobReminderGridSelectNothingToolStripMenuItem.Name = "CallJobReminderGridSelectNothingToolStripMenuItem";
            this.CallJobReminderGridSelectNothingToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.CallJobReminderGridSelectNothingToolStripMenuItem.Text = "Auswahl aufheben";
            this.CallJobReminderGridSelectNothingToolStripMenuItem.Click += new System.EventHandler(this.CallJobReminderGridSelectNothingToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.textBoxCountReminders);
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(569, 29);
            this.panel1.TabIndex = 14;
            // 
            // textBoxCountReminders
            // 
            this.textBoxCountReminders.Location = new System.Drawing.Point(222, 3);
            this.textBoxCountReminders.MaxLength = 10;
            this.textBoxCountReminders.Name = "textBoxCountReminders";
            this.textBoxCountReminders.ReadOnly = true;
            this.textBoxCountReminders.Size = new System.Drawing.Size(60, 20);
            this.textBoxCountReminders.TabIndex = 37;
            // 
            // dataGridViewReminders
            // 
            this.dataGridViewReminders.AllowUserToAddRows = false;
            this.dataGridViewReminders.AllowUserToDeleteRows = false;
            this.dataGridViewReminders.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridViewReminders.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewReminders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewReminders.AutoGenerateColumns = false;
            this.dataGridViewReminders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewReminders.ContextMenuStrip = this.contextMenuStripCallJobReminder;
            this.dataGridViewReminders.DataSource = this.bindingSourceCallJobReminder;
            this.dataGridViewReminders.Location = new System.Drawing.Point(0, 57);
            this.dataGridViewReminders.Name = "dataGridViewReminders";
            this.dataGridViewReminders.ReadOnly = true;
            this.dataGridViewReminders.RowHeadersWidth = 32;
            this.dataGridViewReminders.RowTemplate.Height = 18;
            this.dataGridViewReminders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewReminders.Size = new System.Drawing.Size(569, 298);
            this.dataGridViewReminders.TabIndex = 15;
            this.dataGridViewReminders.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewReminders_CellMouseDoubleClick);
            this.dataGridViewReminders.SelectionChanged += new System.EventHandler(this.dataGridViewReminders_SelectionChanged);
            // 
            // teamDeleteToolStripMenuItem
            // 
            this.teamDeleteToolStripMenuItem.Name = "teamDeleteToolStripMenuItem";
            this.teamDeleteToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.teamDeleteToolStripMenuItem.Text = "Team löschen";
            // 
            // CallJobReminderGetSetDoneToolStripMenuItem
            // 
            this.CallJobReminderGetSetDoneToolStripMenuItem.Name = "CallJobReminderGetSetDoneToolStripMenuItem";
            this.CallJobReminderGetSetDoneToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.CallJobReminderGetSetDoneToolStripMenuItem.Text = "Erledigt";
            this.CallJobReminderGetSetDoneToolStripMenuItem.Click += new System.EventHandler(this.CallJobReminderGetSetDoneToolStripMenuItem_Click);
            // 
            // ReminderViewInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridViewReminders);
            this.Controls.Add(this.toolStripMenue);
            this.Controls.Add(this.panel1);
            this.Name = "ReminderViewInfo";
            this.Size = new System.Drawing.Size(569, 355);
            this.Load += new System.EventHandler(this.ReminderViewInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCallJobReminder)).EndInit();
            this.toolStripMenue.ResumeLayout(false);
            this.toolStripMenue.PerformLayout();
            this.contextMenuStripCallJobReminder.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReminders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceUser)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSourceCallJobReminder;
        private System.Windows.Forms.ToolStrip toolStripMenue;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton editToolStripButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripCallJobReminder;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridViewReminders;
        private System.Windows.Forms.ToolStripMenuItem teamDeleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CallJobReminderGridSelectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CallJobReminderGridSelectNothingToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSourceUser;
        private System.Windows.Forms.TextBox textBoxCountReminders;
        private System.Windows.Forms.ToolStripMenuItem CallJobReminderGetSetDoneToolStripMenuItem;
    }
}
