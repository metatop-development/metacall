using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class CreateDurringClosure : UserControl
    {
        private int formHeight = 0;
        private int formHeightRest = 0;

        public CreateDurringClosure()
        {
            InitializeComponent();
        }

        public int FormHeight
        {
            get { return formHeight; }
        }

        public int FormHeightRest
        {
            set
            {
                formHeightRest = value;

                this.Height = formHeightRest;

                grpQuestion.Parent = this;
                grpQuestion.Location = new Point(10, 0);
                grpQuestion.Size = new Size(480, formHeightRest - 5);

                formHeight = formHeightRest;

            }
        }
    }
}
