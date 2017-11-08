namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class UserForm
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
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.bindingSourceCallJobReminder = new System.Windows.Forms.BindingSource(this.components);
            this.groupBoxSetProjectList = new System.Windows.Forms.GroupBox();
            this.checkBoxCompleted = new System.Windows.Forms.CheckBox();
            this.checkBoxActiv = new System.Windows.Forms.CheckBox();
            this.tabPageReminder = new System.Windows.Forms.TabPage();
            this.tabPageProject = new System.Windows.Forms.TabPage();
            this.tabPageBasicData = new System.Windows.Forms.TabPage();
            this.anmeldungEmailgroupBox = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.additionalInfo2TextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.additionalInfo1TextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.passwordEmailButton = new System.Windows.Forms.Button();
            this.anmeldungEmailTextBox = new System.Windows.Forms.TextBox();
            this.checkBoxDunning = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dialModeComboBox = new System.Windows.Forms.ComboBox();
            this.buttonSetPassWord = new System.Windows.Forms.Button();
            this.checkBoxWorkingTimeEditPermit = new System.Windows.Forms.CheckBox();
            this.checkBoxDeleted = new System.Windows.Forms.CheckBox();
            this.checkBoxProjectSearchPermit = new System.Windows.Forms.CheckBox();
            this.checkBoxReminderEditPermit = new System.Windows.Forms.CheckBox();
            this.signatureFileTextBox = new System.Windows.Forms.TextBox();
            this.selectSignatureFileButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.mwUserComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.partnerNummerTextBox = new System.Windows.Forms.TextBox();
            this.nachNameTextBox = new System.Windows.Forms.TextBox();
            this.vornameTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.isTeamleiterCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rolesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.teamComboBox = new System.Windows.Forms.ComboBox();
            this.centerComboBox = new System.Windows.Forms.ComboBox();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.tabControlUser = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCallJobReminder)).BeginInit();
            this.groupBoxSetProjectList.SuspendLayout();
            this.tabPageBasicData.SuspendLayout();
            this.anmeldungEmailgroupBox.SuspendLayout();
            this.tabControlUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(745, 417);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Speichern";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(833, 417);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Abbrechen";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBoxSetProjectList
            // 
            this.groupBoxSetProjectList.Controls.Add(this.checkBoxCompleted);
            this.groupBoxSetProjectList.Controls.Add(this.checkBoxActiv);
            this.groupBoxSetProjectList.Location = new System.Drawing.Point(20, 397);
            this.groupBoxSetProjectList.Name = "groupBoxSetProjectList";
            this.groupBoxSetProjectList.Size = new System.Drawing.Size(164, 43);
            this.groupBoxSetProjectList.TabIndex = 25;
            this.groupBoxSetProjectList.TabStop = false;
            this.groupBoxSetProjectList.Text = "Projekte";
            // 
            // checkBoxCompleted
            // 
            this.checkBoxCompleted.AutoSize = true;
            this.checkBoxCompleted.Location = new System.Drawing.Point(65, 17);
            this.checkBoxCompleted.Name = "checkBoxCompleted";
            this.checkBoxCompleted.Size = new System.Drawing.Size(98, 17);
            this.checkBoxCompleted.TabIndex = 1;
            this.checkBoxCompleted.Text = "Abgeschlossen";
            this.checkBoxCompleted.UseVisualStyleBackColor = true;
            this.checkBoxCompleted.Click += new System.EventHandler(this.checkBoxCompleted_Click);
            // 
            // checkBoxActiv
            // 
            this.checkBoxActiv.AutoSize = true;
            this.checkBoxActiv.Location = new System.Drawing.Point(9, 17);
            this.checkBoxActiv.Name = "checkBoxActiv";
            this.checkBoxActiv.Size = new System.Drawing.Size(50, 17);
            this.checkBoxActiv.TabIndex = 0;
            this.checkBoxActiv.Text = "Aktiv";
            this.checkBoxActiv.UseVisualStyleBackColor = true;
            this.checkBoxActiv.Click += new System.EventHandler(this.checkBoxActiv_Click);
            // 
            // tabPageReminder
            // 
            this.tabPageReminder.Location = new System.Drawing.Point(4, 22);
            this.tabPageReminder.Name = "tabPageReminder";
            this.tabPageReminder.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReminder.Size = new System.Drawing.Size(894, 345);
            this.tabPageReminder.TabIndex = 3;
            this.tabPageReminder.Text = "Wiedervorlage";
            this.tabPageReminder.UseVisualStyleBackColor = true;
            // 
            // tabPageProject
            // 
            this.tabPageProject.Location = new System.Drawing.Point(4, 22);
            this.tabPageProject.Name = "tabPageProject";
            this.tabPageProject.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProject.Size = new System.Drawing.Size(894, 345);
            this.tabPageProject.TabIndex = 1;
            this.tabPageProject.Text = "Projekte";
            this.tabPageProject.UseVisualStyleBackColor = true;
            // 
            // tabPageBasicData
            // 
            this.tabPageBasicData.Controls.Add(this.anmeldungEmailgroupBox);
            this.tabPageBasicData.Controls.Add(this.checkBoxDunning);
            this.tabPageBasicData.Controls.Add(this.label9);
            this.tabPageBasicData.Controls.Add(this.dialModeComboBox);
            this.tabPageBasicData.Controls.Add(this.buttonSetPassWord);
            this.tabPageBasicData.Controls.Add(this.checkBoxWorkingTimeEditPermit);
            this.tabPageBasicData.Controls.Add(this.checkBoxDeleted);
            this.tabPageBasicData.Controls.Add(this.checkBoxProjectSearchPermit);
            this.tabPageBasicData.Controls.Add(this.checkBoxReminderEditPermit);
            this.tabPageBasicData.Controls.Add(this.signatureFileTextBox);
            this.tabPageBasicData.Controls.Add(this.selectSignatureFileButton);
            this.tabPageBasicData.Controls.Add(this.label8);
            this.tabPageBasicData.Controls.Add(this.mwUserComboBox);
            this.tabPageBasicData.Controls.Add(this.label7);
            this.tabPageBasicData.Controls.Add(this.partnerNummerTextBox);
            this.tabPageBasicData.Controls.Add(this.nachNameTextBox);
            this.tabPageBasicData.Controls.Add(this.vornameTextBox);
            this.tabPageBasicData.Controls.Add(this.label6);
            this.tabPageBasicData.Controls.Add(this.label5);
            this.tabPageBasicData.Controls.Add(this.isTeamleiterCheckBox);
            this.tabPageBasicData.Controls.Add(this.label4);
            this.tabPageBasicData.Controls.Add(this.rolesCheckedListBox);
            this.tabPageBasicData.Controls.Add(this.label3);
            this.tabPageBasicData.Controls.Add(this.label2);
            this.tabPageBasicData.Controls.Add(this.label1);
            this.tabPageBasicData.Controls.Add(this.teamComboBox);
            this.tabPageBasicData.Controls.Add(this.centerComboBox);
            this.tabPageBasicData.Controls.Add(this.userNameTextBox);
            this.tabPageBasicData.Location = new System.Drawing.Point(4, 22);
            this.tabPageBasicData.Name = "tabPageBasicData";
            this.tabPageBasicData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBasicData.Size = new System.Drawing.Size(894, 345);
            this.tabPageBasicData.TabIndex = 0;
            this.tabPageBasicData.Text = "Stammdaten";
            this.tabPageBasicData.UseVisualStyleBackColor = true;
            // 
            // anmeldungEmailgroupBox
            // 
            this.anmeldungEmailgroupBox.Controls.Add(this.label12);
            this.anmeldungEmailgroupBox.Controls.Add(this.additionalInfo2TextBox);
            this.anmeldungEmailgroupBox.Controls.Add(this.label11);
            this.anmeldungEmailgroupBox.Controls.Add(this.additionalInfo1TextBox);
            this.anmeldungEmailgroupBox.Controls.Add(this.label10);
            this.anmeldungEmailgroupBox.Controls.Add(this.passwordEmailButton);
            this.anmeldungEmailgroupBox.Controls.Add(this.anmeldungEmailTextBox);
            this.anmeldungEmailgroupBox.Location = new System.Drawing.Point(572, 150);
            this.anmeldungEmailgroupBox.Name = "anmeldungEmailgroupBox";
            this.anmeldungEmailgroupBox.Size = new System.Drawing.Size(295, 131);
            this.anmeldungEmailgroupBox.TabIndex = 50;
            this.anmeldungEmailgroupBox.TabStop = false;
            this.anmeldungEmailgroupBox.Text = "Anmeldung Email";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 101);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(66, 13);
            this.label12.TabIndex = 36;
            this.label12.Text = "ZusatzInfo 2";
            // 
            // additionalInfo2TextBox
            // 
            this.additionalInfo2TextBox.Location = new System.Drawing.Point(102, 98);
            this.additionalInfo2TextBox.Name = "additionalInfo2TextBox";
            this.additionalInfo2TextBox.Size = new System.Drawing.Size(187, 20);
            this.additionalInfo2TextBox.TabIndex = 35;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 77);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 13);
            this.label11.TabIndex = 34;
            this.label11.Text = "Zusatzinfo 1";
            // 
            // additionalInfo1TextBox
            // 
            this.additionalInfo1TextBox.Location = new System.Drawing.Point(102, 74);
            this.additionalInfo1TextBox.Name = "additionalInfo1TextBox";
            this.additionalInfo1TextBox.Size = new System.Drawing.Size(187, 20);
            this.additionalInfo1TextBox.TabIndex = 33;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 32;
            this.label10.Text = "Anmeldename";
            // 
            // passwordEmailButton
            // 
            this.passwordEmailButton.Location = new System.Drawing.Point(102, 43);
            this.passwordEmailButton.Name = "passwordEmailButton";
            this.passwordEmailButton.Size = new System.Drawing.Size(142, 25);
            this.passwordEmailButton.TabIndex = 15;
            this.passwordEmailButton.Text = "Passwort (Email)";
            this.passwordEmailButton.UseVisualStyleBackColor = true;
            this.passwordEmailButton.Click += new System.EventHandler(this.passwordEmailButton_Click);
            // 
            // anmeldungEmailTextBox
            // 
            this.anmeldungEmailTextBox.Location = new System.Drawing.Point(102, 17);
            this.anmeldungEmailTextBox.Name = "anmeldungEmailTextBox";
            this.anmeldungEmailTextBox.Size = new System.Drawing.Size(187, 20);
            this.anmeldungEmailTextBox.TabIndex = 0;
            // 
            // checkBoxDunning
            // 
            this.checkBoxDunning.AutoSize = true;
            this.checkBoxDunning.Location = new System.Drawing.Point(333, 122);
            this.checkBoxDunning.Name = "checkBoxDunning";
            this.checkBoxDunning.Size = new System.Drawing.Size(65, 17);
            this.checkBoxDunning.TabIndex = 12;
            this.checkBoxDunning.Text = "Mahnen";
            this.checkBoxDunning.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(569, 73);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 13);
            this.label9.TabIndex = 49;
            this.label9.Text = "Wählmodus";
            // 
            // dialModeComboBox
            // 
            this.dialModeComboBox.FormattingEnabled = true;
            this.dialModeComboBox.Location = new System.Drawing.Point(646, 69);
            this.dialModeComboBox.Name = "dialModeComboBox";
            this.dialModeComboBox.Size = new System.Drawing.Size(121, 21);
            this.dialModeComboBox.TabIndex = 15;
            this.dialModeComboBox.SelectedIndexChanged += new System.EventHandler(this.dialModeComboBox_SelectedIndexChanged);
            // 
            // buttonSetPassWord
            // 
            this.buttonSetPassWord.Location = new System.Drawing.Point(572, 17);
            this.buttonSetPassWord.Name = "buttonSetPassWord";
            this.buttonSetPassWord.Size = new System.Drawing.Size(142, 25);
            this.buttonSetPassWord.TabIndex = 14;
            this.buttonSetPassWord.Text = "Passwort (metaCall)";
            this.buttonSetPassWord.UseVisualStyleBackColor = true;
            this.buttonSetPassWord.Click += new System.EventHandler(this.buttonSetPassWord_Click);
            // 
            // checkBoxWorkingTimeEditPermit
            // 
            this.checkBoxWorkingTimeEditPermit.AutoSize = true;
            this.checkBoxWorkingTimeEditPermit.Location = new System.Drawing.Point(333, 96);
            this.checkBoxWorkingTimeEditPermit.Name = "checkBoxWorkingTimeEditPermit";
            this.checkBoxWorkingTimeEditPermit.Size = new System.Drawing.Size(94, 17);
            this.checkBoxWorkingTimeEditPermit.TabIndex = 11;
            this.checkBoxWorkingTimeEditPermit.Text = "Zeiteneingabe";
            this.checkBoxWorkingTimeEditPermit.UseVisualStyleBackColor = true;
            // 
            // checkBoxDeleted
            // 
            this.checkBoxDeleted.AutoSize = true;
            this.checkBoxDeleted.Location = new System.Drawing.Point(102, 122);
            this.checkBoxDeleted.Name = "checkBoxDeleted";
            this.checkBoxDeleted.Size = new System.Drawing.Size(99, 17);
            this.checkBoxDeleted.TabIndex = 4;
            this.checkBoxDeleted.Text = "Ausgeschieden";
            this.checkBoxDeleted.UseVisualStyleBackColor = true;
            this.checkBoxDeleted.Click += new System.EventHandler(this.checkBoxDeleted_Click);
            // 
            // checkBoxProjectSearchPermit
            // 
            this.checkBoxProjectSearchPermit.AutoSize = true;
            this.checkBoxProjectSearchPermit.Location = new System.Drawing.Point(333, 70);
            this.checkBoxProjectSearchPermit.Name = "checkBoxProjectSearchPermit";
            this.checkBoxProjectSearchPermit.Size = new System.Drawing.Size(137, 17);
            this.checkBoxProjectSearchPermit.TabIndex = 10;
            this.checkBoxProjectSearchPermit.Text = "Projektsponsorensuche";
            this.checkBoxProjectSearchPermit.UseVisualStyleBackColor = true;
            // 
            // checkBoxReminderEditPermit
            // 
            this.checkBoxReminderEditPermit.AutoSize = true;
            this.checkBoxReminderEditPermit.Location = new System.Drawing.Point(333, 46);
            this.checkBoxReminderEditPermit.Name = "checkBoxReminderEditPermit";
            this.checkBoxReminderEditPermit.Size = new System.Drawing.Size(154, 17);
            this.checkBoxReminderEditPermit.TabIndex = 9;
            this.checkBoxReminderEditPermit.Text = "Wiedervorlagen bearbeiten";
            this.checkBoxReminderEditPermit.UseVisualStyleBackColor = true;
            // 
            // signatureFileTextBox
            // 
            this.signatureFileTextBox.Location = new System.Drawing.Point(102, 308);
            this.signatureFileTextBox.Name = "signatureFileTextBox";
            this.signatureFileTextBox.ReadOnly = true;
            this.signatureFileTextBox.Size = new System.Drawing.Size(723, 20);
            this.signatureFileTextBox.TabIndex = 16;
            // 
            // selectSignatureFileButton
            // 
            this.selectSignatureFileButton.BackgroundImage = global::metatop.Applications.metaCall.WinForms.Modules.Properties.Resources.document_open;
            this.selectSignatureFileButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.selectSignatureFileButton.Location = new System.Drawing.Point(831, 302);
            this.selectSignatureFileButton.Name = "selectSignatureFileButton";
            this.selectSignatureFileButton.Size = new System.Drawing.Size(30, 30);
            this.selectSignatureFileButton.TabIndex = 17;
            this.selectSignatureFileButton.UseVisualStyleBackColor = true;
            this.selectSignatureFileButton.Click += new System.EventHandler(this.selectSignatureFileButton_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 308);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 40;
            this.label8.Text = "Unterschrift";
            // 
            // mwUserComboBox
            // 
            this.mwUserComboBox.FormattingEnabled = true;
            this.mwUserComboBox.Location = new System.Drawing.Point(330, 17);
            this.mwUserComboBox.Name = "mwUserComboBox";
            this.mwUserComboBox.Size = new System.Drawing.Size(200, 21);
            this.mwUserComboBox.TabIndex = 8;
            this.mwUserComboBox.SelectedIndexChanged += new System.EventHandler(this.mwUserComboBox_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "Partnernr.";
            // 
            // partnerNummerTextBox
            // 
            this.partnerNummerTextBox.Location = new System.Drawing.Point(102, 42);
            this.partnerNummerTextBox.Name = "partnerNummerTextBox";
            this.partnerNummerTextBox.Size = new System.Drawing.Size(79, 20);
            this.partnerNummerTextBox.TabIndex = 1;
            // 
            // nachNameTextBox
            // 
            this.nachNameTextBox.Location = new System.Drawing.Point(102, 94);
            this.nachNameTextBox.Name = "nachNameTextBox";
            this.nachNameTextBox.Size = new System.Drawing.Size(156, 20);
            this.nachNameTextBox.TabIndex = 3;
            // 
            // vornameTextBox
            // 
            this.vornameTextBox.Location = new System.Drawing.Point(102, 68);
            this.vornameTextBox.Name = "vornameTextBox";
            this.vornameTextBox.Size = new System.Drawing.Size(156, 20);
            this.vornameTextBox.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 98);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "Nachname";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "Vorname";
            // 
            // isTeamleiterCheckBox
            // 
            this.isTeamleiterCheckBox.AutoSize = true;
            this.isTeamleiterCheckBox.Location = new System.Drawing.Point(102, 212);
            this.isTeamleiterCheckBox.Name = "isTeamleiterCheckBox";
            this.isTeamleiterCheckBox.Size = new System.Drawing.Size(75, 17);
            this.isTeamleiterCheckBox.TabIndex = 7;
            this.isTeamleiterCheckBox.Text = "Teamleiter";
            this.isTeamleiterCheckBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(277, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "Gruppen";
            // 
            // rolesCheckedListBox
            // 
            this.rolesCheckedListBox.CheckOnClick = true;
            this.rolesCheckedListBox.FormattingEnabled = true;
            this.rolesCheckedListBox.Location = new System.Drawing.Point(330, 157);
            this.rolesCheckedListBox.Name = "rolesCheckedListBox";
            this.rolesCheckedListBox.Size = new System.Drawing.Size(200, 124);
            this.rolesCheckedListBox.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Team";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 150);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Center";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Anmeldename";
            // 
            // teamComboBox
            // 
            this.teamComboBox.FormattingEnabled = true;
            this.teamComboBox.Location = new System.Drawing.Point(102, 172);
            this.teamComboBox.Name = "teamComboBox";
            this.teamComboBox.Size = new System.Drawing.Size(121, 21);
            this.teamComboBox.TabIndex = 6;
            // 
            // centerComboBox
            // 
            this.centerComboBox.FormattingEnabled = true;
            this.centerComboBox.Location = new System.Drawing.Point(102, 146);
            this.centerComboBox.Name = "centerComboBox";
            this.centerComboBox.Size = new System.Drawing.Size(121, 21);
            this.centerComboBox.TabIndex = 5;
            this.centerComboBox.SelectedIndexChanged += new System.EventHandler(this.centerComboBox_SelectedIndexChanged);
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Location = new System.Drawing.Point(102, 16);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(156, 20);
            this.userNameTextBox.TabIndex = 0;
            // 
            // tabControlUser
            // 
            this.tabControlUser.Controls.Add(this.tabPageBasicData);
            this.tabControlUser.Controls.Add(this.tabPageReminder);
            this.tabControlUser.Controls.Add(this.tabPageProject);
            this.tabControlUser.Location = new System.Drawing.Point(20, 20);
            this.tabControlUser.Name = "tabControlUser";
            this.tabControlUser.SelectedIndex = 0;
            this.tabControlUser.Size = new System.Drawing.Size(902, 371);
            this.tabControlUser.TabIndex = 1;
            this.tabControlUser.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControlUser_Selected);
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 442);
            this.ControlBox = false;
            this.Controls.Add(this.groupBoxSetProjectList);
            this.Controls.Add(this.tabControlUser);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Benutzer bearbeiten";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UserForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCallJobReminder)).EndInit();
            this.groupBoxSetProjectList.ResumeLayout(false);
            this.groupBoxSetProjectList.PerformLayout();
            this.tabPageBasicData.ResumeLayout(false);
            this.tabPageBasicData.PerformLayout();
            this.anmeldungEmailgroupBox.ResumeLayout(false);
            this.anmeldungEmailgroupBox.PerformLayout();
            this.tabControlUser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.BindingSource bindingSourceCallJobReminder;
        private System.Windows.Forms.GroupBox groupBoxSetProjectList;
        private System.Windows.Forms.CheckBox checkBoxCompleted;
        private System.Windows.Forms.CheckBox checkBoxActiv;
        private System.Windows.Forms.TabPage tabPageReminder;
        private System.Windows.Forms.TabPage tabPageProject;
        private System.Windows.Forms.TabPage tabPageBasicData;
        private System.Windows.Forms.CheckBox checkBoxDeleted;
        private System.Windows.Forms.CheckBox checkBoxProjectSearchPermit;
        private System.Windows.Forms.CheckBox checkBoxReminderEditPermit;
        private System.Windows.Forms.TextBox signatureFileTextBox;
        private System.Windows.Forms.Button selectSignatureFileButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox mwUserComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox partnerNummerTextBox;
        private System.Windows.Forms.TextBox nachNameTextBox;
        private System.Windows.Forms.TextBox vornameTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox isTeamleiterCheckBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckedListBox rolesCheckedListBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox teamComboBox;
        private System.Windows.Forms.ComboBox centerComboBox;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.TabControl tabControlUser;
        private System.Windows.Forms.CheckBox checkBoxWorkingTimeEditPermit;
        private System.Windows.Forms.Button buttonSetPassWord;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox dialModeComboBox;
        private System.Windows.Forms.CheckBox checkBoxDunning;
        private System.Windows.Forms.GroupBox anmeldungEmailgroupBox;
        private System.Windows.Forms.TextBox anmeldungEmailTextBox;
        private System.Windows.Forms.Button passwordEmailButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox additionalInfo2TextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox additionalInfo1TextBox;
    }
}