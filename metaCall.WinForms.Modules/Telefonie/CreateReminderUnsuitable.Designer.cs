namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class CreateReminderUnsuitable
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
            this.grpQuestion = new System.Windows.Forms.GroupBox();
            this.cBResultContactTypeParticipationUnsuitable = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpQuestion.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpQuestion
            // 
            this.grpQuestion.Controls.Add(this.cBResultContactTypeParticipationUnsuitable);
            this.grpQuestion.Controls.Add(this.label1);
            this.grpQuestion.Location = new System.Drawing.Point(10, 0);
            this.grpQuestion.Margin = new System.Windows.Forms.Padding(0);
            this.grpQuestion.Name = "grpQuestion";
            this.grpQuestion.Size = new System.Drawing.Size(480, 50);
            this.grpQuestion.TabIndex = 1;
            this.grpQuestion.TabStop = false;
            // 
            // cBResultContactTypeParticipationUnsuitable
            // 
            this.cBResultContactTypeParticipationUnsuitable.FormattingEnabled = true;
            this.cBResultContactTypeParticipationUnsuitable.Location = new System.Drawing.Point(149, 13);
            this.cBResultContactTypeParticipationUnsuitable.Name = "cBResultContactTypeParticipationUnsuitable";
            this.cBResultContactTypeParticipationUnsuitable.Size = new System.Drawing.Size(216, 21);
            this.cBResultContactTypeParticipationUnsuitable.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Grund";
            // 
            // CreateReminderUnsuitable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.Controls.Add(this.grpQuestion);
            this.Name = "CreateReminderUnsuitable";
            this.Size = new System.Drawing.Size(480, 60);
            this.Load += new System.EventHandler(this.CreateReminderUnsuitable_Load);
            this.grpQuestion.ResumeLayout(false);
            this.grpQuestion.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpQuestion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cBResultContactTypeParticipationUnsuitable;
    }
}
