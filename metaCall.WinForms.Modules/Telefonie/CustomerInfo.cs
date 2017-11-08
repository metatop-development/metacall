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
    public partial class CustomerInfo : ExpandableUserControl, IInitializeCustomer
    {
        private Customer customer;

        public Customer Customer
        {
            get { return customer; }
        }

        public CustomerInfo()
        {
            InitializeComponent();
        }

        private void UpdateCustomerInformations()
        {

            if (this.customer != null)
            {
                
                this.lblStrasse.Text = customer.Strasse;
                this.lblDisplayResidence.Text = customer.DisplayResidence;

                this.lblContactDisplay.Text = customer.ContactPerson.DisplayName;

            }
            else
            {
                this.lblStrasse.Text = string.Empty;
                this.lblDisplayResidence.Text = string.Empty;

                this.lblContactDisplay.Text = string.Empty;

            }
        }

        public void InitializeCustomer(Customer customer)
        {
            this.customer = customer;
            UpdateCustomerInformations();
        }
    }
}
