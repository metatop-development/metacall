namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class ProjectDocumentEdit
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
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.displayNameTextBox = new System.Windows.Forms.TextBox();
            this.categoryComboBox = new System.Windows.Forms.ComboBox();
            this.filenameTextBox = new System.Windows.Forms.TextBox();
            this.filenameButton = new System.Windows.Forms.Button();
            this.dateCreatedTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.packetSelectCheckBox = new System.Windows.Forms.CheckBox();
            this.packetSelectLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.acceptButton.Location = new System.Drawing.Point(391, 117);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(82, 23);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "Übernehmen";
            this.acceptButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(391, 147);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(82, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Abbrechen";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Anzeigename";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Kategorie";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Datei";
            // 
            // displayNameTextBox
            // 
            this.displayNameTextBox.Location = new System.Drawing.Point(90, 41);
            this.displayNameTextBox.Name = "displayNameTextBox";
            this.displayNameTextBox.Size = new System.Drawing.Size(195, 20);
            this.displayNameTextBox.TabIndex = 5;
            // 
            // categoryComboBox
            // 
            this.categoryComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.categoryComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.categoryComboBox.FormattingEnabled = true;
            this.categoryComboBox.Location = new System.Drawing.Point(90, 68);
            this.categoryComboBox.Name = "categoryComboBox";
            this.categoryComboBox.Size = new System.Drawing.Size(141, 21);
            this.categoryComboBox.TabIndex = 6;
            // 
            // filenameTextBox
            // 
            this.filenameTextBox.Location = new System.Drawing.Point(90, 96);
            this.filenameTextBox.Name = "filenameTextBox";
            this.filenameTextBox.Size = new System.Drawing.Size(195, 20);
            this.filenameTextBox.TabIndex = 7;
            // 
            // filenameButton
            // 
            this.filenameButton.Location = new System.Drawing.Point(292, 96);
            this.filenameButton.Name = "filenameButton";
            this.filenameButton.Size = new System.Drawing.Size(58, 23);
            this.filenameButton.TabIndex = 8;
            this.filenameButton.Text = "wählen...";
            this.filenameButton.UseVisualStyleBackColor = true;
            this.filenameButton.Click += new System.EventHandler(this.filenameButton_Click);
            // 
            // dateCreatedTextBox
            // 
            this.dateCreatedTextBox.Location = new System.Drawing.Point(90, 123);
            this.dateCreatedTextBox.Name = "dateCreatedTextBox";
            this.dateCreatedTextBox.ReadOnly = true;
            this.dateCreatedTextBox.Size = new System.Drawing.Size(100, 20);
            this.dateCreatedTextBox.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Erstelldatum";
            // 
            // packetSelectCheckBox
            // 
            this.packetSelectCheckBox.AutoSize = true;
            this.packetSelectCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.packetSelectCheckBox.Location = new System.Drawing.Point(216, 156);
            this.packetSelectCheckBox.Name = "packetSelectCheckBox";
            this.packetSelectCheckBox.Size = new System.Drawing.Size(15, 14);
            this.packetSelectCheckBox.TabIndex = 11;
            this.packetSelectCheckBox.UseVisualStyleBackColor = true;
            // 
            // packetSelectLabel
            // 
            this.packetSelectLabel.AutoSize = true;
            this.packetSelectLabel.Location = new System.Drawing.Point(12, 156);
            this.packetSelectLabel.Name = "packetSelectLabel";
            this.packetSelectLabel.Size = new System.Drawing.Size(177, 13);
            this.packetSelectLabel.TabIndex = 12;
            this.packetSelectLabel.Text = "Pflichtauswahl für Sponsorenpakete";
            // 
            // ProjectDocumentEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(485, 182);
            this.ControlBox = false;
            this.Controls.Add(this.packetSelectLabel);
            this.Controls.Add(this.packetSelectCheckBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateCreatedTextBox);
            this.Controls.Add(this.filenameButton);
            this.Controls.Add(this.filenameTextBox);
            this.Controls.Add(this.categoryComboBox);
            this.Controls.Add(this.displayNameTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ProjectDocumentEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Projektdokumente bearbeiten";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProjectDocumentEdit_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox displayNameTextBox;
        private System.Windows.Forms.ComboBox categoryComboBox;
        private System.Windows.Forms.TextBox filenameTextBox;
        private System.Windows.Forms.Button filenameButton;
        private System.Windows.Forms.TextBox dateCreatedTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox packetSelectCheckBox;
        private System.Windows.Forms.Label packetSelectLabel;
    }
}