namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class PhoneView
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.lblUwe = new System.Windows.Forms.Label();
            this.lblUweDurring = new System.Windows.Forms.Label();
            this.dialButton = new System.Windows.Forms.Button();
            this.RealCallTimer = new System.Windows.Forms.Timer(this.components);
            this.createReminderUnsuitable1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateReminderUnsuitable();
            this.createReminderQuestion1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateReminderQuestion();
            this.createReminderDurringQuestion1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateReminderDurringQuestion();
            this.createCallNotice1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateCallNotice();
            this.createCallDurringNotice1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateCallDurringNotice();
            this.createCallPossibleReminder1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateCallPossibleReminder();
            this.customerInfo1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CustomerInfo();
            this.mwProjectInfo1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.mwProjectInfo();
            this.createContactTypeReminder1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateContactTypeReminder();
            this.createContactTypeDurringReminder1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateContactTypeDurringReminder();
            this.createReminder1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateReminder();
            this.historieInfo1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.HistorieInfo();
            this.invoiceInfo1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.InvoiceInfo();
            this.sponsorInfo1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.SponsorInfo();
            this.createReminderDurring1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateReminderDurring();
            this.createDurringClosure1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateDurringClosure();
            ((System.ComponentModel.ISupportInitialize)(this.createReminderQuestion1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.createReminderDurringQuestion1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.createCallNotice1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.createCallDurringNotice1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.createCallPossibleReminder1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.createContactTypeReminder1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.createContactTypeDurringReminder1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.createReminder1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.createReminderDurring1)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(607, 740);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "Beenden";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(688, 740);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Abbrechen";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // lblUwe
            // 
            this.lblUwe.BackColor = System.Drawing.Color.Honeydew;
            this.lblUwe.Location = new System.Drawing.Point(393, 4);
            this.lblUwe.Name = "lblUwe";
            this.lblUwe.Size = new System.Drawing.Size(20, 116);
            this.lblUwe.TabIndex = 12;
            this.lblUwe.Text = " ";
            this.lblUwe.Visible = false;
            // 
            // lblUweDurring
            // 
            this.lblUweDurring.BackColor = System.Drawing.Color.MistyRose;
            this.lblUweDurring.Location = new System.Drawing.Point(393, 4);
            this.lblUweDurring.Name = "lblUweDurring";
            this.lblUweDurring.Size = new System.Drawing.Size(20, 116);
            this.lblUweDurring.TabIndex = 12;
            this.lblUweDurring.Text = " ";
            this.lblUweDurring.Visible = false;
            // 
            // dialButton
            // 
            this.dialButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.dialButton.Location = new System.Drawing.Point(321, 740);
            this.dialButton.Name = "dialButton";
            this.dialButton.Size = new System.Drawing.Size(85, 23);
            this.dialButton.TabIndex = 13;
            this.dialButton.Text = "wählen";
            this.dialButton.UseVisualStyleBackColor = true;
            this.dialButton.Click += new System.EventHandler(this.dialButton_Click);
            // 
            // RealCallTimer
            // 
            this.RealCallTimer.Interval = 1000;
            this.RealCallTimer.Tick += new System.EventHandler(this.RealCallTimer_Tick);
            // 
            // createReminderUnsuitable1
            // 
            this.createReminderUnsuitable1.BackColor = System.Drawing.Color.Honeydew;
            this.createReminderUnsuitable1.Location = new System.Drawing.Point(383, 567);
            this.createReminderUnsuitable1.Name = "createReminderUnsuitable1";
            this.createReminderUnsuitable1.Size = new System.Drawing.Size(267, 100);
            this.createReminderUnsuitable1.TabIndex = 15;
            // 
            // createReminderQuestion1
            // 
            this.createReminderQuestion1.BackColor = System.Drawing.Color.Honeydew;
            this.createReminderQuestion1.Location = new System.Drawing.Point(331, 254);
            this.createReminderQuestion1.Name = "createReminderQuestion1";
            this.createReminderQuestion1.ReminderAnswer = null;
            this.createReminderQuestion1.Size = new System.Drawing.Size(391, 70);
            this.createReminderQuestion1.TabIndex = 14;
            // 
            // createReminderDurringQuestion1
            // 
            this.createReminderDurringQuestion1.BackColor = System.Drawing.Color.MistyRose;
            this.createReminderDurringQuestion1.Location = new System.Drawing.Point(331, 194);
            this.createReminderDurringQuestion1.Name = "createReminderDurringQuestion1";
            this.createReminderDurringQuestion1.ReminderAnswer = null;
            this.createReminderDurringQuestion1.Size = new System.Drawing.Size(391, 70);
            this.createReminderDurringQuestion1.TabIndex = 14;
            // 
            // createCallNotice1
            // 
            this.createCallNotice1.BackColor = System.Drawing.Color.Honeydew;
            this.createCallNotice1.Location = new System.Drawing.Point(319, 459);
            this.createCallNotice1.Name = "createCallNotice1";
            this.createCallNotice1.Notice = "";
            this.createCallNotice1.SelectedBranch = null;
            this.createCallNotice1.SelectedBranchGroup = null;
            this.createCallNotice1.Size = new System.Drawing.Size(390, 102);
            this.createCallNotice1.TabIndex = 11;
            // 
            // createCallDurringNotice1
            // 
            this.createCallDurringNotice1.BackColor = System.Drawing.Color.MistyRose;
            this.createCallDurringNotice1.Location = new System.Drawing.Point(319, 459);
            this.createCallDurringNotice1.MaximumSize = new System.Drawing.Size(1024, 153);
            this.createCallDurringNotice1.MinimumSize = new System.Drawing.Size(480, 115);
            this.createCallDurringNotice1.Name = "createCallDurringNotice1";
            this.createCallDurringNotice1.Size = new System.Drawing.Size(480, 115);
            this.createCallDurringNotice1.TabIndex = 11;
            // 
            // createCallPossibleReminder1
            // 
            this.createCallPossibleReminder1.BackColor = System.Drawing.Color.Honeydew;
            this.createCallPossibleReminder1.Call = null;
            this.createCallPossibleReminder1.Location = new System.Drawing.Point(331, 254);
            this.createCallPossibleReminder1.Name = "createCallPossibleReminder1";
            this.createCallPossibleReminder1.ReminderAnswer = "Der Sponsor wird Ihnen täglich zwischen 06:00 und 08:00 Uhr als Wiedervorlage ang" +
    "eboten";
            this.createCallPossibleReminder1.Size = new System.Drawing.Size(393, 275);
            this.createCallPossibleReminder1.TabIndex = 10;
            this.createCallPossibleReminder1.ResultChanged += new System.EventHandler(this.createCallPossibleReminder1_ResultChanged);
            // 
            // customerInfo1
            // 
            this.customerInfo1.BackColor = System.Drawing.Color.Lavender;
            this.customerInfo1.CollapsedSize = new System.Drawing.Size(350, 80);
            this.customerInfo1.ExpandedSize = new System.Drawing.Size(350, 75);
            this.customerInfo1.Location = new System.Drawing.Point(10, 457);
            this.customerInfo1.Name = "customerInfo1";
            this.customerInfo1.Size = new System.Drawing.Size(350, 75);
            this.customerInfo1.TabIndex = 3;
            // 
            // mwProjectInfo1
            // 
            this.mwProjectInfo1.BackColor = System.Drawing.Color.Lavender;
            this.mwProjectInfo1.CollapsedSize = new System.Drawing.Size(350, 75);
            this.mwProjectInfo1.ExpandedSize = new System.Drawing.Size(350, 285);
            this.mwProjectInfo1.Location = new System.Drawing.Point(10, 216);
            this.mwProjectInfo1.Name = "mwProjectInfo1";
            this.mwProjectInfo1.Size = new System.Drawing.Size(350, 285);
            this.mwProjectInfo1.TabIndex = 2;
            this.mwProjectInfo1.Venue = null;
            // 
            // createContactTypeReminder1
            // 
            this.createContactTypeReminder1.AlternativNummer = null;
            this.createContactTypeReminder1.BackColor = System.Drawing.Color.Honeydew;
            this.createContactTypeReminder1.Location = new System.Drawing.Point(349, 10);
            this.createContactTypeReminder1.MobilNummer = null;
            this.createContactTypeReminder1.Name = "createContactTypeReminder1";
            this.createContactTypeReminder1.Size = new System.Drawing.Size(395, 126);
            this.createContactTypeReminder1.TabIndex = 0;
            this.createContactTypeReminder1.TelefonNummer = null;
            // 
            // createContactTypeDurringReminder1
            // 
            this.createContactTypeDurringReminder1.AlternativNummer = null;
            this.createContactTypeDurringReminder1.BackColor = System.Drawing.Color.MistyRose;
            this.createContactTypeDurringReminder1.Location = new System.Drawing.Point(378, 10);
            this.createContactTypeDurringReminder1.MobilNummer = null;
            this.createContactTypeDurringReminder1.Name = "createContactTypeDurringReminder1";
            this.createContactTypeDurringReminder1.Size = new System.Drawing.Size(395, 126);
            this.createContactTypeDurringReminder1.TabIndex = 55;
            this.createContactTypeDurringReminder1.TelefonNummer = null;
            // 
            // createReminder1
            // 
            this.createReminder1.BackColor = System.Drawing.Color.Honeydew;
            this.createReminder1.Location = new System.Drawing.Point(331, 223);
            this.createReminder1.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.createReminder1.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.createReminder1.Name = "createReminder1";
            this.createReminder1.Padding = new System.Windows.Forms.Padding(7);
            this.createReminder1.ReminderDateStart = new System.DateTime(2006, 8, 15, 14, 10, 0, 0);
            this.createReminder1.ReminderDateStop = new System.DateTime(2006, 8, 15, 14, 10, 0, 0);
            this.createReminder1.ReminderTracking = metatop.Applications.metaCall.DataObjects.CallJobReminderTracking.ExactDateAndTime;
            this.createReminder1.Size = new System.Drawing.Size(395, 278);
            this.createReminder1.TabIndex = 5;
            this.createReminder1.ValueChanged += new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateReminderHandler(this.createReminder1_ValueChanged);
            // 
            // historieInfo1
            // 
            this.historieInfo1.BackColor = System.Drawing.Color.Lavender;
            this.historieInfo1.Location = new System.Drawing.Point(10, 599);
            this.historieInfo1.Name = "historieInfo1";
            this.historieInfo1.Size = new System.Drawing.Size(350, 100);
            this.historieInfo1.TabIndex = 4;
            // 
            // invoiceInfo1
            // 
            this.invoiceInfo1.BackColor = System.Drawing.Color.MistyRose;
            this.invoiceInfo1.Location = new System.Drawing.Point(319, 459);
            this.invoiceInfo1.MinimumSize = new System.Drawing.Size(521, 288);
            this.invoiceInfo1.Name = "invoiceInfo1";
            this.invoiceInfo1.Size = new System.Drawing.Size(521, 288);
            this.invoiceInfo1.TabIndex = 11;
            // 
            // sponsorInfo1
            // 
            this.sponsorInfo1.BackColor = System.Drawing.Color.Lavender;
            this.sponsorInfo1.CheckValidSponsorInformation = false;
            this.sponsorInfo1.CollapsedSize = new System.Drawing.Size(350, 178);
            this.sponsorInfo1.ExpandedSize = new System.Drawing.Size(350, 363);
            this.sponsorInfo1.Location = new System.Drawing.Point(10, 10);
            this.sponsorInfo1.Name = "sponsorInfo1";
            this.sponsorInfo1.Size = new System.Drawing.Size(350, 178);
            this.sponsorInfo1.TabIndex = 1;
            // 
            // createReminderDurring1
            // 
            this.createReminderDurring1.BackColor = System.Drawing.Color.MistyRose;
            this.createReminderDurring1.Location = new System.Drawing.Point(308, 175);
            this.createReminderDurring1.MinimumSize = new System.Drawing.Size(468, 194);
            this.createReminderDurring1.Name = "createReminderDurring1";
            this.createReminderDurring1.Padding = new System.Windows.Forms.Padding(7);
            this.createReminderDurring1.ReminderDateStart = new System.DateTime(2006, 8, 15, 14, 10, 0, 0);
            this.createReminderDurring1.ReminderDateStop = new System.DateTime(2006, 8, 15, 14, 10, 0, 0);
            this.createReminderDurring1.Size = new System.Drawing.Size(468, 278);
            this.createReminderDurring1.TabIndex = 5;
            this.createReminderDurring1.ValueChanged += new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateReminderDurringHandler(this.createReminderDurring1_ValueChanged);
            // 
            // createDurringClosure1
            // 
            this.createDurringClosure1.BackColor = System.Drawing.Color.MistyRose;
            this.createDurringClosure1.Location = new System.Drawing.Point(367, 143);
            this.createDurringClosure1.Name = "createDurringClosure1";
            this.createDurringClosure1.Size = new System.Drawing.Size(480, 60);
            this.createDurringClosure1.TabIndex = 56;
            // 
            // PhoneView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.createDurringClosure1);
            this.Controls.Add(this.createReminderUnsuitable1);
            this.Controls.Add(this.createReminderQuestion1);
            this.Controls.Add(this.createReminderDurringQuestion1);
            this.Controls.Add(this.dialButton);
            this.Controls.Add(this.lblUwe);
            this.Controls.Add(this.lblUweDurring);
            this.Controls.Add(this.createCallNotice1);
            this.Controls.Add(this.createCallDurringNotice1);
            this.Controls.Add(this.createCallPossibleReminder1);
            this.Controls.Add(this.customerInfo1);
            this.Controls.Add(this.mwProjectInfo1);
            this.Controls.Add(this.createContactTypeReminder1);
            this.Controls.Add(this.createContactTypeDurringReminder1);
            this.Controls.Add(this.createReminder1);
            this.Controls.Add(this.historieInfo1);
            this.Controls.Add(this.invoiceInfo1);
            this.Controls.Add(this.sponsorInfo1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.createReminderDurring1);
            this.Name = "PhoneView";
            this.Size = new System.Drawing.Size(776, 766);
            ((System.ComponentModel.ISupportInitialize)(this.createReminderQuestion1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.createReminderDurringQuestion1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.createCallNotice1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.createCallDurringNotice1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.createCallPossibleReminder1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.createContactTypeReminder1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.createContactTypeDurringReminder1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.createReminder1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.createReminderDurring1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private SponsorInfo sponsorInfo1;
        private CustomerInfo customerInfo1;
        private HistorieInfo historieInfo1;
        private InvoiceInfo invoiceInfo1;
        private CreateReminder createReminder1;
        private CreateReminderDurring createReminderDurring1;
        private CreateContactTypeReminder createContactTypeReminder1;
        private CreateContactTypeDurringReminder createContactTypeDurringReminder1;
        private mwProjectInfo mwProjectInfo1;
        private CreateCallPossibleReminder createCallPossibleReminder1;
        private CreateCallNotice createCallNotice1;
        private CreateCallDurringNotice createCallDurringNotice1;
        private System.Windows.Forms.Label lblUwe;
        private System.Windows.Forms.Label lblUweDurring;
        private System.Windows.Forms.Button dialButton;
        private CreateReminderQuestion createReminderQuestion1;
        private CreateReminderDurringQuestion createReminderDurringQuestion1;
        private CreateReminderUnsuitable createReminderUnsuitable1;
        private System.Windows.Forms.Timer RealCallTimer;
        private CreateDurringClosure createDurringClosure1;
    }
}
