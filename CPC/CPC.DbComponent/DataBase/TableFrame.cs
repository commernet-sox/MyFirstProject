using System;
using System.Collections;
using System.Data;

namespace CPC.DbComponent
{
    internal class TableFrame
    {
        // Fields
        private readonly Hashtable ht = new Hashtable();

        // Methods

        internal void AddTableFrame(string tableName, DataTable tableFrame) => ht[tableName] = tableFrame;

        internal bool ColumnIsNull(string tableName, string columnName)
        {
            var table = (DataTable)ht[tableName];
            for (var i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][0].ToString().Trim().ToUpper() == columnName.ToUpper())
                {
                    return Convert.ToBoolean(table.Rows[i][5]);
                }
            }
            return false;
        }

        internal bool ColumnIsPk(string tableName, string columnName)
        {
            var table = (DataTable)ht[tableName];
            for (var i = 0; i < table.Rows.Count; i++)
            {
                if ((table.Rows[i][0].ToString().Trim().ToUpper() == columnName.ToUpper()) &&
                    (table.Rows[i][9].ToString().Trim() != ""))
                {
                    return true;
                }
            }
            return false;
        }

        internal bool ExistColumn(string tableName, string columnName)
        {
            var table = (DataTable)ht[tableName];
            for (var i = 0; i < table.Rows.Count; i++)
            {
                if (columnName.Equals(table.Rows[i][0].ToString(), StringComparison.OrdinalIgnoreCase)
                    || columnName.Substring(1, columnName.Length - 2).Equals(table.Rows[i][0].ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        internal bool ExistTableFrame(string tableName) => ht.Contains(tableName);

        internal DataTable GetTableFrame(string tableName) => (DataTable)ht[tableName];
    }
}