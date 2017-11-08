namespace metatop.Applications.metaCall.WinForms.Modules
{
    partial class CallJobGroupEdit
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
            this.displayNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.typeTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.teamUserTreeView = new System.Windows.Forms.TreeView();
            this.delCallJobGroupsButton = new System.Windows.Forms.Button();
            this.selectCallJobGroupButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // displayNameTextBox
            // 
            this.displayNameTextBox.Location = new System.Drawing.Point(90, 10);
            this.displayNameTextBox.Name = "displayNameTextBox";
            this.displayNameTextBox.Size = new System.Drawing.Size(231, 20);
            this.displayNameTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Anzeigename";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Beschreibung";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(90, 36);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(436, 20);
            this.descriptionTextBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(340, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Art";
            // 
            // typeTextBox
            // 
            this.typeTextBox.Location = new System.Drawing.Point(366, 10);
            this.typeTextBox.Name = "typeTextBox";
            this.typeTextBox.ReadOnly = true;
            this.typeTextBox.Size = new System.Drawing.Size(160, 20);
            this.typeTextBox.TabIndex = 5;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(448, 171);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(78, 23);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "Übernehmen";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(448, 201);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(78, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Abbrechen";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // teamUserTreeView
            // 
            this.teamUserTreeView.CheckBoxes = true;
            this.teamUserTreeView.Location = new System.Drawing.Point(11, 70);
            this.teamUserTreeView.Name = "teamUserTreeView";
            this.teamUserTreeView.Size = new System.Drawing.Size(310, 154);
            this.teamUserTreeView.TabIndex = 8;
            this.teamUserTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.teamUserTreeView_AfterCheck);
            // 
            // delCallJobGroupsButton
            // 
            this.delCallJobGroupsButton.Location = new System.Drawing.Point(354, 141);
            this.delCallJobGroupsButton.Name = "delCallJobGroupsButton";
            this.delCallJobGroupsButton.Size = new System.Drawing.Size(75, 38);
            this.delCallJobGroupsButton.TabIndex = 20;
            this.delCallJobGroupsButton.Text = "alle Agents löschen";
            this.delCallJobGroupsButton.UseVisualStyleBackColor = true;
            this.delCallJobGroupsButton.Click += new System.EventHandler(this.delCallJobGroupsButton_Click);
            // 
            // selectCallJobGroupButton
            // 
            this.selectCallJobGroupButton.Location = new System.Drawing.Point(354, 186);
            this.selectCallJobGroupButton.Name = "selectCallJobGroupButton";
            this.selectCallJobGroupButton.Size = new System.Drawing.Size(75, 38);
            this.selectCallJobGroupButton.TabIndex = 19;
            this.selectCallJobGroupButton.Text = "alle Agents zuordnen";
            this.selectCallJobGroupButton.UseVisualStyleBackColor = true;
            this.selectCallJobGroupButton.Click += new System.EventHandler(this.selectCallJobGroupButton_Click);
            // 
            // CallJobGroupEdit
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(541, 243);
            this.ControlBox = false;
            this.Controls.Add(this.delCallJobGroupsButton);
            this.Controls.Add(this.selectCallJobGroupButton);
            this.Controls.Add(this.teamUserTreeView);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.typeTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.displayNameTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CallJobGroupEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CallJobGroupEdit";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CallJobGroupEdit_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox displayNameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox typeTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TreeView teamUserTreeView;
        private System.Windows.Forms.Button delCallJobGroupsButton;
        private System.Windows.Forms.Button selectCallJobGroupButton;
    }
}