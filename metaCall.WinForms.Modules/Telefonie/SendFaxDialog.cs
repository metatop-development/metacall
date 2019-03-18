using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using metatop.Applications.metaCall.DataObjects;
using System.Drawing.Printing;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    public partial class SendFaxDialog : Form
    {
        private const int blinkFrequency = 500;//1/4 of a second
        private const string mailBetreff = "Förderaktion";
        private const string projektBezeichnungListenerMailBetreff = "PluSport";

        private MessageTranferMode messageTranferMode = MessageTranferMode.Fax;
        private CallJob callJob;
        private DocumentCategory category;

        List<ProjectDocument> documents;
        List<mwProjekt_SponsorPacket> sponsorPackets;

        public SendFaxDialog()
        {
            InitializeComponent();
            faxLabelTimer.Interval = blinkFrequency;
            reminderFaxSelectionLabel.Visible = false;
        }

        public SendFaxDialog(CallJob callJob, DocumentCategory category)  : this(callJob, category, MessageTranferMode.Fax)
        {
        }

        public SendFaxDialog(CallJob callJob, DocumentCategory category, MessageTranferMode transferMode)
            : this()
        {
            if (callJob == null)
            {
                throw new ArgumentNullException("callJob");
            }

            this.callJob = callJob;
            this.category = category;
            this.MessageTransferMode = transferMode;

            FillControls();

            Application.Idle += new EventHandler(Application_Idle);
        }

        void Application_Idle(object sender, EventArgs e)
        {
            
            bool addressFilled = (
                 ((this.messageTranferMode == MessageTranferMode.Fax) && (!string.IsNullOrEmpty(this.faxNumberTextBox.Text))) ||
                 ((this.messageTranferMode == MessageTranferMode.EMail) && (!string.IsNullOrEmpty(this.eMailTextBox.Text))) ||
                 ((this.messageTranferMode == MessageTranferMode.PrintOut) && (this.printersComboBox.SelectedIndex > -1))
                 );

            if (this.SelectedDocument != null)
            {
                ProjectDocument projectDocument = this.SelectedDocument;
                if (projectDocument.PacketSelect == true)
                {
                    this.sponsorPacketGroupBox.Enabled = true;
                    this.okButton.Enabled = addressFilled &&
                        SponsorPacketsSelected.Count > 0 && 
                        SponsorPacketsSelected.Count < 4;
                }
                else
                {
                    this.sponsorPacketGroupBox.Enabled = false;
                    this.okButton.Enabled = addressFilled;
                }
            }
            else
            {
                this.sponsorPacketGroupBox.Enabled = false;
            }

            if (this.SelectedEmailTemplate == null)
            {
                eMailRadioButton.Enabled = false;
            }
            else
            {
                eMailRadioButton.Enabled = true;
            }
        }

        private string GetSalutation(ProjectInfo projectInfo, Sponsor sponsor)
        {
            string salutation = string.Empty;
            string key = string.Empty;
            StringBuilder sb = new StringBuilder();
            IDictionary<string, string> salutations;
            
            mwProject mwp = MetaCall.Business.Projects.GetMwProject(projectInfo);
            salutations = MetaCall.Business.Addresses.GetSalutaions(mwp.Language);

            if (sponsor != null &&
                sponsor.ContactPerson != null &&
                sponsor.ContactPerson.Anrede != null &&
                salutations.ContainsKey(sponsor.ContactPerson.Anrede))
            {
                key = sponsor.ContactPerson.Anrede;

                if (!string.IsNullOrEmpty(sponsor.ContactPerson.Titel))
                {
                    sb.Append(sponsor.ContactPerson.Titel);
                }

                if (sb.Length > 0)
                {
                    sb.Append(" ");
                }

                if (!string.IsNullOrEmpty(sponsor.ContactPerson.Nachname))
                {
                    sb.Append(sponsor.ContactPerson.Nachname);
                }
            }

            salutation = salutations[key];

            return string.Format(salutation, sb.ToString());
        }

        private void FillControls()
        {
            if (this.callJob == null)
            {
                return;
            }

            ProjectInfo project = this.callJob.Project;
            Sponsor sponsor = this.callJob.Sponsor;

            string betreff = "";

            if (project.BezeichnungRechnung.Contains(projektBezeichnungListenerMailBetreff) == false)
            {
                betreff = mailBetreff + " ";
            }

            betreff += project.BezeichnungRechnung;

            this.ProjektTextBox.Text = betreff;
            this.ProjektTextBox.Enabled = false;
            this.AnredeTextBox.Text = GetSalutation(project, sponsor); //briefanrede

            FillFaxTemplateGroupBox();
            FillSponsorPacketGroupBox();
            FillEmailTemplateGroupBox();

            InitPrintersComboBox();

            this.projectInfoLabel.Text = project.Bezeichnung;
            this.faxNumberTextBox.Text = sponsor.FaxNummer;
            this.eMailTextBox.Text = sponsor.EMail;

            SetCountryPhoneNumber();

            ToggleTransferModeControls();
        }

        private void SetCountryPhoneNumber()
        {
            if (string.IsNullOrEmpty(this.callJob.Sponsor.Land))
            {
                return;
            }

            CountryPhoneNumber countryPhoneNumber = MetaCall.Business.CountryPhoneNumber.GetCountryCodeNumber(this.callJob.Sponsor.Land);
            string currentNumber = this.faxNumberTextBox.Text;

            if (string.IsNullOrEmpty(currentNumber))
            {
                //Faxnummer ist leer, also Ländervorwahl anzeigen
                this.faxNumberTextBox.Text = countryPhoneNumber.PhoneNumber;
            }
            else
            {
                if (currentNumber.Substring(0, countryPhoneNumber.PhoneNumber.Length) != countryPhoneNumber.PhoneNumber)
                {
                    //ländervorwahl ist nicht enthalten
                    if (currentNumber.Substring(0, 1) == "0")
                    {
                        //aber es gibt eine führende 0, diese wird entfernt
                        currentNumber = countryPhoneNumber.PhoneNumber + currentNumber.Substring(1);
                    }
                    else
                    {
                        //keine führende 0 wird also komplett ausgegeben
                        currentNumber = countryPhoneNumber.PhoneNumber + currentNumber;
                    }
                }
                else
                {
                    //ländervorwahl ist enthalten, überprüfen ob danach eine 0 kommt, diese muss entfernt werden.
                    if(currentNumber.Substring(countryPhoneNumber.PhoneNumber.Length + 1, 1) == "0")
                    {
                        currentNumber = currentNumber.Substring(0, countryPhoneNumber.PhoneNumber.Length) + currentNumber.Substring(countryPhoneNumber.PhoneNumber.Length + 1);
                    }
                }
            }

            this.faxNumberTextBox.Text = currentNumber;
        }

        private void ToggleTransferModeControls()
        {
            switch (this.messageTranferMode)
            {
                case MessageTranferMode.Fax:
                    this.faxNumberTextBox.Enabled = true;
                    this.eMailTextBox.Enabled = false;
                    this.printersComboBox.Enabled = false;
                    this.faxTemplateGroupBox.Text = "Faxvorlage";
                    this.emailAreaGroupbox.Enabled = false;
                    break;
                case MessageTranferMode.EMail:
                    this.faxNumberTextBox.Enabled = false;
                    this.eMailTextBox.Enabled = true;
                    this.printersComboBox.Enabled = false;
                    this.faxTemplateGroupBox.Text = "Emailanhang";
                    this.emailAreaGroupbox.Enabled = true;
                    break;
                case MessageTranferMode.PrintOut:
                    this.faxNumberTextBox.Enabled = false;
                    this.eMailTextBox.Enabled = false;
                    this.printersComboBox.Enabled = true;
                    this.faxTemplateGroupBox.Text = "Faxvorlage";
                    this.emailAreaGroupbox.Enabled = false;
                    break;
            }
        }

        private void FillSponsorPacketGroupBox()
        {
            int mwProject = (int)(this.callJob.Project.mwProjektNummer == null ? -1 : this.callJob.Project.mwProjektNummer );
            this.sponsorPackets = MetaCall.Business.mwProjekt_SponsorPacket.mwProjekt_SponsorPackets(mwProject);

            int x = this.sponsorPacketGroupBox.Margin.Horizontal;
            int y = x * 3;
            int maxWidth = 100;

            for (int i = 0; i < sponsorPackets.Count; i++)
            {
                if (sponsorPackets[i].FaxText1_de != null)
                {
                    CheckBox box = new CheckBox();
                    box.AutoSize = true;
                    box.Location = new Point(x, y);
                    box.Text = sponsorPackets[i].Bezeichnung.ToString();
                    box.Parent = this.sponsorPacketGroupBox;
                    box.Tag = sponsorPackets[i];
                    box.TabStop = true;

                    y += (box.Height + box.Margin.Horizontal);
                    maxWidth = Math.Max(maxWidth, box.Width);

                    if ((y + box.Height) > this.sponsorPacketGroupBox.ClientSize.Height)
                    {
                        x += maxWidth + box.Margin.Vertical;
                        y = (this.sponsorPacketGroupBox.Margin.Horizontal * 3);
                        maxWidth = 0;
                    }
                }
            }
        }

        private void FillFaxTemplateGroupBox()
        {
            ProjectInfo project = this.callJob.Project;

            this.documents = MetaCall.Business.ProjectDocuments.GetDocumentsByProjectAndCategory(project, this.category);

            int x = this.faxTemplateGroupBox.Margin.Horizontal ;
            int y = x * 3;
            int maxWidth = 100;

            for (int i = 0; i < documents.Count; i++)
            {                
                RadioButton button = new RadioButton();
                button.AutoSize = true;
                button.Location = new Point(x, y);
                button.Text = documents[i].DisplayName;
                button.Parent = this.faxTemplateGroupBox;
                button.Tag = documents[i];
                button.TabStop = true;

                if (i == 0)
                {
                    button.Select();
                }

                y += (button.Height + button.Margin.Horizontal);
                maxWidth  = Math.Max(maxWidth , button.Width);

                if ((y + button.Height) > this.faxTemplateGroupBox.ClientSize.Height)
                {
                    x+= maxWidth + button.Margin.Vertical;
                    y = (this.faxTemplateGroupBox.Margin.Horizontal * 3);
                    maxWidth = 0;
                }
            }
        }

        private void FillEmailTemplateGroupBox()
        {
            ProjectInfo project = this.callJob.Project;

            this.documents = MetaCall.Business.ProjectDocuments.GetDocumentsByProjectAndCategory(project, DocumentCategory.Emailvorlage);

            int x = this.emailTemplateGroupBox.Margin.Horizontal;
            int y = x * 3;
            int maxWidth = 100;

            for (int i = 0; i < documents.Count; i++)
            {
                RadioButton button = new RadioButton();
                button.AutoSize = true;
                button.Location = new Point(x, y);
                button.Text = documents[i].DisplayName;
                button.Parent = this.emailTemplateGroupBox;
                button.Tag = documents[i];
                button.TabStop = true;

                if (i == 0)
                {
                    button.Select();
                }

                y += (button.Height + button.Margin.Horizontal);
                maxWidth = Math.Max(maxWidth, button.Width);

                if ((y + button.Height) > this.emailTemplateGroupBox.ClientSize.Height)
                {
                    x += maxWidth + button.Margin.Vertical;
                    y = (this.emailTemplateGroupBox.Margin.Horizontal * 3);
                    maxWidth = 0;
                }
            }
        }


        private void InitPrintersComboBox()
        {
            this.printersComboBox.Items.Clear();

            int defaultPrinter = -1;

            PrinterSettings settings = new PrinterSettings();

            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                string printerName = PrinterSettings.InstalledPrinters[i];

                // Der ActiveFax-Drucker darf nocht ausgewählt werden 
                if (string.Compare(printerName, "ActiveFax", true, System.Globalization.CultureInfo.InvariantCulture) == 0)
                {
                    continue;
                }

                settings.PrinterName = printerName;

                this.printersComboBox.Items.Add(printerName);

                if (settings.IsDefaultPrinter)
                {
                    defaultPrinter = i;
                }
                
            }

            if (this.printersComboBox.Items.Count > 0 )
            {
                if (defaultPrinter > -1)
                {
                    this.printersComboBox.SelectedIndex = defaultPrinter;
                }
                else
                {
                    this.printersComboBox.SelectedIndex = 0;
                }
            }
        }

        public List<mwProjekt_SponsorPacket> SponsorPacketsSelected
        {
            get
            {
                List<mwProjekt_SponsorPacket> sponsorPacketsSelected = new List<mwProjekt_SponsorPacket>();
                foreach (CheckBox box in this.sponsorPacketGroupBox.Controls)
                {
                    if (box.Checked)
                    {
                        sponsorPacketsSelected.Add((mwProjekt_SponsorPacket)box.Tag);
                    }
                }
                return sponsorPacketsSelected;
            }
        }

        public ProjectDocument SelectedDocument
        {
            get
            {
                foreach (RadioButton button in this.faxTemplateGroupBox.Controls)
                {
                    if (button.Checked)
                    {
                        return button.Tag as ProjectDocument;
                    }   
                }
                return null;
            }
        }

        public ProjectDocument SelectedEmailTemplate
        {
            get
            {
                foreach (RadioButton button in this.emailTemplateGroupBox.Controls)
                {
                    if (button.Checked)
                    {
                        return button.Tag as ProjectDocument;
                    }
                 }
                return null;
            }
        }


        public string FaxNumber
        {
            get
            {
                return this.faxNumberTextBox.Text;
            }
        }

        public string EMail
        {
            get
            {
              return this.eMailTextBox.Text;
            }
        }

        public string Betreff
        {
            get { return this.ProjektTextBox.Text; }
        }

        public string Briefanrede
        {
            get { return this.AnredeTextBox.Text; }
        }

        public string Printer
        {
            get
            {
                return this.printersComboBox.SelectedItem as string;
            }
        }

        public MessageTranferMode MessageTransferMode
        {
            get
            {
                return this.messageTranferMode;
            }
            private set
            {
                switch (value)
                {
                    case MessageTranferMode.Fax:
                        if (!this.faxRadioButton.Checked )
                            this.faxRadioButton.Checked = true;
                        break;
                    case MessageTranferMode.EMail:
                        if (!this.eMailRadioButton.Checked)
                            this.eMailRadioButton.Checked = true;
                        break;
                    case MessageTranferMode.PrintOut:
                        if (this.printOutRadioButton.Checked)
                            this.printOutRadioButton.Checked = true;
                        break;
                    default:
                        if (!this.faxRadioButton.Checked)
                            this.faxRadioButton.Checked = true;
                        break;
                }

                this.messageTranferMode = value;
                ToggleTransferModeControls();
                ShowFaxSelectionLabel();
            }
        }

        private void ShowFaxSelectionLabel()
        {
            ProjectInfo project = this.callJob.Project;
            this.documents = MetaCall.Business.ProjectDocuments.GetDocumentsByProjectAndCategory(project, DocumentCategory.Faxangebot);

            int docCounter = 0;

            if (this.documents != null)
            {
                foreach (var document in documents)
                {
                         docCounter++;
                }
            }

            if (this.messageTranferMode == MessageTranferMode.EMail && docCounter > 1)
            {
                faxLabelTimer.Start();
           }
            else
            {
                faxLabelTimer.Stop();
                reminderFaxSelectionLabel.Visible = false;
            }
        }

        private void SendFaxDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                if (this.callJob != null)
                {
                    if (this.messageTranferMode == MessageTranferMode.Fax)
                        this.callJob.Sponsor.FaxNummer = this.faxNumberTextBox.Text;

                    if (this.messageTranferMode == MessageTranferMode.EMail)
                        this.callJob.Sponsor.EMail = this.eMailTextBox.Text;
                }
            }
        }

        private void MessageTransferMode_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == this.faxRadioButton && this.faxRadioButton.Checked)
                this.MessageTransferMode = MessageTranferMode.Fax;

            if (sender == this.eMailRadioButton && this.eMailRadioButton.Checked)
                this.MessageTransferMode = MessageTranferMode.EMail;

            if (sender == this.printOutRadioButton && this.printOutRadioButton.Checked)
                this.MessageTransferMode = MessageTranferMode.PrintOut;
        }

        private void AnredeTextBox_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AnredeTextBox.Text) && AnredeTextBox.Text.EndsWith(","))
                AnredeTextBox.Text = AnredeTextBox.Text.Substring(0, AnredeTextBox.Text.Length - 1);
        }

        private void faxLabelTimer_Tick(object sender, EventArgs e)
        {
            reminderFaxSelectionLabel.Visible = !reminderFaxSelectionLabel.Visible;
        }
    }

    public enum MessageTranferMode
    {
        Fax,
        EMail,
        PrintOut,
    }
}