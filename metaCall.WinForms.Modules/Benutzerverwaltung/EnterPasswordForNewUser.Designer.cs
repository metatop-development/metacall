namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class EnterPasswordForNewUser
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
            this.firstPasswordTextBox = new System.Windows.Forms.TextBox();
            this.secondPasswordTextbox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // firstPasswordTextBox
            // 
            this.firstPasswordTextBox.Location = new System.Drawing.Point(136, 12);
            this.firstPasswordTextBox.Name = "firstPasswordTextBox";
            this.firstPasswordTextBox.PasswordChar = '*';
            this.firstPasswordTextBox.Size = new System.Drawing.Size(111, 20);
            this.firstPasswordTextBox.TabIndex = 0;
            // 
            // secondPasswordTextbox
            // 
            this.secondPasswordTextbox.Location = new System.Drawing.Point(136, 38);
            this.secondPasswordTextbox.Name = "secondPasswordTextbox";
            this.secondPasswordTextbox.PasswordChar = '*';
            this.secondPasswordTextbox.Size = new System.Drawing.Size(111, 20);
            this.secondPasswordTextbox.TabIndex = 1;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(285, 9);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(86, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "Übernehmen";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(285, 35);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(86, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Abbrechen";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Passwort";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Passwort wiederholen";
            // 
            // EnterPasswordForNewUser
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(401, 74);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.secondPasswordTextbox);
            this.Controls.Add(this.firstPasswordTextBox);
            this.Name = "EnterPasswordForNewUser";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Passworteingabe";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EnterPasswordForNewUser_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox firstPasswordTextBox;
        private System.Windows.Forms.TextBox secondPasswordTextbox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}