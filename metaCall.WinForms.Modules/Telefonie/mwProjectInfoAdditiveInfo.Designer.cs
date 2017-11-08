namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class mwProjectInfoAdditiveInfo
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
            this.AdditiveInfoLabel = new System.Windows.Forms.Label();
            this.OkButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AdditiveInfoLabel
            // 
            this.AdditiveInfoLabel.AutoSize = true;
            this.AdditiveInfoLabel.Location = new System.Drawing.Point(12, 9);
            this.AdditiveInfoLabel.Name = "AdditiveInfoLabel";
            this.AdditiveInfoLabel.Size = new System.Drawing.Size(41, 13);
            this.AdditiveInfoLabel.TabIndex = 54;
            this.AdditiveInfoLabel.Text = "label21";
            // 
            // OkButton
            // 
            this.OkButton.AutoSize = true;
            this.OkButton.Location = new System.Drawing.Point(397, 231);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(50, 23);
            this.OkButton.TabIndex = 55;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // mwProjectInfoAdditiveInfo
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(459, 266);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.AdditiveInfoLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "mwProjectInfoAdditiveInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Zusatzinfo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AdditiveInfoLabel;
        private System.Windows.Forms.Button OkButton;
    }
}