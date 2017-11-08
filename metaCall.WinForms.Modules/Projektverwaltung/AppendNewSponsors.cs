using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Globalization;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    [ToolboxItem(false)]
    public partial class AppendNewSponsors : Form
    {

        public AppendNewSponsors()
        {
            InitializeComponent();



            this.startDateTimePicker.Value = DateTime.Today;
            //TODO: Anwendungseinstellungen
            this.stopDateTimePicker.Checked = false;
            this.stopDateTimePicker.Value = DateTime.Today.AddMonths(3);

            this.Text = "Neue Adressen aus metaware übernehmen.";

            Application.Idle += new EventHandler(Application_Idle);
        }

        void Application_Idle(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            //TODO: Implement method private void UpdateUI()
          //  throw new Exception("The method or operation is not implemented.");
        }


        public DateTime StartDate
        {
            get
            {
                return this.startDateTimePicker.Value;
            }
        }

        public DateTime StopDate
        {
            get
            {
                if (this.stopDateTimePicker.Checked)
                    return this.stopDateTimePicker.Value;
                else
                    return DateTime.MaxValue;
            }
        }

        private void AppendNewSponsors_Validating(object sender, CancelEventArgs e)
        {
            string msg = null;
            if (this.StopDate < this.StartDate)
            {
                msg = "Das Stopdatum muss größer oder gleich dem Startdatum sein.";
            }

            if (msg != null)
            {
                MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : 0;

                MessageBox.Show (this, msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, options);

                e.Cancel = true;

            }
        }

    }
}