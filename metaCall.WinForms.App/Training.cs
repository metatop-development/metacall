using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.WinForms.App
{
    public partial class Training : Form
    {
        public Training()
        {
            InitializeComponent();
        }

        private void BindTrainingGrunds()
        {
            List<TrainingGrund> trainingGrunds = new List<TrainingGrund>();

                trainingGrunds.AddRange(MetaCall.Business.TrainingGrund.GetAllTrainingGrund());

            this.comboBoxTrainingGrund.DisplayMember = "TrainingGrundItem";
            this.comboBoxTrainingGrund.DataSource = trainingGrunds;

            if (trainingGrunds.Count > 0)
            {
                comboBoxTrainingGrund.SelectedIndex = 0;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            TrainingGrund trainingGrund = comboBoxTrainingGrund.SelectedItem as TrainingGrund;

            MetaCall.Business.TrainingGrundItem = trainingGrund.TrainingGrundItem;
            MetaCall.Business.TrainingNotice = txtNotice.Text;
            MetaCall.Business.Training = false;

        }

        private void Training_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            BindTrainingGrunds();
        }
    }
}