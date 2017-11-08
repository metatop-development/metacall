namespace metatop.Applications.metaCall.WinForms.App.LogOnServices
{
    partial class ChangePassWord
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
            this.txtPwEdit = new System.Windows.Forms.TextBox();
            this.txtPwEditConfirm = new System.Windows.Forms.TextBox();
            this.lblPwEdit = new System.Windows.Forms.Label();
            this.lblPwEditConfirm = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.TabIndex = 5;
            // 
            // btnLogIn
            // 
            this.btnLogIn.TabIndex = 4;
            // 
            // txtPwEdit
            // 
            this.txtPwEdit.Location = new System.Drawing.Point(210, 71);
            this.txtPwEdit.Name = "txtPwEdit";
            this.txtPwEdit.Size = new System.Drawing.Size(179, 20);
            this.txtPwEdit.TabIndex = 2;
            // 
            // txtPwEditConfirm
            // 
            this.txtPwEditConfirm.Location = new System.Drawing.Point(210, 97);
            this.txtPwEditConfirm.Name = "txtPwEditConfirm";
            this.txtPwEditConfirm.Size = new System.Drawing.Size(179, 20);
            this.txtPwEditConfirm.TabIndex = 3;
            // 
            // lblPwEdit
            // 
            this.lblPwEdit.AutoSize = true;
            this.lblPwEdit.Location = new System.Drawing.Point(100, 74);
            this.lblPwEdit.Name = "lblPwEdit";
            this.lblPwEdit.Size = new System.Drawing.Size(84, 13);
            this.lblPwEdit.TabIndex = 11;
            this.lblPwEdit.Text = "Neues Passwort";
            // 
            // lblPwEditConfirm
            // 
            this.lblPwEditConfirm.AutoSize = true;
            this.lblPwEditConfirm.Location = new System.Drawing.Point(100, 100);
            this.lblPwEditConfirm.Name = "lblPwEditConfirm";
            this.lblPwEditConfirm.Size = new System.Drawing.Size(73, 13);
            this.lblPwEditConfirm.TabIndex = 12;
            this.lblPwEditConfirm.Text = "Wiederholung";
            // 
            // ChangePassWord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(422, 191);
            this.Controls.Add(this.lblPwEditConfirm);
            this.Controls.Add(this.lblPwEdit);
            this.Controls.Add(this.txtPwEditConfirm);
            this.Controls.Add(this.txtPwEdit);
            this.Name = "ChangePassWord";
            this.Text = "metaCall Anmeldung";
            this.Controls.SetChildIndex(this.btnLogIn, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.txtUserName, 0);
            this.Controls.SetChildIndex(this.lblInfo, 0);
            this.Controls.SetChildIndex(this.txtPwEdit, 0);
            this.Controls.SetChildIndex(this.txtPwEditConfirm, 0);
            this.Controls.SetChildIndex(this.lblPwEdit, 0);
            this.Controls.SetChildIndex(this.lblPwEditConfirm, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPwEdit;
        private System.Windows.Forms.TextBox txtPwEditConfirm;
        private System.Windows.Forms.Label lblPwEdit;
        private System.Windows.Forms.Label lblPwEditConfirm;
    }
}
