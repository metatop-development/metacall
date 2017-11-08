using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer;
using System.Globalization;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class HistorieInfo : UserControl, metatop.Applications.metaCall.WinForms.Modules.Telefonie.IInitializeCall
    {
        private Call call;
        private DataTable dtCallJobResults = new DataTable("CallJobResults");
        

        public HistorieInfo()
        {
           
            InitializeComponent();

            dtCallJobResults.Locale = CultureInfo.CurrentUICulture;
            bindingSource1.DataSource = dtCallJobResults;

            SetupDataTable();
            
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }
        
        public HistorieInfo(Call call)
            : this()
        {
            this.call = call;
        }

        public void InitializeCall(Call call)
        {
            this.call = call;
            LoadCallJobResultsIntoDataTable(this.call);
        }

        private void SetupDataTable()
        {
            
            DataTableHelper.AddColumn(this.dtCallJobResults, "Date", "Date", typeof(DateTime));
            DataTableHelper.AddColumn(this.dtCallJobResults, "Description", "Description", typeof(string));
            //DataTableHelper.AddColumn(this.dtCallJobResults, "Notice", "Notice", typeof(string)); 
            DataTableHelper.AddColumn(this.dtCallJobResults, "CallJobResult", string.Empty, typeof(CallJobResult), MappingType.Hidden);

            DataTableHelper.FillGridView(this.dtCallJobResults, this.dgvCallJobResults);
        }

        private void LoadCallJobResultsIntoDataTable(Call call)
        {
            if (call.CallJob == null)
                return;


            bindingSource1.SuspendBinding();

            try
            {

                CallJob callJob = this.call.CallJob;

                List<CallJobResult> results = MetaCall.Business.CallJobs.GetCallJobResults(callJob);
                dtCallJobResults.Rows.Clear();
                foreach (CallJobResult result in results)
                {

                    StringBuilder description = new StringBuilder();
                    description.AppendFormat("{0} durch {1}", result.ContactType.DisplayName, result.User.DisplayName);

                    object[] objectData = new object[]{
                        result.Start, 
                        description, 
                        //result.Notice,
                        result
                        };
                    this.dtCallJobResults.Rows.Add(objectData);
                }
            }
            finally
            {
                bindingSource1.ResumeBinding();
            }

        }

        private void dgvCallJobResults_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            CallJobResult result = dtCallJobResults.Rows[e.RowIndex]["CallJobResult"] as CallJobResult;

            if (result != null)
                e.ToolTipText = FormatToolTip(result);

        }


        private string FormatToolTip(CallJobResult callJobResult)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0:d} {0:t} ({1}):\n", callJobResult.Start, callJobResult.User.DisplayName);
            sb.AppendFormat("Kontaktart: {0}\n", callJobResult.ContactType.DisplayName);
            sb.AppendFormat("Notiz: {0}", callJobResult.Notice);

            return sb.ToString();
        }

    }
}
