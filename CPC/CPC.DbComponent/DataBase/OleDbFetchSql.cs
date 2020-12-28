using System;
using System.Data;
using System.Text;

namespace CPC.DbComponent
{
    public class OleDbFetchSql : FetchSql
    {
        // Fields
        private readonly IDbUtil _dbUtil;

        public IDbUtil DbUtil => _dbUtil;

        private StringBuilder retCondiSqlMode;
        private StringBuilder retInCurCondiSqlMode;

        // Methods
        public OleDbFetchSql(IDbUtil dbUtil) => _dbUtil = dbUtil;

        public override string FetchAddedSql(DataRowView drv)
        {
            var table = drv.Row.Table;
            var tableName = table.TableName;
            var format = "INSERT INTO {0} ({1}) VALUES ({2})";
            var builder = new StringBuilder();
            var builder2 = new StringBuilder();
            for (var i = 0; i < table.Columns.Count; i++)
            {
                if (table.Columns[i].AutoIncrement)
                {
                    continue;
                }

                if ((drv[i] != DBNull.Value) && (drv[i] != null))
                {
                    var columnName = string.Format("[{0}]", table.Columns[i].ColumnName);
                    if (TableInfo.ExistColumn(_dbUtil, tableName, columnName))
                    {
                        builder.Append(columnName + ",");
                        builder2.Append(Sundry.RowValueToString(drv[i], DbUtil.DatabaseType) + ",");
                    }
                }
            }
            var str4 = builder.ToString();
            var str5 = builder2.ToString();
            if (str4 == "")
            {
                return "";
            }
            str4 = str4.Substring(0, str4.Length - 1);
            str5 = str5.Substring(0, str5.Length - 1);
            return string.Format(format, tableName, str4, str5);
        }

        public override string FetchDeleteSql(DataRowView drv, bool interCurrent, string[] obj)
        {
            var tableName = drv.Row.Table.TableName;
            var format = "DELETE FROM {0} WHERE {1}";
            if (interCurrent)
            {
                return string.Format(format, tableName, GetInterCurrentCondition(drv, obj));
            }
            return string.Format(format, tableName, GetMasterKeyCondition(drv));
        }

        public override string FetchModifySql(DataRowView drv, bool interCurrent, string[] obj)
        {
            var format = "UPDATE {0} SET {1} WHERE {2}";
            var builder = new StringBuilder();
            var table = drv.Row.Table;
            var tableName = table.TableName;
            for (var i = 0; i < table.Columns.Count; i++)
            {
                var columnName = table.Columns[i].ColumnName;
                if (TableInfo.ExistColumn(_dbUtil, tableName, columnName))
                {
                    var str4 = " {0} = {1},";
                    str4 = string.Format(str4, columnName,
                                         Sundry.RowValueToString(drv[columnName], DbUtil.DatabaseType));
                    builder.Append(str4);
                }
            }
            var str5 = builder.ToString();
            if (str5 == "")
            {
                return "";
            }
            str5 = str5.Substring(0, str5.Length - 1);
            if (interCurrent)
            {
                format = string.Format(format, tableName, str5, GetInterCurrentCondition(drv, obj));
            }
            else
            {
                format = string.Format(format, tableName, str5, GetMasterKeyCondition(drv));
            }
            return format;
        }

