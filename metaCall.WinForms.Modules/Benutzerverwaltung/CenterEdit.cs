using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    public partial class CenterEdit : Form
    {
        private Center center;
        bool isNewCenter;
        private List<mwCenter> mwCenters = new List<mwCenter>();
        
        public CenterEdit()
        {
            InitializeComponent();
        }

        public CenterEdit(Center center, bool isNewCenter):this()
        {
            if (center == null)
                throw new ArgumentNullException("center");

            this.center = center;
            this.isNewCenter = isNewCenter;
            FillmwCenterComboBox();
            UpdateControls();

            Application.Idle += new EventHandler(Application_Idle);

        }

        void Application_Idle(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (!this.mwCenterComboBox.DroppedDown)
                this.bezeichnungTextBox.ReadOnly = (this.mwCenterComboBox.SelectedIndex != 0);

            this.acceptButton.Enabled = !string.IsNullOrEmpty(this.bezeichnungTextBox.Text);
        }

        private void UpdateControls()
        {
            this.Text = string.Format("Center {0}", center.Bezeichnung);
            this.bezeichnungTextBox.Text = center.Bezeichnung;

            if (center.mwCenter == null)
            {
                this.mwCenterComboBox.SelectedIndex = 0;
            }
            else
            {
                foreach (mwCenter mwCenter in this.mwCenters)
                {
                    if (mwCenter.CenterNummer == center.mwCenter.CenterNummer)
                    {
                        this.mwCenterComboBox.SelectedItem = mwCenter;
                        break;
                    }
                }
            }
        
        }

        private void StoreToObject()
        {
            this.center.Bezeichnung = this.bezeichnungTextBox.Text;
            if (mwCenterComboBox.SelectedIndex > 0)
                this.center.mwCenter = this.mwCenterComboBox.SelectedItem as mwCenter;
            else
                this.center.mwCenter = null;
        
        }

        private void CenterEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                StoreToObject();
            }
            Application.Idle -= new EventHandler(this.Application_Idle);
        }

        private void CenterEdit_Load(object sender, EventArgs e)
        {
            FillmwCenterComboBox();
        }

        private void FillmwCenterComboBox()
        {
            this.mwCenters = MetaCall.Business.Centers.MetaWareCenters;
            int maxWidth = 0;
            Graphics grfx = CreateGraphics();

            mwCenterComboBox.Items.Clear();
            mwCenterComboBox.DisplayMember = "Bezeichnung";
            mwCenterComboBox.Items.Add("<keine Zuordnung>");
            foreach (mwCenter center in this.mwCenters)
            {
                mwCenterComboBox.Items.Add(center);

                SizeF size = grfx.MeasureString(center.Bezeichnung, this.mwCenterComboBox.Font);
                maxWidth = Math.Max(maxWidth, size.ToSize().Width);
            }

            this.mwCenterComboBox.DropDownWidth = maxWidth;
            
        }

        private void mwCenterComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            mwCenter selectedCenter = this.mwCenterComboBox.SelectedItem as mwCenter;

            if (selectedCenter != null)
            {
                this.bezeichnungTextBox.Text = selectedCenter.Bezeichnung;
            }
            UpdateUI();
        }

    }
}