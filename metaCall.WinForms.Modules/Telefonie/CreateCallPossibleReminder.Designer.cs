namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class CreateCallPossibleReminder
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
            this.lblTeilnahme = new System.Windows.Forms.Label();
            this.cBResult0 = new System.Windows.Forms.ComboBox();
            this.cBResult1 = new System.Windows.Forms.ComboBox();
            this.lblResult1 = new System.Windows.Forms.Label();
            this.lblResult2 = new System.Windows.Forms.Label();
            this.cBResult2 = new System.Windows.Forms.ComboBox();
            this.lblResult3 = new System.Windows.Forms.Label();
            this.cBResult3 = new System.Windows.Forms.ComboBox();
            this.txtAnzahl = new System.Windows.Forms.TextBox();
            this.cBSet4 = new System.Windows.Forms.Button();
            this.cBSet5 = new System.Windows.Forms.Button();
            this.cBSet6 = new System.Windows.Forms.Button();
            this.cBSet3 = new System.Windows.Forms.Button();
            this.cBSet2 = new System.Windows.Forms.Button();
            this.cBSet1 = new System.Windows.Forms.Button();
            this.checkedListBoxThangingForm = new System.Windows.Forms.CheckedListBox();
            this.lblResult4 = new System.Windows.Forms.Label();
            this.lBPreisBez = new System.Windows.Forms.Label();
            this.txtPreis = new System.Windows.Forms.TextBox();
            this.txtResult5 = new System.Windows.Forms.TextBox();
            this.lblResult5 = new System.Windows.Forms.Label();
            this.secondCallDesiredGroupBox = new System.Windows.Forms.GroupBox();
            this.secondCallUndesiredRadioButton = new System.Windows.Forms.RadioButton();
            this.secondCallDesiredRadioButton = new System.Windows.Forms.RadioButton();
            this.sendFaxButton = new System.Windows.Forms.Button();
            this.DateTimePickerPaymentTarget = new System.Windows.Forms.DateTimePicker();
            this.createReminder1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateReminder();
            this.secondCallDesiredGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.createReminder1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTeilnahme
            // 
            this.lblTeilnahme.Location = new System.Drawing.Point(19, 18);
            this.lblTeilnahme.Name = "lblTeilnahme";
            this.lblTeilnahme.Size = new System.Drawing.Size(130, 13);
            this.lblTeilnahme.TabIndex = 1;
            this.lblTeilnahme.Text = "Teilnahme an der Aktion";
            // 
            // cBResult0
            // 
            this.cBResult0.FormattingEnabled = true;
            this.cBResult0.Location = new System.Drawing.Point(159, 13);
            this.cBResult0.Name = "cBResult0";
            this.cBResult0.Size = new System.Drawing.Size(120, 21);
            this.cBResult0.TabIndex = 2;
            this.cBResult0.SelectedIndexChanged += new System.EventHandler(this.cBResult0_SelectedIndexChanged);
            // 
            // cBResult1
            // 
            this.cBResult1.FormattingEnabled = true;
            this.cBResult1.Location = new System.Drawing.Point(159, 38);
            this.cBResult1.Name = "cBResult1";
            this.cBResult1.Size = new System.Drawing.Size(120, 21);
            this.cBResult1.TabIndex = 3;
            this.cBResult1.SelectedValueChanged += new System.EventHandler(this.cBResult1_SelectedValueChanged);
            // 
            // lblResult1
            // 
            this.lblResult1.Location = new System.Drawing.Point(19, 43);
            this.lblResult1.Name = "lblResult1";
            this.lblResult1.Size = new System.Drawing.Size(130, 13);
            this.lblResult1.TabIndex = 4;
            this.lblResult1.Text = "res1";
            // 
            // lblResult2
            // 
            this.lblResult2.Location = new System.Drawing.Point(19, 68);
            this.lblResult2.Name = "lblResult2";
            this.lblResult2.Size = new System.Drawing.Size(130, 13);
            this.lblResult2.TabIndex = 5;
            this.lblResult2.Text = "res2";
            // 
            // cBResult2
            // 
            this.cBResult2.FormattingEnabled = true;
            this.cBResult2.Location = new System.Drawing.Point(159, 63);
            this.cBResult2.Name = "cBResult2";
            this.cBResult2.Size = new System.Drawing.Size(120, 21);
            this.cBResult2.TabIndex = 6;
            // 
            // lblResult3
            // 
            this.lblResult3.Location = new System.Drawing.Point(19, 93);
            this.lblResult3.Name = "lblResult3";
            this.lblResult3.Size = new System.Drawing.Size(130, 13);
            this.lblResult3.TabIndex = 7;
            this.lblResult3.Text = "res3";
            // 
            // cBResult3
            // 
            this.cBResult3.FormattingEnabled = true;
            this.cBResult3.Location = new System.Drawing.Point(159, 90);
            this.cBResult3.Name = "cBResult3";
            this.cBResult3.Size = new System.Drawing.Size(120, 21);
            this.cBResult3.TabIndex = 8;
            // 
            // txtAnzahl
            // 
            this.txtAnzahl.Location = new System.Drawing.Point(159, 64);
            this.txtAnzahl.Name = "txtAnzahl";
            this.txtAnzahl.Size = new System.Drawing.Size(61, 20);
            this.txtAnzahl.TabIndex = 9;
            // 
            // cBSet4
            // 
            this.cBSet4.Location = new System.Drawing.Point(338, 63);
            this.cBSet4.Name = "cBSet4";
            this.cBSet4.Size = new System.Drawing.Size(30, 24);
            this.cBSet4.TabIndex = 10;
            this.cBSet4.Text = "4";
            this.cBSet4.UseVisualStyleBackColor = true;
            this.cBSet4.Click += new System.EventHandler(this.cBSet4_Click);
            // 
            // cBSet5
            // 
            this.cBSet5.Location = new System.Drawing.Point(374, 63);
            this.cBSet5.Name = "cBSet5";
            this.cBSet5.Size = new System.Drawing.Size(30, 24);
            this.cBSet5.TabIndex = 11;
            this.cBSet5.Text = "5";
            this.cBSet5.UseVisualStyleBackColor = true;
            this.cBSet5.Click += new System.EventHandler(this.cBSet5_Click);
            // 
            // cBSet6
            // 
            this.cBSet6.Location = new System.Drawing.Point(410, 63);
            this.cBSet6.Name = "cBSet6";
            this.cBSet6.Size = new System.Drawing.Size(30, 24);
            this.cBSet6.TabIndex = 12;
            this.cBSet6.Text = "10";
            this.cBSet6.UseVisualStyleBackColor = true;
            this.cBSet6.Click += new System.EventHandler(this.cBSet6_Click);
            // 
            // cBSet3
            // 
            this.cBSet3.Location = new System.Drawing.Point(302, 63);
            this.cBSet3.Name = "cBSet3";
            this.cBSet3.Size = new System.Drawing.Size(30, 24);
            this.cBSet3.TabIndex = 13;
            this.cBSet3.Text = "3";
            this.cBSet3.UseVisualStyleBackColor = true;
            this.cBSet3.Click += new System.EventHandler(this.cBSet3_Click);
            // 
            // cBSet2
            // 
            this.cBSet2.Location = new System.Drawing.Point(266, 63);
            this.cBSet2.Name = "cBSet2";
            this.cBSet2.Size = new System.Drawing.Size(30, 24);
            this.cBSet2.TabIndex = 14;
            this.cBSet2.Text = "2";
            this.cBSet2.UseVisualStyleBackColor = true;
            this.cBSet2.Click += new System.EventHandler(this.cBSet2_Click);
            // 
            // cBSet1
            // 
            this.cBSet1.Location = new System.Drawing.Point(230, 63);
            this.cBSet1.Name = "cBSet1";
            this.cBSet1.Size = new System.Drawing.Size(30, 24);
            this.cBSet1.TabIndex = 15;
            this.cBSet1.Text = "1";
            this.cBSet1.UseVisualStyleBackColor = true;
            this.cBSet1.Click += new System.EventHandler(this.cBSet1_Click);
            // 
            // checkedListBoxThangingForm
            // 
            this.checkedListBoxThangingForm.FormattingEnabled = true;
            this.checkedListBoxThangingForm.Location = new System.Drawing.Point(159, 117);
            this.checkedListBoxThangingForm.Name = "checkedListBoxThangingForm";
            this.checkedListBoxThangingForm.Size = new System.Drawing.Size(210, 79);
            this.checkedListBoxThangingForm.TabIndex = 16;
            // 
            // lblResult4
            // 
            this.lblResult4.Location = new System.Drawing.Point(19, 117);
            this.lblResult4.Name = "lblResult4";
            this.lblResult4.Size = new System.Drawing.Size(130, 13);
            this.lblResult4.TabIndex = 17;
            this.lblResult4.Text = "res3";
            // 
            // lBPreisBez
            // 
            this.lBPreisBez.Location = new System.Drawing.Point(375, 43);
            this.lBPreisBez.Name = "lBPreisBez";
            this.lBPreisBez.Size = new System.Drawing.Size(40, 13);
            this.lBPreisBez.TabIndex = 19;
            this.lBPreisBez.Text = "Netto";
            // 
            // txtPreis
            // 
            this.txtPreis.Location = new System.Drawing.Point(410, 38);
            this.txtPreis.Name = "txtPreis";
            this.txtPreis.ReadOnly = true;
            this.txtPreis.Size = new System.Drawing.Size(70, 20);
            this.txtPreis.TabIndex = 20;
            // 
            // txtResult5
            // 
            this.txtResult5.Location = new System.Drawing.Point(159, 202);
            this.txtResult5.Multiline = true;
            this.txtResult5.Name = "txtResult5";
            this.txtResult5.Size = new System.Drawing.Size(210, 55);
            this.txtResult5.TabIndex = 21;
            // 
            // lblResult5
            // 
            this.lblResult5.AutoSize = true;
            this.lblResult5.Location = new System.Drawing.Point(19, 202);
            this.lblResult5.Name = "lblResult5";
            this.lblResult5.Size = new System.Drawing.Size(27, 13);
            this.lblResult5.TabIndex = 22;
            this.lblResult5.Text = "res5";
            // 
            // secondCallDesiredGroupBox
            // 
            this.secondCallDesiredGroupBox.Controls.Add(this.secondCallUndesiredRadioButton);
            this.secondCallDesiredGroupBox.Controls.Add(this.secondCallDesiredRadioButton);
            this.secondCallDesiredGroupBox.Location = new System.Drawing.Point(105, 265);
            this.secondCallDesiredGroupBox.Name = "secondCallDesiredGroupBox";
            this.secondCallDesiredGroupBox.Size = new System.Drawing.Size(264, 44);
            this.secondCallDesiredGroupBox.TabIndex = 23;
            this.secondCallDesiredGroupBox.TabStop = false;
            this.secondCallDesiredGroupBox.Text = "Diese Adresse für einen Zweitanruf vorsehen?";
            // 
            // secondCallUndesiredRadioButton
            // 
            this.secondCallUndesiredRadioButton.AutoSize = true;
            this.secondCallUndesiredRadioButton.Location = new System.Drawing.Point(49, 20);
            this.secondCallUndesiredRadioButton.Name = "secondCallUndesiredRadioButton";
            this.secondCallUndesiredRadioButton.Size = new System.Drawing.Size(47, 17);
            this.secondCallUndesiredRadioButton.TabIndex = 1;
            this.secondCallUndesiredRadioButton.TabStop = true;
            this.secondCallUndesiredRadioButton.Text = "Nein";
            this.secondCallUndesiredRadioButton.UseVisualStyleBackColor = true;
            // 
            // secondCallDesiredRadioButton
            // 
            this.secondCallDesiredRadioButton.AutoSize = true;
            this.secondCallDesiredRadioButton.Location = new System.Drawing.Point(6, 19);
            this.secondCallDesiredRadioButton.Name = "secondCallDesiredRadioButton";
            this.secondCallDesiredRadioButton.Size = new System.Drawing.Size(36, 17);
            this.secondCallDesiredRadioButton.TabIndex = 0;
            this.secondCallDesiredRadioButton.TabStop = true;
            this.secondCallDesiredRadioButton.Text = "Ja";
            this.secondCallDesiredRadioButton.UseVisualStyleBackColor = true;
            // 
            // sendFaxButton
            // 
            this.sendFaxButton.Location = new System.Drawing.Point(286, 13);
            this.sendFaxButton.Name = "sendFaxButton";
            this.sendFaxButton.Size = new System.Drawing.Size(75, 23);
            this.sendFaxButton.TabIndex = 24;
            this.sendFaxButton.Text = "Fax senden";
            this.sendFaxButton.UseVisualStyleBackColor = true;
            this.sendFaxButton.Click += new System.EventHandler(this.sendFaxButton_Click);
            // 
            // DateTimePickerPaymentTarget
            // 
            this.DateTimePickerPaymentTarget.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DateTimePickerPaymentTarget.Location = new System.Drawing.Point(159, 90);
            this.DateTimePickerPaymentTarget.Name = "DateTimePickerPaymentTarget";
            this.DateTimePickerPaymentTarget.ShowCheckBox = true;
            this.DateTimePickerPaymentTarget.Size = new System.Drawing.Size(95, 20);
            this.DateTimePickerPaymentTarget.TabIndex = 36;
            // 
            // createReminder1
            // 
            this.createReminder1.BackColor = System.Drawing.Color.Honeydew;
            this.createReminder1.Location = new System.Drawing.Point(19, 65);
            this.createReminder1.Name = "createReminder1";
            this.createReminder1.Padding = new System.Windows.Forms.Padding(7);
            this.createReminder1.ReminderDateStart = new System.DateTime(2006, 8, 15, 14, 10, 0, 0);
            this.createReminder1.ReminderDateStop = new System.DateTime(2006, 8, 15, 14, 10, 0, 0);
            this.createReminder1.Size = new System.Drawing.Size(470, 194);
            this.createReminder1.TabIndex = 18;
            this.createReminder1.ValueChanged += new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateReminderHandler(this.createReminder1_ValueChanged);
            // 
            // CreateCallPossibleReminder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.Controls.Add(this.DateTimePickerPaymentTarget);
            this.Controls.Add(this.sendFaxButton);
            this.Controls.Add(this.secondCallDesiredGroupBox);
            this.Controls.Add(this.lblResult5);
            this.Controls.Add(this.txtResult5);
            this.Controls.Add(this.txtPreis);
            this.Controls.Add(this.lBPreisBez);
            this.Controls.Add(this.lblResult4);
            this.Controls.Add(this.checkedListBoxThangingForm);
            this.Controls.Add(this.cBSet1);
            this.Controls.Add(this.cBSet2);
            this.Controls.Add(this.cBSet3);
            this.Controls.Add(this.cBSet6);
            this.Controls.Add(this.cBSet5);
            this.Controls.Add(this.cBSet4);
            this.Controls.Add(this.txtAnzahl);
            this.Controls.Add(this.cBResult3);
            this.Controls.Add(this.lblResult3);
            this.Controls.Add(this.cBResult2);
            this.Controls.Add(this.lblResult2);
            this.Controls.Add(this.lblResult1);
            this.Controls.Add(this.cBResult1);
            this.Controls.Add(this.cBResult0);
            this.Controls.Add(this.lblTeilnahme);
            this.Controls.Add(this.createReminder1);
            this.Name = "CreateCallPossibleReminder";
            this.Size = new System.Drawing.Size(510, 321);
            this.Load += new System.EventHandler(this.CreateCallPossibleReminder_Load);
            this.secondCallDesiredGroupBox.ResumeLayout(false);
            this.secondCallDesiredGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.createReminder1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTeilnahme;
        private System.Windows.Forms.ComboBox cBResult0;
        private System.Windows.Forms.ComboBox cBResult1;
        private System.Windows.Forms.Label lblResult1;
        private System.Windows.Forms.Label lblResult2;
        private System.Windows.Forms.ComboBox cBResult2;
        private System.Windows.Forms.Label lblResult3;
        private System.Windows.Forms.ComboBox cBResult3;
        private System.Windows.Forms.TextBox txtAnzahl;
        private System.Windows.Forms.Button cBSet4;
        private System.Windows.Forms.Button cBSet5;
        private System.Windows.Forms.Button cBSet6;
        private System.Windows.Forms.Button cBSet3;
        private System.Windows.Forms.Button cBSet2;
        private System.Windows.Forms.Button cBSet1;
        private System.Windows.Forms.CheckedListBox checkedListBoxThangingForm;
        private System.Windows.Forms.Label lblResult4;
        private CreateReminder createReminder1;
        private System.Windows.Forms.Label lBPreisBez;
        private System.Windows.Forms.TextBox txtPreis;
        private System.Windows.Forms.TextBox txtResult5;
        private System.Windows.Forms.Label lblResult5;
        private System.Windows.Forms.GroupBox secondCallDesiredGroupBox;
        private System.Windows.Forms.RadioButton secondCallDesiredRadioButton;
        private System.Windows.Forms.RadioButton secondCallUndesiredRadioButton;
        private System.Windows.Forms.Button sendFaxButton;
        private System.Windows.Forms.DateTimePicker DateTimePickerPaymentTarget;
    }
}
