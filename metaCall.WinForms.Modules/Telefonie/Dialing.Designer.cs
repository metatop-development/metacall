namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class Dialing
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
            this.phoneNumberTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dialHangUpButton = new System.Windows.Forms.Button();
            this.informationLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // phoneNumberTextBox
            // 
            this.phoneNumberTextBox.Location = new System.Drawing.Point(101, 44);
            this.phoneNumberTextBox.Name = "phoneNumberTextBox";
            this.phoneNumberTextBox.Size = new System.Drawing.Size(222, 20);
            this.phoneNumberTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Telefonnummer:";
            // 
            // dialHangUpButton
            // 
            this.dialHangUpButton.AutoSize = true;
            this.dialHangUpButton.Location = new System.Drawing.Point(101, 73);
            this.dialHangUpButton.Name = "dialHangUpButton";
            this.dialHangUpButton.Size = new System.Drawing.Size(141, 23);
            this.dialHangUpButton.TabIndex = 2;
            this.dialHangUpButton.Text = "button1";
            this.dialHangUpButton.UseVisualStyleBackColor = true;
            this.dialHangUpButton.Click += new System.EventHandler(this.dialHangUpButton_Click);
            // 
            // informationLabel
            // 
            this.informationLabel.Location = new System.Drawing.Point(13, 13);
            this.informationLabel.Name = "informationLabel";
            this.informationLabel.Size = new System.Drawing.Size(341, 28);
            this.informationLabel.TabIndex = 3;
            this.informationLabel.Text = "Informationen";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(248, 73);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Abbrechen";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // Dialing
            // 
            this.AcceptButton = this.dialHangUpButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LemonChiffon;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(366, 115);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.informationLabel);
            this.Controls.Add(this.dialHangUpButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.phoneNumberTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Dialing";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Dialing_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox phoneNumberTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button dialHangUpButton;
        private System.Windows.Forms.Label informationLabel;
        private System.Windows.Forms.Button cancelButton;
    }
}