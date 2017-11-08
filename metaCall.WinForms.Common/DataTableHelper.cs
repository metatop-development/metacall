using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace metatop.Applications.metaCall.WinForms
{
    public static class DataTableHelper
    {
        public static DataColumn AddColumn(DataTable target, string name, string caption, Type dataType)
        {
            DataColumn col = new DataColumn();

            col.ColumnName = name;
            col.Caption = caption;
            col.DataType = dataType;
            
            target.Columns.Add(col);

            return col;

        }

        public static DataColumn AddColumn(DataTable target, string name, string caption, Type dataType, MappingType mappingType)
        {
            DataColumn col = AddColumn(target, name, caption, dataType);
            col.ColumnMapping = mappingType;

            return col;
        }

        public static void FillGridView(DataTable dataTable, DataGridView targetGrid)
        {
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                if (dataColumn.ColumnMapping != MappingType.Hidden)
                {
                    DataGridViewColumn col;

                    if ((dataColumn.DataType == typeof(Icon)) ||
                        (dataColumn.DataType == typeof(Image)))
                    {
                        col = new DataGridViewImageColumn();
                        ((DataGridViewImageColumn)col).ImageLayout = DataGridViewImageCellLayout.Zoom;
                    }
                    else if (dataColumn.DataType == typeof(Boolean))
                    {
                        col = new DataGridViewCheckBoxColumn();
                    }
                    else
                    {
                        col = new DataGridViewTextBoxColumn();
                    }


                    col.DataPropertyName = dataColumn.ColumnName;
                    col.HeaderText = dataColumn.Caption;
                    targetGrid.Columns.Add(col);
                }
            }
        }

    }
}
