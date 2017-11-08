using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    public partial class EnterPasswordForNewUser : Form
    {
        public EnterPasswordForNewUser()
        {
            InitializeComponent();

            Application.Idle += new EventHandler(Application_Idle);

        }

        void Application_Idle(object sender, EventArgs e)
        {
            string password1 = this.firstPasswordTextBox.Text;
            string password2 = this.secondPasswordTextbox.Text;

            this.okButton.Enabled = ((!string.IsNullOrEmpty(password1)) &&
                (!string.IsNullOrEmpty(password2)) &&
                (string.Compare(password1, password2, 
                                false, System.Globalization.CultureInfo.InvariantCulture) == 0));

        }

        private void EnterPasswordForNewUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                this.password = this.firstPasswordTextBox.Text;
            }

            Application.Idle -= new EventHandler(this.Application_Idle);
        }

        private string password ;
        public string Password
        {
            get { return password; }
        }
	
    }
}