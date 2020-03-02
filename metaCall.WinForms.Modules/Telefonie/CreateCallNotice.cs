using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;
using MaDaNet.Common.AppFrameWork.WinUI.Controls;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class CreateCallNotice : UserControl, ISupportInitialize, IInitializeCall
    {
        List<Branch> branchList;
        List<BranchGroup> branchGroupList;
        List<CallJobResult> callJobResultList;

        Call currentCall;

        public event EventHandler<SponsorInfoChangeEventArgs> SponsorInfoChange;

        Branch selectedBranch;
        BranchGroup selectedBranchGroup;

        private int formHeight = 0;

        public int FormHeight
        {
            get { return formHeight; }
        }

        /// <summary>
        /// Sucht die Branche aufgrund der Branchennummer und wählt diese in der ComboBox aus
        /// </summary>
        /// <param name="BranchNumber"></param>
        private void SetSelectedBranch_BranchGroup()
        {
            if (this.isInitializing)
            {
                return;
            }

            //Wenn die Branchennummer null ist wird nichts selektiert
            if (this.selectedBranch == null && this.selectedBranchGroup == null)
            {
                this.Branch_BranchGroupComboBox.SelectedItem = null;
                return;
            }

            if (this.selectedBranch.Equals(Branch.Unknown) && this.selectedBranchGroup == null)
            {
                this.Branch_BranchGroupComboBox.SelectedItem = null;
                return;
            }

            if (this.selectedBranch == null && this.selectedBranchGroup != null)
            {
                //Ansosnetn wird in der Liste die BranchenGruppe gesucht 
                // und dem ComboFeld als selectedItem 
                // zugewiesen
                foreach (BranchGroup branchGroupItem in this.branchGroupList)
                {
                    if (branchGroupItem.BranchenGruppenID.Equals(this.selectedBranchGroup.BranchenGruppenID))
                    {
                        this.Branch_BranchGroupComboBox.SelectedItem = branchGroupItem;
                        return;
                    }
                }

                return;
            }

            if (this.selectedBranch.Equals(Branch.Unknown) && this.selectedBranchGroup != null)
            {
                //Ansosnetn wird in der Liste die BranchenGruppe gesucht 
                // und dem ComboFeld als selectedItem zugewiesen
                foreach (BranchGroup branchGroupItem in this.branchGroupList)
                {
                    if (branchGroupItem.BranchenGruppenID.Equals(this.selectedBranchGroup.BranchenGruppenID))
                    {
                        this.Branch_BranchGroupComboBox.SelectedItem = branchGroupItem;
                        return;
                    }
                }

                return;
            }

            //Ansosnetn wird in der Liste die Branche gesucht 
            // und dem ComboFeld als selectedItem zugewiesen
            foreach (Branch branchItem in this.branchList)
            {
                if (branchItem.Branchennummer.Equals(this.selectedBranch.Branchennummer))
                {
                    this.Branch_BranchGroupComboBox.SelectedItem = branchItem;
                    return;
                }
            }

            selectedBranchGroup = selectedBranch.BranchGroup;
        }       

        public Branch SelectedBranch
        {
            get 
            {
                return this.selectedBranch;
            }
            set
            {
                if (this.isInitializing) return;
                selectedBranch = value;
                SetSelectedBranch_BranchGroup();
            }
        }

        public BranchGroup SelectedBranchGroup
        {
            get
            {
                return this.selectedBranchGroup;
            }
            set
            {
                if (this.isInitializing) return;
                selectedBranchGroup = value;
                SetSelectedBranch_BranchGroup();
            }
        }

        public string Notice
        {
            get 
            {
                return this.noticeTextBox.Text; 
            }
            set
            {
                this.noticeTextBox.Text = value;
            }
        }

        public List<string> Diagnosis
        {
            set
            {
                this.listBoxDiagnosis.DataSource = value;
            }
        }

        public CreateCallNotice()
        {
            InitializeComponent();
            this.noticeTabControl.TabPages.RemoveAt(this.noticeTabControl.TabCount - 1);
        }

        private void FillBranch_BranchGroupComboBox()
        {
            if (this.isInitializing)
            {
                return;
            }

            this.branchGroupList = MetaCall.Business.BranchGroup.BranchGroups;
            this.Branch_BranchGroupComboBox.Items.Clear();

            if (this.branchGroupList.Count > 0)
            {
                this.Branch_BranchGroupComboBox.Items.Add(" ");
                this.Branch_BranchGroupComboBox.Items.Add("[ Branchengruppe ]");
                this.Branch_BranchGroupComboBox.Items.Add("------------------------------------------------------------------------");
            }

            foreach (BranchGroup branchGroups in this.branchGroupList)
            {
                this.Branch_BranchGroupComboBox.Items.Add(branchGroups);
            }

            if (this.branchGroupList.Count > 0)
            {
                this.Branch_BranchGroupComboBox.Items.Add(" ");
            }

            this.branchList = MetaCall.Business.Branch.Branches;

            if (this.branchList.Count > 0)
            {
                this.Branch_BranchGroupComboBox.Items.Add("[ Branche ]");
                this.Branch_BranchGroupComboBox.Items.Add("------------------------------------------------------------------------");

                foreach (Branch branch in this.branchList)
                {
                    this.Branch_BranchGroupComboBox.Items.Add(branch);
                }
            }

            this.Branch_BranchGroupComboBox.Items.Add(Branch.Unknown);
        }

        private void FillNoticeAdministration(Call call)
        {
            if (this.isInitializing)
            {
                return;
            }

            if (call == null)
            {
                throw new ArgumentNullException("call");
            }

            if (call.CallJob == null)
            {
                throw new ArgumentNullException("call.CallJob");
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(MetaCall.Business.Addresses.GetAddress_NoticeAdministration(call.CallJob.Sponsor.AddressId));

            this.noticeAdministrationTextBox.Text = sb.ToString();

        }

        private void FillNoticeHistory(Call call)
        {
            if (this.isInitializing)
            {
                return;
            }

            if (call == null)
            {
                throw new ArgumentNullException("call");
            }

            if (call.CallJob == null)
            {
                throw new ArgumentNullException("call.CallJob");
            }

            this.callJobResultList = MetaCall.Business.CallJobs.GetCallJobResults(call.CallJob);

            StringBuilder sb = new StringBuilder();

            foreach (CallJobResult result in this.callJobResultList)
            {
                sb.Append(FormatResultHistory(result));
            }

            sb.AppendLine();
            sb.Append(MetaCall.Business.Addresses.GetAddress_HistoryNotice(call.CallJob.Sponsor.AddressId));

            this.noticeHistoryTextBox.Text = sb.ToString();
        }

        private string FormatResultHistory(CallJobResult result)
        {
            if (result == null)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(result.Notice))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0:d} {0:t} {1}:", result.Start, result.User.DisplayName);
            sb.AppendLine();
            sb.AppendLine(result.Notice);
            sb.AppendLine(new string('=', 70));

            return sb.ToString();
        }

        private void CreateCallNotice_Load(object sender, EventArgs e)
        {
            if (this.isInitializing)
            {
                return;
            }

            GroupBox grpBranch;
            grpBranch = new GroupBox();
            grpBranch.Parent = this;
            grpBranch.Text = "Branche/Branchengruppe";
            grpBranch.Location = new Point(10, 5);
            grpBranch.Size = new Size(480, 50);

            GroupBox grpNotic;
            grpNotic = new GroupBox();
            grpNotic.Parent = this;
            grpNotic.Text = "Notizen";
            grpNotic.Location = new Point(10, 65);
            grpNotic.Size = new Size(480, this.noticeTabControl.Height + 30);
            this.noticeTabControl.Location = new Point(grpNotic.Left + 10, grpNotic.Top + 20);

            formHeight = grpNotic.Height + grpBranch.Height + 30;

            if (!DesignMode)
            {
                FillBranch_BranchGroupComboBox();
            }
        }

        #region ISupportInitialize Member

        bool isInitializing;
        public void BeginInit()
        {
            this.isInitializing = true;
        }

        public void EndInit()
        {
            this.isInitializing = false;
        }

        #endregion

        private void Branch_BranchGroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Branch_BranchGroupComboBox.SelectedItem != null)
            {
                if (this.Branch_BranchGroupComboBox.SelectedItem is Branch)
                {
                    selectedBranch = (Branch)this.Branch_BranchGroupComboBox.SelectedItem;
                    selectedBranchGroup = selectedBranch.BranchGroup;
                }
                else if (this.Branch_BranchGroupComboBox.SelectedItem is BranchGroup)
                {
                    selectedBranch = Branch.Unknown;
                    selectedBranchGroup = (BranchGroup)this.Branch_BranchGroupComboBox.SelectedItem;
                }
                else
                {
                    SetSelectedBranch_BranchGroup();
                }

                this.currentCall.CallJob.Sponsor.Branch = SelectedBranch;
                this.currentCall.CallJob.Sponsor.BranchGroup = SelectedBranchGroup;

                OnSponsorInfoChange(new SponsorInfoChangeEventArgs(this.currentCall.CallJob.Sponsor));
            }
        }

        protected void OnSponsorInfoChange(SponsorInfoChangeEventArgs e)
        {
            if (SponsorInfoChange != null)
            {
                SponsorInfoChange(this, e);
            }
        }

        #region IInitializeCall Member

        public void InitializeCall(Call call)
        {
            if (this.isInitializing) return;

            if (call == null)
                throw new ArgumentNullException("call");

            this.currentCall = call;
            this.noticeTextBox.Text = null;
            selectedBranchGroup = call.CallJob.Sponsor.BranchGroup;
            selectedBranch = call.CallJob.Sponsor.Branch;
            if (selectedBranchGroup == null && (selectedBranch == null || selectedBranch.Equals(Branch.Unknown)))
            {
                selectedBranch = null;
            }
            SetSelectedBranch_BranchGroup();

            FillNoticeHistory(call);
            FillNoticeAdministration(call);

            foreach (Control ctl in this.Controls)
            {
                IInitializeCall initializeCallControl = ctl as IInitializeCall;
                if (initializeCallControl != null)
                {
                    initializeCallControl.InitializeCall(call);
                }
            }
        }

        #endregion

        private void Branch_BranchGroupComboBox_NotInList(object sender, CancelEventArgs e)
        {
            if (SelectedBranchGroup != null)
            {
                MessageBox.Show("Sie können nur einen Eintrag aus der Liste wählen!", "Kein Element der Liste");
                e.Cancel = true;
            }
        }
    }
}