        protected string GetInterCurrentCondition(DataRowView drv, string[] obj)
        {
            var table = drv.Row.Table;
            var tableName = table.TableName;
            if (retInCurCondiSqlMode.Length == 0)
            {
                for (var j = 0; j < table.Columns.Count; j++)
                {
                    var columnName = table.Columns[j].ColumnName;
                    if (TableInfo.ColumnIsPK(_dbUtil, tableName, columnName))
                    {
                        var str3 = "(" + columnName + " = {" + j.ToString() + "} ) AND ";
                        retInCurCondiSqlMode.Append(str3);
                    }
                    else
                    {
                        string str4;
                        if (obj != null)
                        {
                            for (var k = 0; k < obj.Length; k++)
                            {
                                if (obj[k].Trim() == columnName)
                                {
                                    if (TableInfo.ColumnIsNull(_dbUtil, tableName, columnName))
                                    {
                                        str4 = "({0} = {1} OR {1} IS NULL AND {0} IS NULL) AND ";
                                        str4 = string.Format(str4, columnName, "{" + j.ToString() + "}");
                                        retInCurCondiSqlMode.Append(str4);
                                    }
                                    else
                                    {
                                        str4 = "({0} = {1} ) AND ";
                                        str4 = string.Format(str4, columnName, "{" + j.ToString() + "}");
                                        retInCurCondiSqlMode.Append(str4);
                                    }
                                    break;
                                }
                            }
                        }
                        else if (TableInfo.ExistColumn(_dbUtil, tableName, table.Columns[j].ColumnName))
                        {
                            if (TableInfo.ColumnIsNull(_dbUtil, tableName, columnName))
                            {
                                str4 = "({0} = {1} OR {1} IS NULL AND {0} IS NULL) AND ";
                                str4 = string.Format(str4, columnName, "{" + j.ToString() + "}");
                                retInCurCondiSqlMode.Append(str4);
                            }
                            else
                            {
                                str4 = "({0} = {1} ) AND ";
                                str4 = string.Format(str4, columnName, "{" + j.ToString() + "}");
                                retInCurCondiSqlMode.Append(str4);
                            }
                        }
                    }
                }
            }
            var format = retInCurCondiSqlMode.ToString();
            if (format == "")
            {
                throw new Exception("找不到表" + tableName + "的主键");
            }
            format = format.Substring(0, format.Length - 4);
            var args = new object[table.Columns.Count];
            for (var i = 0; i < table.Columns.Count; i++)
            {
                args[i] = Sundry.RowValueToString(drv.Row[i, DataRowVersion.Original], DbUtil.DatabaseType);
            }
            return string.Format(format, args);
        }

        protected string GetMasterKeyCondition(DataRowView drv)
        {
            var table = drv.Row.Table;
            var tableName = table.TableName;
            if (retCondiSqlMode.Length == 0)
            {
                for (var j = 0; j < table.Columns.Count; j++)
                {
                    var columnName = table.Columns[j].ColumnName;
                    if (TableInfo.ColumnIsPK(_dbUtil, tableName, columnName))
                    {
                        var str3 = "{0} = {1} AND ";
                        str3 = string.Format(str3, columnName, "{" + j.ToString() + "}");
                        retCondiSqlMode.Append(str3);
                    }
                }
            }
            var format = retCondiSqlMode.ToString();
            if (format == "")
            {
                throw new Exception("找不到表" + tableName + "的主键");
            }
            format = format.Substring(0, format.Length - 4);
            var args = new object[table.Columns.Count];
            for (var i = 0; i < table.Columns.Count; i++)
            {
                args[i] = Sundry.RowValueToString(drv.Row[i, DataRowVersion.Original], DbUtil.DatabaseType);
            }
            return string.Format(format, args);
        }

        private string GetSelectMasterKeyCondition(DataRowView drv)
        {
            var table = drv.Row.Table;
            var tableName = table.TableName;
            if (retCondiSqlMode.Length == 0)
            {
                for (var j = 0; j < table.Columns.Count; j++)
                {
                    var columnName = table.Columns[j].ColumnName;
                    if (TableInfo.ColumnIsPK(_dbUtil, tableName, columnName))
                    {
                        var str3 = "{0} = {1} AND ";
                        str3 = string.Format(str3, columnName, "{" + j.ToString() + "}");
                        retCondiSqlMode.Append(str3);
                    }
                }
            }
            var format = retCondiSqlMode.ToString();
            if (format == "")
            {
                throw new Exception("找不到表" + tableName + "的主键");
            }
            format = format.Substring(0, format.Length - 4);
            var args = new object[table.Columns.Count];
            for (var i = 0; i < table.Columns.Count; i++)
            {
                args[i] = Sundry.RowValueToString(drv.Row[i, DataRowVersion.Current], DbUtil.DatabaseType);
            }
            return string.Format(format, args);
        }

        public override void Init()
        {
            retInCurCondiSqlMode = new StringBuilder();
            retCondiSqlMode = new StringBuilder();
        }

        public override string ReFillRowItem(DataRowView drv)
        {
            var tableName = drv.Row.Table.TableName;
            var format = "SELECT * FROM {0} WHERE {1}";
            return string.Format(format, tableName, GetSelectMasterKeyCondition(drv));
        }
    }
}