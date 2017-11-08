using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

using metatop.Applications.metaCall.DataObjects;
using System.Xml;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class InvoiceDAL
    {
        #region StoredProcedures

        private static string spInvoice_GetBySingle = "dbo.Invoice_GetBySingle";
        private static string spInvoiceItems_GetByInvoice = "dbo.InvoiceItems_GetByInvoice";
        private static string spInvoicePayments_GetByInvoice = "dbo.InvoicePayments_GetByInvoice";

        #endregion

        public static Invoice GetInvoice(Guid? callJobId)
        {
            if (!callJobId.HasValue)
                return null;

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobId", callJobId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spInvoice_GetBySingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToInvoice(dataTable.Rows[0]);
        }

        private static Invoice ConvertToInvoice(DataRow row)
        {

            Invoice invoice = new Invoice();
            invoice.Auftragsdatum = (DateTime)row["Auftragsdatum"];
            invoice.Auftragsnummer = (int)row["Auftragsnummer"];
            invoice.Bemerkung = (string)SqlHelper.GetNullableDBValue(row["Bemerkung"]);
            
            invoice.FaelligAm = (DateTime?)SqlHelper.GetNullableDBValue(row["Faellig_Am"]);
            invoice.Mahnstufe2 = (int?)SqlHelper.GetNullableDBValue(row["Mahnstufe2"]);
              
            invoice.ProjekteRechnungsnummer = (int)row["ProjekteRechnungsnummer"];
            invoice.Rechnungsdatum = (DateTime)row["Rechnungsdatum"];
            invoice.Rechnungsnummer  = (int)row["Rechnungsnummer"];
            invoice.Werbetext = (string)SqlHelper.GetNullableDBValue(row["Werbetext"]);
            invoice.IstBezahlt = (bool)SqlHelper.GetNullableDBValue(row["IstBezahlt"]);
            invoice.Verkaeufer = (string)row["Verkaeufer"];
            
            invoice.InvoiceItems = GetInvoiceItems(invoice.ProjekteRechnungsnummer);
            invoice.InvoicePayments = GetInvoicePayments(invoice.ProjekteRechnungsnummer);
            
            return invoice;
        }

        public static InvoiceItem[] GetInvoiceItems(int projekteRechnungsnummer)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjekteRechnungsnummer", projekteRechnungsnummer);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spInvoiceItems_GetByInvoice, parameters);

            return (InvoiceItem[])ConvertToInvoiceItems(dataTable);
        }

        private static InvoiceItem[] ConvertToInvoiceItems(DataTable dataTable)
        {
            InvoiceItem[] invoiceItems = new InvoiceItem[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                invoiceItems[i] = ConvertToInvoiceItem(row);
            }
            
            return invoiceItems;
        }

        private static InvoiceItem ConvertToInvoiceItem(DataRow row)
        {
            InvoiceItem invoiceItem = new InvoiceItem();

            invoiceItem.Betrag_netto = (decimal)row["Betrag_netto"];
            invoiceItem.Bezeichnung = (string)row["Bezeichnung"];
            invoiceItem.ProjekteRechnungenPositionenNummer = (int)row["ProjekteRechnungenPositionenNummer"];
            invoiceItem.Stueckzahl = (decimal)row["Stueckzahl"];

            return invoiceItem;
        }

        public static InvoicePayment[] GetInvoicePayments(int projekteRechnungsnummer)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjekteRechnungsnummer", projekteRechnungsnummer);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spInvoicePayments_GetByInvoice, parameters);

            return (InvoicePayment[])ConvertToInvoicePayments(dataTable);

        }

        private static InvoicePayment[] ConvertToInvoicePayments(DataTable dataTable)
        {
            InvoicePayment[] invoicePayments = new InvoicePayment[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                invoicePayments[i] = ConvertToInvoicePayment(row);
            }

            return invoicePayments;
        }
    
        private static InvoicePayment ConvertToInvoicePayment(DataRow row)
        {
            InvoicePayment invoicePayment = new InvoicePayment();

            invoicePayment.Art = (string)row["Art"];
            invoicePayment.Bemerkung = (string)row["Bemerkung"];
            invoicePayment.Betrag = (decimal)row["Betrag"];
            invoicePayment.Buchungsdatum = (DateTime)row["Buchungsdatum"];
            invoicePayment.ProjekteRechnungenZahlungsnummer = (int)row["ProjekteRechnungenZahlungsnummer"];
            invoicePayment.Sachkonto = (string)row["Sachkonto"];
            invoicePayment.Waehrung = (string)row["Waehrung"];

            return invoicePayment;
        }
    
    }
}
