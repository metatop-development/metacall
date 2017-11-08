using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.DataObjects;
using System.Globalization;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class InvoiceInfo : UserControl, IInitializeCall
    {
        private DurringCallJob callJob;
        private DataTable dtInvoiceItems = new DataTable();

        private int formHeightRest;

        public InvoiceInfo()
        {
            InitializeComponent();
            //SetupDataTableInvoiceItems();
        }

        public int FormHeightRest
        {
            set
            {
                formHeightRest = value;

            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.groupBoxInvoice.Size = new Size(480, this.Height - 15);
        }

        private void UpdateInvoiceInformations()
        {
            if (callJob != null)
            {
                Invoice invoice = callJob.Invoice;

                this.labelRechnungsnummer.Text = invoice.Rechnungsnummer.ToString();
                this.labelRechnungsdatum.Text = invoice.Rechnungsdatum.ToShortDateString();
                this.labelAuftragsnummer.Text = invoice.Auftragsnummer.ToString();
                this.labelAuftragsdatum.Text = invoice.Auftragsdatum.ToShortDateString();
                this.labelMahnstufe.Text = invoice.Mahnstufe2.ToString();
                this.labelFaellig_am.Text = string.Format("{0:d}", invoice.FaelligAm);
                this.labelVerkaeufer.Text = invoice.Verkaeufer;

                FillInvoiceItemDataTable(invoice.InvoiceItems);
            }
            else
            {
                this.labelRechnungsnummer.Text = null;
                this.labelRechnungsdatum.Text = null;
                this.labelAuftragsnummer.Text = null;
                this.labelAuftragsdatum.Text = null;
                this.labelMahnstufe.Text = null;
                this.labelFaellig_am.Text = null;
                this.labelVerkaeufer.Text = "";

                FillInvoiceItemDataTable(null);

            }
        }

        private void SetupDataTableInvoiceItems()
        {
            dtInvoiceItems = new DataTable();

            DataTableHelper.AddColumn(this.dtInvoiceItems, "Bezeichnung", "Bezeichnung", typeof(string));
            DataTableHelper.AddColumn(this.dtInvoiceItems, "Stueckzahl", "Stückzahl", typeof(decimal));
            DataTableHelper.AddColumn(this.dtInvoiceItems, "Betrag_netto", "Nettobetrag", typeof(decimal));

            DataTableHelper.FillGridView(this.dtInvoiceItems,
                this.dGVInvioceItems);

            dGVInvioceItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGVInvioceItems.RowHeadersVisible = false;
            dGVInvioceItems.ColumnHeadersVisible = true;
            dGVInvioceItems.AutoGenerateColumns = false;

            dGVInvioceItems.Columns[0].Width = 350;
            dGVInvioceItems.Columns[1].Width = 40;
            dGVInvioceItems.Columns[1].DefaultCellStyle.Format = "#,##0.00";
            dGVInvioceItems.Columns[1].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;
            dGVInvioceItems.Columns[2].Width = 40;
            dGVInvioceItems.Columns[2].DefaultCellStyle.Format = "#,##0.00";
            dGVInvioceItems.Columns[2].DefaultCellStyle.Alignment = 
                DataGridViewContentAlignment.MiddleRight;

        }

        private void FillInvoiceItemDataTable(InvoiceItem[] invoiceItem)
        {

            dtInvoiceItems.Rows.Clear();
            if (invoiceItem != null)
            {
                foreach (InvoiceItem iItem in invoiceItem)
                {
                    object[] rowData = new object[]{
                        iItem.Bezeichnung,
                        iItem.Stueckzahl, 
                        iItem.Betrag_netto};

                    dtInvoiceItems.Rows.Add(rowData);
                }
            }

            dGVInvioceItems.DataSource = dtInvoiceItems;
        }

        #region IInitializeCall Member

        public void InitializeCall(Call call)
        {
            if (call.CallJob.GetType() == (typeof(DurringCallJob)))
                this.callJob = (DurringCallJob)call.CallJob;
            else
                this.callJob = null;

            UpdateInvoiceInformations();

        }

        #endregion

        private void InvoiceInfo_Load(object sender, EventArgs e)
        {
            SetupDataTableInvoiceItems();
        }
    }
}
