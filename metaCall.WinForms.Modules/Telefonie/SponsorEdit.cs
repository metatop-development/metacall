using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;

using MaDaNet.Common.AppFrameWork.WinUI.Controls;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    public partial class SponsorEdit : Form
    {
        private Sponsor sponsor;
        private Sponsor originalSponsor;
        private Language language;
        private IDictionary<string, string> salutations;

        private List<Branch> branchList;
        private List<BranchGroup> branchGroupList;

        //public event SponsorPhoneChangeEventHandler SponsorEditPhoneChanged;

        Branch selectedBranch;
        BranchGroup selectedBranchGroup;

        bool checkValidDataForOrdering = false;

        public SponsorEdit()
        {
            InitializeComponent();
        }

        public SponsorEdit(Sponsor sponsor, Language language)
            : this()
        {
            if (sponsor == null)
                throw new ArgumentNullException("sponsor");

            this.landComboBox.DataSource = MetaCall.Business.Addresses.Countries;
            this.salutations = MetaCall.Business.Addresses.GetSalutaions(language);

            this.contactPersonAnredeComboBox.DataSource = new string[] {"", "Herr", "Frau" };

            this.contactPersonAnredeComboBox.SelectedIndex = -1;

            FillBranch_BranchGroupComboBox();
            this.checkBoxTransferToSponsor.Checked = false;
            this.language = language;
            this.originalSponsor = sponsor;
            this.sponsor = MetaCall.Business.GetDeepCopy<Sponsor>(this.originalSponsor);
            this.sponsor.PropertyChanged += new PropertyChangedEventHandler(sponsor_PropertyChanged);
            this.sponsor.ContactPerson.PropertyChanged += new PropertyChangedEventHandler(ContactPerson_PropertyChanged);

            BindSponsor();
        }

        public SponsorEdit(Sponsor sponsor, Language language, bool checkValidDataForOrdering)
            : this(sponsor, language)
        {

            this.checkValidDataForOrdering = checkValidDataForOrdering;
            if (checkValidDataForOrdering)
            {
               List<string> wrongFields;
               bool isValid = MetaCall.Business.Addresses.IsSponsorValidForOrdering(sponsor, out wrongFields);
               if (!isValid)
               {
                   //TODO: Meldung anzeigen oder einblenden
                   // Evtl. einen Timer starten der die erforderlichen Daten in 
                   // regelmäßigem Abstand prüft
                   ;
               }
            }
        }

        //protected void OnSponsorEditPhoneChange(SponsorPhoneChangeEventArgs e)
        //{
        //    if (SponsorEditPhoneChanged != null)
        //        SponsorEditPhoneChanged(this, e);
        //}
        
        /// <summary>
        /// Sucht die Branche aufgrund der Branchennummer und wählt diese in der ComboBox aus
        /// </summary>
        /// <param name="BranchNumber"></param>
        private void SetSelectedBranch_BranchGroup()
        {
            if (this.isInitializing) return;

            //Wenn die Branchennummer null ist wird nichts selektiert
            if (this.selectedBranch == null && this.selectedBranchGroup == null)
            {
                this.Branch_BranchGroupComboBox.SelectedItem = null;
                return;
            }

            if (this.selectedBranch.Equals(Branch.Unknown) && this.selectedBranchGroup == null)
            {
                this.Branch_BranchGroupComboBox.SelectedItem = null;
                return;
            }

            if (this.selectedBranch == null && this.selectedBranchGroup != null)
            {
                //Ansonsten wird in der Liste die BranchenGruppe gesucht 
                // und dem ComboFeld als selectedItem 
                // zugewiesen
                foreach (BranchGroup branchGroupItem in this.branchGroupList)
                {
                    if (branchGroupItem.BranchenGruppenID.Equals(this.selectedBranchGroup.BranchenGruppenID))
                    {
                        this.Branch_BranchGroupComboBox.SelectedItem = branchGroupItem;
                        return;
                    }
                }
                return;
            }

            if (this.selectedBranch.Equals(Branch.Unknown) && this.selectedBranchGroup != null)
            {
                //Ansonsten wird in der Liste die BranchenGruppe gesucht 
                // und dem ComboFeld als selectedItem zugewiesen
                foreach (BranchGroup branchGroupItem in this.branchGroupList)
                {
                    if (branchGroupItem.BranchenGruppenID.Equals(this.selectedBranchGroup.BranchenGruppenID))
                    {
                        this.Branch_BranchGroupComboBox.SelectedItem = branchGroupItem;
                        return;
                    }
                }
                return;
            }

            //Ansonsten wird in der Liste die Branche gesucht 
            // und dem ComboFeld als selectedItem zugewiesen
            foreach (Branch branchItem in this.branchList)
            {
                if (branchItem.Branchennummer.Equals(this.selectedBranch.Branchennummer))
                {
                    this.Branch_BranchGroupComboBox.SelectedItem = branchItem;
                    return;
                }
            }
            selectedBranchGroup = selectedBranch.BranchGroup;
        }   

        public Branch SelectedBranch
        {
            get
            {
                return this.selectedBranch;
            }
            set
            {
                if (this.isInitializing) return;
                selectedBranch = value;
                SetSelectedBranch_BranchGroup();
            }
        }

        public BranchGroup SelectedBranchGroup
        {
            get
            {
                return this.selectedBranchGroup;
            }
            set
            {
                if (this.isInitializing) return;
                selectedBranchGroup = value;
                SetSelectedBranch_BranchGroup();
            }
        }

        void ContactPerson_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateContactPersonSalutationInfo((ContactPerson)sender);
        }

        void sponsor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateCompleteAddressInfo();
        }

        private void UpdateCompleteAddressInfo()
        {
            this.completeAddressInfoLabel.Text = this.sponsor.GetCompleteAdress();
        }

        private void UpdateEinzugsermaechtigungAusgabe()
        {
            if ( this.checkBoxEinzug.Checked == true)
            {
                this.bankCodeTextBox.Enabled = true;
                this.bankTextBox.Enabled = true;
                this.accountNumberTextbox.Enabled = true;
            }
            else
            {
                this.bankCodeTextBox.Enabled = false;
                this.bankTextBox.Enabled = false;
                this.accountNumberTextbox.Enabled = false;
                this.bankCodeTextBox.Text = null;
                this.bankTextBox.Text = null;
                this.accountNumberTextbox.Text = null;
            }
        }

        private void UpdateContactPersonSalutationInfo(ContactPerson contactPerson)
        {
            string salutation = string.Empty;
            string key = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (contactPerson != null &&
                contactPerson.Anrede != null &&
                this.salutations.ContainsKey(contactPerson.Anrede))
            {
                key = contactPerson.Anrede;


                if (!string.IsNullOrEmpty(contactPerson.Titel))
                    sb.Append(contactPerson.Titel);

                if (sb.Length > 0) sb.Append(" ");

                if (!string.IsNullOrEmpty(contactPerson.Nachname))
                    sb.Append(contactPerson.Nachname);
            }

            salutation = this.salutations[key];

            contactPersonSalutationLabel.Text = string.Format(salutation, sb.ToString());

            if (this.checkBoxTransferToSponsor.Checked == true)
            {
                this.text2TextBox.Text = contactPerson.DisplayName;
                this.sponsor.Text2 = contactPerson.DisplayName;
                UpdateCompleteAddressInfo();
            }
        }

        private void BindSponsor()
        {
                        
            this.anredeTextBox.DataBindings.Add("Text", this.sponsor, "Anrede");
            this.vornameTextBox.DataBindings.Add("Text", this.sponsor, "Vorname");
            this.nachnameTextBox.DataBindings.Add("Text", this.sponsor, "Nachname");
            this.text1TextBox.DataBindings.Add("Text", this.sponsor, "Text1");
            this.text2TextBox.DataBindings.Add("Text", this.sponsor, "Text2");
            this.strasseTextBox.DataBindings.Add("Text", this.sponsor, "Strasse");
            this.landComboBox.DataBindings.Add("SelectedItem", this.sponsor, "Land");
            this.plzTextBox.DataBindings.Add("Text", this.sponsor, "PLZ");
            this.OrtTextBox.DataBindings.Add("Text", this.sponsor, "Ort");
            this.telefonTextBox.DataBindings.Add("Text", this.sponsor, "TelefonNummer");
            this.faxTextBox.DataBindings.Add("Text", this.sponsor, "FaxNummer");
            this.mobilTextBox.DataBindings.Add("Text", this.sponsor, "MobilNummer");
            this.alternativTextBox.DataBindings.Add("Text", this.sponsor.Additions, "Phone3");
            this.eMailTextBox.DataBindings.Add("Text", this.sponsor, "EMail");
            this.checkBoxEinzug.DataBindings.Add("Checked", this.sponsor, "EinzugsermaechtigungAusgabe");
            this.bankCodeTextBox.DataBindings.Add("Text", this.sponsor, "BankNumber");
            this.bankTextBox.DataBindings.Add("Text", this.sponsor, "Bank");
            this.accountNumberTextbox.DataBindings.Add("Text", this.sponsor, "AccountNumber");
            this.zusatzTextBox.DataBindings.Add("Text", this.sponsor, "Zusatz");                                                     
            this.contactPersonAnredeComboBox.DataBindings.Add("SelectedItem", this.sponsor.ContactPerson, "Anrede");
            this.contactPersonTitelTextBox.DataBindings.Add("Text", this.sponsor.ContactPerson, "Titel");
            this.contactPersonVornameTextBox.DataBindings.Add("Text", this.sponsor.ContactPerson, "Vorname");
            this.contactPersonNachnameTextBox.DataBindings.Add("Text", this.sponsor.ContactPerson, "Nachname");
            this.webAdresseTextBox.DataBindings.Add("Text", this.sponsor, "Webadresse");
            this.sponsorenUrkunde1TextBox.DataBindings.Add("Text", this.sponsor, "SponsorenUrkunde1");
            this.sponsorenUrkunde2TextBox.DataBindings.Add("Text", this.sponsor, "SponsorenUrkunde2");

            selectedBranchGroup = this.sponsor.BranchGroup;
            selectedBranch = this.sponsor.Branch;
            if (selectedBranchGroup == null && (selectedBranch == null || selectedBranch.Equals(Branch.Unknown)))
            {
                selectedBranch = null;
            }
            SetSelectedBranch_BranchGroup();

            UpdateCompleteAddressInfo();
            UpdateContactPersonSalutationInfo(this.sponsor.ContactPerson);
            UpdateEinzugsermaechtigungAusgabe();
        }

        private void FillBranch_BranchGroupComboBox()
        {
            if (this.isInitializing) return;

            this.branchGroupList = MetaCall.Business.BranchGroup.BranchGroups;

            this.Branch_BranchGroupComboBox.Items.Clear();

            if (this.branchGroupList.Count > 0)
            {
                this.Branch_BranchGroupComboBox.Items.Add(" ");
                this.Branch_BranchGroupComboBox.Items.Add("[ Branchengruppe ]");
                this.Branch_BranchGroupComboBox.Items.Add("------------------------------------------------------------------------");
            }

            foreach (BranchGroup branchGroups in this.branchGroupList)
            {
                this.Branch_BranchGroupComboBox.Items.Add(branchGroups);
            }

            if (this.branchGroupList.Count > 0)
            {
                this.Branch_BranchGroupComboBox.Items.Add(" ");
            }

            this.branchList = MetaCall.Business.Branch.Branches;
            if (this.branchList.Count > 0)
            {
                this.Branch_BranchGroupComboBox.Items.Add("[ Branche ]");
                this.Branch_BranchGroupComboBox.Items.Add("------------------------------------------------------------------------");

                foreach (Branch branch in this.branchList)
                {
                    this.Branch_BranchGroupComboBox.Items.Add(branch);
                }
                this.Branch_BranchGroupComboBox.Items.Add(Branch.Unknown);

            }
        }

        private void SponsorEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
 
            if (DialogResult == DialogResult.OK)
            {
                if (!this.Validate()) 
                    return;

                //Kopiert den alle Eigenschaften des Sponsors in den OrginalSponsor
                Type type = typeof(Sponsor);

                System.Reflection.PropertyInfo[] properties = type.GetProperties();

                foreach (System.Reflection.PropertyInfo property in properties)
                {

                    if (property.CanWrite && property.CanRead)
                    {
                        property.SetValue(this.originalSponsor,
                            property.GetValue(this.sponsor, null),
                            null);
                    }
                }
                this.originalSponsor.Branch = this.selectedBranch;
                this.originalSponsor.BranchGroup = this.selectedBranchGroup;

                //OnSponsorEditPhoneChange(new SponsorPhoneChangeEventArgs(this.originalSponsor));
            }
        }

        private void checkBoxEinzug_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEinzugsermaechtigungAusgabe();
        }

        private void Branch_BranchGroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Branch_BranchGroupComboBox.SelectedItem != null)
            {
                if (this.Branch_BranchGroupComboBox.SelectedItem is Branch)
                {
                    selectedBranch = (Branch)this.Branch_BranchGroupComboBox.SelectedItem;
                    selectedBranchGroup = selectedBranch.BranchGroup;
                }
                else if (this.Branch_BranchGroupComboBox.SelectedItem is BranchGroup)
                {
                    selectedBranch = Branch.Unknown;
                    selectedBranchGroup = (BranchGroup)this.Branch_BranchGroupComboBox.SelectedItem;
                }
                else
                {
                    SetSelectedBranch_BranchGroup();
                }
            }
        }

        #region ISupportInitialize Member

        bool isInitializing;
        public void BeginInit()
        {
            this.isInitializing = true;
        }

        public void EndInit()
        {
            this.isInitializing = false;
        }

        #endregion


        private void SponsorEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
                e.Cancel = !DoValidate();
        }

        private bool DoValidate()
        {
            if (this.checkValidDataForOrdering)
            {

                List<string> wrongFields;
                bool isValid = MetaCall.Business.Addresses.IsSponsorValidForOrdering(this.sponsor, out wrongFields);

                if (!isValid)
                {
                    StringBuilder sb = new StringBuilder();

                    if (wrongFields.Count > 1)
                    {
                        sb.Append("Die folgenden Felder weisen ungültige Werte auf:\n");
                        foreach (string field in wrongFields)
                        {
                            sb.AppendFormat("\t{0}\n", field);
                        }
                    }
                    else
                        sb.AppendFormat("das Feld '{0}' weist einen ungültigen Werte auf.", wrongFields[0]);

                    MessageBox.Show(this, sb.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return isValid;
            }

            return true;
        }

        private void checkBoxTransferToSponsor_CheckedChanged(object sender, EventArgs e)
        {
            UpdateContactPersonSalutationInfo(this.sponsor.ContactPerson);
        }

        private void Branch_BranchGroupComboBox_NotInList(object sender, CancelEventArgs e)
        {
            if (SelectedBranchGroup != null)
            {
                MessageBox.Show("Sie können nur einen Eintrag aus der Liste wählen!", "Kein Element der Liste");
                e.Cancel = true;
            }
        }

        private void sponsorUrkundeButton_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            if(!string.IsNullOrEmpty(this.vornameTextBox.Text))
            {
                sb.Append(this.vornameTextBox.Text);
                sb.Append(" ");
            }
            sb.Append(this.nachnameTextBox.Text);

            this.sponsorenUrkunde1TextBox.Text = sb.ToString();
            this.sponsor.SponsorenUrkunde1 = sb.ToString();
        }

        private void webAdresseButton_Click(object sender, EventArgs e)
        {
            string webadresse;
            if(!string.IsNullOrEmpty(this.webAdresseTextBox.Text))
            {
                if (this.webAdresseTextBox.Text.StartsWith("http"))
                    webadresse = this.webAdresseTextBox.Text;
                else
                    webadresse = "http://" + this.webAdresseTextBox.Text;

                System.Diagnostics.Process.Start(webadresse);
            }
        }

    }
}