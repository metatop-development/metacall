namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class Stammdatenverwaltung
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
            this.toolStripMenue = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabelSave = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabelCancel = new System.Windows.Forms.ToolStripLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxAbortDialingTime = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonAddressSafeActivTrue = new System.Windows.Forms.RadioButton();
            this.radioButtonAddressSafeActivFalse = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxAddressSafeTime = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonPaymentTargetVisibleTrue = new System.Windows.Forms.RadioButton();
            this.radioButtonPaymentTargetVisibleFalse = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxShutDownCountdownSec = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxActivitiesMaxSec = new System.Windows.Forms.TextBox();
            this.teamDeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBlockAddresses = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.toolStripMenue.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenue
            // 
            this.toolStripMenue.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.editToolStripButton,
            this.toolStripLabelSave,
            this.toolStripLabelCancel});
            this.toolStripMenue.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenue.Name = "toolStripMenue";
            this.toolStripMenue.Size = new System.Drawing.Size(569, 25);
            this.toolStripMenue.TabIndex = 9;
            this.toolStripMenue.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newToolStripButton.Image = global::metatop.Applications.metaCall.WinForms.Modules.Properties.Resources.UserIcon;
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(33, 22);
            this.newToolStripButton.Text = "Neu";
            // 
            // editToolStripButton
            // 
            this.editToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.editToolStripButton.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.editToolStripButton.Name = "editToolStripButton";
            this.editToolStripButton.Size = new System.Drawing.Size(67, 22);
            this.editToolStripButton.Text = "Bearbeiten";
            this.editToolStripButton.Click += new System.EventHandler(this.editToolStripButton_Click);
            // 
            // toolStripLabelSave
            // 
            this.toolStripLabelSave.Name = "toolStripLabelSave";
            this.toolStripLabelSave.Size = new System.Drawing.Size(59, 22);
            this.toolStripLabelSave.Text = "Speichern";
            this.toolStripLabelSave.Click += new System.EventHandler(this.toolStripLabelSave_Click);
            // 
            // toolStripLabelCancel
            // 
            this.toolStripLabelCancel.Name = "toolStripLabelCancel";
            this.toolStripLabelCancel.Size = new System.Drawing.Size(65, 22);
            this.toolStripLabelCancel.Text = "Abbrechen";
            this.toolStripLabelCancel.Click += new System.EventHandler(this.toolStripLabelCancel_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.btnBlockAddresses);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.textBoxAbortDialingTime);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.textBoxAddressSafeTime);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBoxShutDownCountdownSec);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxActivitiesMaxSec);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(569, 330);
            this.panel1.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(85, 192);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Sek.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(150, 192);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(245, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Zeit bis zum automatischen Auflegen beim Wählen";
            // 
            // textBoxAbortDialingTime
            // 
            this.textBoxAbortDialingTime.Location = new System.Drawing.Point(20, 189);
            this.textBoxAbortDialingTime.Name = "textBoxAbortDialingTime";
            this.textBoxAbortDialingTime.Size = new System.Drawing.Size(55, 20);
            this.textBoxAbortDialingTime.TabIndex = 14;
            this.textBoxAbortDialingTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox2
            // 
            this.groupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox2.Controls.Add(this.radioButtonAddressSafeActivTrue);
            this.groupBox2.Controls.Add(this.radioButtonAddressSafeActivFalse);
            this.groupBox2.Location = new System.Drawing.Point(10, 142);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(120, 31);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            // 
            // radioButtonAddressSafeActivTrue
            // 
            this.radioButtonAddressSafeActivTrue.AutoSize = true;
            this.radioButtonAddressSafeActivTrue.Location = new System.Drawing.Point(10, 10);
            this.radioButtonAddressSafeActivTrue.Name = "radioButtonAddressSafeActivTrue";
            this.radioButtonAddressSafeActivTrue.Size = new System.Drawing.Size(36, 17);
            this.radioButtonAddressSafeActivTrue.TabIndex = 7;
            this.radioButtonAddressSafeActivTrue.TabStop = true;
            this.radioButtonAddressSafeActivTrue.Text = "Ja";
            this.radioButtonAddressSafeActivTrue.UseVisualStyleBackColor = true;
            // 
            // radioButtonAddressSafeActivFalse
            // 
            this.radioButtonAddressSafeActivFalse.AutoSize = true;
            this.radioButtonAddressSafeActivFalse.Location = new System.Drawing.Point(60, 10);
            this.radioButtonAddressSafeActivFalse.Name = "radioButtonAddressSafeActivFalse";
            this.radioButtonAddressSafeActivFalse.Size = new System.Drawing.Size(47, 17);
            this.radioButtonAddressSafeActivFalse.TabIndex = 6;
            this.radioButtonAddressSafeActivFalse.TabStop = true;
            this.radioButtonAddressSafeActivFalse.Text = "Nein";
            this.radioButtonAddressSafeActivFalse.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(85, 119);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Tage";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(150, 153);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(131, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Adressenschutz aktivieren";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(150, 119);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(277, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "Anzahl der Tage wie lange eine Adresse  geschützt bleibt";
            // 
            // textBoxAddressSafeTime
            // 
            this.textBoxAddressSafeTime.Location = new System.Drawing.Point(20, 116);
            this.textBoxAddressSafeTime.Name = "textBoxAddressSafeTime";
            this.textBoxAddressSafeTime.Size = new System.Drawing.Size(55, 20);
            this.textBoxAddressSafeTime.TabIndex = 8;
            this.textBoxAddressSafeTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(85, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Sek.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(85, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Sek.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(150, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Zahlungszieleingabe aktiv";
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox1.Controls.Add(this.radioButtonPaymentTargetVisibleTrue);
            this.groupBox1.Controls.Add(this.radioButtonPaymentTargetVisibleFalse);
            this.groupBox1.Location = new System.Drawing.Point(10, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(120, 31);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // radioButtonPaymentTargetVisibleTrue
            // 
            this.radioButtonPaymentTargetVisibleTrue.AutoSize = true;
            this.radioButtonPaymentTargetVisibleTrue.Location = new System.Drawing.Point(10, 10);
            this.radioButtonPaymentTargetVisibleTrue.Name = "radioButtonPaymentTargetVisibleTrue";
            this.radioButtonPaymentTargetVisibleTrue.Size = new System.Drawing.Size(36, 17);
            this.radioButtonPaymentTargetVisibleTrue.TabIndex = 7;
            this.radioButtonPaymentTargetVisibleTrue.TabStop = true;
            this.radioButtonPaymentTargetVisibleTrue.Text = "Ja";
            this.radioButtonPaymentTargetVisibleTrue.UseVisualStyleBackColor = true;
            // 
            // radioButtonPaymentTargetVisibleFalse
            // 
            this.radioButtonPaymentTargetVisibleFalse.AutoSize = true;
            this.radioButtonPaymentTargetVisibleFalse.Location = new System.Drawing.Point(60, 10);
            this.radioButtonPaymentTargetVisibleFalse.Name = "radioButtonPaymentTargetVisibleFalse";
            this.radioButtonPaymentTargetVisibleFalse.Size = new System.Drawing.Size(47, 17);
            this.radioButtonPaymentTargetVisibleFalse.TabIndex = 6;
            this.radioButtonPaymentTargetVisibleFalse.TabStop = true;
            this.radioButtonPaymentTargetVisibleFalse.Text = "Nein";
            this.radioButtonPaymentTargetVisibleFalse.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(150, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(232, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Dauer der Anzeige des automatischen Beendes";
            // 
            // textBoxShutDownCountdownSec
            // 
            this.textBoxShutDownCountdownSec.Location = new System.Drawing.Point(20, 46);
            this.textBoxShutDownCountdownSec.Name = "textBoxShutDownCountdownSec";
            this.textBoxShutDownCountdownSec.Size = new System.Drawing.Size(55, 20);
            this.textBoxShutDownCountdownSec.TabIndex = 2;
            this.textBoxShutDownCountdownSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(150, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Zeit bis zum automatischen Abmelden";
            // 
            // textBoxActivitiesMaxSec
            // 
            this.textBoxActivitiesMaxSec.Location = new System.Drawing.Point(20, 20);
            this.textBoxActivitiesMaxSec.Name = "textBoxActivitiesMaxSec";
            this.textBoxActivitiesMaxSec.Size = new System.Drawing.Size(55, 20);
            this.textBoxActivitiesMaxSec.TabIndex = 0;
            this.textBoxActivitiesMaxSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // teamDeleteToolStripMenuItem
            // 
            this.teamDeleteToolStripMenuItem.Name = "teamDeleteToolStripMenuItem";
            this.teamDeleteToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.teamDeleteToolStripMenuItem.Text = "Team löschen";
            // 
            // btnBlockAddresses
            // 
            this.btnBlockAddresses.Location = new System.Drawing.Point(20, 240);
            this.btnBlockAddresses.Name = "btnBlockAddresses";
            this.btnBlockAddresses.Size = new System.Drawing.Size(97, 45);
            this.btnBlockAddresses.TabIndex = 17;
            this.btnBlockAddresses.Text = "fehlerhafte Anrufjobs sperren";
            this.btnBlockAddresses.UseVisualStyleBackColor = true;
            this.btnBlockAddresses.Click += new System.EventHandler(this.btnBlockAddresses_Click);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(150, 248);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(315, 37);
            this.label11.TabIndex = 18;
            this.label11.Text = "Mit dieser Schaltfläche können fehlerhafte Anrufjobs gesperrt werden, für die kei" +
                "ne Adresseinformationen vorliegen.";
            // 
            // Stammdatenverwaltung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStripMenue);
            this.Name = "Stammdatenverwaltung";
            this.Size = new System.Drawing.Size(569, 355);
            this.Load += new System.EventHandler(this.Stammdatenverwaltung_Load);
            this.toolStripMenue.ResumeLayout(false);
            this.toolStripMenue.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMenue;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton editToolStripButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem teamDeleteToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxActivitiesMaxSec;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxShutDownCountdownSec;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonPaymentTargetVisibleTrue;
        private System.Windows.Forms.RadioButton radioButtonPaymentTargetVisibleFalse;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSave;
        private System.Windows.Forms.ToolStripLabel toolStripLabelCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxAddressSafeTime;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonAddressSafeActivTrue;
        private System.Windows.Forms.RadioButton radioButtonAddressSafeActivFalse;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxAbortDialingTime;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnBlockAddresses;
    }
}
