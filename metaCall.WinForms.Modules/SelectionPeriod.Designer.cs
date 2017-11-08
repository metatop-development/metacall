namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class SelectionPeriod
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
            this.timeModeSelection = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.monthMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.toDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.fromDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.timeModeMonth = new System.Windows.Forms.RadioButton();
            this.timeModeFromTo = new System.Windows.Forms.RadioButton();
            this.timeModeSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // timeModeSelection
            // 
            this.timeModeSelection.Controls.Add(this.label1);
            this.timeModeSelection.Controls.Add(this.monthMaskedTextBox);
            this.timeModeSelection.Controls.Add(this.toDateTimePicker);
            this.timeModeSelection.Controls.Add(this.fromDateTimePicker);
            this.timeModeSelection.Controls.Add(this.timeModeMonth);
            this.timeModeSelection.Controls.Add(this.timeModeFromTo);
            this.timeModeSelection.Location = new System.Drawing.Point(10, 10);
            this.timeModeSelection.Name = "timeModeSelection";
            this.timeModeSelection.Size = new System.Drawing.Size(590, 48);
            this.timeModeSelection.TabIndex = 21;
            this.timeModeSelection.TabStop = false;
            this.timeModeSelection.Text = "Auswahl Zeitraum";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(200, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "bis";
            // 
            // monthMaskedTextBox
            // 
            this.monthMaskedTextBox.Location = new System.Drawing.Point(510, 18);
            this.monthMaskedTextBox.Mask = "00\\/0000";
            this.monthMaskedTextBox.Name = "monthMaskedTextBox";
            this.monthMaskedTextBox.Size = new System.Drawing.Size(70, 20);
            this.monthMaskedTextBox.TabIndex = 3;
            this.monthMaskedTextBox.Text = "052006";
            this.monthMaskedTextBox.Validated += new System.EventHandler(this.monthMaskedTextBox_Validated);
            // 
            // toDateTimePicker
            // 
            this.toDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.toDateTimePicker.Location = new System.Drawing.Point(230, 18);
            this.toDateTimePicker.MinDate = new System.DateTime(2007, 5, 6, 0, 0, 0, 0);
            this.toDateTimePicker.Name = "toDateTimePicker";
            this.toDateTimePicker.Size = new System.Drawing.Size(101, 20);
            this.toDateTimePicker.TabIndex = 2;
            this.toDateTimePicker.Value = new System.DateTime(2007, 5, 7, 0, 0, 0, 0);
            this.toDateTimePicker.ValueChanged += new System.EventHandler(this.toDateTimePicker_ValueChanged);
            // 
            // fromDateTimePicker
            // 
            this.fromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.fromDateTimePicker.Location = new System.Drawing.Point(98, 18);
            this.fromDateTimePicker.MinDate = new System.DateTime(2007, 5, 6, 0, 0, 0, 0);
            this.fromDateTimePicker.Name = "fromDateTimePicker";
            this.fromDateTimePicker.Size = new System.Drawing.Size(100, 20);
            this.fromDateTimePicker.TabIndex = 1;
            this.fromDateTimePicker.Value = new System.DateTime(2007, 5, 7, 0, 0, 0, 0);
            this.fromDateTimePicker.ValueChanged += new System.EventHandler(this.fromDateTimePicker_ValueChanged);
            // 
            // timeModeMonth
            // 
            this.timeModeMonth.AutoSize = true;
            this.timeModeMonth.Location = new System.Drawing.Point(360, 20);
            this.timeModeMonth.Name = "timeModeMonth";
            this.timeModeMonth.Size = new System.Drawing.Size(138, 17);
            this.timeModeMonth.TabIndex = 1;
            this.timeModeMonth.TabStop = true;
            this.timeModeMonth.Text = "Auswahl  (Monat / Jahr)";
            this.timeModeMonth.UseVisualStyleBackColor = true;
            this.timeModeMonth.Click += new System.EventHandler(this.SetTimeMode);
            // 
            // timeModeFromTo
            // 
            this.timeModeFromTo.AutoSize = true;
            this.timeModeFromTo.Checked = true;
            this.timeModeFromTo.Location = new System.Drawing.Point(10, 20);
            this.timeModeFromTo.Name = "timeModeFromTo";
            this.timeModeFromTo.Size = new System.Drawing.Size(86, 17);
            this.timeModeFromTo.TabIndex = 0;
            this.timeModeFromTo.TabStop = true;
            this.timeModeFromTo.Text = "Auswahl von";
            this.timeModeFromTo.UseVisualStyleBackColor = true;
            this.timeModeFromTo.Click += new System.EventHandler(this.SetTimeMode);
            // 
            // SelectionPeriod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.timeModeSelection);
            this.Name = "SelectionPeriod";
            this.Size = new System.Drawing.Size(610, 68);
            this.Load += new System.EventHandler(this.SelectionPeriod_Load);
            this.timeModeSelection.ResumeLayout(false);
            this.timeModeSelection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox timeModeSelection;
        private System.Windows.Forms.MaskedTextBox monthMaskedTextBox;
        private System.Windows.Forms.DateTimePicker toDateTimePicker;
        private System.Windows.Forms.DateTimePicker fromDateTimePicker;
        private System.Windows.Forms.RadioButton timeModeMonth;
        private System.Windows.Forms.RadioButton timeModeFromTo;
        private System.Windows.Forms.Label label1;
    }
}
