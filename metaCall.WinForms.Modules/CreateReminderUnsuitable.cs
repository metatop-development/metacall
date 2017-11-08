using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;
using MaDaNet.Common.AppFrameWork.Validation;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class CreateReminderUnsuitable : UserControl
    {
        private DataTable dtContactTypesParticipationUnsuitable = new DataTable();

        private int formHeight = 0;
        private int formHeightRest = 0;

        public CreateReminderUnsuitable()
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

        public ContactTypesParticipationUnsuitable ContactTypeParticipationUnsuitable
        {
            get
            {
                if (cBResultContactTypeParticipationUnsuitable.SelectedItem != null)
                    return (ContactTypesParticipationUnsuitable)dtContactTypesParticipationUnsuitable.Rows[cBResultContactTypeParticipationUnsuitable.SelectedIndex]["ContactTypesParticipationUnsuitable"];
                else
                    return null;
            }
        }


        private void setupDataTableCTPU()
        {
            dtContactTypesParticipationUnsuitable = new DataTable("ContactTypesParticipationUnsuitable");

            AddColumn(dtContactTypesParticipationUnsuitable, "ContactTypesParticipationUnsuitableId", "ContactTypesParticipationUnsuitble", typeof(Guid));
            AddColumn(dtContactTypesParticipationUnsuitable, "DisplayName", "ContactTypesParticipationUnsuitableId", typeof(string));
            AddColumn(dtContactTypesParticipationUnsuitable, "ContactTypesParticipationUnsuitable", "", typeof(ContactTypesParticipationUnsuitable));

            dtContactTypesParticipationUnsuitable.Rows.Clear();
            foreach (ContactTypesParticipationUnsuitable contactTypesParticipationUnsuitable in MetaCall.Business.ContactTypesParticipationUnsuitable.ContactTypesParticipationsUnsuitable)
            {
                object[] rowData = new object[]{
                    contactTypesParticipationUnsuitable.ContactTypesParticipationUnsuitableId,
                    contactTypesParticipationUnsuitable.DisplayName,
                    contactTypesParticipationUnsuitable};

                dtContactTypesParticipationUnsuitable.Rows.Add(rowData);
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

        private void BindComboBox(DataTable dataTable, ComboBox cbTarget, int displayColumnIndex, string valueMember)
        {
            for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dataTable.Rows[i];
                if ((Guid)dr[0] == new Guid("{7F500022-097C-447A-B02F-8F2B8BAA87DC}") ||
                     (Guid)dr[0] == new Guid("{45D708DB-106B-4FDF-9002-9C1C3E38A605}"))
                {
                    dr.Delete();
                }
            }
            dataTable.AcceptChanges();

            cbTarget.DataSource = dataTable;
            cbTarget.DisplayMember = dataTable.Columns[displayColumnIndex].ColumnName;

            if (valueMember != "")
            {
                cbTarget.ValueMember = valueMember;
            }

        }
        private void CreateReminderUnsuitable_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            setupDataTableCTPU();
            BindComboBox(dtContactTypesParticipationUnsuitable, this.cBResultContactTypeParticipationUnsuitable, 1, "DisplayName");
        }

    }
}
