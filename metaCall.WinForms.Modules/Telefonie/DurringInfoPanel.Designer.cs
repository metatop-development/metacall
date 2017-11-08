namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class DurringInfoPanel
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
            this.dataGridViewDurring = new System.Windows.Forms.DataGridView();
            this.bindingSourceDurring = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDurring)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceDurring)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewDurring
            // 
            this.dataGridViewDurring.AllowUserToAddRows = false;
            this.dataGridViewDurring.AllowUserToDeleteRows = false;
            this.dataGridViewDurring.AllowUserToOrderColumns = true;
            this.dataGridViewDurring.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridViewDurring.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewDurring.AutoGenerateColumns = false;
            this.dataGridViewDurring.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewDurring.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.dataGridViewDurring.DataSource = this.bindingSourceDurring;
            this.dataGridViewDurring.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDurring.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewDurring.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewDurring.MultiSelect = false;
            this.dataGridViewDurring.Name = "dataGridViewDurring";
            this.dataGridViewDurring.ReadOnly = true;
            this.dataGridViewDurring.RowHeadersVisible = false;
            this.dataGridViewDurring.RowTemplate.ReadOnly = true;
            this.dataGridViewDurring.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewDurring.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDurring.ShowCellErrors = false;
            this.dataGridViewDurring.ShowEditingIcon = false;
            this.dataGridViewDurring.ShowRowErrors = false;
            this.dataGridViewDurring.Size = new System.Drawing.Size(1000, 463);
            this.dataGridViewDurring.StandardTab = true;
            this.dataGridViewDurring.TabIndex = 1;
            this.dataGridViewDurring.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDurring_CellDoubleClick);
            // 
            // DurringInfoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridViewDurring);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DurringInfoPanel";
            this.Size = new System.Drawing.Size(1000, 463);
            this.Load += new System.EventHandler(this.DurringInfoPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDurring)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceDurring)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewDurring;
        private System.Windows.Forms.BindingSource bindingSourceDurring;
    }
}
