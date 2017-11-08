using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MaDaNet.Common.AppFrameWork.ApplicationModul;
using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Globalization;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    public partial class UserWorkTimeAdditionInfoView : UserControl
    {
        private User_KeyData user_KeyData;
        private RecoveriesSum recoveriesSum;
        private DataTable dtUserWorkTimes = new DataTable();
        private DataTable dtOrderHistory = new DataTable();
        private DataTable dtRecoveries = new DataTable();
        private DataTable dtProjectOrders = new DataTable();
        private DataTable dtProjectCounts = new DataTable();
        private User user = new User();
        private DateTime from;
        private DateTime to;
        private DisplayForm displayForm;
        private DisplayFormConfirmed displayFormConfirmed;
        private AnalysisForm analysisForm;

        private enum DisplayForm
        {
            Group,
            All
        };

        private enum DisplayFormConfirmed
        {
            Confirmed,
            All
        };

        private enum AnalysisForm
        {
            TimeSelected,
            Accounting
        }

        private MetaCallPrincipal principal;

        public UserWorkTimeAdditionInfoView()
        {
            InitializeComponent();
        }

        public UserWorkTimeAdditionInfoView(User user, DateTime from, DateTime to): this()
        {
            this.principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;
            dtUserWorkTimes.Locale = CultureInfo.CurrentUICulture;
            dtOrderHistory.Locale = CultureInfo.CurrentUICulture;
            dtRecoveries.Locale = CultureInfo.CurrentUICulture;
            dtProjectCounts.Locale = CultureInfo.CurrentUICulture;
            dtProjectOrders.Locale = CultureInfo.CurrentUICulture;

            bindingSourceUserWorkTimeAdditionInfo.DataSource = dtUserWorkTimes;
            bindingSourceOrderDetails.DataSource = dtOrderHistory;
            bindingSourceRecoveries.DataSource = dtRecoveries;
            bindingSourceProjectCounts.DataSource = dtProjectCounts;
            bindingSourceProjectOrders.DataSource = dtProjectOrders;

            this.user = user;
            this.from = from;
            this.to = to;
            this.radioButtonViewAnalysisTimeSelect.Checked = true;

            if (this.from.Date == this.to.Date)
            {
                this.toolStripButtonChangeDisplayForm.Enabled = true;
            }
            else
            {
                this.toolStripButtonChangeDisplayForm.Enabled = false;
            }

            this.displayForm = DisplayForm.Group;
            this.displayFormConfirmed = DisplayFormConfirmed.All;

            if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName) || this.principal.IsInRole(MetaCallPrincipal.CenterAdminRoleName))
            {
                this.toolStripButtonChangeDisplayForm.Visible = true;
            }
            else
            {
                this.toolStripButtonChangeDisplayForm.Visible = false;
            }
            SetupDataTables();
        }

        public int UserWorkTimeAdditionInfoViewCount
        {
            get
            {
                return this.bindingSourceUserWorkTimeAdditionInfo.Count; ;
            }
        }

        private int SelectedSalaryStatementNumbers
        {
            get 
            {
                SalaryStatementNumbers selectedSalaryStatementNumbers = this.ComboBoxSalaryStatement.SelectedItem as SalaryStatementNumbers;
                if (selectedSalaryStatementNumbers != null)
                    return selectedSalaryStatementNumbers.SalaryStatementNumber;
                else
                    return 0;
            }
        }

        private ProjectInfo SelectedProjectOverview2
        {
            get
            {
                ProjectInfo selectedProjectOverview2 = this.comboBoxProjectsOverView2.SelectedItem as ProjectInfo;
                if (selectedProjectOverview2 != null)
                    return selectedProjectOverview2;
                else
                    return null;
            }
        }

        private ProjectInfo SelectedProjectOverview3
        {
            get
            {
                ProjectInfo selectedProjectOverview3 = this.comboBoxProjectsOverView3.SelectedItem as ProjectInfo;
                if (selectedProjectOverview3 != null)
                    return selectedProjectOverview3;
                else
                    return null;
            }
        }

        private DisplayForm DisplayFormCurrent
        {
            get
            {
                return this.displayForm;
            }
            set
            {
                this.displayForm = value;
                LoadWorkTimeIntoDataTable();
            }
        }


        private DisplayFormConfirmed DisplayFormConfirmedCurrent
        {
            get
            {
                return this.displayFormConfirmed;
            }
            set
            {
                this.displayFormConfirmed = value;
                SetFilter();
            }
        }

        private AnalysisForm AnalysisFormCurrent
        {
            get
            {
                return this.analysisForm;
            }
            set
            {
                this.analysisForm = value;
                LoadRecoveriesDetailsIntoDataTable();
            }
        }

        private void SetFilter()
        {
            StringBuilder filter = new StringBuilder();

            if(DisplayFormConfirmedCurrent == DisplayFormConfirmed.Confirmed)
                filter.AppendFormat("Confirmed = false");

            if (filter.Length > 0)
                this.bindingSourceUserWorkTimeAdditionInfo.Filter = filter.ToString();
            else
                this.bindingSourceUserWorkTimeAdditionInfo.RemoveFilter();
        }

        public ToolStripMenuItem[] CreateMainMenuItems()
        {
            return null;
        }

        private void UserWorkTimeInfoInfoView_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
        }

        private void SetupDataTables()
        {
            SetupDataTableWorkTime();
            LoadKeyDatas();
            FillComboBoxProjectsOverView();

        }

        private void SetupDataTableRecoveries()
        {
            DataTableHelper.AddColumn(this.dtRecoveries, "InvoiceNumber", "Re.-Nr.", typeof(int));
            DataTableHelper.AddColumn(this.dtRecoveries, "State", "Status", typeof(string));
            DataTableHelper.AddColumn(this.dtRecoveries, "PaymentDate", "bezahlt am", typeof(string));
            DataTableHelper.AddColumn(this.dtRecoveries, "OrderDate", "Auftragsdatum", typeof(string));
            DataTableHelper.AddColumn(this.dtRecoveries, "Currency", "Wä.", typeof(string));
            DataTableHelper.AddColumn(this.dtRecoveries, "Count", "Stück", typeof(string));
            DataTableHelper.AddColumn(this.dtRecoveries, "Value", "Betrag", typeof(string));
            DataTableHelper.AddColumn(this.dtRecoveries, "Paid", "Bezahlt", typeof(string));
            DataTableHelper.AddColumn(this.dtRecoveries, "Project", "Projektbezeichnung", typeof(string));
            DataTableHelper.AddColumn(this.dtRecoveries, "Sponsor", "Sponsor", typeof(string));

            DataTableHelper.FillGridView(this.dtRecoveries, this.dataGridViewRecoveriesDetails);

            this.dataGridViewRecoveriesDetails.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewRecoveriesDetails.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewRecoveriesDetails.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewRecoveriesDetails.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewRecoveriesDetails.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewRecoveriesDetails.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewRecoveriesDetails.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewRecoveriesDetails.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewRecoveriesDetails.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewRecoveriesDetails.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            LoadRecoveriesDetailsIntoDataTable();
        }

        private void SetupDataTableOrderHistory()
        {
            DataTableHelper.AddColumn(this.dtOrderHistory, "OrderNumber", "Nr", typeof(int));
            DataTableHelper.AddColumn(this.dtOrderHistory, "OrderDate", "Datum", typeof(string));
            DataTableHelper.AddColumn(this.dtOrderHistory, "OrderCount", "Anzahl", typeof(double));
            DataTableHelper.AddColumn(this.dtOrderHistory, "Umsatz", "Umsatz", typeof(double));
            DataTableHelper.AddColumn(this.dtOrderHistory, "Sponsor", "Sponsor", typeof(string));
            DataTableHelper.AddColumn(this.dtOrderHistory, "Customer", "Verein", typeof(string));
            DataTableHelper.AddColumn(this.dtOrderHistory, "OrderState", "Status", typeof(string));
            DataTableHelper.AddColumn(this.dtOrderHistory, "SponsorOrder", "Sponsorauftrag", typeof(bool));

            DataTableHelper.FillGridView(this.dtOrderHistory, this.dataGridViewOrderDetails);

            this.dataGridViewOrderDetails.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewOrderDetails.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewOrderDetails.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewOrderDetails.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewOrderDetails.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewOrderDetails.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewOrderDetails.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewOrderDetails.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            LoadOrderHistoryIntoDataTable();
        }

        private void SetupDataTableWorkTime()
        {
            DataTableHelper.AddColumn(this.dtUserWorkTimes, "UserDisplayName", "Benutzer", typeof(string));
            DataTableHelper.AddColumn(this.dtUserWorkTimes, "WorkTimeAdditionItemTypeDisplayName", "Tätigkeit", typeof(string));
            DataTableHelper.AddColumn(this.dtUserWorkTimes, "WorkDay", "Datum", typeof(string));
            DataTableHelper.AddColumn(this.dtUserWorkTimes, "Start", "Von", typeof(string));
            DataTableHelper.AddColumn(this.dtUserWorkTimes, "Stop", "Bis", typeof(string));
            DataTableHelper.AddColumn(this.dtUserWorkTimes, "Duration", "Dauer (min)", typeof(string));
            DataTableHelper.AddColumn(this.dtUserWorkTimes, "Confirmed", "Bestätigt", typeof(Boolean));
            DataTableHelper.AddColumn(this.dtUserWorkTimes, "Notice", "Notize", typeof(string));
            
            DataTableHelper.AddColumn(this.dtUserWorkTimes, "WorkTimeAdditions", string.Empty, typeof(Object), MappingType.Hidden);
            DataTableHelper.FillGridView(this.dtUserWorkTimes, this.dataGridViewWorkTimeAdditions);

            this.dataGridViewWorkTimeAdditions.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewWorkTimeAdditions.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewWorkTimeAdditions.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewWorkTimeAdditions.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewWorkTimeAdditions.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewWorkTimeAdditions.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewWorkTimeAdditions.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewWorkTimeAdditions.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            LoadWorkTimeIntoDataTable();
        }

        private void SetupDataTableProjectCounts()
        {
            DataTableHelper.AddColumn(this.dtProjectCounts, "Projektnummer", "Projektnummer", typeof(int));
            DataTableHelper.AddColumn(this.dtProjectCounts, "Bezeichnung", "Bezeichnung", typeof(string));
            DataTableHelper.AddColumn(this.dtProjectCounts, "Projektjahr", "Projektjahr", typeof(string));
            DataTableHelper.AddColumn(this.dtProjectCounts, "Count", "Stückzahl", typeof(string));

            DataTableHelper.FillGridView(this.dtProjectCounts, this.dataGridViewProjectCounts);

            this.dataGridViewProjectCounts.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewProjectCounts.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewProjectCounts.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewProjectCounts.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            LoadProjectCounts();
        }

        private void SetupDataTableProjectOrders()
        {
            DataTableHelper.AddColumn(this.dtProjectOrders, "OrderNumber", "Projektnummer", typeof(int));
            DataTableHelper.AddColumn(this.dtProjectOrders, "OrderDate", "Auftragsdatum", typeof(string));
            DataTableHelper.AddColumn(this.dtProjectOrders, "PaymentTarget", "Zahlungsziel", typeof(string));
            DataTableHelper.AddColumn(this.dtProjectOrders, "Sponsor", "Sponsor", typeof(string));
            DataTableHelper.AddColumn(this.dtProjectOrders, "Count", "Anzahl", typeof(string));
            DataTableHelper.AddColumn(this.dtProjectOrders, "State", "Status", typeof(string));

            DataTableHelper.FillGridView(this.dtProjectOrders, this.dataGridViewProjectOrders);

            this.dataGridViewProjectOrders.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewProjectOrders.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewProjectOrders.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewProjectOrders.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewProjectOrders.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewProjectOrders.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }


        private void FillListBoxWorkDays()
        {
            List<WorkDayList> workDayLists = new List<WorkDayList>();
            if (this.user != null)
            {
                workDayLists = MetaCall.Business.Users.WorkDayList_GetByUser(this.user.UserId, this.from, this.to);

                this.listBoxWorkDays.Items.Clear();
                this.listBoxWorkDays.DisplayMember = "DisplayDate";
                foreach (WorkDayList workDayList in workDayLists)
                {
                    this.listBoxWorkDays.Items.Add(workDayList);
                }
                if (workDayLists.Count > 0)
                    this.listBoxWorkDays.SelectedIndex = 0;
            }
        }

        private void FillComboBoxProjectsOverView()
        {
            List<ProjectInfo> projectInfos = new List<ProjectInfo>();

            if (this.user != null)
            {
                projectInfos = MetaCall.Business.Projects.GetByUser_KeyData(this.user.UserId);

                this.comboBoxProjectsOverView.Items.Clear();
                this.comboBoxProjectsOverView2.Items.Clear();
                this.comboBoxProjectsOverView3.Items.Clear();
                this.comboBoxProjectsOverView.DisplayMember = "Bezeichnung";
                this.comboBoxProjectsOverView2.DisplayMember = "Bezeichnung";
                this.comboBoxProjectsOverView3.DisplayMember = "Bezeichnung";
                foreach (ProjectInfo projectInfo in projectInfos)
                {
                    this.comboBoxProjectsOverView.Items.Add(projectInfo);
                    this.comboBoxProjectsOverView2.Items.Add(projectInfo);
                    this.comboBoxProjectsOverView3.Items.Add(projectInfo);
                }
                if (projectInfos.Count > 0)
                    this.ComboBoxSalaryStatement.SelectedItem = 0;
                    
                this.comboBoxProjectsOverView2.SelectedIndex = -1;
                this.comboBoxProjectsOverView3.SelectedIndex = -1;
            }
        }

        private void FillComboBoxSalaryStatement()
        {
            List<SalaryStatementNumbers> salaryStatementNumbers = new List<SalaryStatementNumbers>();

            if (this.user != null)
            {
                salaryStatementNumbers = MetaCall.Business.Recoveries.GetSalaryStatementNumbers_GetByUser(this.user.UserId);

                this.ComboBoxSalaryStatement.Items.Clear();
                this.ComboBoxSalaryStatement.DisplayMember = "SalaryStatementDate";
                foreach (SalaryStatementNumbers salaryStatementNumber in salaryStatementNumbers)
                {
                    this.ComboBoxSalaryStatement.Items.Add(salaryStatementNumber);
                }
                if (ComboBoxSalaryStatement.Items.Count > 0) 
                    this.ComboBoxSalaryStatement.SelectedIndex = 0;
            }

        }

        private void LoadRecoveriesSum()
        {
            if (this.user != null)
            {
                if (AnalysisFormCurrent == AnalysisForm.TimeSelected)
                    this.recoveriesSum = MetaCall.Business.Recoveries.GetRecoveriesSum_GetByUser(this.user.UserId, this.from, this.to, 0, 1);
                else
                    this.recoveriesSum = MetaCall.Business.Recoveries.GetRecoveriesSum_GetByUser(this.user.UserId, this.from, this.to, SelectedSalaryStatementNumbers, 2);
            }
            else
            {
                this.recoveriesSum = null;
            }

            this.textBoxPaid.Text = string.Format("{0:f2}", this.recoveriesSum.Paid);
            this.textBoxCount.Text = string.Format("{0:f2}", this.recoveriesSum.Count);
            this.textBoxCountCanceled.Text = string.Format("{0:f2}", this.recoveriesSum.Count_Canceled);
            this.textBoxAmountBrutto.Text = string.Format("{0:f2}", this.recoveriesSum.AmountBrutto);
            this.textBoxAmountBrutto_Canceled.Text  = string.Format("{0:f2}", this.recoveriesSum.AmountBrutto_Canceled);

        }

        private void LoadKeyDatas()
        {
            if (this.user != null)
            {
                this.user_KeyData = MetaCall.Business.Users.User_KeyData_GetByUser(this.user.UserId, this.from, this.to);
            }
            else
            {
                this.user_KeyData = null;
            }
            if (this.user_KeyData != null)
            {
                this.textBoxCountOrders.Text = string.Format("{0:f2}", this.user_KeyData.CountOrders);
                this.textBoxCountWorkDays.Text = string.Format("{0:g}",this.user_KeyData.CountWorkDays);
                this.textBoxSumWorkTime.Text = string.Format("{0:f2}", this.user_KeyData.SumWorkTime);
                this.textBoxAvergaeTimePerWorkDay.Text = string.Format("{0:f2}", this.user_KeyData.AverageTimePerWorkDay);
                this.textBoxSumPauseTime.Text = string.Format("{0:f2}", this.user_KeyData.SumPauseTime);
                this.textBoxSumPresentTime.Text = string.Format("{0:f2}", this.user_KeyData.SumPresentTime);
                this.textBoxSumSecondaryTime.Text = string.Format("{0:f2}", this.user_KeyData.SumSecondaryTime);
                this.textBoxSumTrainingTimeConfirmed.Text = string.Format("{0:f2}", this.user_KeyData.SumTrainingTimeConfirmed);
                this.textBoxSumTrainingTimeNotConfirmed.Text = string.Format("{0:f2}", this.user_KeyData.SumTrainingTimeNotConfirmed);


                this.textBoxCountOrders2.Text = string.Format("{0:f2}", this.user_KeyData.CountOrders);
                this.textBoxCountWorkDays2.Text = string.Format("{0:f2}", this.user_KeyData.CountWorkDays);
                this.textBoxSumWorkTime2.Text = string.Format("{0:f2}", this.user_KeyData.SumWorkTime);
                this.textBoxAvergaeTimePerWorkDay2.Text = string.Format("{0:f2}", this.user_KeyData.AverageTimePerWorkDay);
                this.textBoxAvergaeCountPerWorkDay.Text = this.user_KeyData.AvergaeCountPerWorkDay;

                this.textBoxSumPresentTimeWithoutHolidayIllness.Text =
                    string.Format("{0:f2}", this.user_KeyData.SumPresentTime - this.user_KeyData.SumHolidayIllness);
                this.textBoxSumPhoneTime.Text = string.Format("{0:f2}", this.user_KeyData.SumPhoneTime);
                this.textBoxSumNumberOfCalls.Text = string.Format("{0:g}", this.user_KeyData.SumNumberOfCalls);
            }
            else
            {
                this.textBoxCountOrders.Text = null;
                this.textBoxCountWorkDays.Text = null;
                this.textBoxSumWorkTime.Text = null;
                this.textBoxAvergaeTimePerWorkDay.Text = null;
                this.textBoxSumPauseTime.Text = null;
                this.textBoxSumPresentTime.Text = null;
                this.textBoxSumSecondaryTime.Text = null;
                this.textBoxSumTrainingTimeConfirmed.Text = null;
                this.textBoxSumTrainingTimeNotConfirmed.Text = null;


                this.textBoxCountOrders2.Text = null;
                this.textBoxCountWorkDays2.Text = null;
                this.textBoxSumWorkTime2.Text = null;
                this.textBoxAvergaeTimePerWorkDay2.Text = null;
                this.textBoxAvergaeCountPerWorkDay.Text = null;

                this.textBoxSumPresentTimeWithoutHolidayIllness.Text = null;
                this.textBoxSumNumberOfCalls.Text = null;
                this.textBoxSumNumberOfCalls.Text = null;
            }

        }

        private void LoadProjectKeyDatas()
        {
            User_KeyData projectKeyData = new User_KeyData();

            if (this.user != null && SelectedProjectOverview2 != null)
            {
                projectKeyData = MetaCall.Business.Users.Project_KeyData_GetByUserAndProject(this.user.UserId, SelectedProjectOverview2.ProjectId);
            }
            else
            {
                projectKeyData = null;
            }

            this.textBoxProjectCountOrders.Text = string.Format("{0:f2}", projectKeyData.CountOrders);
            this.textBoxProjectCountWorkDays.Text = string.Format("{0:f2}", projectKeyData.CountWorkDays);
            this.textBoxProjectSumWorkTime.Text = string.Format("{0:f2}", projectKeyData.SumWorkTime);
            this.textBoxProjectAvergaeTimePerWorkDay.Text = string.Format("{0:f2}", projectKeyData.AverageTimePerWorkDay);
            this.textBoxProjectAvergaeCountPerWorkDay.Text = projectKeyData.AvergaeCountPerWorkDay;
        }

        private void LoadProjectCounts()
        {
            List<ProjectCounts> projectCounts = new List<ProjectCounts>();

            if (this.user != null)
            {
                projectCounts = MetaCall.Business.mwProjektBusiness.GetAllProjectCounts(this.user.UserId);
            }
            else
            {
                projectCounts = null;
            }

            bindingSourceProjectCounts.SuspendBinding();
            try
            {
                dtProjectCounts.Rows.Clear();

                if (projectCounts != null)
                {
                    foreach (ProjectCounts projectCount in projectCounts)
                    {
                        object[] objectData = new object[]
                        {
                            projectCount.Projektnummer,
                            projectCount.Bezeichnung,
                            projectCount.Projektjahr,
                            string.Format("{0:f2}", projectCount.Count),
                        };

                        dtProjectCounts.Rows.Add(objectData);
                    }
                }
            }

            finally
            {
                bindingSourceProjectCounts.ResumeBinding();
            }
        }

        private void LoadProjectOrders()
        {
            List<ProjectOrders> projectOrders = new List<ProjectOrders>();

            if (this.user != null)
            {
                projectOrders = MetaCall.Business.mwProjektBusiness.GetAllProjectOrders(this.user.UserId, SelectedProjectOverview3.ProjectId);
            }
            else
            {
                projectOrders = null;
            }

            bindingSourceProjectOrders.SuspendBinding();
            try
            {
                dtProjectOrders.Rows.Clear();

                if (projectOrders != null)
                {
                    foreach (ProjectOrders projectOrder in projectOrders)
                    {
                        string orderDate;
                        string paymentTargetDate;

                        orderDate = string.Format("{0:d}", projectOrder.OrderDate);
                        paymentTargetDate = string.Format("{0:d}", projectOrder.PaymentTarget);

                        object[] objectData = new object[]
                        {
                            projectOrder.OrderNumber,
                            orderDate,
                            paymentTargetDate,
                            projectOrder.Sponsor,
                            string.Format("{0:f2}", projectOrder.Count),
                            projectOrder.State
                        };

                        dtProjectOrders.Rows.Add(objectData);
                    }
                }
            }

            finally
            {
                bindingSourceProjectOrders.ResumeBinding();
            }
        }

        private void LoadRecoveriesDetailsIntoDataTable()
        {

            LoadRecoveriesSum();

            List<RecoveriesDetails> recoveriesDetails = new List<RecoveriesDetails>();

            if (this.user != null)
            {
                if (AnalysisFormCurrent == AnalysisForm.TimeSelected)
                    recoveriesDetails = MetaCall.Business.Recoveries.GetRecoveriesDetails_GetByUser(this.user.UserId, this.from, this.to, 0, 1);
                else
                    recoveriesDetails = MetaCall.Business.Recoveries.GetRecoveriesDetails_GetByUser(this.user.UserId, this.from, this.to, SelectedSalaryStatementNumbers, 2);
            }
            else
            {
                recoveriesDetails = null;
            }

            bindingSourceRecoveries.SuspendBinding();
            try
            {
                dtRecoveries.Rows.Clear();

                if (recoveriesDetails != null)
                {
                    foreach (RecoveriesDetails recoveriesDetail in recoveriesDetails)
                    {
                        string orderDate;
                        string paymentDate;

                        orderDate = string.Format("{0:d}", recoveriesDetail.OrderDate);
                        paymentDate = string.Format("{0:d}", recoveriesDetail.PaymentDate);

                        object[] objectData = new object[]
                        {
                            recoveriesDetail.InvoiceNumber,
                            recoveriesDetail.State,
                            paymentDate,
                            orderDate,
                            recoveriesDetail.Currency,
                            string.Format("{0:f2}", recoveriesDetail.Count),
                            string.Format("{0:f2}", recoveriesDetail.Value),
                            string.Format("{0:f2}", recoveriesDetail.Paid),
                            recoveriesDetail.Project,
                            recoveriesDetail.Sponsor
                        };

                        dtRecoveries.Rows.Add(objectData);
                    }
                }
            }

            finally
            {
                bindingSourceRecoveries.ResumeBinding();
            }
        }


        private void LoadOrderHistoryIntoDataTable()
        {
            List<OrderHistory> orderHistories = new List<OrderHistory>();

            if (this.user != null)
            {
                orderHistories = MetaCall.Business.mwProjekt_ProjekOrderHistorie.GetOrderHistoy_GetByUser(this.user.UserId, this.from, this.to);
            }
            else
            {
                orderHistories = null;
            }


            bindingSourceOrderDetails.SuspendBinding();
            try
            {
                dtOrderHistory.Rows.Clear();

                if (orderHistories != null)
                {
                    foreach (OrderHistory orderHistory in orderHistories)
                    {
                        string orderDate;

                        orderDate = string.Format("{0:d}", orderHistory.OrderDate);

                        object[] objectData = new object[]
                        {
                            orderHistory.OrderNumber,
                            orderDate,
                            orderHistory.OrderCount,
                            orderHistory.Umsatz,
                            orderHistory.Sponsor,
                            orderHistory.Customer,
                            orderHistory.OrderState,
                            orderHistory.SponsorOrder
                        };

                        dtOrderHistory.Rows.Add(objectData);
                    }
                }
            }

            finally
            {
                bindingSourceOrderDetails.ResumeBinding();
            }
        }


        private void LoadWorkTimeIntoDataTable()
        {
            List<WorkTimes> workTimes = new List<WorkTimes>();

            if (this.user != null)
            {
                switch (DisplayFormCurrent)
                {
                    case DisplayForm.Group:
                        workTimes = MetaCall.Business.Users.WorkTimes_GROUP_GetByUser(this.user.UserId, this.from, this.to);
                        break;
                    case DisplayForm.All:
                        workTimes = MetaCall.Business.Users.WorkTimes_ALL_GetByUser(this.user.UserId, this.from, this.to);
                        break;
                    default:
                        workTimes = MetaCall.Business.Users.WorkTimes_GROUP_GetByUser(this.user.UserId, this.from, this.to);
                        break;
                }
            }
            else             {
                workTimes = null;
            }


            bindingSourceUserWorkTimeAdditionInfo.SuspendBinding();
            try
            {
                dtUserWorkTimes.Rows.Clear();

                if (workTimes != null)
                {
                    foreach (WorkTimes workTime in workTimes)
                    {
                        Object objectWorkTime;
                        switch (workTime.RowObject)
                        {
                            case "WorkTimeAdditions":
                                if (workTime.WorkTimesId != null)
                                {
                                    Guid guidWorkTime = (Guid)workTime.WorkTimesId;
                                    WorkTimeAdditions objectWorkTimeAddition = MetaCall.Business.Users.WorkTimeAdditions_GetSingle(guidWorkTime);
                                    objectWorkTime = objectWorkTimeAddition;
                                }
                                else
                                {
                                    objectWorkTime = null;
                                }
                                //  WorkTimeAdditionItems workTimeAdditionItem = MetaCall.Business.Users.WorkTimeAdditionItems_GetSingle(objectWorkTimeAddition.WorkTimeAdditionItemType);
                                break;
                            case "CallJob_ActivityTimes":
                                objectWorkTime = null;
                                break;
                            case "Projects_LogOnTimes":
                                objectWorkTime = null;
                                break;
                            case "Users_WorkTimes":
                                objectWorkTime = null;
                                break;
                            case "Project":
                                objectWorkTime = null;
                                break;
                            default:
                                objectWorkTime = null;
                                break;
                        }

                        double newDuration;

                        string duration;

                        if (workTime.RowDescription == "Login" || workTime.RowDescription == "Logoff")
                        {
                            duration = string.Empty;
                        }
                        else
                        {
                            newDuration = (double)workTime.Duration / 60;
                            newDuration = System.Math.Round(newDuration, 2);
                            duration = newDuration.ToString();
                        }

                        string stopDate;
                        string startDate;

                        if (workTime.Stop == null || workTime.RowDescription == "Login")
                        {
                            stopDate = string.Empty;
                        }
                        else
                        {
                            stopDate = string.Format("{0:T}", workTime.Stop);
                        }

                        if (workTime.Start == null || workTime.RowDescription == "Logoff")
                        {
                            startDate = string.Empty;
                        }
                        else
                        {
                            startDate = string.Format("{0:T}", workTime.Start);
                        }

                        object[] objectData = new object[]
                        {
                        workTime.User.DisplayName,
                        workTime.RowDescription,
                        string.Format("{0:d}", workTime.WorkDate),
                        startDate,
                        stopDate,
                        duration,
                        workTime.UploadMetaWare,
                        workTime.Notice,
                        objectWorkTime 
                        };

                        dtUserWorkTimes.Rows.Add(objectData);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            finally
            {
                bindingSourceUserWorkTimeAdditionInfo.ResumeBinding();
            }
        }

        private void Reload()
        {
            LoadWorkTimeIntoDataTable();
        }

        private void UserWorkTimeAdditionNew()
        {
            WorkTimeAdditions newWorkTimeAdditions = new WorkTimeAdditions();
            newWorkTimeAdditions.WorkTimeAdditionId = Guid.NewGuid();
            newWorkTimeAdditions.User = MetaCall.Business.Users.GetUserInfo(this.user);

            using (UserWorkTimeAdditionEdit dlg = new UserWorkTimeAdditionEdit(newWorkTimeAdditions))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        MetaCall.Business.Users.CreateWorkTimeAddition(newWorkTimeAdditions);
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                        if (rethrow)
                            throw;
                    }
                }
            }
       }

        private void UserWorkTimeAdditionEdit()
        {
            if (SelectedWorkTimeAdditions != null)
            {
                using (UserWorkTimeAdditionEdit dlg = new UserWorkTimeAdditionEdit(SelectedWorkTimeAdditions))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            MetaCall.Business.Users.UpdateWorkTimeAddition(SelectedWorkTimeAdditions);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                            if (rethrow)
                                throw;
                        }
                    }
                }
            }
        }

        private void UserWorkTimeAdditionDelete()
        {
            if (SelectedWorkTimeAdditions != null)
            {
                MessageBoxOptions options = CultureInfo.CurrentCulture.TextInfo.IsRightToLeft ?
                    MessageBoxOptions.RtlReading : 0;

                string msg = string.Format("Möchten Sie diese Arbeitszeit wirklich löschen?");

                if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, options) == DialogResult.Yes)
                {
                    try
                    {
                        MetaCall.Business.Users.DeleteWorkTimeAddition(SelectedWorkTimeAdditions);
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                        if (rethrow)
                            throw;
                    }
                }
            }
        }

        private void editToolStripButton_Click(object sender, EventArgs e)
        {
            UserWorkTimeAdditionEdit();
            Reload();
        }

        public WorkTimeAdditions SelectedWorkTimeAdditions
        {
            get
            {
                if (dataGridViewWorkTimeAdditions.CurrentRow == null)
                    return null;

                DataRowView currentRowView =
                    (DataRowView)dataGridViewWorkTimeAdditions.CurrentRow.DataBoundItem;

                if (currentRowView == null || currentRowView.Row == null)
                    return null;

                Object rowObject;

                rowObject = currentRowView.Row.ItemArray[
                    currentRowView.Row.ItemArray.Length - 1];

                if (rowObject.GetType() == typeof(WorkTimeAdditions))
                    return (WorkTimeAdditions)rowObject;
                else
                    return null;
            }
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            UserWorkTimeAdditionNew();
            Reload();
        }

        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            UserWorkTimeAdditionDelete();
            Reload();
        }

        private void dataGridViewUsers_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                this.dataGridViewWorkTimeAdditions.Rows[e.RowIndex].Selected = true;
                UserWorkTimeAdditionEdit();
            }
        }

        private void UserWorkTimeAdditionGridSelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dataGridViewWorkTimeAdditions.SelectAll();
        }

        private void UserWorkTimeAdditionGridSelectNothingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dataGridViewWorkTimeAdditions.ClearSelection();
        }

        private void UserWorkTimeAdditionGridSetConfirmedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Testen ob alle bestätigt werden können oder ob es Konflikte in metaware gibt
            int isCorrect = 0;
            foreach (DataGridViewRow row in this.dataGridViewWorkTimeAdditions.SelectedRows)
            {

                DataRowView currentRowView = (DataRowView)row.DataBoundItem;

                if (currentRowView.Row.ItemArray[currentRowView.Row.ItemArray.Length - 1].GetType() == typeof(WorkTimeAdditions))
                {
                    WorkTimeAdditions curWorkTimeAdditions = (WorkTimeAdditions)currentRowView.Row.ItemArray[currentRowView.Row.ItemArray.Length - 1];
                    isCorrect += MetaCall.Business.Users.mwWorkTime_Test(curWorkTimeAdditions);
                }
            }

            if (isCorrect == 0)
            {
                foreach (DataGridViewRow row in this.dataGridViewWorkTimeAdditions.SelectedRows)
                {
                    DataRowView currentRowView = (DataRowView)row.DataBoundItem;

                    if (currentRowView.Row.ItemArray[currentRowView.Row.ItemArray.Length - 1].GetType() == typeof(WorkTimeAdditions))
                    {
                        WorkTimeAdditions curWorkTimeAdditions = (WorkTimeAdditions)currentRowView.Row.ItemArray[currentRowView.Row.ItemArray.Length - 1];

                        curWorkTimeAdditions.Confirmed = true;

                        MetaCall.Business.Users.UpdateWorkTimeAddition(curWorkTimeAdditions);
                    }
                }
                LoadWorkTimeIntoDataTable();

            }
            else
            {
                string msg;
                if (isCorrect == 1)
                    msg = string.Format("Es konnte eine Arbeitszeit wegen doppeltem Eintrag in metaWare nicht aktualisiert werden!");
                else
                    msg = string.Format("Es konnten {0} Arbeitszeiten wegen doppelter Einträge in metaWare nicht aktualisiert werden!", isCorrect);

                MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : 0;
                MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1, options);

            }
        }

        private void UserWorkTimeAdditionGridSetUnConfirmedToolStripMenuItem_Click(object sender, EventArgs e)
        {
                foreach (DataGridViewRow row in this.dataGridViewWorkTimeAdditions.SelectedRows)
                {
                    DataRowView currentRowView = (DataRowView)row.DataBoundItem;

                    if (currentRowView.Row.ItemArray[currentRowView.Row.ItemArray.Length - 1].GetType() == typeof(WorkTimeAdditions))
                    {
                        WorkTimeAdditions curWorkTimeAdditions = (WorkTimeAdditions)currentRowView.Row.ItemArray[currentRowView.Row.ItemArray.Length - 1];

                        curWorkTimeAdditions.Confirmed = false;

                        MetaCall.Business.Users.UpdateWorkTimeAddition(curWorkTimeAdditions);
                    }
                }
                LoadWorkTimeIntoDataTable();
        }


        private void contextMenuStripUserWorkTimeAddition_Opening(object sender, CancelEventArgs e)
        {
            this.contextMenuStripUserWorkTimeAddition.Items.Clear();

            if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
            {
                //nur wenn Admin kann er Reminder à Block manipulieren.
                ToolStripMenuItem selectedAll = new ToolStripMenuItem("Alle auswählen");
                selectedAll.Click += new EventHandler(UserWorkTimeAdditionGridSelectAllToolStripMenuItem_Click);
                this.contextMenuStripUserWorkTimeAddition.Items.Add(selectedAll);

                ToolStripMenuItem selectedNothing = new ToolStripMenuItem("Auswahl aufheben");
                selectedNothing.Click += new EventHandler(UserWorkTimeAdditionGridSelectNothingToolStripMenuItem_Click);
                this.contextMenuStripUserWorkTimeAddition.Items.Add(selectedNothing);

                if (MetaCall.Business.Users.WorkTimeEditable(this.user.UserId, this.from) == true)
                {
                    ToolStripMenuItem setDone = new ToolStripMenuItem("Bestätigen");
                    setDone.Click += new EventHandler(UserWorkTimeAdditionGridSetConfirmedToolStripMenuItem_Click);
                    this.contextMenuStripUserWorkTimeAddition.Items.Add(setDone);

                    ToolStripMenuItem setUnDone = new ToolStripMenuItem("Bestätigen aufheben");
                    setUnDone.Click += new EventHandler(UserWorkTimeAdditionGridSetUnConfirmedToolStripMenuItem_Click);
                    this.contextMenuStripUserWorkTimeAddition.Items.Add(setUnDone);

                    ToolStripMenuItem delete = new ToolStripMenuItem("Löschen");
                    delete.Click += new EventHandler(DeleteToolStripMenuItem_Click);
                    this.contextMenuStripUserWorkTimeAddition.Items.Add(delete);
                }
            }

            this.contextMenuStripUserWorkTimeAddition.AutoClose = true;
            this.contextMenuStripUserWorkTimeAddition.ShowCheckMargin = false;
            this.contextMenuStripUserWorkTimeAddition.ShowImageMargin = false;
            this.contextMenuStripUserWorkTimeAddition.ShowItemToolTips = true;

            e.Cancel = (this.contextMenuStripUserWorkTimeAddition.Items.Count < 1);
        }

        private void dataGridViewWorkTimeAdditions_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dataGridViewWorkTimeAdditions.SelectedRows.Count == 1)
            {
                if (SelectedWorkTimeAdditions != null)
                {
                    this.editToolStripButton.Enabled = true;
                    this.deleteToolStripButton.Enabled = true;
                }
                else
                {
                    this.editToolStripButton.Enabled = false;
                    this.deleteToolStripButton.Enabled = false;
                }
            }
            else if (this.dataGridViewWorkTimeAdditions.SelectedRows.Count < 1)
            {
                this.editToolStripButton.Enabled = false;
                this.deleteToolStripButton.Enabled = false;
            }
            else
            {
                this.editToolStripButton.Enabled = false;
                this.deleteToolStripButton.Enabled = false;
            }
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg;
            msg = string.Format("Möchten Sie diesen Arbeitszeiteneintrag wirklich löschen?");

            MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : 0;
            if (MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1, options) == DialogResult.Yes)
                MetaCall.Business.Users.DeleteWorkTimeAddition(SelectedWorkTimeAdditions);

            LoadWorkTimeIntoDataTable();
        }

        private void toolStripButtonChangeDisplayForm_Click(object sender, EventArgs e)
        {
            if (DisplayFormCurrent == DisplayForm.All)
            {
                DisplayFormCurrent = DisplayForm.Group;
                this.toolStripButtonChangeDisplayForm.Text = "Detail";
            }
            else
            {
                DisplayFormCurrent = DisplayForm.All;
                this.toolStripButtonChangeDisplayForm.Text = "Summen";
            }
        }

        private void radioButtonViewAnalysisTimeSelect_Click(object sender, EventArgs e)
        {
            AnalysisFormCurrent = AnalysisForm.TimeSelected;
            this.radioButtonViewAnalysisAccounting.Checked = false;
            this.radioButtonViewAnalysisTimeSelect.Checked = true;
            this.ComboBoxSalaryStatement.Enabled = false;
        }

        private void radioButtonViewAnalysisAccounting_Click(object sender, EventArgs e)
        {
            AnalysisFormCurrent = AnalysisForm.Accounting;
            this.radioButtonViewAnalysisAccounting.Checked = true;
            this.radioButtonViewAnalysisTimeSelect.Checked = false;
            this.ComboBoxSalaryStatement.Enabled = true;
        }

        private void ComboBoxSalaryStatement_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioButtonViewAnalysisAccounting.Checked == true)
            {
                LoadRecoveriesDetailsIntoDataTable();
            }
        }

        private void buttonPrintRecoveries_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Rechnungsübersicht drucken ... kommt in der nächsten Version");
        }

        private void buttonTagesbericht_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tagesbericht drucken ... kommt in der nächsten Version");
        }

        private void buttonProjectOverView_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Projektübersicht drucken ... kommt in der nächsten Version");
        }

        private void comboBoxProjectsOverView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjectKeyDatas();
        }

        private void comboBoxProjectsOverView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjectOrders();
        }

        Boolean loadTabPageKeyData = false;
        Boolean loadTabPageOrderOverview = false;
        Boolean loadTabPageReports = false;
        Boolean loadTabPageInvoiceOverview = false;
          

        private void tabControlWorkTime_Selected(object sender, TabControlEventArgs e)
        {
            TabPage selectedTabPage = this.tabControlWorkTime.TabPages[this.tabControlWorkTime.SelectedIndex];

            if(selectedTabPage == this.tabPageInvoiceOverview)
                {
                    if (this.loadTabPageInvoiceOverview == false)
                    {
                        SetupDataTableRecoveries();
                        LoadRecoveriesSum();
                        FillComboBoxSalaryStatement();

                        this.loadTabPageInvoiceOverview = true;
                    }
                }
            else if (selectedTabPage == this.tabPageKeyData)
                {
                    if (this.loadTabPageKeyData == false)
                    {
                        SetupDataTableProjectOrders();
                        SetupDataTableProjectCounts();

                        this.loadTabPageKeyData = true;
                    }
                }
            else if (selectedTabPage == this.tabPageOrderOverview)
                {
                    if (this.loadTabPageOrderOverview == false)
                    {
                        SetupDataTableOrderHistory();
                        this.loadTabPageOrderOverview = true;
                    }
                }
            else if (selectedTabPage == this.tabPageReports)
                {
                    if (this.loadTabPageReports == false)
                    {
                        FillListBoxWorkDays();
                        this.loadTabPageReports = true;
                    }
                }


         /*
            if (this.tabControlUser.TabPages[this.tabControlUser.SelectedIndex].Text.ToString() == "Projekte")
            {
                this.groupBoxSetProjectList.Visible = true;
            }
            else
            {
                this.groupBoxSetProjectList.Visible = false;
            }
          */

        }

        private void toolStripButtonConfirmed_Click(object sender, EventArgs e)
        {
            if (DisplayFormConfirmedCurrent == DisplayFormConfirmed.Confirmed)
            {
                DisplayFormConfirmedCurrent = DisplayFormConfirmed.All;
                this.toolStripButtonConfirmed.Text = "unbestätigt";
            }
            else
            {
                DisplayFormConfirmedCurrent = DisplayFormConfirmed.Confirmed;
                this.toolStripButtonConfirmed.Text = "Alle";
            }
        }
    }
}
