namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class UserWorkTimeAdditionEdit
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
            this.tabPageBasicData = new System.Windows.Forms.TabPage();
            this.maskedTextBoxTo = new System.Windows.Forms.MaskedTextBox();
            this.MaskedTextBoxFrom = new System.Windows.Forms.MaskedTextBox();
            this.dateTimePickerWorkDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxNotice = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxConfirmed = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ComboBoxUserWorkTimeItem = new System.Windows.Forms.ComboBox();
            this.tabControlWorkTimeAddition = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCallJobReminder)).BeginInit();
            this.tabPageBasicData.SuspendLayout();
            this.tabControlWorkTimeAddition.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(646, 320);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Speichern";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(742, 320);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Abbrechen";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // tabPageBasicData
            // 
            this.tabPageBasicData.Controls.Add(this.maskedTextBoxTo);
            this.tabPageBasicData.Controls.Add(this.MaskedTextBoxFrom);
            this.tabPageBasicData.Controls.Add(this.dateTimePickerWorkDate);
            this.tabPageBasicData.Controls.Add(this.label4);
            this.tabPageBasicData.Controls.Add(this.textBoxNotice);
            this.tabPageBasicData.Controls.Add(this.label3);
            this.tabPageBasicData.Controls.Add(this.checkBoxConfirmed);
            this.tabPageBasicData.Controls.Add(this.label7);
            this.tabPageBasicData.Controls.Add(this.label2);
            this.tabPageBasicData.Controls.Add(this.label1);
            this.tabPageBasicData.Controls.Add(this.ComboBoxUserWorkTimeItem);
            this.tabPageBasicData.Location = new System.Drawing.Point(4, 22);
            this.tabPageBasicData.Name = "tabPageBasicData";
            this.tabPageBasicData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBasicData.Size = new System.Drawing.Size(789, 258);
            this.tabPageBasicData.TabIndex = 0;
            this.tabPageBasicData.Text = "Stammdaten";
            this.tabPageBasicData.UseVisualStyleBackColor = true;
            // 
            // maskedTextBoxTo
            // 
            this.maskedTextBoxTo.Location = new System.Drawing.Point(102, 65);
            this.maskedTextBoxTo.Mask = "00\\:00";
            this.maskedTextBoxTo.Name = "maskedTextBoxTo";
            this.maskedTextBoxTo.Size = new System.Drawing.Size(70, 20);
            this.maskedTextBoxTo.TabIndex = 2;
            this.maskedTextBoxTo.Text = "1600";
            this.maskedTextBoxTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MaskedTextBoxFrom
            // 
            this.MaskedTextBoxFrom.Location = new System.Drawing.Point(102, 40);
            this.MaskedTextBoxFrom.Mask = "00\\:00";
            this.MaskedTextBoxFrom.Name = "MaskedTextBoxFrom";
            this.MaskedTextBoxFrom.Size = new System.Drawing.Size(70, 20);
            this.MaskedTextBoxFrom.TabIndex = 1;
            this.MaskedTextBoxFrom.Text = "0800";
            this.MaskedTextBoxFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dateTimePickerWorkDate
            // 
            this.dateTimePickerWorkDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerWorkDate.Location = new System.Drawing.Point(102, 13);
            this.dateTimePickerWorkDate.Name = "dateTimePickerWorkDate";
            this.dateTimePickerWorkDate.Size = new System.Drawing.Size(100, 20);
            this.dateTimePickerWorkDate.TabIndex = 0;
            this.dateTimePickerWorkDate.Value = new System.DateTime(2007, 4, 21, 0, 0, 0, 0);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 146);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 49;
            this.label4.Text = "Notiz";
            // 
            // textBoxNotice
            // 
            this.textBoxNotice.Location = new System.Drawing.Point(102, 146);
            this.textBoxNotice.Multiline = true;
            this.textBoxNotice.Name = "textBoxNotice";
            this.textBoxNotice.Size = new System.Drawing.Size(666, 95);
            this.textBoxNotice.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 47;
            this.label3.Text = "Ende";
            // 
            // checkBoxConfirmed
            // 
            this.checkBoxConfirmed.AutoSize = true;
            this.checkBoxConfirmed.Location = new System.Drawing.Point(102, 121);
            this.checkBoxConfirmed.Name = "checkBoxConfirmed";
            this.checkBoxConfirmed.Size = new System.Drawing.Size(66, 17);
            this.checkBoxConfirmed.TabIndex = 4;
            this.checkBoxConfirmed.Text = "bestätigt";
            this.checkBoxConfirmed.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "Anfang";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Tätigkeit";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Datum";
            // 
            // ComboBoxUserWorkTimeItem
            // 
            this.ComboBoxUserWorkTimeItem.FormattingEnabled = true;
            this.ComboBoxUserWorkTimeItem.Location = new System.Drawing.Point(102, 94);
            this.ComboBoxUserWorkTimeItem.Name = "ComboBoxUserWorkTimeItem";
            this.ComboBoxUserWorkTimeItem.Size = new System.Drawing.Size(200, 21);
            this.ComboBoxUserWorkTimeItem.TabIndex = 3;
            this.ComboBoxUserWorkTimeItem.SelectedIndexChanged += new System.EventHandler(this.ComboBoxUserWorkTimeItem_SelectedIndexChanged);
            // 
            // tabControlWorkTimeAddition
            // 
            this.tabControlWorkTimeAddition.Controls.Add(this.tabPageBasicData);
            this.tabControlWorkTimeAddition.Location = new System.Drawing.Point(20, 20);
            this.tabControlWorkTimeAddition.Name = "tabControlWorkTimeAddition";
            this.tabControlWorkTimeAddition.SelectedIndex = 0;
            this.tabControlWorkTimeAddition.Size = new System.Drawing.Size(797, 284);
            this.tabControlWorkTimeAddition.TabIndex = 0;
            // 
            // UserWorkTimeAdditionEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 356);
            this.ControlBox = false;
            this.Controls.Add(this.tabControlWorkTimeAddition);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserWorkTimeAdditionEdit";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Arbeitszeit bearbeiten";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UserForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCallJobReminder)).EndInit();
            this.tabPageBasicData.ResumeLayout(false);
            this.tabPageBasicData.PerformLayout();
            this.tabControlWorkTimeAddition.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.BindingSource bindingSourceCallJobReminder;
        private System.Windows.Forms.TabPage tabPageBasicData;
        private System.Windows.Forms.CheckBox checkBoxConfirmed;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ComboBoxUserWorkTimeItem;
        private System.Windows.Forms.TabControl tabControlWorkTimeAddition;
        private System.Windows.Forms.DateTimePicker dateTimePickerWorkDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxNotice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxTo;
        private System.Windows.Forms.MaskedTextBox MaskedTextBoxFrom;
    }
}