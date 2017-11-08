namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class ProjectViewInfo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bindingSourceProjects = new System.Windows.Forms.BindingSource(this.components);
            this.DataGridViewProjects = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.filterTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceProjects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewProjects)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridViewProjects
            // 
            this.DataGridViewProjects.AllowUserToAddRows = false;
            this.DataGridViewProjects.AllowUserToDeleteRows = false;
            this.DataGridViewProjects.AllowUserToResizeColumns = false;
            this.DataGridViewProjects.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Honeydew;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.PaleGreen;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.DataGridViewProjects.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.DataGridViewProjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridViewProjects.AutoGenerateColumns = false;
            this.DataGridViewProjects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.DataGridViewProjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewProjects.DataSource = this.bindingSourceProjects;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.PaleGreen;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridViewProjects.DefaultCellStyle = dataGridViewCellStyle4;
            this.DataGridViewProjects.Location = new System.Drawing.Point(10, 44);
            this.DataGridViewProjects.Margin = new System.Windows.Forms.Padding(10);
            this.DataGridViewProjects.MultiSelect = false;
            this.DataGridViewProjects.Name = "DataGridViewProjects";
            this.DataGridViewProjects.ReadOnly = true;
            this.DataGridViewProjects.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DataGridViewProjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridViewProjects.ShowCellErrors = false;
            this.DataGridViewProjects.ShowEditingIcon = false;
            this.DataGridViewProjects.ShowRowErrors = false;
            this.DataGridViewProjects.Size = new System.Drawing.Size(730, 96);
            this.DataGridViewProjects.TabIndex = 15;
            this.DataGridViewProjects.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewProjects_CellContentDoubleClick);
            this.DataGridViewProjects.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridViewProjects_CellFormatting);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Filter";
            // 
            // filterTextBox
            // 
            this.filterTextBox.Location = new System.Drawing.Point(57, 11);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(170, 20);
            this.filterTextBox.TabIndex = 17;
            this.filterTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.filterTextBox_KeyUp);
            // 
            // ProjectViewInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.filterTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DataGridViewProjects);
            this.Name = "ProjectViewInfo";
            this.Size = new System.Drawing.Size(750, 150);
            this.SizeChanged += new System.EventHandler(this.ProjectViewInfo_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceProjects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewProjects)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSourceProjects;
        private System.Windows.Forms.DataGridView DataGridViewProjects;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox filterTextBox;
    }
}
