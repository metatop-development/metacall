namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class TeamEdit
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
            this.bindingSourceCallJobReminder = new System.Windows.Forms.BindingSource(this.components);
            this.groupBoxSetProjectList = new System.Windows.Forms.GroupBox();
            this.checkBoxCompleted = new System.Windows.Forms.CheckBox();
            this.checkBoxActiv = new System.Windows.Forms.CheckBox();
            this.tabPageReminder = new System.Windows.Forms.TabPage();
            this.tabPageProject = new System.Windows.Forms.TabPage();
            this.tabPageBasicData = new System.Windows.Forms.TabPage();
            this.checkBoxDeleted = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TextBoxBeschreibung = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.centerComboBox = new System.Windows.Forms.ComboBox();
            this.TextBoxBezeichnung = new System.Windows.Forms.TextBox();
            this.tabControlUser = new System.Windows.Forms.TabControl();
            this.tabPageUser = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCallJobReminder)).BeginInit();
            this.groupBoxSetProjectList.SuspendLayout();
            this.tabPageBasicData.SuspendLayout();
            this.tabControlUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(641, 323);
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
            this.cancelButton.Location = new System.Drawing.Point(738, 323);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Abbrechen";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // groupBoxSetProjectList
            // 
            this.groupBoxSetProjectList.Controls.Add(this.checkBoxCompleted);
            this.groupBoxSetProjectList.Controls.Add(this.checkBoxActiv);
            this.groupBoxSetProjectList.Location = new System.Drawing.Point(24, 310);
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
            this.tabPageReminder.Size = new System.Drawing.Size(789, 258);
            this.tabPageReminder.TabIndex = 3;
            this.tabPageReminder.Text = "Wiedervorlage";
            this.tabPageReminder.UseVisualStyleBackColor = true;
            // 
            // tabPageProject
            // 
            this.tabPageProject.Location = new System.Drawing.Point(4, 22);
            this.tabPageProject.Name = "tabPageProject";
            this.tabPageProject.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProject.Size = new System.Drawing.Size(789, 258);
            this.tabPageProject.TabIndex = 1;
            this.tabPageProject.Text = "Projekte";
            this.tabPageProject.UseVisualStyleBackColor = true;
            // 
            // tabPageBasicData
            // 
            this.tabPageBasicData.Controls.Add(this.checkBoxDeleted);
            this.tabPageBasicData.Controls.Add(this.label7);
            this.tabPageBasicData.Controls.Add(this.TextBoxBeschreibung);
            this.tabPageBasicData.Controls.Add(this.label2);
            this.tabPageBasicData.Controls.Add(this.label1);
            this.tabPageBasicData.Controls.Add(this.centerComboBox);
            this.tabPageBasicData.Controls.Add(this.TextBoxBezeichnung);
            this.tabPageBasicData.Location = new System.Drawing.Point(4, 22);
            this.tabPageBasicData.Name = "tabPageBasicData";
            this.tabPageBasicData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBasicData.Size = new System.Drawing.Size(789, 258);
            this.tabPageBasicData.TabIndex = 0;
            this.tabPageBasicData.Text = "Stammdaten";
            this.tabPageBasicData.UseVisualStyleBackColor = true;
            // 
            // checkBoxDeleted
            // 
            this.checkBoxDeleted.AutoSize = true;
            this.checkBoxDeleted.Location = new System.Drawing.Point(102, 95);
            this.checkBoxDeleted.Name = "checkBoxDeleted";
            this.checkBoxDeleted.Size = new System.Drawing.Size(75, 17);
            this.checkBoxDeleted.TabIndex = 45;
            this.checkBoxDeleted.Text = "deaktiviert";
            this.checkBoxDeleted.UseVisualStyleBackColor = true;
            this.checkBoxDeleted.Click += new System.EventHandler(this.checkBoxDeleted_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "Beschreibung";
            // 
            // TextBoxBeschreibung
            // 
            this.TextBoxBeschreibung.Location = new System.Drawing.Point(102, 42);
            this.TextBoxBeschreibung.Name = "TextBoxBeschreibung";
            this.TextBoxBeschreibung.Size = new System.Drawing.Size(400, 20);
            this.TextBoxBeschreibung.TabIndex = 37;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 72);
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
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Bezeichnung";
            // 
            // centerComboBox
            // 
            this.centerComboBox.FormattingEnabled = true;
            this.centerComboBox.Location = new System.Drawing.Point(102, 68);
            this.centerComboBox.Name = "centerComboBox";
            this.centerComboBox.Size = new System.Drawing.Size(121, 21);
            this.centerComboBox.TabIndex = 25;
            this.centerComboBox.SelectedIndexChanged += new System.EventHandler(this.centerComboBox_SelectedIndexChanged);
            // 
            // TextBoxBezeichnung
            // 
            this.TextBoxBezeichnung.Location = new System.Drawing.Point(102, 16);
            this.TextBoxBezeichnung.Name = "TextBoxBezeichnung";
            this.TextBoxBezeichnung.Size = new System.Drawing.Size(200, 20);
            this.TextBoxBezeichnung.TabIndex = 24;
            // 
            // tabControlUser
            // 
            this.tabControlUser.Controls.Add(this.tabPageBasicData);
            this.tabControlUser.Controls.Add(this.tabPageProject);
            this.tabControlUser.Controls.Add(this.tabPageReminder);
            this.tabControlUser.Controls.Add(this.tabPageUser);
            this.tabControlUser.Location = new System.Drawing.Point(20, 20);
            this.tabControlUser.Name = "tabControlUser";
            this.tabControlUser.SelectedIndex = 0;
            this.tabControlUser.Size = new System.Drawing.Size(797, 284);
            this.tabControlUser.TabIndex = 24;
            this.tabControlUser.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControlUser_Selected);
            // 
            // tabPageUser
            // 
            this.tabPageUser.Location = new System.Drawing.Point(4, 22);
            this.tabPageUser.Name = "tabPageUser";
            this.tabPageUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUser.Size = new System.Drawing.Size(789, 258);
            this.tabPageUser.TabIndex = 4;
            this.tabPageUser.Text = "Benutzer";
            this.tabPageUser.UseVisualStyleBackColor = true;
            // 
            // TeamEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 356);
            this.ControlBox = false;
            this.Controls.Add(this.groupBoxSetProjectList);
            this.Controls.Add(this.tabControlUser);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TeamEdit";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Team bearbeiten";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UserForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCallJobReminder)).EndInit();
            this.groupBoxSetProjectList.ResumeLayout(false);
            this.groupBoxSetProjectList.PerformLayout();
            this.tabPageBasicData.ResumeLayout(false);
            this.tabPageBasicData.PerformLayout();
            this.tabControlUser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.BindingSource bindingSourceCallJobReminder;
        private System.Windows.Forms.GroupBox groupBoxSetProjectList;
        private System.Windows.Forms.CheckBox checkBoxCompleted;
        private System.Windows.Forms.CheckBox checkBoxActiv;
        private System.Windows.Forms.TabPage tabPageReminder;
        private System.Windows.Forms.TabPage tabPageProject;
        private System.Windows.Forms.TabPage tabPageBasicData;
        private System.Windows.Forms.CheckBox checkBoxDeleted;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TextBoxBeschreibung;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox centerComboBox;
        private System.Windows.Forms.TextBox TextBoxBezeichnung;
        private System.Windows.Forms.TabControl tabControlUser;
        private System.Windows.Forms.TabPage tabPageUser;
    }
}