namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class MainView
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.zeitübersicht1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.Zeituebersicht();
            this.projektanmeldung1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.Projektanmeldung();
            this.userInfoPanel1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.UserInfoPanel();
            this.durringInfoPanel1 = new metatop.Applications.metaCall.WinForms.Modules.Telefonie.DurringInfoPanel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zeitübersicht1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.88034F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.11966F));
            this.tableLayoutPanel1.Controls.Add(this.zeitübersicht1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.projektanmeldung1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.userInfoPanel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.durringInfoPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 79.61165F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.38835F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(919, 515);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // zeitübersicht1
            // 
            this.zeitübersicht1.AutoSize = true;
            this.zeitübersicht1.BackColor = System.Drawing.SystemColors.Window;
            this.zeitübersicht1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zeitübersicht1.Location = new System.Drawing.Point(4, 413);
            this.zeitübersicht1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.zeitübersicht1.Name = "zeitübersicht1";
            this.zeitübersicht1.Size = new System.Drawing.Size(376, 98);
            this.zeitübersicht1.TabIndex = 0;
            // 
            // projektanmeldung1
            // 
            this.projektanmeldung1.AutoSize = true;
            this.projektanmeldung1.BackColor = System.Drawing.Color.Transparent;
            this.projektanmeldung1.Location = new System.Drawing.Point(388, 4);
            this.projektanmeldung1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.projektanmeldung1.Name = "projektanmeldung1";
            this.projektanmeldung1.Size = new System.Drawing.Size(503, 223);
            this.projektanmeldung1.TabIndex = 1;
            this.projektanmeldung1.ProjectChanged += new metatop.Applications.metaCall.WinForms.Modules.Telefonie.ProjectChangedEventHandler(this.projektanmeldung1_ProjectChanged);
            this.projektanmeldung1.DurringActivChanged += new metatop.Applications.metaCall.WinForms.Modules.Telefonie.DurringChangedEventHandler(this.durringInfo1_DurringActivChanged);
            this.projektanmeldung1.DurringLevelInfoChanged += new metatop.Applications.metaCall.BusinessLayer.DurringLevelInfoChangedEventHandler(this.projektanmeldung1_DurringLevelInfoChanged);
            this.projektanmeldung1.NewCustomer += new metatop.Applications.metaCall.WinForms.Modules.Telefonie.NewCustomerEventHandler(this.projektanmeldung1_NewCustomer);
            // 
            // userInfoPanel1
            // 
            this.userInfoPanel1.AutoSize = true;
            this.userInfoPanel1.BackColor = System.Drawing.SystemColors.Window;
            this.userInfoPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userInfoPanel1.Location = new System.Drawing.Point(388, 413);
            this.userInfoPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.userInfoPanel1.Name = "userInfoPanel1";
            this.userInfoPanel1.Size = new System.Drawing.Size(527, 98);
            this.userInfoPanel1.TabIndex = 2;
            this.userInfoPanel1.UserWantSpecialCall += new metatop.Applications.metaCall.WinForms.Modules.Telefonie.UserWantSpecialCallHandler(this.userInfoPanel1_UserWantSpecialCall);
            // 
            // durringInfoPanel1
            // 
            this.durringInfoPanel1.BackColor = System.Drawing.SystemColors.Window;
            this.durringInfoPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.durringInfoPanel1.Location = new System.Drawing.Point(4, 4);
            this.durringInfoPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.durringInfoPanel1.Name = "durringInfoPanel1";
            this.durringInfoPanel1.Size = new System.Drawing.Size(376, 401);
            this.durringInfoPanel1.TabIndex = 12;
            this.durringInfoPanel1.UserWantSpecialCall += new metatop.Applications.metaCall.WinForms.Modules.Telefonie.UserWantSpecialCallHandler(this.durringInfoPanel1_UserWantSpecialCall);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainView";
            this.Size = new System.Drawing.Size(919, 515);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zeitübersicht1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Zeituebersicht zeitübersicht1;
        private Projektanmeldung projektanmeldung1;
        private UserInfoPanel userInfoPanel1;
        private DurringInfoPanel durringInfoPanel1;

    }
}
