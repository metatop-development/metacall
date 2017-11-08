namespace metatop.Applications.metaCall.WinForms.App
{
    partial class Training
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label = new System.Windows.Forms.Label();
            this.comboBoxTrainingGrund = new System.Windows.Forms.ComboBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.txtNotice = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::metatop.Applications.metaCall.WinForms.App.Properties.Resources.schulung;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(387, 227);
            this.panel1.TabIndex = 0;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(17, 256);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(72, 13);
            this.label.TabIndex = 16;
            this.label.Text = "Schulungsart:";
            // 
            // comboBoxTrainingGrund
            // 
            this.comboBoxTrainingGrund.FormattingEnabled = true;
            this.comboBoxTrainingGrund.Location = new System.Drawing.Point(105, 253);
            this.comboBoxTrainingGrund.Name = "comboBoxTrainingGrund";
            this.comboBoxTrainingGrund.Size = new System.Drawing.Size(250, 21);
            this.comboBoxTrainingGrund.TabIndex = 15;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(292, 394);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(63, 24);
            this.buttonOk.TabIndex = 17;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // txtNotice
            // 
            this.txtNotice.Location = new System.Drawing.Point(105, 281);
            this.txtNotice.Multiline = true;
            this.txtNotice.Name = "txtNotice";
            this.txtNotice.Size = new System.Drawing.Size(250, 107);
            this.txtNotice.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 284);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Notiz::";
            // 
            // Training
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MintCream;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(385, 458);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNotice);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.label);
            this.Controls.Add(this.comboBoxTrainingGrund);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Training";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Schulung";
            this.Load += new System.EventHandler(this.Training_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.ComboBox comboBoxTrainingGrund;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.TextBox txtNotice;
        private System.Windows.Forms.Label label1;

    }
}