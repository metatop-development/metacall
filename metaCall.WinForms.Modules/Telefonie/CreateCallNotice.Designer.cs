using MaDaNet.Common.AppFrameWork.WinUI.Controls;
namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    partial class CreateCallNotice
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
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.noticeAdministrationTextBox = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.noticeHistoryTextBox = new System.Windows.Forms.TextBox();
            this.Diagnose = new System.Windows.Forms.TabPage();
            this.listBoxDiagnosis = new System.Windows.Forms.ListBox();
            this.Branch_BranchGroupComboBox = new MaDaNet.Common.AppFrameWork.WinUI.Controls.ComboBoxAutoComplete();
            this.noticeTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.Diagnose.SuspendLayout();
            this.SuspendLayout();
            // 
            // noticeTextBox
            // 
            this.noticeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noticeTextBox.Location = new System.Drawing.Point(5, 5);
            this.noticeTextBox.Margin = new System.Windows.Forms.Padding(5);
            this.noticeTextBox.Multiline = true;
            this.noticeTextBox.Name = "noticeTextBox";
            this.noticeTextBox.Size = new System.Drawing.Size(672, 197);
            this.noticeTextBox.TabIndex = 8;
            // 
            // noticeTabControl
            // 
            this.noticeTabControl.Controls.Add(this.tabPage1);
            this.noticeTabControl.Controls.Add(this.tabPage3);
            this.noticeTabControl.Controls.Add(this.tabPage2);
            this.noticeTabControl.Controls.Add(this.Diagnose);
            this.noticeTabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noticeTabControl.HotTrack = true;
            this.noticeTabControl.Location = new System.Drawing.Point(15, 107);
            this.noticeTabControl.Margin = new System.Windows.Forms.Padding(5);
            this.noticeTabControl.Name = "noticeTabControl";
            this.noticeTabControl.SelectedIndex = 0;
            this.noticeTabControl.ShowToolTips = true;
            this.noticeTabControl.Size = new System.Drawing.Size(690, 240);
            this.noticeTabControl.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.noticeTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(5);
            this.tabPage1.Size = new System.Drawing.Size(682, 207);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Notiz";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.noticeAdministrationTextBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(5);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(5);
            this.tabPage3.Size = new System.Drawing.Size(682, 207);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Notiz Verwaltung";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // noticeAdministrationTextBox
            // 
            this.noticeAdministrationTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noticeAdministrationTextBox.Location = new System.Drawing.Point(5, 5);
            this.noticeAdministrationTextBox.Margin = new System.Windows.Forms.Padding(5);
            this.noticeAdministrationTextBox.Multiline = true;
            this.noticeAdministrationTextBox.Name = "noticeAdministrationTextBox";
            this.noticeAdministrationTextBox.Size = new System.Drawing.Size(672, 197);
            this.noticeAdministrationTextBox.TabIndex = 9;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.noticeHistoryTextBox);
            this.tabPage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(5);
            this.tabPage2.Size = new System.Drawing.Size(682, 207);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Historie";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // noticeHistoryTextBox
            // 
            this.noticeHistoryTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.noticeHistoryTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noticeHistoryTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noticeHistoryTextBox.Location = new System.Drawing.Point(5, 5);
            this.noticeHistoryTextBox.Margin = new System.Windows.Forms.Padding(5);
            this.noticeHistoryTextBox.Multiline = true;
            this.noticeHistoryTextBox.Name = "noticeHistoryTextBox";
            this.noticeHistoryTextBox.ReadOnly = true;
            this.noticeHistoryTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.noticeHistoryTextBox.Size = new System.Drawing.Size(672, 197);
            this.noticeHistoryTextBox.TabIndex = 0;
            // 
            // Diagnose
            // 
            this.Diagnose.Controls.Add(this.listBoxDiagnosis);
            this.Diagnose.Location = new System.Drawing.Point(4, 29);
            this.Diagnose.Margin = new System.Windows.Forms.Padding(2);
            this.Diagnose.Name = "Diagnose";
            this.Diagnose.Padding = new System.Windows.Forms.Padding(2);
            this.Diagnose.Size = new System.Drawing.Size(682, 207);
            this.Diagnose.TabIndex = 3;
            this.Diagnose.Text = "Diagnose";
            this.Diagnose.UseVisualStyleBackColor = true;
            // 
            // listBoxDiagnosis
            // 
            this.listBoxDiagnosis.FormattingEnabled = true;
            this.listBoxDiagnosis.ItemHeight = 20;
            this.listBoxDiagnosis.Location = new System.Drawing.Point(5, 7);
            this.listBoxDiagnosis.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxDiagnosis.Name = "listBoxDiagnosis";
            this.listBoxDiagnosis.ScrollAlwaysVisible = true;
            this.listBoxDiagnosis.Size = new System.Drawing.Size(669, 204);
            this.listBoxDiagnosis.TabIndex = 0;
            // 
            // Branch_BranchGroupComboBox
            // 
            this.Branch_BranchGroupComboBox.FormattingEnabled = true;
            this.Branch_BranchGroupComboBox.LimitToList = true;
            this.Branch_BranchGroupComboBox.Location = new System.Drawing.Point(30, 38);
            this.Branch_BranchGroupComboBox.Margin = new System.Windows.Forms.Padding(5);
            this.Branch_BranchGroupComboBox.MaxDropDownItems = 40;
            this.Branch_BranchGroupComboBox.Name = "Branch_BranchGroupComboBox";
            this.Branch_BranchGroupComboBox.Size = new System.Drawing.Size(388, 28);
            this.Branch_BranchGroupComboBox.TabIndex = 12;
            this.Branch_BranchGroupComboBox.NotInList += new System.ComponentModel.CancelEventHandler(this.Branch_BranchGroupComboBox_NotInList);
            this.Branch_BranchGroupComboBox.SelectedIndexChanged += new System.EventHandler(this.Branch_BranchGroupComboBox_SelectedIndexChanged);
            // 
            // CreateCallNotice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.Controls.Add(this.Branch_BranchGroupComboBox);
            this.Controls.Add(this.noticeTabControl);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "CreateCallNotice";
            this.Size = new System.Drawing.Size(720, 375);
            this.Load += new System.EventHandler(this.CreateCallNotice_Load);
            this.noticeTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.Diagnose.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox noticeTextBox;
        private System.Windows.Forms.TabControl noticeTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox noticeHistoryTextBox;
        private ComboBoxAutoComplete Branch_BranchGroupComboBox;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox noticeAdministrationTextBox;
        private System.Windows.Forms.TabPage Diagnose;
        private System.Windows.Forms.ListBox listBoxDiagnosis;
    }
}
