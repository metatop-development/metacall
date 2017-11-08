using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace metatop.Applications.metaCall.WinForms.App
{
    public partial class SplashForm : Form
    {
        private string statusInfo;

        public SplashForm()
        {
            InitializeComponent();
        }

        public string StatusInfo
        {
            get { return this.statusInfo; }
            set {
                statusInfo = value;
                ChangeStatusText();
            }
        }

        public void ChangeStatusText()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(this.ChangeStatusText));
                    return;
                }

                this.statusLabel.Text = this.statusInfo;
            }
            catch 
            {
                //	do something here...
                throw;
            }
        }

        
    }
}