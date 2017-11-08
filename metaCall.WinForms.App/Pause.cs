using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace metatop.Applications.metaCall.WinForms.App
{
    public partial class Pause : Form
    {
        public Pause()
        {
            InitializeComponent();
            labelInfo.BackColor = Color.Transparent;
        }

        private void Pause_DoubleClick(object sender, EventArgs e)
        {
            MetaCall.Business.Pause = false;
        }
    }
}