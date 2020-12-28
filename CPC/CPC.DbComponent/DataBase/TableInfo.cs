using System;
using System.Collections;
using System.Data;

namespace CPC.DbComponent
{
    public class TableInfo
    {
        // Fields
        private static readonly Hashtable infoList = new Hashtable();

        // Methods
        internal TableInfo() => throw new Exception("TableInfo不能实例化");

        public static void Clear(IDbUtil dbUtil)
        {
            if (infoList.Contains(dbUtil))
            {
                infoList.Remove(dbUtil);
            }
        }

        public static void AddTableFrame(object dbUtil, string tableName)
        {
            if (!(dbUtil is IDbUtil))
            {
                throw new Exception("KEY非IDataBase类型");
            }
            var iDbUtil = dbUtil as IDbUtil;
            if (infoList.Contains(dbUtil))
            {
                var frame = (TableFrame)infoList[dbUtil];
                frame.AddTableFrame(tableName, iDbUtil.GetTableFrame(tableName));
                infoList[dbUtil] = frame;
            }
            else
            {
                var frame2 = new TableFrame();
                frame2.AddTableFrame(tableName, iDbUtil.GetTableFrame(tableName));
                infoList[dbUtil] = frame2;
            }
        }

        public static bool ColumnIsNull(object dbUtil, string tableName, string columnName) => ((TableFrame)infoList[dbUtil]).ColumnIsNull(tableName, columnName);

        public static bool ColumnIsPK(object dbUtil, string tableName, string columnName) => ((TableFrame)infoList[dbUtil]).ColumnIsPk(tableName, columnName);

        public static bool ExistColumn(object dbUtil, string tableName, string columnName) => ((TableFrame)infoList[dbUtil]).ExistColumn(tableName, columnName);

        public static bool ExistTableFrame(object dbUtil, string tableName) => (infoList.Contains(dbUtil) && ((TableFrame)infoList[dbUtil]).ExistTableFrame(tableName));

        public static DataTable GetTableFrame(object dbUtil, string tableName)
        {
            if (!infoList.Contains(dbUtil))
            {
                AddTableFrame(dbUtil, tableName);
            }
            else if (!((TableFrame)infoList[dbUtil]).ExistTableFrame(tableName))
            {
                AddTableFrame(dbUtil, tableName);
            }
            return ((TableFrame)infoList[dbUtil]).GetTableFrame(tableName);
        }

        public static void Remove(object dbUtil) => infoList.Remove(dbUtil);
    }
}