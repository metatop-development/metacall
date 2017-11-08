namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class ReminderEdit
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
            this.comboBoxCenter = new System.Windows.Forms.ComboBox();
            this.comboBoxTeam = new System.Windows.Forms.ComboBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxAgent = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxProject = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxSponsor = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBoxStatus = new System.Windows.Forms.GroupBox();
            this.radioButtonStatusFinished = new System.Windows.Forms.RadioButton();
            this.radioButtonStatusOpen = new System.Windows.Forms.RadioButton();
            this.lblReminderAnswer = new System.Windows.Forms.Label();
            this.createReminder1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateReminder();
            this.groupBoxStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.createReminder1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxCenter
            // 
            this.comboBoxCenter.FormattingEnabled = true;
            this.comboBoxCenter.Location = new System.Drawing.Point(80, 20);
            this.comboBoxCenter.Name = "comboBoxCenter";
            this.comboBoxCenter.Size = new System.Drawing.Size(187, 21);
            this.comboBoxCenter.TabIndex = 1;
            this.comboBoxCenter.SelectedIndexChanged += new System.EventHandler(this.centerComboBox_SelectedIndexChanged);
            // 
            // comboBoxTeam
            // 
            this.comboBoxTeam.FormattingEnabled = true;
            this.comboBoxTeam.Location = new System.Drawing.Point(80, 46);
            this.comboBoxTeam.Name = "comboBoxTeam";
            this.comboBoxTeam.Size = new System.Drawing.Size(186, 21);
            this.comboBoxTeam.TabIndex = 2;
            this.comboBoxTeam.SelectedIndexChanged += new System.EventHandler(this.comboBoxTeam_SelectedIndexChanged);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(336, 380);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 9;
            this.saveButton.Text = "Speichern";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(435, 380);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Abbrechen";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Center";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Team";
            // 
            // comboBoxAgent
            // 
            this.comboBoxAgent.FormattingEnabled = true;
            this.comboBoxAgent.Location = new System.Drawing.Point(80, 72);
            this.comboBoxAgent.Name = "comboBoxAgent";
            this.comboBoxAgent.Size = new System.Drawing.Size(186, 21);
            this.comboBoxAgent.TabIndex = 3;
            this.comboBoxAgent.SelectedIndexChanged += new System.EventHandler(this.comboBoxAgent_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Agent";
            // 
            // comboBoxProject
            // 
            this.comboBoxProject.FormattingEnabled = true;
            this.comboBoxProject.Location = new System.Drawing.Point(80, 98);
            this.comboBoxProject.Name = "comboBoxProject";
            this.comboBoxProject.Size = new System.Drawing.Size(430, 21);
            this.comboBoxProject.TabIndex = 5;
            this.comboBoxProject.SelectedIndexChanged += new System.EventHandler(this.comboBoxProject_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Projekt";
            // 
            // comboBoxSponsor
            // 
            this.comboBoxSponsor.FormattingEnabled = true;
            this.comboBoxSponsor.Location = new System.Drawing.Point(80, 124);
            this.comboBoxSponsor.Name = "comboBoxSponsor";
            this.comboBoxSponsor.Size = new System.Drawing.Size(430, 21);
            this.comboBoxSponsor.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Sponsor";
            // 
            // groupBoxStatus
            // 
            this.groupBoxStatus.Controls.Add(this.radioButtonStatusFinished);
            this.groupBoxStatus.Controls.Add(this.radioButtonStatusOpen);
            this.groupBoxStatus.Location = new System.Drawing.Point(390, 20);
            this.groupBoxStatus.Name = "groupBoxStatus";
            this.groupBoxStatus.Size = new System.Drawing.Size(120, 72);
            this.groupBoxStatus.TabIndex = 7;
            this.groupBoxStatus.TabStop = false;
            this.groupBoxStatus.Text = "Status";
            // 
            // radioButtonStatusFinished
            // 
            this.radioButtonStatusFinished.AutoSize = true;
            this.radioButtonStatusFinished.Location = new System.Drawing.Point(27, 42);
            this.radioButtonStatusFinished.Name = "radioButtonStatusFinished";
            this.radioButtonStatusFinished.Size = new System.Drawing.Size(59, 17);
            this.radioButtonStatusFinished.TabIndex = 6;
            this.radioButtonStatusFinished.TabStop = true;
            this.radioButtonStatusFinished.Text = "erledigt";
            this.radioButtonStatusFinished.UseVisualStyleBackColor = true;
            // 
            // radioButtonStatusOpen
            // 
            this.radioButtonStatusOpen.AutoSize = true;
            this.radioButtonStatusOpen.Location = new System.Drawing.Point(27, 21);
            this.radioButtonStatusOpen.Name = "radioButtonStatusOpen";
            this.radioButtonStatusOpen.Size = new System.Drawing.Size(49, 17);
            this.radioButtonStatusOpen.TabIndex = 5;
            this.radioButtonStatusOpen.TabStop = true;
            this.radioButtonStatusOpen.Text = "offen";
            this.radioButtonStatusOpen.UseVisualStyleBackColor = true;
            // 
            // lblReminderAnswer
            // 
            this.lblReminderAnswer.AutoSize = true;
            this.lblReminderAnswer.Location = new System.Drawing.Point(80, 155);
            this.lblReminderAnswer.Name = "lblReminderAnswer";
            this.lblReminderAnswer.Size = new System.Drawing.Size(35, 13);
            this.lblReminderAnswer.TabIndex = 24;
            this.lblReminderAnswer.Text = "label6";
            // 
            // createReminder1
            // 
            this.createReminder1.BackColor = System.Drawing.Color.Transparent;
            this.createReminder1.Location = new System.Drawing.Point(43, 180);
            this.createReminder1.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.createReminder1.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.createReminder1.Name = "createReminder1";
            this.createReminder1.Padding = new System.Windows.Forms.Padding(7);
            this.createReminder1.ReminderDateStart = new System.DateTime(2006, 8, 15, 14, 10, 0, 0);
            this.createReminder1.ReminderDateStop = new System.DateTime(2006, 8, 15, 14, 10, 0, 0);
            this.createReminder1.ReminderTracking = metatop.Applications.metaCall.DataObjects.CallJobReminderTracking.ExactDateAndTime;
            this.createReminder1.Size = new System.Drawing.Size(468, 194);
            this.createReminder1.TabIndex = 8;
            this.createReminder1.ValueChanged += new metatop.Applications.metaCall.WinForms.Modules.Telefonie.CreateReminderHandler(this.createReminder1_ValueChanged);
            // 
            // ReminderEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(517, 414);
            this.ControlBox = false;
            this.Controls.Add(this.lblReminderAnswer);
            this.Controls.Add(this.groupBoxStatus);
            this.Controls.Add(this.comboBoxSponsor);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxProject);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxAgent);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.comboBoxTeam);
            this.Controls.Add(this.comboBoxCenter);
            this.Controls.Add(this.createReminder1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ReminderEdit";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wiedervorlage bearbeiten";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UserForm_FormClosed);
            this.Load += new System.EventHandler(this.ReminderEdit_Load);
            this.groupBoxStatus.ResumeLayout(false);
            this.groupBoxStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.createReminder1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telefonie.CreateReminder createReminder1;
        private System.Windows.Forms.ComboBox comboBoxCenter;
        private System.Windows.Forms.ComboBox comboBoxTeam;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxAgent;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxProject;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxSponsor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBoxStatus;
        private System.Windows.Forms.RadioButton radioButtonStatusFinished;
        private System.Windows.Forms.RadioButton radioButtonStatusOpen;
        private System.Windows.Forms.Label lblReminderAnswer;
    }
}