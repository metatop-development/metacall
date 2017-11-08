namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class HistorieInfo
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
            this.dgvCallJobResults = new System.Windows.Forms.DataGridView();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCallJobResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCallJobResults
            // 
            this.dgvCallJobResults.AllowUserToAddRows = false;
            this.dgvCallJobResults.AllowUserToDeleteRows = false;
            this.dgvCallJobResults.AllowUserToResizeColumns = false;
            this.dgvCallJobResults.AllowUserToResizeRows = false;
            this.dgvCallJobResults.AutoGenerateColumns = false;
            this.dgvCallJobResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvCallJobResults.BackgroundColor = System.Drawing.Color.Lavender;
            this.dgvCallJobResults.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvCallJobResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCallJobResults.ColumnHeadersVisible = false;
            this.dgvCallJobResults.DataSource = this.bindingSource1;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCallJobResults.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCallJobResults.Location = new System.Drawing.Point(10, 10);
            this.dgvCallJobResults.Name = "dgvCallJobResults";
            this.dgvCallJobResults.ReadOnly = true;
            this.dgvCallJobResults.RowHeadersVisible = false;
            this.dgvCallJobResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCallJobResults.ShowCellErrors = false;
            this.dgvCallJobResults.ShowEditingIcon = false;
            this.dgvCallJobResults.ShowRowErrors = false;
            this.dgvCallJobResults.Size = new System.Drawing.Size(330, 80);
            this.dgvCallJobResults.TabIndex = 0;
            this.dgvCallJobResults.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.dgvCallJobResults_CellToolTipTextNeeded);
            // 
            // HistorieInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.Controls.Add(this.dgvCallJobResults);
            this.Name = "HistorieInfo";
            this.Size = new System.Drawing.Size(350, 100);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCallJobResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCallJobResults;
        private System.Windows.Forms.BindingSource bindingSource1;

    }
}
