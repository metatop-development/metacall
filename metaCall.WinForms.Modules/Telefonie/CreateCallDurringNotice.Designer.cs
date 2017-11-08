namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class CreateCallDurringNotice
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
            this.noticeTextBox = new System.Windows.Forms.TextBox();
            this.noticeTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.noticeHistoryTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.noticeTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // noticeTextBox
            // 
            this.noticeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noticeTextBox.Location = new System.Drawing.Point(3, 3);
            this.noticeTextBox.Multiline = true;
            this.noticeTextBox.Name = "noticeTextBox";
            this.noticeTextBox.Size = new System.Drawing.Size(430, 41);
            this.noticeTextBox.TabIndex = 8;
            // 
            // noticeTabControl
            // 
            this.noticeTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.noticeTabControl.Controls.Add(this.tabPage1);
            this.noticeTabControl.Controls.Add(this.tabPage2);
            this.noticeTabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noticeTabControl.HotTrack = true;
            this.noticeTabControl.Location = new System.Drawing.Point(10, 19);
            this.noticeTabControl.Name = "noticeTabControl";
            this.noticeTabControl.SelectedIndex = 0;
            this.noticeTabControl.ShowToolTips = true;
            this.noticeTabControl.Size = new System.Drawing.Size(444, 73);
            this.noticeTabControl.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.noticeTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(436, 47);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Notiz";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.noticeHistoryTextBox);
            this.tabPage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(436, 89);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Historie";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // noticeHistoryTextBox
            // 
            this.noticeHistoryTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.noticeHistoryTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noticeHistoryTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noticeHistoryTextBox.Location = new System.Drawing.Point(3, 3);
            this.noticeHistoryTextBox.Multiline = true;
            this.noticeHistoryTextBox.Name = "noticeHistoryTextBox";
            this.noticeHistoryTextBox.ReadOnly = true;
            this.noticeHistoryTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.noticeHistoryTextBox.Size = new System.Drawing.Size(430, 83);
            this.noticeHistoryTextBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.noticeTabControl);
            this.groupBox1.Location = new System.Drawing.Point(10, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(480, 98);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Notizen";
            // 
            // CreateCallDurringNotice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MistyRose;
            this.Controls.Add(this.groupBox1);
            this.MaximumSize = new System.Drawing.Size(1024, 153);
            this.MinimumSize = new System.Drawing.Size(480, 115);
            this.Name = "CreateCallDurringNotice";
            this.Size = new System.Drawing.Size(503, 125);
            this.Load += new System.EventHandler(this.CreateCallNotice_Load);
            this.noticeTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox noticeTextBox;
        private System.Windows.Forms.TabControl noticeTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox noticeHistoryTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
