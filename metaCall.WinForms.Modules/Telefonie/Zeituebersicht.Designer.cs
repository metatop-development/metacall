namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class Zeituebersicht
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.lblArbeitszeit = new System.Windows.Forms.Label();
            this.lblPausen = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblProjektzeit = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTelefonzeit = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblNacharbeit = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblUnbestimmt = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblMahnzeit = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Arbeitszeit:";
            // 
            // lblArbeitszeit
            // 
            this.lblArbeitszeit.AutoSize = true;
            this.lblArbeitszeit.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.lblArbeitszeit.Location = new System.Drawing.Point(90, 20);
            this.lblArbeitszeit.Name = "lblArbeitszeit";
            this.lblArbeitszeit.Size = new System.Drawing.Size(0, 13);
            this.lblArbeitszeit.TabIndex = 1;
            // 
            // lblPausen
            // 
            this.lblPausen.AutoSize = true;
            this.lblPausen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.lblPausen.Location = new System.Drawing.Point(90, 46);
            this.lblPausen.Name = "lblPausen";
            this.lblPausen.Size = new System.Drawing.Size(0, 13);
            this.lblPausen.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Pausen:";
            // 
            // lblProjektzeit
            // 
            this.lblProjektzeit.AutoSize = true;
            this.lblProjektzeit.ForeColor = System.Drawing.Color.Blue;
            this.lblProjektzeit.Location = new System.Drawing.Point(230, 20);
            this.lblProjektzeit.Name = "lblProjektzeit";
            this.lblProjektzeit.Size = new System.Drawing.Size(0, 13);
            this.lblProjektzeit.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(150, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Projektzeit:";
            // 
            // lblTelefonzeit
            // 
            this.lblTelefonzeit.AutoSize = true;
            this.lblTelefonzeit.ForeColor = System.Drawing.Color.Blue;
            this.lblTelefonzeit.Location = new System.Drawing.Point(230, 46);
            this.lblTelefonzeit.Name = "lblTelefonzeit";
            this.lblTelefonzeit.Size = new System.Drawing.Size(0, 13);
            this.lblTelefonzeit.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(150, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Telefonzeit:";
            // 
            // lblNacharbeit
            // 
            this.lblNacharbeit.AutoSize = true;
            this.lblNacharbeit.ForeColor = System.Drawing.Color.Blue;
            this.lblNacharbeit.Location = new System.Drawing.Point(230, 72);
            this.lblNacharbeit.Name = "lblNacharbeit";
            this.lblNacharbeit.Size = new System.Drawing.Size(0, 13);
            this.lblNacharbeit.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(150, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Nacharbeit:";
            // 
            // lblUnbestimmt
            // 
            this.lblUnbestimmt.AutoSize = true;
            this.lblUnbestimmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblUnbestimmt.Location = new System.Drawing.Point(89, 72);
            this.lblUnbestimmt.Name = "lblUnbestimmt";
            this.lblUnbestimmt.Size = new System.Drawing.Size(0, 13);
            this.lblUnbestimmt.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(10, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "unbestimmt:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.lblMahnzeit);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.lblUnbestimmt);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lblNacharbeit);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblTelefonzeit);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblProjektzeit);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblPausen);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblArbeitszeit);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 128);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Zeitüberischt";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // lblMahnzeit
            // 
            this.lblMahnzeit.AutoSize = true;
            this.lblMahnzeit.ForeColor = System.Drawing.Color.Blue;
            this.lblMahnzeit.Location = new System.Drawing.Point(230, 100);
            this.lblMahnzeit.Name = "lblMahnzeit";
            this.lblMahnzeit.Size = new System.Drawing.Size(0, 13);
            this.lblMahnzeit.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(150, 100);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Mahnzeit:";
            // 
            // Zeituebersicht
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox1);
            this.Name = "Zeituebersicht";
            this.Size = new System.Drawing.Size(400, 153);
            this.Load += new System.EventHandler(this.Zeituebersicht_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblArbeitszeit;
        private System.Windows.Forms.Label lblPausen;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblProjektzeit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTelefonzeit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblNacharbeit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblUnbestimmt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblMahnzeit;
        private System.Windows.Forms.Label label8;
    }
}
