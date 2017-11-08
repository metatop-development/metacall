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
    public partial class mwProjectInfo : ExpandableUserControl, IInitializeCall
    {
        private Timer lostFocusTimer = new Timer();

        private DataTable dtMwprojekt_ProjektOrderHistorie;
        private DataTable dtThankingForms;
        
        private string venue;
        /// <summary>
        /// Liefert den Austragungsort oder legt diesen fest.
        /// </summary>
        public string Venue
        {
            get { return venue; }
            set { venue = value; }
        }

        private mwProject mwProject;
         /// <summary>
         /// Liefert das mwProjekt oder legt dieses fest.
         /// </summary>
        public mwProject MwProject
        {
            get { return mwProject; }
        }

        public mwProjectInfo()
        {
            InitializeComponent();
        }

        private void UpdateProjectInformations()
        {
            if (this.mwProject != null)
            {
                this.txtBezeichnung.Text = this.mwProject.Bezeichnung;
                this.txtAdditiveInfo.Text = this.venue;

                if (this.venue != null)
                {
                    if (this.venue.Length > 0)
                        this.AdditiveInfoButton.Visible = true;
                    else
                        this.AdditiveInfoButton.Visible = false;
                }
                else
                    this.AdditiveInfoButton.Visible = false;

                this.lblAnzahlProjects.Text = mwProject.AnzahlProjekte.ToString();
                this.lblMitgliederVerein.Text = mwProject.MitgliederVerein.ToString() + " / " + mwProject.MitgliederAbteilung.ToString();
            }
            else
            {
                this.txtBezeichnung.Text = string.Empty;
                this.txtAdditiveInfo.Text = string.Empty;
                this.lblAnzahlProjects.Text = string.Empty;
                this.lblMitgliederVerein.Text = string.Empty;
                this.AdditiveInfoButton.Visible = false;
            }
        }

        private DataColumn AddColumn(DataTable target, string name, string caption, Type dataType)
        {
            DataColumn col = new DataColumn();
            col.ColumnName = name;
            col.Caption = caption;
            col.DataType = dataType;

            target.Columns.Add(col);

            return col;
        }

        private void setupDataTableProjektOrders()
        {
            if (this.mwProject != null)
            {
                dtMwprojekt_ProjektOrderHistorie = new DataTable("Mwprojekt_ProjektOrderHistorie");

                AddColumn(dtMwprojekt_ProjektOrderHistorie, "Sponsor", "Sponsor", typeof(string));
                AddColumn(dtMwprojekt_ProjektOrderHistorie, "Stückzahl", "Stückzahl", typeof(decimal));
                AddColumn(dtMwprojekt_ProjektOrderHistorie, "Umsatz", "Umsatz", typeof(decimal));

                //BindComboBox(dtMwProjekt_SponsorPacket, this.cBContactTypesParticipation, 1, "ProjekteSponsorenpaketNummer");

                dtMwprojekt_ProjektOrderHistorie.Rows.Clear();

                foreach (mwProjekt_ProjektOrderHistorie mwprojekt_ProjektOrderHistorie in MetaCall.Business.mwProjekt_ProjekOrderHistorie.GetmwProjekt_ProjektOrderHistorie(this.mwProject.Projektnummer))
                {
                    object[] rowData = new object[]{
                    mwprojekt_ProjektOrderHistorie.Sponsor,
                    mwprojekt_ProjektOrderHistorie.Stueckzahl,
                    mwprojekt_ProjektOrderHistorie.Umsatz};

                    dtMwprojekt_ProjektOrderHistorie.Rows.Add(rowData);
                }

                dGVOrders.DataSource = dtMwprojekt_ProjektOrderHistorie;

                dGVOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dGVOrders.RowHeadersVisible = false;
                dGVOrders.ColumnHeadersVisible = false;
                dGVOrders.AutoGenerateColumns = false;
                dGVOrders.Columns[0].DefaultCellStyle.BackColor = System.Drawing.Color.Lavender; ;
                dGVOrders.Columns[1].DefaultCellStyle.BackColor = System.Drawing.Color.Lavender; ;
                dGVOrders.Columns[2].DefaultCellStyle.BackColor = System.Drawing.Color.Lavender; ;
                dGVOrders.Columns[2].DefaultCellStyle.Format = ("#,##0.00");
                dGVOrders.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dGVOrders.Columns[0].Width = 220;
                dGVOrders.Columns[1].Width = 40;
                dGVOrders.Columns[2].Width = 50;
            }
        }

        private void setupDataTableThankingForms()
        {
            if (this.mwProject != null)
            {
                dtThankingForms = new DataTable("ThankingForms");

                AddColumn(dtThankingForms, "BedankungsformDE", "BedankungsformDE", typeof(string));

                dtThankingForms.Rows.Clear();

                foreach (ThankingsFormsProject thankingsFormsProject in MetaCall.Business.ThankingsFormsProject.GetThankingsFormsByProject(this.mwProject.Projektnummer))
                {
                    object[] rowData = new object[]{
                    thankingsFormsProject.BedankungsformDe};

                    dtThankingForms.Rows.Add(rowData);
                }

                dGVThankingForms.DataSource = dtThankingForms;

                dGVThankingForms.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dGVThankingForms.RowHeadersVisible = false;
                dGVThankingForms.ColumnHeadersVisible = false;
                dGVThankingForms.AutoGenerateColumns = false;
                dGVThankingForms.Columns[0].DefaultCellStyle.BackColor = System.Drawing.Color.Lavender; ;

                dGVThankingForms.Columns[0].Width = 310;
            }
        }

        #region IInitializeCall Member

        public void InitializeCall(Call call)
        {
            this.mwProject = MetaCall.Business.Projects.GetMwProject(call.CallJob.Project);
            this.venue = call.CallJob.Project.Venue;

            List<ProjectCounts> projectCounts = MetaCall.Business.mwProjektBusiness.GetAllProjectCounts(null, call.CallJob.Project.ProjectId);
            if (projectCounts.Count == 0)
                this.lblProjectOrders.Text = "0";
            else
                this.lblProjectOrders.Text = projectCounts[0].Count.ToString();
            
            UpdateProjectInformations();
            setupDataTableProjektOrders();
            setupDataTableThankingForms();
        }

        #endregion

        private void AdditiveInfoButton_Click(object sender, EventArgs e)
        {
            mwProjectInfoAdditiveInfo mwPIAdditiveInfo = new mwProjectInfoAdditiveInfo(this.venue);
            mwPIAdditiveInfo.ShowDialog(this); 
        }
    }
}
