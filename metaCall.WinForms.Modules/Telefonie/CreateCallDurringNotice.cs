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
    public partial class CreateCallDurringNotice : UserControl, ISupportInitialize, IInitializeCall
    {
        List<CallJobResult> callJobResultList;

        private int formHeight = 0;

        public int FormHeight
        {
            get {

                foreach (Control ctl in this.Controls)
                {
                    if (ctl is GroupBox)
                    {
                        return ctl.Height + 30;
                    }
                }
                return 0;
            }
        }

        public string Notice
        {
            get 
            {
                return this.noticeTextBox.Text; 
            }
        }

        public CreateCallDurringNotice()
        {
            InitializeComponent();
        }

        private void FillNoticeHistory(Call call)
        {
            if (this.isInitializing) return; 
            
            if (call == null)
                throw new ArgumentNullException("call");

            if (call.CallJob == null)
                throw new ArgumentNullException("call.CallJob");

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
                return string.Empty;

            if (string.IsNullOrEmpty(result.Notice))
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0:d} {0:t} {1}:", result.Start, result.User.DisplayName);
            sb.AppendLine();
            sb.AppendLine(result.Notice);
            sb.AppendLine(new string('=', 70));

            return sb.ToString();
        }

        private void CreateCallNotice_Load(object sender, EventArgs e)
        {
            if (this.isInitializing) return;

            //this.SuspendLayout();

            //GroupBox grpNotic;
            //grpNotic = new GroupBox();
            //grpNotic.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
            //grpNotic.Text = "Notizen";
            //grpNotic.Location = new Point(10, 10);
            //grpNotic.Size = new Size(480, this.noticeTabControl.Height + 30);
            //this.noticeTabControl.Location = new Point(grpNotic.Left + 10, grpNotic.Top + 20);

            //grpNotic.Parent = this;


            //this.ResumeLayout();
            //this.PerformLayout();
            //this.noticeTabControl.Parent = grpNotic;
            //formHeight = grpNotic.Height + 30;

        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            //Size grpBoxSize = new Size(480,
            //    this.Height - 20);
            //Point grpBoxLocation = new Point(10, 10);
            //this.groupBox1.Location = grpBoxLocation;
            //this.groupBox1.Size = grpBoxSize;

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


        #region IInitializeCall Member

        public void InitializeCall(Call call)
        {
            if (this.isInitializing) return;

            if (call == null)
                throw new ArgumentNullException("call");

            this.noticeTextBox.Text = null;
            FillNoticeHistory(call);
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
    }
}
