using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.DataObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    [ToolboxItem(false)]
    public partial class CreateProject : Form
    {
        private List<CenterInfo> centerList;
        private List<mwProject> projectList;

        private mwProject selectedProject;
        public mwProject SelectedProject
        {
            get { return selectedProject; }
        }

        public DateTime Start
        {
            get
            {
                return this.startDateTimePicker.Value.Date;
            }
        }

        public DateTime Stop
        {
            get
            {
                if (this.stopDateTimePicker.Checked)
                    return this.stopDateTimePicker.Value.Date;
                else
                    return DateTime.MaxValue;
            }
        }

        public CenterInfo SelectedCenter
        {
            get
            {
                return this.targetCenterComboBox.SelectedItem as CenterInfo;
            }
        }

        public CreateProject()
        {
            InitializeComponent();

            this.startDateTimePicker.Value = DateTime.Today;
            //TODO: Anwendungseinstellungen
            this.stopDateTimePicker.Checked = false;
            this.stopDateTimePicker.Value = DateTime.Today.AddMonths(3);

            this.Text = "Neues Projekt aus metaware übernehmen.";

            Application.Idle += new EventHandler(Application_Idle);
        }

        void Application_Idle(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void BindCenters()
        {
            try
            {
                this.centerList = MetaCall.Business.Centers.Centers;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
            }

            this.sourceCenterComboBox.Items.Clear();
            this.targetCenterComboBox.Items.Clear();
            this.sourceCenterComboBox.DisplayMember = "Bezeichnung";
            this.targetCenterComboBox.DisplayMember = "Bezeichnung";

            foreach (CenterInfo center in this.centerList)
            {
                this.sourceCenterComboBox.Items.Add(center);
                this.targetCenterComboBox.Items.Add(center);
            }

            if (this.centerList.Count > 0)
            {
                this.sourceCenterComboBox.SelectedItem = this.centerList[0];
                this.targetCenterComboBox.SelectedItem = this.centerList[0];
            }


            
        }


        private void BindProjects(CenterInfo center)
        {
            try
            {
                this.projectList = MetaCall.Business.Projects.GetMwProjectsForTransfer(center);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
            }

            this.projectComboBox.Items.Clear();
            this.projectComboBox.DisplayMember = "Bezeichnung";

            foreach (mwProject project in this.projectList)
            {
                projectComboBox.Items.Add(project);
            }
        }

        private void sourceCenterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CenterInfo center = sourceCenterComboBox.SelectedItem as CenterInfo;

            if (center != null)
            {
                BindProjects(center);
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            if (!this.sourceCenterComboBox.DroppedDown)
                this.projectComboBox.Enabled = this.sourceCenterComboBox.SelectedItem != null;

            if (!this.projectComboBox.DroppedDown)
                this.targetCenterComboBox.Enabled = this.projectComboBox.SelectedIndex > -1;
            else
                this.targetCenterComboBox.Enabled = false;


            if (!this.targetCenterComboBox.DroppedDown && 
                this.targetCenterComboBox.SelectedIndex > -1 &&
                !this.sourceCenterComboBox.DroppedDown && 
                this.sourceCenterComboBox.SelectedIndex > -1 && 
                !this.projectComboBox.DroppedDown && 
                this.projectComboBox.SelectedIndex > -1)
            {
                this.okButton.Enabled = true;
            }
            else
            {
                this.okButton.Enabled = false;
            }
        }

        private void CreateProject_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
                BindCenters();
        }

        private void CreateProject_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                this.selectedProject = this.projectList[this.projectComboBox.SelectedIndex];
            }

            Application.Idle -= new EventHandler(Application_Idle);
        }

        private void stopDateTimePicker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void startDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            this.stopDateTimePicker.MinDate = this.startDateTimePicker.Value;
        }
    }
}