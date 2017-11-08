using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    public partial class mwProjectInfoAdditiveInfo : Form
    {
        public mwProjectInfoAdditiveInfo(string Venue)
        {
            InitializeComponent();

            this.AdditiveInfoLabel.Text = Venue;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}