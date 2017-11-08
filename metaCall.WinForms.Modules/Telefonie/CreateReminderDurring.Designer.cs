namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class CreateReminderDurring
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
            this.groupBoxTeamReminder = new System.Windows.Forms.GroupBox();
            this.lblTeam = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.timeSpanComboBox = new System.Windows.Forms.ComboBox();
            this.grpZeitfenster = new System.Windows.Forms.GroupBox();
            this.dateTimePickerExt1 = new metatop.Applications.metaCall.WinForms.Modules.DateTimePickerExt();
            this.groupBoxTeamReminder.SuspendLayout();
            this.grpZeitfenster.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTeamReminder
            // 
            this.groupBoxTeamReminder.Controls.Add(this.lblTeam);
            this.groupBoxTeamReminder.Controls.Add(this.radioButton2);
            this.groupBoxTeamReminder.Controls.Add(this.radioButton1);
            this.groupBoxTeamReminder.Location = new System.Drawing.Point(315, 97);
            this.groupBoxTeamReminder.Name = "groupBoxTeamReminder";
            this.groupBoxTeamReminder.Size = new System.Drawing.Size(148, 90);
            this.groupBoxTeamReminder.TabIndex = 3;
            this.groupBoxTeamReminder.TabStop = false;
            this.groupBoxTeamReminder.Text = "Art";
            // 
            // lblTeam
            // 
            this.lblTeam.AutoSize = true;
            this.lblTeam.Location = new System.Drawing.Point(25, 62);
            this.lblTeam.Name = "lblTeam";
            this.lblTeam.Size = new System.Drawing.Size(35, 13);
            this.lblTeam.TabIndex = 2;
            this.lblTeam.Text = "label2";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 42);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(124, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Team Wiedervorlage";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(120, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "pers. Wiedervorlage";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // timeSpanComboBox
            // 
            this.timeSpanComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.timeSpanComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timeSpanComboBox.FormattingEnabled = true;
            this.timeSpanComboBox.Location = new System.Drawing.Point(6, 25);
            this.timeSpanComboBox.Name = "timeSpanComboBox";
            this.timeSpanComboBox.Size = new System.Drawing.Size(136, 21);
            this.timeSpanComboBox.TabIndex = 5;
            this.timeSpanComboBox.SelectedIndexChanged += new System.EventHandler(this.timeSpanComboBox_SelectedIndexChanged);
            // 
            // grpZeitfenster
            // 
            this.grpZeitfenster.Controls.Add(this.timeSpanComboBox);
            this.grpZeitfenster.Location = new System.Drawing.Point(315, 0);
            this.grpZeitfenster.Name = "grpZeitfenster";
            this.grpZeitfenster.Size = new System.Drawing.Size(148, 60);
            this.grpZeitfenster.TabIndex = 7;
            this.grpZeitfenster.TabStop = false;
            this.grpZeitfenster.Text = "Zeitfenster";
            // 
            // dateTimePickerExt1
            // 
            this.dateTimePickerExt1.CurrentDate = new System.DateTime(2006, 8, 15, 14, 10, 0, 0);
            this.dateTimePickerExt1.Location = new System.Drawing.Point(0, 0);
            this.dateTimePickerExt1.MaximumSize = new System.Drawing.Size(311, 187);
            this.dateTimePickerExt1.MinimumSize = new System.Drawing.Size(311, 187);
            this.dateTimePickerExt1.Name = "dateTimePickerExt1";
            this.dateTimePickerExt1.Size = new System.Drawing.Size(311, 187);
            this.dateTimePickerExt1.TabIndex = 4;
            this.dateTimePickerExt1.Value = new System.DateTime(2006, 8, 15, 14, 10, 0, 0);
            this.dateTimePickerExt1.ValueChanged += new System.EventHandler(this.dateTimePickerExt1_ValueChanged);
            // 
            // CreateReminderDurring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MistyRose;
            this.Controls.Add(this.grpZeitfenster);
            this.Controls.Add(this.dateTimePickerExt1);
            this.Controls.Add(this.groupBoxTeamReminder);
            this.MinimumSize = new System.Drawing.Size(468, 194);
            this.Name = "CreateReminderDurring";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.Size = new System.Drawing.Size(468, 194);
            this.Load += new System.EventHandler(this.CreateReminderDurring_Load);
            this.groupBoxTeamReminder.ResumeLayout(false);
            this.groupBoxTeamReminder.PerformLayout();
            this.grpZeitfenster.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTeamReminder;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private DateTimePickerExt dateTimePickerExt1;
        private System.Windows.Forms.ComboBox timeSpanComboBox;
        private System.Windows.Forms.Label lblTeam;
        private System.Windows.Forms.GroupBox grpZeitfenster;
    }
}
