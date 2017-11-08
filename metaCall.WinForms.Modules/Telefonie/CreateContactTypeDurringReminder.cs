using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class CreateContactTypeDurringReminder : UserControl, ISupportInitialize, IInitializeCall
    {
        public event EventHandler<ContactTypeChangedEventArgs> ContactTypeChanged;

        private GroupBox grpContactType;
        private GroupBox grpPhoneNumber;
        private ContactType selectedItem;
        private int counterCT = 1;
        private int formHeight = 0;
        private PhoneArt phoneArt;

        private string telefonNummer;
        private string mobilNummer;
        private string alternativNummer;
        private string dialerNummer;

        private enum PhoneArt
        {
            Telefon,
            Mobil,
            Alternativ
        }

        public string TelefonNummer
        {
            get { return this.telefonNummer; }
            set { this.telefonNummer = value; }
        }

        public string MobilNummer
        {
            get { return this.mobilNummer; }
            set { this.mobilNummer = value; }
        }

        public string AlternativNummer
        {
            get { return this.alternativNummer; }
            set { this.alternativNummer = value; }
        }

        public string DialerNummer
        {
            set { this.dialerNummer = value; }
        }

        public void SetPhoneNumber()
        {
            ClearPhoneButtons();
            txtCurrentNumber.ReadOnly = false;
            btnAlternativ.Enabled = true;
            btnTelefon.Enabled = true;
            btnMobil.Enabled = true;
            this.txtCurrentNumber.Text = this.dialerNummer;

            if (this.dialerNummer == this.telefonNummer)
            {
                this.btnTelefon.ForeColor = System.Drawing.Color.Red;
                phoneArt = PhoneArt.Telefon;
            }
            else if (this.dialerNummer == this.mobilNummer)
            {
                this.btnMobil.ForeColor = System.Drawing.Color.Red;
                phoneArt = PhoneArt.Mobil;
            }
            else
            {
                this.btnAlternativ.ForeColor = System.Drawing.Color.Red;
                phoneArt = PhoneArt.Alternativ;
            }
        }

        public string UsedPhoneNumber
        {
            get
            {
                return this.txtCurrentNumber.Text;
            }
        }

        private void ClearPhoneButtons()
        {
            this.btnTelefon.ForeColor = System.Drawing.Color.Black;
            this.btnMobil.ForeColor = System.Drawing.Color.Black;
            this.btnAlternativ.ForeColor = System.Drawing.Color.Black;
        }

        public ContactType SelectedItem
        {
            get
            {
                return this.selectedItem;
            }
        }

        public CreateContactTypeDurringReminder()
        {
            InitializeComponent();
        }

        public int FormHeight
        {
            get { return formHeight; }
        }

        private void UpdateContactTypeInformations()
        {
            if (!DesignMode)
           {
               this.txtCurrentNumber.Text = this.telefonNummer;
               this.txtCurrentNumber.ReadOnly = true;

                grpPhoneNumber = new GroupBox();
                grpPhoneNumber.Parent = this;
                grpPhoneNumber.Location = new Point(10, 0);
                grpPhoneNumber.Size = new Size(480, 50);

                List<ContactType> contactTypes = MetaCall.Business.ContactType.ContactTypesDurringCallJob;

                int countContactTypes;
                int countRows;

                const int AnzahlColumns = 3;

                countContactTypes = contactTypes.Count;

                countRows = countContactTypes / AnzahlColumns;

                if ((countRows * AnzahlColumns) < countContactTypes)
                {
                    countRows++;
                }

                grpContactType = new GroupBox();
                grpContactType.Parent = this;
                grpContactType.Text = "Kontaktart";
                grpContactType.Location = new Point(10, 58);
                grpContactType.Size = new Size(480, ((1 + (contactTypes.Count / countRows)  ) * 20) + 30);
                int buttonStart = 10;

                foreach (ContactType contactType in contactTypes)
                {
                    
                    RadioButton radiobtn = new RadioButton();

                    radiobtn.Parent = grpContactType;
                    radiobtn.Visible = true;
                    radiobtn.Text = contactType.DisplayName;
                    radiobtn.Tag = contactType;
                    if (counterCT == 4)
                    {
                        counterCT = 1;
                        buttonStart = buttonStart + 155;
                    }
                    radiobtn.Location = new Point(buttonStart, counterCT * 20);
                    radiobtn.Size = new Size(150, 20);

                    radiobtn.CheckedChanged += new EventHandler(radiobtn_CheckedChanged);
                    counterCT++;

                }

                this.formHeight = grpContactType.Size.Height + grpPhoneNumber.Size.Height + 10; // +15;

            }
        }

        void radiobtn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radiobtn = sender as RadioButton;

            if (radiobtn == null)
                return;
            if (!radiobtn.Checked)
                return;

            ContactType contactType = radiobtn.Tag as ContactType;
            this.selectedItem = contactType;
            if (contactType == null)
                return;

            txtCurrentNumber.ReadOnly = true;
            btnAlternativ.Enabled = false;
            btnTelefon.Enabled = false;
            btnMobil.Enabled = false;

            OnContactTypeChanged(new ContactTypeChangedEventArgs(contactType));
        }

        protected void OnContactTypeChanged(ContactTypeChangedEventArgs e)
        {
            if (ContactTypeChanged != null)
                ContactTypeChanged(this, e);
        }

        private void btnMobil_Click(object sender, EventArgs e)
        {
            ClearPhoneButtons();
            this.btnMobil.ForeColor = System.Drawing.Color.Red;
            if (phoneArt == PhoneArt.Alternativ)
            {
                this.alternativNummer = this.txtCurrentNumber.Text;
            }
            phoneArt = PhoneArt.Mobil;

            this.txtCurrentNumber.Text = this.mobilNummer;
            this.txtCurrentNumber.ReadOnly = true;
        }

        private void btnTelefon_Click(object sender, EventArgs e)
        {
            ClearPhoneButtons();
            this.btnTelefon.ForeColor = System.Drawing.Color.Red;
            if (phoneArt == PhoneArt.Alternativ)
            {
                this.alternativNummer = this.txtCurrentNumber.Text;
            }
            phoneArt = PhoneArt.Telefon;

            this.txtCurrentNumber.Text = this.telefonNummer;
            this.txtCurrentNumber.ReadOnly = true;
        }

        private void btnAlternativ_Click(object sender, EventArgs e)
        {
            ClearPhoneButtons();
            this.btnAlternativ.ForeColor =  System.Drawing.Color.Red;
            phoneArt = PhoneArt.Alternativ;
            this.txtCurrentNumber.Text = this.alternativNummer;
            this.txtCurrentNumber.ReadOnly = false;
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

        #region IInitializeCall Member

        public void InitializeCall(Call call)
        {
            this.selectedItem = null;

            ResetSelection();
            
            this.telefonNummer = call.CallJob.Sponsor.TelefonNummer;
            this.mobilNummer = call.CallJob.Sponsor.MobilNummer;
            this.alternativNummer = null;

            this.dialerNummer = call.PhoneNumber;

            SetPhoneNumber();
            foreach (Control ctl in this.Controls)
            {
                IInitializeCall initializeCallControl = ctl as IInitializeCall;
                if (initializeCallControl != null)
                {
                    initializeCallControl.InitializeCall(call);
                }
            }
 
        }

        private void ResetSelection()
        {
            foreach (RadioButton btn in this.grpContactType.Controls)
            {
                btn.Checked = false;
            }
        }

        #endregion

        private void CreateContactTypeDurringReminder_Load(object sender, EventArgs e)
        {
            UpdateContactTypeInformations();
        }
    }
}
