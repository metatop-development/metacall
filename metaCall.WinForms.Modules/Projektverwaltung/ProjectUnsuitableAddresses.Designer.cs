namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class ProjectUnsuitableAddresses
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
            this.lblProject = new System.Windows.Forms.Label();
            this.userComboBox = new MaDaNet.Common.AppFrameWork.WinUI.Controls.ComboBoxAutoComplete();
            this.reasonComboBox = new MaDaNet.Common.AppFrameWork.WinUI.Controls.ComboBoxAutoComplete();
            this.dataGridViewUnsuitableCallJobs = new System.Windows.Forms.DataGridView();
            this.unsuitableContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bindingSourceUnsuitableCallJobs = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUnsuitableCallJobs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceUnsuitableCallJobs)).BeginInit();
            this.SuspendLayout();
            // 
            // lblProject
            // 
            this.lblProject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProject.Location = new System.Drawing.Point(30, 18);
            this.lblProject.Name = "lblProject";
            this.lblProject.Size = new System.Drawing.Size(559, 31);
            this.lblProject.TabIndex = 0;
            this.lblProject.Text = "Projekt";
            this.lblProject.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // userComboBox
            // 
            this.userComboBox.FormattingEnabled = true;
            this.userComboBox.LimitToList = true;
            this.userComboBox.Location = new System.Drawing.Point(79, 61);
            this.userComboBox.Name = "userComboBox";
            this.userComboBox.Size = new System.Drawing.Size(212, 21);
            this.userComboBox.TabIndex = 1;
            this.userComboBox.NotInList += new System.ComponentModel.CancelEventHandler(this.userComboBox_NotInList);
            this.userComboBox.SelectedValueChanged += new System.EventHandler(this.userComboBox_SelectedValueChanged);
            // 
            // reasonComboBox
            // 
            this.reasonComboBox.FormattingEnabled = true;
            this.reasonComboBox.LimitToList = true;
            this.reasonComboBox.Location = new System.Drawing.Point(377, 60);
            this.reasonComboBox.MaxDropDownItems = 15;
            this.reasonComboBox.Name = "reasonComboBox";
            this.reasonComboBox.Size = new System.Drawing.Size(212, 21);
            this.reasonComboBox.TabIndex = 2;
            this.reasonComboBox.NotInList += new System.ComponentModel.CancelEventHandler(this.reasonComboBox_NotInList);
            this.reasonComboBox.SelectedValueChanged += new System.EventHandler(this.reasonComboBox_SelectedValueChanged);
            // 
            // dataGridViewUnsuitableCallJobs
            // 
            this.dataGridViewUnsuitableCallJobs.AllowUserToAddRows = false;
            this.dataGridViewUnsuitableCallJobs.AllowUserToDeleteRows = false;
            this.dataGridViewUnsuitableCallJobs.AllowUserToOrderColumns = true;
            this.dataGridViewUnsuitableCallJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewUnsuitableCallJobs.AutoGenerateColumns = false;
            this.dataGridViewUnsuitableCallJobs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUnsuitableCallJobs.ContextMenuStrip = this.unsuitableContextMenuStrip;
            this.dataGridViewUnsuitableCallJobs.DataSource = this.bindingSourceUnsuitableCallJobs;
            this.dataGridViewUnsuitableCallJobs.Location = new System.Drawing.Point(3, 107);
            this.dataGridViewUnsuitableCallJobs.Name = "dataGridViewUnsuitableCallJobs";
            this.dataGridViewUnsuitableCallJobs.ReadOnly = true;
            this.dataGridViewUnsuitableCallJobs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewUnsuitableCallJobs.Size = new System.Drawing.Size(632, 376);
            this.dataGridViewUnsuitableCallJobs.TabIndex = 3;
            // 
            // unsuitableContextMenuStrip
            // 
            this.unsuitableContextMenuStrip.Name = "unsuitableContextMenuStrip";
            this.unsuitableContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            this.unsuitableContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.unsuitableContextMenuStrip_Opening);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Agent:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(332, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Grund:";
            // 
            // ProjectUnsuitableAddresses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewUnsuitableCallJobs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.reasonComboBox);
            this.Controls.Add(this.userComboBox);
            this.Controls.Add(this.lblProject);
            this.Name = "ProjectUnsuitableAddresses";
            this.Size = new System.Drawing.Size(638, 486);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUnsuitableCallJobs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceUnsuitableCallJobs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProject;
        private MaDaNet.Common.AppFrameWork.WinUI.Controls.ComboBoxAutoComplete userComboBox;
        private MaDaNet.Common.AppFrameWork.WinUI.Controls.ComboBoxAutoComplete reasonComboBox;
        private System.Windows.Forms.DataGridView dataGridViewUnsuitableCallJobs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.BindingSource bindingSourceUnsuitableCallJobs;
        private System.Windows.Forms.ContextMenuStrip unsuitableContextMenuStrip;
    }
}
