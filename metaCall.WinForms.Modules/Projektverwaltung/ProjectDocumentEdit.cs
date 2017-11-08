using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;
using System.IO;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    public partial class ProjectDocumentEdit : Form
    {

        private ProjectDocument document;
        
        public ProjectDocumentEdit()
        {
            InitializeComponent();
        }

        public ProjectDocumentEdit(ProjectDocument document) :this()
        {
            this.document = document;


            if (string.IsNullOrEmpty(this.document.DisplayName))
            {
                this.Text = "neues Projektdokument erstellen";
            }
            else
            {
                this.Text = "Projektdokument bearbeiten";
            }

            FillCategoryComboBox();
            FillControls();

            Application.Idle += new EventHandler(Application_Idle);
        }

        void Application_Idle(object sender, EventArgs e)
        {
            if (categoryComboBox.SelectedIndex >= 0 && (DocumentCategory)categoryComboBox.SelectedValue == DocumentCategory.Faxangebot)
            {
                packetSelectLabel.Enabled = true;
                packetSelectCheckBox.Enabled = true;
            }
            else
            {
                 packetSelectLabel.Enabled = false;
                packetSelectCheckBox.Enabled = false;
            }

            //Der AcceptButton wird freigegeben wenn alle benötigten Felder gefüllt sind.
            this.acceptButton.Enabled =
                (!string.IsNullOrEmpty(this.displayNameTextBox.Text)) &&
                ((this.categoryComboBox.SelectedIndex > -1)) &&
                (!string.IsNullOrEmpty(this.filenameTextBox.Text));
        }

        private void FillControls()
        {
            if (this.document == null)
                return;

            this.displayNameTextBox.Text = this.document.DisplayName;
            this.categoryComboBox.SelectedValue = this.document.Category;
            this.filenameTextBox.Text = this.document.Filename;
            this.dateCreatedTextBox.Text = this.document.DateCreated.ToShortDateString();
            this.packetSelectCheckBox.Checked = this.document.PacketSelect;
        }

        private void FillCategoryComboBox()
        {
            List<DocumentCategoryInfo> categories = MetaCall.Business.ProjectDocuments.AllDocumentCategoryInfos;

            this.categoryComboBox.DisplayMember = "DisplayName";
            this.categoryComboBox.ValueMember = "Category";
            this.categoryComboBox.DataSource = categories;
        }

        private void StoreToObject()
        {
            if (this.document == null)
                return;

            this.document.DisplayName = this.displayNameTextBox.Text;
            this.document.Category = (DocumentCategory)this.categoryComboBox.SelectedValue;
            this.document.Filename = this.filenameTextBox.Text;
            this.document.PacketSelect = this.packetSelectCheckBox.Checked;

        }

        private void ProjectDocumentEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                StoreToObject();
            }
        }

        private void filenameButton_Click(object sender, EventArgs e)
        {
            FileDialog fileDialog = new OpenFileDialog();

            fileDialog.CheckFileExists = true;
            fileDialog.CheckPathExists = true;
            fileDialog.DereferenceLinks = true;
            if ((DocumentCategory)categoryComboBox.SelectedValue == DocumentCategory.Faxangebot)
            {
                fileDialog.Filter = "MS Word Dokumente (*.doc, *.docx)|*.doc; *.docx";
                fileDialog.Title = "MS Word Dokument wählen";
            }
            else
            {
                fileDialog.Filter = "Emailvorlage (*.txt, *.htm)|*.txt; *.htm; *.html)";
                fileDialog.Title = "Emailvorlage wählen";
            }
            fileDialog.ShowHelp = false;
            fileDialog.ValidateNames = true;
            if (!string.IsNullOrEmpty(this.document.Filename))
            {
                fileDialog.InitialDirectory = Path.GetDirectoryName(this.document.Filename);
                fileDialog.FileName = Path.GetFileName(this.document.Filename);
            }

            if (fileDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.filenameTextBox.Text = fileDialog.FileName;
            }
        }

    }
}