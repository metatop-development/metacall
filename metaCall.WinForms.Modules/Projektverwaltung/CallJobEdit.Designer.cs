namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class CallJobEdit
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.sponsorTextBox = new System.Windows.Forms.TextBox();
            this.startDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.stopDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.iterationCounterNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.dialModeComboBox = new System.Windows.Forms.ComboBox();
            this.callJobGroupComboBox = new System.Windows.Forms.ComboBox();
            this.labelSponsor = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.statusComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.usersComboBox = new System.Windows.Forms.ComboBox();
            this.checkBoxTransferStartDate = new System.Windows.Forms.CheckBox();
            this.checkBoxTransferDialMode = new System.Windows.Forms.CheckBox();
            this.checkBoxTransferCallJobGroup = new System.Windows.Forms.CheckBox();
            this.checkBoxTransferUser = new System.Windows.Forms.CheckBox();
            this.checkBoxTransferIterationCounter = new System.Windows.Forms.CheckBox();
            this.checkBoxTransferStatus = new System.Windows.Forms.CheckBox();
            this.checkBoxTransferStopDate = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.iterationCounterNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // sponsorTextBox
            // 
            this.sponsorTextBox.Location = new System.Drawing.Point(120, 20);
            this.sponsorTextBox.Name = "sponsorTextBox";
            this.sponsorTextBox.ReadOnly = true;
            this.sponsorTextBox.Size = new System.Drawing.Size(288, 20);
            this.sponsorTextBox.TabIndex = 0;
            // 
            // startDateTimePicker
            // 
            this.startDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDateTimePicker.Location = new System.Drawing.Point(120, 46);
            this.startDateTimePicker.Name = "startDateTimePicker";
            this.startDateTimePicker.Size = new System.Drawing.Size(121, 20);
            this.startDateTimePicker.TabIndex = 1;
            this.startDateTimePicker.ValueChanged += new System.EventHandler(this.startDateTimePicker_ValueChanged);
            // 
            // stopDateTimePicker
            // 
            this.stopDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.stopDateTimePicker.Location = new System.Drawing.Point(120, 72);
            this.stopDateTimePicker.Name = "stopDateTimePicker";
            this.stopDateTimePicker.ShowCheckBox = true;
            this.stopDateTimePicker.Size = new System.Drawing.Size(121, 20);
            this.stopDateTimePicker.TabIndex = 2;
            this.stopDateTimePicker.ValueChanged += new System.EventHandler(this.stopDateTimePicker_ValueChanged);
            // 
            // iterationCounterNumericUpDown
            // 
            this.iterationCounterNumericUpDown.Location = new System.Drawing.Point(120, 124);
            this.iterationCounterNumericUpDown.Name = "iterationCounterNumericUpDown";
            this.iterationCounterNumericUpDown.Size = new System.Drawing.Size(57, 20);
            this.iterationCounterNumericUpDown.TabIndex = 4;
            this.iterationCounterNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.iterationCounterNumericUpDown.ValueChanged += new System.EventHandler(this.iterationCounterNumericUpDown_ValueChanged);
            // 
            // dialModeComboBox
            // 
            this.dialModeComboBox.FormattingEnabled = true;
            this.dialModeComboBox.Location = new System.Drawing.Point(120, 150);
            this.dialModeComboBox.Name = "dialModeComboBox";
            this.dialModeComboBox.Size = new System.Drawing.Size(121, 21);
            this.dialModeComboBox.TabIndex = 5;
            this.dialModeComboBox.Visible = false;
            this.dialModeComboBox.SelectedIndexChanged += new System.EventHandler(this.dialModeComboBox_SelectedIndexChanged);
            // 
            // callJobGroupComboBox
            // 
            this.callJobGroupComboBox.FormattingEnabled = true;
            this.callJobGroupComboBox.Location = new System.Drawing.Point(120, 176);
            this.callJobGroupComboBox.Name = "callJobGroupComboBox";
            this.callJobGroupComboBox.Size = new System.Drawing.Size(190, 21);
            this.callJobGroupComboBox.TabIndex = 6;
            this.callJobGroupComboBox.SelectedIndexChanged += new System.EventHandler(this.callJobGroupComboBox_SelectedIndexChanged);
            // 
            // labelSponsor
            // 
            this.labelSponsor.AutoSize = true;
            this.labelSponsor.Location = new System.Drawing.Point(20, 20);
            this.labelSponsor.Name = "labelSponsor";
            this.labelSponsor.Size = new System.Drawing.Size(46, 13);
            this.labelSponsor.TabIndex = 7;
            this.labelSponsor.Text = "Sponsor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Start";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Stop";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Anrufzähler";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Wählmodus";
            this.label5.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 176);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Anrufgruppe";
            // 
            // acceptButton
            // 
            this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.acceptButton.Location = new System.Drawing.Point(326, 171);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(82, 23);
            this.acceptButton.TabIndex = 13;
            this.acceptButton.Text = "Übernehmen";
            this.acceptButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(326, 200);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(82, 23);
            this.cancelButton.TabIndex = 14;
            this.cancelButton.Text = "Abbrechen";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // statusComboBox
            // 
            this.statusComboBox.FormattingEnabled = true;
            this.statusComboBox.Location = new System.Drawing.Point(120, 98);
            this.statusComboBox.Name = "statusComboBox";
            this.statusComboBox.Size = new System.Drawing.Size(121, 21);
            this.statusComboBox.TabIndex = 15;
            this.statusComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.statusComboBox_Validating);
            this.statusComboBox.SelectedIndexChanged += new System.EventHandler(this.statusComboBox_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Status";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 202);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Benutzer";
            // 
            // usersComboBox
            // 
            this.usersComboBox.FormattingEnabled = true;
            this.usersComboBox.Location = new System.Drawing.Point(120, 202);
            this.usersComboBox.Name = "usersComboBox";
            this.usersComboBox.Size = new System.Drawing.Size(190, 21);
            this.usersComboBox.TabIndex = 17;
            this.usersComboBox.SelectedIndexChanged += new System.EventHandler(this.usersComboBox_SelectedIndexChanged);
            // 
            // checkBoxTransferStartDate
            // 
            this.checkBoxTransferStartDate.AutoSize = true;
            this.checkBoxTransferStartDate.Location = new System.Drawing.Point(10, 45);
            this.checkBoxTransferStartDate.Name = "checkBoxTransferStartDate";
            this.checkBoxTransferStartDate.Size = new System.Drawing.Size(15, 14);
            this.checkBoxTransferStartDate.TabIndex = 19;
            this.checkBoxTransferStartDate.UseVisualStyleBackColor = true;
            // 
            // checkBoxTransferDialMode
            // 
            this.checkBoxTransferDialMode.AutoSize = true;
            this.checkBoxTransferDialMode.Location = new System.Drawing.Point(10, 149);
            this.checkBoxTransferDialMode.Name = "checkBoxTransferDialMode";
            this.checkBoxTransferDialMode.Size = new System.Drawing.Size(15, 14);
            this.checkBoxTransferDialMode.TabIndex = 20;
            this.checkBoxTransferDialMode.UseVisualStyleBackColor = true;
            this.checkBoxTransferDialMode.Visible = false;
            // 
            // checkBoxTransferCallJobGroup
            // 
            this.checkBoxTransferCallJobGroup.AutoSize = true;
            this.checkBoxTransferCallJobGroup.Location = new System.Drawing.Point(10, 175);
            this.checkBoxTransferCallJobGroup.Name = "checkBoxTransferCallJobGroup";
            this.checkBoxTransferCallJobGroup.Size = new System.Drawing.Size(15, 14);
            this.checkBoxTransferCallJobGroup.TabIndex = 21;
            this.checkBoxTransferCallJobGroup.UseVisualStyleBackColor = true;
            // 
            // checkBoxTransferUser
            // 
            this.checkBoxTransferUser.AutoSize = true;
            this.checkBoxTransferUser.Location = new System.Drawing.Point(10, 200);
            this.checkBoxTransferUser.Name = "checkBoxTransferUser";
            this.checkBoxTransferUser.Size = new System.Drawing.Size(15, 14);
            this.checkBoxTransferUser.TabIndex = 22;
            this.checkBoxTransferUser.UseVisualStyleBackColor = true;
            // 
            // checkBoxTransferIterationCounter
            // 
            this.checkBoxTransferIterationCounter.AutoSize = true;
            this.checkBoxTransferIterationCounter.Location = new System.Drawing.Point(10, 123);
            this.checkBoxTransferIterationCounter.Name = "checkBoxTransferIterationCounter";
            this.checkBoxTransferIterationCounter.Size = new System.Drawing.Size(15, 14);
            this.checkBoxTransferIterationCounter.TabIndex = 23;
            this.checkBoxTransferIterationCounter.UseVisualStyleBackColor = true;
            // 
            // checkBoxTransferStatus
            // 
            this.checkBoxTransferStatus.AutoSize = true;
            this.checkBoxTransferStatus.Location = new System.Drawing.Point(10, 97);
            this.checkBoxTransferStatus.Name = "checkBoxTransferStatus";
            this.checkBoxTransferStatus.Size = new System.Drawing.Size(15, 14);
            this.checkBoxTransferStatus.TabIndex = 24;
            this.checkBoxTransferStatus.UseVisualStyleBackColor = true;
            // 
            // checkBoxTransferStopDate
            // 
            this.checkBoxTransferStopDate.AutoSize = true;
            this.checkBoxTransferStopDate.Location = new System.Drawing.Point(10, 72);
            this.checkBoxTransferStopDate.Name = "checkBoxTransferStopDate";
            this.checkBoxTransferStopDate.Size = new System.Drawing.Size(15, 14);
            this.checkBoxTransferStopDate.TabIndex = 25;
            this.checkBoxTransferStopDate.UseVisualStyleBackColor = true;
            // 
            // CallJobEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(428, 242);
            this.ControlBox = false;
            this.Controls.Add(this.checkBoxTransferStopDate);
            this.Controls.Add(this.checkBoxTransferStatus);
            this.Controls.Add(this.checkBoxTransferIterationCounter);
            this.Controls.Add(this.checkBoxTransferUser);
            this.Controls.Add(this.checkBoxTransferCallJobGroup);
            this.Controls.Add(this.checkBoxTransferDialMode);
            this.Controls.Add(this.checkBoxTransferStartDate);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.usersComboBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.statusComboBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelSponsor);
            this.Controls.Add(this.callJobGroupComboBox);
            this.Controls.Add(this.dialModeComboBox);
            this.Controls.Add(this.iterationCounterNumericUpDown);
            this.Controls.Add(this.stopDateTimePicker);
            this.Controls.Add(this.startDateTimePicker);
            this.Controls.Add(this.sponsorTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CallJobEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CallJobEdit";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CallJobEdit_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.iterationCounterNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sponsorTextBox;
        private System.Windows.Forms.DateTimePicker startDateTimePicker;
        private System.Windows.Forms.DateTimePicker stopDateTimePicker;
        private System.Windows.Forms.NumericUpDown iterationCounterNumericUpDown;
        private System.Windows.Forms.ComboBox dialModeComboBox;
        private System.Windows.Forms.ComboBox callJobGroupComboBox;
        private System.Windows.Forms.Label labelSponsor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox statusComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox usersComboBox;
        private System.Windows.Forms.CheckBox checkBoxTransferStartDate;
        private System.Windows.Forms.CheckBox checkBoxTransferDialMode;
        private System.Windows.Forms.CheckBox checkBoxTransferCallJobGroup;
        private System.Windows.Forms.CheckBox checkBoxTransferUser;
        private System.Windows.Forms.CheckBox checkBoxTransferIterationCounter;
        private System.Windows.Forms.CheckBox checkBoxTransferStatus;
        private System.Windows.Forms.CheckBox checkBoxTransferStopDate;
    }
}