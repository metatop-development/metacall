namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class Projektanmeldung
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
            this.cboCenter = new System.Windows.Forms.ComboBox();
            this.cboTeam = new System.Windows.Forms.ComboBox();
            this.cboProjekt = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnChangeCurrentProject = new System.Windows.Forms.Button();
            this.btnNewCustomer = new System.Windows.Forms.Button();
            this.cboCallJobGroups = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxProjektanmeldung = new System.Windows.Forms.GroupBox();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.listBoxPhoneLines = new System.Windows.Forms.ListBox();
            this.labelDurringLevel = new System.Windows.Forms.Label();
            this.cboDurringLevel = new System.Windows.Forms.ComboBox();
            this.buttonDurring = new System.Windows.Forms.Button();
            this.groupBoxProjektanmeldung.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboCenter
            // 
            this.cboCenter.FormattingEnabled = true;
            this.cboCenter.Location = new System.Drawing.Point(183, 52);
            this.cboCenter.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cboCenter.Name = "cboCenter";
            this.cboCenter.Size = new System.Drawing.Size(308, 32);
            this.cboCenter.TabIndex = 0;
            this.cboCenter.SelectedIndexChanged += new System.EventHandler(this.cboCenter_SelectedIndexChanged);
            // 
            // cboTeam
            // 
            this.cboTeam.FormattingEnabled = true;
            this.cboTeam.Location = new System.Drawing.Point(183, 100);
            this.cboTeam.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cboTeam.Name = "cboTeam";
            this.cboTeam.Size = new System.Drawing.Size(308, 32);
            this.cboTeam.TabIndex = 1;
            this.cboTeam.SelectedIndexChanged += new System.EventHandler(this.cboTeam_SelectedIndexChanged);
            // 
            // cboProjekt
            // 
            this.cboProjekt.FormattingEnabled = true;
            this.cboProjekt.Location = new System.Drawing.Point(183, 148);
            this.cboProjekt.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cboProjekt.Name = "cboProjekt";
            this.cboProjekt.Size = new System.Drawing.Size(693, 32);
            this.cboProjekt.TabIndex = 3;
            this.cboProjekt.SelectedIndexChanged += new System.EventHandler(this.cboProjekt_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 62);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Center:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 110);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Team:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 158);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 25);
            this.label4.TabIndex = 7;
            this.label4.Text = "Projekt";
            // 
            // btnChangeCurrentProject
            // 
            this.btnChangeCurrentProject.Location = new System.Drawing.Point(389, 312);
            this.btnChangeCurrentProject.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnChangeCurrentProject.Name = "btnChangeCurrentProject";
            this.btnChangeCurrentProject.Size = new System.Drawing.Size(165, 42);
            this.btnChangeCurrentProject.TabIndex = 8;
            this.btnChangeCurrentProject.Text = "Anmelden";
            this.btnChangeCurrentProject.UseVisualStyleBackColor = true;
            this.btnChangeCurrentProject.Click += new System.EventHandler(this.btnChangeCurrentProject_Click);
            // 
            // btnNewCustomer
            // 
            this.btnNewCustomer.Location = new System.Drawing.Point(183, 312);
            this.btnNewCustomer.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnNewCustomer.Name = "btnNewCustomer";
            this.btnNewCustomer.Size = new System.Drawing.Size(165, 42);
            this.btnNewCustomer.TabIndex = 9;
            this.btnNewCustomer.Text = "neuer Kunde";
            this.btnNewCustomer.UseVisualStyleBackColor = true;
            this.btnNewCustomer.Click += new System.EventHandler(this.btnNewCustomer_Click);
            // 
            // cboCallJobGroups
            // 
            this.cboCallJobGroups.FormattingEnabled = true;
            this.cboCallJobGroups.Location = new System.Drawing.Point(183, 196);
            this.cboCallJobGroups.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cboCallJobGroups.Name = "cboCallJobGroups";
            this.cboCallJobGroups.Size = new System.Drawing.Size(455, 32);
            this.cboCallJobGroups.TabIndex = 10;
            this.cboCallJobGroups.SelectionChangeCommitted += new System.EventHandler(this.cboCallJobGroups_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 203);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 25);
            this.label3.TabIndex = 11;
            this.label3.Text = "Gruppe";
            // 
            // groupBoxProjektanmeldung
            // 
            this.groupBoxProjektanmeldung.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxProjektanmeldung.Controls.Add(this.textBoxUsername);
            this.groupBoxProjektanmeldung.Controls.Add(this.listBoxPhoneLines);
            this.groupBoxProjektanmeldung.Controls.Add(this.labelDurringLevel);
            this.groupBoxProjektanmeldung.Controls.Add(this.cboDurringLevel);
            this.groupBoxProjektanmeldung.Controls.Add(this.buttonDurring);
            this.groupBoxProjektanmeldung.Controls.Add(this.label3);
            this.groupBoxProjektanmeldung.Controls.Add(this.cboCallJobGroups);
            this.groupBoxProjektanmeldung.Controls.Add(this.btnNewCustomer);
            this.groupBoxProjektanmeldung.Controls.Add(this.btnChangeCurrentProject);
            this.groupBoxProjektanmeldung.Controls.Add(this.label4);
            this.groupBoxProjektanmeldung.Controls.Add(this.label2);
            this.groupBoxProjektanmeldung.Controls.Add(this.label1);
            this.groupBoxProjektanmeldung.Controls.Add(this.cboProjekt);
            this.groupBoxProjektanmeldung.Controls.Add(this.cboTeam);
            this.groupBoxProjektanmeldung.Controls.Add(this.cboCenter);
            this.groupBoxProjektanmeldung.Location = new System.Drawing.Point(18, 18);
            this.groupBoxProjektanmeldung.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBoxProjektanmeldung.Name = "groupBoxProjektanmeldung";
            this.groupBoxProjektanmeldung.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBoxProjektanmeldung.Size = new System.Drawing.Size(898, 732);
            this.groupBoxProjektanmeldung.TabIndex = 12;
            this.groupBoxProjektanmeldung.TabStop = false;
            this.groupBoxProjektanmeldung.Text = "Projektanmeldung";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(183, 376);
            this.textBoxUsername.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(577, 29);
            this.textBoxUsername.TabIndex = 16;
            this.textBoxUsername.Visible = false;
            // 
            // listBoxPhoneLines
            // 
            this.listBoxPhoneLines.FormattingEnabled = true;
            this.listBoxPhoneLines.ItemHeight = 24;
            this.listBoxPhoneLines.Location = new System.Drawing.Point(183, 438);
            this.listBoxPhoneLines.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBoxPhoneLines.Name = "listBoxPhoneLines";
            this.listBoxPhoneLines.Size = new System.Drawing.Size(577, 268);
            this.listBoxPhoneLines.TabIndex = 15;
            this.listBoxPhoneLines.UseWaitCursor = true;
            this.listBoxPhoneLines.Visible = false;
            // 
            // labelDurringLevel
            // 
            this.labelDurringLevel.AutoSize = true;
            this.labelDurringLevel.Location = new System.Drawing.Point(22, 247);
            this.labelDurringLevel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelDurringLevel.Name = "labelDurringLevel";
            this.labelDurringLevel.Size = new System.Drawing.Size(104, 25);
            this.labelDurringLevel.TabIndex = 14;
            this.labelDurringLevel.Text = "Mahnstufe";
            // 
            // cboDurringLevel
            // 
            this.cboDurringLevel.FormattingEnabled = true;
            this.cboDurringLevel.Location = new System.Drawing.Point(183, 242);
            this.cboDurringLevel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cboDurringLevel.Name = "cboDurringLevel";
            this.cboDurringLevel.Size = new System.Drawing.Size(455, 32);
            this.cboDurringLevel.TabIndex = 13;
            this.cboDurringLevel.SelectedIndexChanged += new System.EventHandler(this.cboDurringLevel_SelectedIndexChanged);
            // 
            // buttonDurring
            // 
            this.buttonDurring.Location = new System.Drawing.Point(596, 312);
            this.buttonDurring.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonDurring.Name = "buttonDurring";
            this.buttonDurring.Size = new System.Drawing.Size(165, 42);
            this.buttonDurring.TabIndex = 12;
            this.buttonDurring.Text = "Mahnungen";
            this.buttonDurring.UseVisualStyleBackColor = true;
            this.buttonDurring.Click += new System.EventHandler(this.buttonDurring_Click);
            // 
            // Projektanmeldung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBoxProjektanmeldung);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "Projektanmeldung";
            this.Size = new System.Drawing.Size(953, 822);
            this.Load += new System.EventHandler(this.Projektanmeldung_Load);
            this.groupBoxProjektanmeldung.ResumeLayout(false);
            this.groupBoxProjektanmeldung.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboCenter;
        private System.Windows.Forms.ComboBox cboTeam;
        private System.Windows.Forms.ComboBox cboProjekt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnChangeCurrentProject;
        private System.Windows.Forms.Button btnNewCustomer;
        private System.Windows.Forms.ComboBox cboCallJobGroups;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBoxProjektanmeldung;
        private System.Windows.Forms.Button buttonDurring;
        private System.Windows.Forms.Label labelDurringLevel;
        private System.Windows.Forms.ComboBox cboDurringLevel;
        private System.Windows.Forms.ListBox listBoxPhoneLines;
        private System.Windows.Forms.TextBox textBoxUsername;
    }
}
