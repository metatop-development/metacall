namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class SendFaxDialog
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
            this.components = new System.ComponentModel.Container();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.projectInfoLabel = new System.Windows.Forms.Label();
            this.faxTemplateGroupBox = new System.Windows.Forms.GroupBox();
            this.eMailTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.printOutRadioButton = new System.Windows.Forms.RadioButton();
            this.printersComboBox = new System.Windows.Forms.ComboBox();
            this.eMailRadioButton = new System.Windows.Forms.RadioButton();
            this.faxRadioButton = new System.Windows.Forms.RadioButton();
            this.sponsorPacketGroupBox = new System.Windows.Forms.GroupBox();
            this.emailAreaGroupbox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.emailTemplateGroupBox = new System.Windows.Forms.GroupBox();
            this.AnredeTextBox = new System.Windows.Forms.TextBox();
            this.ProjektTextBox = new System.Windows.Forms.TextBox();
            this.reminderFaxSelectionLabel = new System.Windows.Forms.Label();
            this.faxLabelTimer = new System.Windows.Forms.Timer(this.components);
            this.faxNumberTextBox = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.PhoneNumberTextBox();
            this.groupBox1.SuspendLayout();
            this.emailAreaGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(360, 557);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "Senden";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(452, 557);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Schließen";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // projectInfoLabel
            // 
            this.projectInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectInfoLabel.Location = new System.Drawing.Point(13, 13);
            this.projectInfoLabel.Name = "projectInfoLabel";
            this.projectInfoLabel.Size = new System.Drawing.Size(512, 42);
            this.projectInfoLabel.TabIndex = 2;
            this.projectInfoLabel.Text = "projectInfoLabel";
            // 
            // faxTemplateGroupBox
            // 
            this.faxTemplateGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.faxTemplateGroupBox.Location = new System.Drawing.Point(18, 359);
            this.faxTemplateGroupBox.Name = "faxTemplateGroupBox";
            this.faxTemplateGroupBox.Size = new System.Drawing.Size(506, 88);
            this.faxTemplateGroupBox.TabIndex = 1;
            this.faxTemplateGroupBox.TabStop = false;
            this.faxTemplateGroupBox.Text = "Faxvorlage";
            // 
            // eMailTextBox
            // 
            this.eMailTextBox.Location = new System.Drawing.Point(92, 37);
            this.eMailTextBox.Name = "eMailTextBox";
            this.eMailTextBox.Size = new System.Drawing.Size(213, 20);
            this.eMailTextBox.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.printOutRadioButton);
            this.groupBox1.Controls.Add(this.printersComboBox);
            this.groupBox1.Controls.Add(this.eMailRadioButton);
            this.groupBox1.Controls.Add(this.faxRadioButton);
            this.groupBox1.Controls.Add(this.eMailTextBox);
            this.groupBox1.Controls.Add(this.faxNumberTextBox);
            this.groupBox1.Location = new System.Drawing.Point(18, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(506, 100);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // printOutRadioButton
            // 
            this.printOutRadioButton.AutoSize = true;
            this.printOutRadioButton.Location = new System.Drawing.Point(7, 65);
            this.printOutRadioButton.Name = "printOutRadioButton";
            this.printOutRadioButton.Size = new System.Drawing.Size(64, 17);
            this.printOutRadioButton.TabIndex = 10;
            this.printOutRadioButton.TabStop = true;
            this.printOutRadioButton.Text = "drucken";
            this.printOutRadioButton.UseVisualStyleBackColor = true;
            this.printOutRadioButton.CheckedChanged += new System.EventHandler(this.MessageTransferMode_CheckedChanged);
            // 
            // printersComboBox
            // 
            this.printersComboBox.FormattingEnabled = true;
            this.printersComboBox.Location = new System.Drawing.Point(92, 64);
            this.printersComboBox.Name = "printersComboBox";
            this.printersComboBox.Size = new System.Drawing.Size(213, 21);
            this.printersComboBox.TabIndex = 9;
            // 
            // eMailRadioButton
            // 
            this.eMailRadioButton.AutoSize = true;
            this.eMailRadioButton.Location = new System.Drawing.Point(7, 38);
            this.eMailRadioButton.Name = "eMailRadioButton";
            this.eMailRadioButton.Size = new System.Drawing.Size(51, 17);
            this.eMailRadioButton.TabIndex = 8;
            this.eMailRadioButton.TabStop = true;
            this.eMailRadioButton.Text = "EMail";
            this.eMailRadioButton.UseVisualStyleBackColor = true;
            this.eMailRadioButton.CheckedChanged += new System.EventHandler(this.MessageTransferMode_CheckedChanged);
            // 
            // faxRadioButton
            // 
            this.faxRadioButton.AutoSize = true;
            this.faxRadioButton.Location = new System.Drawing.Point(7, 12);
            this.faxRadioButton.Name = "faxRadioButton";
            this.faxRadioButton.Size = new System.Drawing.Size(42, 17);
            this.faxRadioButton.TabIndex = 7;
            this.faxRadioButton.TabStop = true;
            this.faxRadioButton.Text = "Fax";
            this.faxRadioButton.UseVisualStyleBackColor = true;
            this.faxRadioButton.CheckedChanged += new System.EventHandler(this.MessageTransferMode_CheckedChanged);
            // 
            // sponsorPacketGroupBox
            // 
            this.sponsorPacketGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sponsorPacketGroupBox.Location = new System.Drawing.Point(18, 453);
            this.sponsorPacketGroupBox.Name = "sponsorPacketGroupBox";
            this.sponsorPacketGroupBox.Size = new System.Drawing.Size(506, 98);
            this.sponsorPacketGroupBox.TabIndex = 8;
            this.sponsorPacketGroupBox.TabStop = false;
            this.sponsorPacketGroupBox.Text = "Sponsorenpakete";
            // 
            // emailAreaGroupbox
            // 
            this.emailAreaGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emailAreaGroupbox.Controls.Add(this.label2);
            this.emailAreaGroupbox.Controls.Add(this.label1);
            this.emailAreaGroupbox.Controls.Add(this.emailTemplateGroupBox);
            this.emailAreaGroupbox.Controls.Add(this.AnredeTextBox);
            this.emailAreaGroupbox.Controls.Add(this.ProjektTextBox);
            this.emailAreaGroupbox.Location = new System.Drawing.Point(18, 159);
            this.emailAreaGroupbox.Name = "emailAreaGroupbox";
            this.emailAreaGroupbox.Size = new System.Drawing.Size(506, 170);
            this.emailAreaGroupbox.TabIndex = 13;
            this.emailAreaGroupbox.TabStop = false;
            this.emailAreaGroupbox.Text = "Emailvorlage";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Briefanrede:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Betreff:";
            // 
            // emailTemplateGroupBox
            // 
            this.emailTemplateGroupBox.Location = new System.Drawing.Point(7, 73);
            this.emailTemplateGroupBox.Name = "emailTemplateGroupBox";
            this.emailTemplateGroupBox.Size = new System.Drawing.Size(493, 92);
            this.emailTemplateGroupBox.TabIndex = 17;
            this.emailTemplateGroupBox.TabStop = false;
            // 
            // AnredeTextBox
            // 
            this.AnredeTextBox.Location = new System.Drawing.Point(92, 47);
            this.AnredeTextBox.Name = "AnredeTextBox";
            this.AnredeTextBox.Size = new System.Drawing.Size(408, 20);
            this.AnredeTextBox.TabIndex = 16;
            this.AnredeTextBox.Leave += new System.EventHandler(this.AnredeTextBox_Leave);
            // 
            // ProjektTextBox
            // 
            this.ProjektTextBox.Location = new System.Drawing.Point(92, 21);
            this.ProjektTextBox.Name = "ProjektTextBox";
            this.ProjektTextBox.Size = new System.Drawing.Size(408, 20);
            this.ProjektTextBox.TabIndex = 15;
            // 
            // reminderFaxSelectionLabel
            // 
            this.reminderFaxSelectionLabel.AutoSize = true;
            this.reminderFaxSelectionLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.reminderFaxSelectionLabel.Location = new System.Drawing.Point(107, 343);
            this.reminderFaxSelectionLabel.Name = "reminderFaxSelectionLabel";
            this.reminderFaxSelectionLabel.Size = new System.Drawing.Size(303, 13);
            this.reminderFaxSelectionLabel.TabIndex = 14;
            this.reminderFaxSelectionLabel.Text = "Bitte wählen Sie den passenden Anhang zu Ihrer Emailvorlage!";
            // 
            // faxLabelTimer
            // 
            this.faxLabelTimer.Tick += new System.EventHandler(this.faxLabelTimer_Tick);
            // 
            // faxNumberTextBox
            // 
            this.faxNumberTextBox.Location = new System.Drawing.Point(92, 11);
            this.faxNumberTextBox.Name = "faxNumberTextBox";
            this.faxNumberTextBox.Size = new System.Drawing.Size(213, 20);
            this.faxNumberTextBox.TabIndex = 0;
            // 
            // SendFaxDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 585);
            this.Controls.Add(this.reminderFaxSelectionLabel);
            this.Controls.Add(this.emailAreaGroupbox);
            this.Controls.Add(this.sponsorPacketGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.faxTemplateGroupBox);
            this.Controls.Add(this.projectInfoLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendFaxDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fax versenden";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SendFaxDialog_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.emailAreaGroupbox.ResumeLayout(false);
            this.emailAreaGroupbox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label projectInfoLabel;
        private PhoneNumberTextBox faxNumberTextBox;
        private System.Windows.Forms.GroupBox faxTemplateGroupBox;
        private System.Windows.Forms.TextBox eMailTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton eMailRadioButton;
        private System.Windows.Forms.RadioButton faxRadioButton;
        private System.Windows.Forms.RadioButton printOutRadioButton;
        private System.Windows.Forms.ComboBox printersComboBox;
        private System.Windows.Forms.GroupBox sponsorPacketGroupBox;
        private System.Windows.Forms.GroupBox emailAreaGroupbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox emailTemplateGroupBox;
        private System.Windows.Forms.TextBox AnredeTextBox;
        private System.Windows.Forms.TextBox ProjektTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label reminderFaxSelectionLabel;
        private System.Windows.Forms.Timer faxLabelTimer;
    }
}