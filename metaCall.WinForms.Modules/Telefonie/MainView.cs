using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class MainView : UserControl
    {
        public event NewCustomerEventHandler NewCustomer;
        public event ProjectChangedEventHandler ProjectChanged;
        public event UserWantSpecialCallHandler UserWantSpecialCall;
        public event DurringChangedEventHandler DurringActivChanged;

        public MainView()
        {
            InitializeComponent();
            if (MetaCall.Business.CallJobs.DurringActiv)
                this.durringInfoPanel1.Visible = true;
            else
                this.durringInfoPanel1.Visible = false;
        }

        private void projektanmeldung1_NewCustomer(object sender, NewCustomerEventArgs e)
        {
            OnNewCustomer(e);
        }

        private void OnNewCustomer(NewCustomerEventArgs e)
        {
            if (NewCustomer != null)
                NewCustomer(this, e);
        }

        private void durringInfo1_DurringActivChanged(object sender, DurringChangedEventArgs e)
        {
            if (e.DurringAcitv == true)
            {
                this.durringInfoPanel1.Visible = true;
            }
            else
            {
                this.durringInfoPanel1.Visible = false;
            }
        }

        private void OnDurringActivChanged(DurringChangedEventArgs e)
        {
            if (DurringActivChanged != null)
                DurringActivChanged(this, e);
        }

        private void projektanmeldung1_ProjectChanged(object sender, ProjectChangedEventArgs e)
        {
            OnProjectChanged(e);
        }

        private void OnProjectChanged(ProjectChangedEventArgs e)
        {
            if (ProjectChanged != null)
                ProjectChanged(this, e);
        }

        private void userInfoPanel1_UserWantSpecialCall(object sender, UserWantSpecialCallEventArgs e)
        {
            OnUserWantSpecialCall(e);
        }

        private void durringInfoPanel1_UserWantSpecialCall(object sender, UserWantSpecialCallEventArgs e)
        {
            OnUserWantSpecialCall(e);
        }

        protected void OnUserWantSpecialCall(UserWantSpecialCallEventArgs e)
        {
            if (UserWantSpecialCall != null)
                UserWantSpecialCall(this, e);
        }


        public void UpdateStatistics(metatop.Applications.metaCall.BusinessLayer.SponsoringCallManager sponsoringCallManager)
        {
            this.projektanmeldung1.UpdateStatistics(sponsoringCallManager);
        }

        private void projektanmeldung1_DurringLevelInfoChanged(object sender, metatop.Applications.metaCall.BusinessLayer.DurringLevelInfoChangedEventArgs e)
        {
            this.durringInfoPanel1.DurringLevelInfoChanged();
        }
    }
}
