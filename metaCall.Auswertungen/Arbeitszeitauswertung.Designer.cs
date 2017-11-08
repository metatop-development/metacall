namespace metatop.Applications.metaCall.WinForms.Modules.Auswertungen
{
    partial class Arbeitszeitauswertung
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.timeModeSelection = new System.Windows.Forms.GroupBox();
            this.monthMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.toDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.fromDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.timeModeMonth = new System.Windows.Forms.RadioButton();
            this.timeModeFromTo = new System.Windows.Forms.RadioButton();
            this.refreshButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.userComboBox = new System.Windows.Forms.ComboBox();
            this.teamComboBox = new System.Windows.Forms.ComboBox();
            this.centerComboBox = new System.Windows.Forms.ComboBox();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.timeModeSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.timeModeSelection);
            this.splitContainer1.Panel1.Controls.Add(this.refreshButton);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.comboBox4);
            this.splitContainer1.Panel1.Controls.Add(this.userComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.teamComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.centerComboBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.crystalReportViewer1);
            this.splitContainer1.Size = new System.Drawing.Size(759, 704);
            this.splitContainer1.SplitterDistance = 253;
            this.splitContainer1.TabIndex = 12;
            // 
            // timeModeSelection
            // 
            this.timeModeSelection.Controls.Add(this.monthMaskedTextBox);
            this.timeModeSelection.Controls.Add(this.toDateTimePicker);
            this.timeModeSelection.Controls.Add(this.fromDateTimePicker);
            this.timeModeSelection.Controls.Add(this.timeModeMonth);
            this.timeModeSelection.Controls.Add(this.timeModeFromTo);
            this.timeModeSelection.Location = new System.Drawing.Point(6, 206);
            this.timeModeSelection.Name = "timeModeSelection";
            this.timeModeSelection.Size = new System.Drawing.Size(238, 119);
            this.timeModeSelection.TabIndex = 20;
            this.timeModeSelection.TabStop = false;
            this.timeModeSelection.Text = "Auswahl des Auswertungszeitraums";
            // 
            // monthMaskedTextBox
            // 
            this.monthMaskedTextBox.Location = new System.Drawing.Point(21, 91);
            this.monthMaskedTextBox.Mask = "00\\/0000";
            this.monthMaskedTextBox.Name = "monthMaskedTextBox";
            this.monthMaskedTextBox.Size = new System.Drawing.Size(70, 20);
            this.monthMaskedTextBox.TabIndex = 3;
            this.monthMaskedTextBox.Text = "052006";
            // 
            // toDateTimePicker
            // 
            this.toDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.toDateTimePicker.Location = new System.Drawing.Point(127, 42);
            this.toDateTimePicker.Name = "toDateTimePicker";
            this.toDateTimePicker.Size = new System.Drawing.Size(101, 20);
            this.toDateTimePicker.TabIndex = 2;
            // 
            // fromDateTimePicker
            // 
            this.fromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.fromDateTimePicker.Location = new System.Drawing.Point(21, 42);
            this.fromDateTimePicker.Name = "fromDateTimePicker";
            this.fromDateTimePicker.Size = new System.Drawing.Size(100, 20);
            this.fromDateTimePicker.TabIndex = 1;
            this.fromDateTimePicker.ValueChanged += new System.EventHandler(this.fromDateTimePicker_ValueChanged);
            // 
            // timeModeMonth
            // 
            this.timeModeMonth.AutoSize = true;
            this.timeModeMonth.Location = new System.Drawing.Point(6, 68);
            this.timeModeMonth.Name = "timeModeMonth";
            this.timeModeMonth.Size = new System.Drawing.Size(201, 17);
            this.timeModeMonth.TabIndex = 1;
            this.timeModeMonth.TabStop = true;
            this.timeModeMonth.Text = "Auswahl eines Monats (Monat / Jahr)";
            this.timeModeMonth.UseVisualStyleBackColor = true;
            this.timeModeMonth.Click += new System.EventHandler(this.SetTimeMode);
            // 
            // timeModeFromTo
            // 
            this.timeModeFromTo.AutoSize = true;
            this.timeModeFromTo.Checked = true;
            this.timeModeFromTo.Location = new System.Drawing.Point(6, 19);
            this.timeModeFromTo.Name = "timeModeFromTo";
            this.timeModeFromTo.Size = new System.Drawing.Size(177, 17);
            this.timeModeFromTo.TabIndex = 0;
            this.timeModeFromTo.TabStop = true;
            this.timeModeFromTo.Text = "Auswahl eines Datums (von/bis)";
            this.timeModeFromTo.UseVisualStyleBackColor = true;
            this.timeModeFromTo.Click += new System.EventHandler(this.SetTimeMode);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(12, 345);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 4;
            this.refreshButton.Text = "aktualisieren";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Center";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Team";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Mitarbeiter";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Projekt";
            this.label2.Visible = false;
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(6, 151);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(121, 21);
            this.comboBox4.TabIndex = 3;
            this.comboBox4.Visible = false;
            // 
            // userComboBox
            // 
            this.userComboBox.FormattingEnabled = true;
            this.userComboBox.Location = new System.Drawing.Point(7, 111);
            this.userComboBox.Name = "userComboBox";
            this.userComboBox.Size = new System.Drawing.Size(121, 21);
            this.userComboBox.TabIndex = 2;
            // 
            // teamComboBox
            // 
            this.teamComboBox.FormattingEnabled = true;
            this.teamComboBox.Location = new System.Drawing.Point(6, 71);
            this.teamComboBox.Name = "teamComboBox";
            this.teamComboBox.Size = new System.Drawing.Size(121, 21);
            this.teamComboBox.TabIndex = 1;
            this.teamComboBox.SelectionChangeCommitted += new System.EventHandler(this.teamComboBox_SelectionChangeCommitted);
            // 
            // centerComboBox
            // 
            this.centerComboBox.FormattingEnabled = true;
            this.centerComboBox.Location = new System.Drawing.Point(6, 31);
            this.centerComboBox.Name = "centerComboBox";
            this.centerComboBox.Size = new System.Drawing.Size(121, 21);
            this.centerComboBox.TabIndex = 0;
            this.centerComboBox.SelectionChangeCommitted += new System.EventHandler(this.centerComboBox_SelectionChangeCommitted);
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.DisplayGroupTree = false;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.EnableToolTips = false;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.SelectionFormula = "";
            this.crystalReportViewer1.ShowCloseButton = false;
            this.crystalReportViewer1.ShowExportButton = false;
            this.crystalReportViewer1.ShowGroupTreeButton = false;
            this.crystalReportViewer1.ShowRefreshButton = false;
            this.crystalReportViewer1.ShowTextSearchButton = false;
            this.crystalReportViewer1.Size = new System.Drawing.Size(502, 704);
            this.crystalReportViewer1.TabIndex = 12;
            this.crystalReportViewer1.ViewTimeSelectionFormula = "";
            this.crystalReportViewer1.ReportRefresh += new CrystalDecisions.Windows.Forms.RefreshEventHandler(this.crystalReportViewer1_ReportRefresh);
            this.crystalReportViewer1.Error += new CrystalDecisions.Windows.Forms.ExceptionEventHandler(this.crystalReportViewer1_Error);
            // 
            // Arbeitszeitauswertung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "Arbeitszeitauswertung";
            this.Size = new System.Drawing.Size(759, 704);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.timeModeSelection.ResumeLayout(false);
            this.timeModeSelection.PerformLayout();
            this.ResumeLayout(false);

}

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox timeModeSelection;
        private System.Windows.Forms.MaskedTextBox monthMaskedTextBox;
        private System.Windows.Forms.DateTimePicker toDateTimePicker;
        private System.Windows.Forms.DateTimePicker fromDateTimePicker;
        private System.Windows.Forms.RadioButton timeModeMonth;
        private System.Windows.Forms.RadioButton timeModeFromTo;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.ComboBox userComboBox;
        private System.Windows.Forms.ComboBox teamComboBox;
        private System.Windows.Forms.ComboBox centerComboBox;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;

    }
}
