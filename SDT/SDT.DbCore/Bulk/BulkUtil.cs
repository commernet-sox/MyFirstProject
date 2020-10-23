using SDT.BaseTool;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SDT.DbCore
{
    internal class BulkUtil
    {
        #region Members
        internal struct PrecisionType
        {
            public string NumericPrecision { get; set; }

            public string NumericScale { get; set; }
        }
        #endregion

        internal static ColumnSelect<T> AddColumnMapping<T>(ColumnSelect<T> column, string propertyName)
        {
            if (column == null)
            {
                return column;
            }

            var proper = typeof(T).GetProperty(propertyName);
            if (proper == null)
            {
                return column;
            }

            var colAttr = proper.GetCustomAttribute<ColumnAttribute>(true);
            if (colAttr != null && !colAttr.Name.IsNull())
            {
                column.Option.CustomColumnMappings[proper.Name] = colAttr.Name;
            }

            return column;
        }

        internal static DataTable ToDataTable<T>(IEnumerable<T> items, HashSet<string> columns, Dictionary<string, string> columnMappings, List<string> matchOnColumns = null, bool? outputIdentity = null, Dictionary<int, T> outputIdentityDic = null)
        {
            var dataTable = new DataTable(typeof(T).Name);

            if (matchOnColumns != null)
            {
                columns = CheckForAdditionalColumns(columns, matchOnColumns);
            }

            if (outputIdentity.HasValue && outputIdentity.Value)
            {
                columns.Add("InternalId");
            }

            //Get all the properties
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var column in columns)
            {
                if (columnMappings.ContainsKey(column))
                {
                    dataTable.Columns.Add(columnMappings[column]);
                }
                else
                {
                    dataTable.Columns.Add(column);
                }
            }

            AssignTypes(props, columns, dataTable, outputIdentity);

            var counter = 0;

            foreach (var item in items)
            {
                var values = new List<object>();

                foreach (var column in columns)
                {
                    if (column == "InternalId")
                    {
                        values.Add(counter);
                        outputIdentityDic.Add(counter, item);
                    }
                    else
                    {
                        for (var i = 0; i < props.Length; i++)
                        {
                            if (props[i].Name == column && item != null)
                            {
                                values.Add(props[i].GetValue(item, null));
                            }
                        }
                    }
                }

                counter++;
                dataTable.Rows.Add(values.ToArray());
            }

            return dataTable;
        }

        private static void AssignTypes(PropertyInfo[] props, HashSet<string> columns, DataTable dataTable, bool? outputIdentity = null)
        {
            var count = 0;

            foreach (var column in columns)
            {
                if (column == "InternalId")
                {
                    dataTable.Columns[count].DataType = typeof(int);
                }
                else
                {
                    for (var i = 0; i < props.Length; i++)
                    {
                        if (props[i].Name == column)
                        {
                            dataTable.Columns[count].DataType = Nullable.GetUnderlyingType(props[i].PropertyType) ??
                                                                props[i].PropertyType;
                        }
                    }
                }

                count++;
            }
        }

        internal static HashSet<string> CheckForAdditionalColumns(HashSet<string> columns, List<string> matchOnColumns)
        {
            foreach (var col in matchOnColumns)
            {
                if (!columns.Contains(col))
                {
                    columns.Add(col);
                }
            }

            return columns;
        }

        internal static void DoColumnMappings(Dictionary<string, string> columnMappings, HashSet<string> columns,
List<string> updateOnList)
        {
            if (columnMappings.Count > 0)
            {
                foreach (var column in columnMappings)
                {
                    if (columns.Contains(column.Key))
                    {
                        columns.Remove(column.Key);
                        columns.Add(column.Value);
                    }

                    for (var i = 0; i < updateOnList.ToArray().Length; i++)
                    {
                        if (updateOnList[i] == column.Key)
                        {
                            updateOnList[i] = column.Value;
                        }
                    }
                }
            }
        }

        internal static string GetFullQualifyingTableName(string databaseName, string schemaName, string tableName)
        {
            var sb = new StringBuilder();
            sb.Append("[");
            sb.Append(databaseName);
            sb.Append("].[");
            sb.Append(schemaName);
            sb.Append("].[");
            sb.Append(tableName);
            sb.Append("]");

            return sb.ToString();
        }

        internal static void MapColumns(SqlBulkCopy bulkCopy, HashSet<string> columns, Dictionary<string, string> customColumnMappings)
        {
            foreach (var column in columns)
            {
                if (customColumnMappings.TryGetValue(column, out var mapping))
                {
                    bulkCopy.ColumnMappings.Add(mapping, mapping);
                }
                else
                {
                    bulkCopy.ColumnMappings.Add(column, column);
                }
            }
        }

        internal static DataTable GetDatabaseSchema(SqlConnection conn, string schema, string tableName)
        {
            var restrictions = new string[4];
            restrictions[0] = conn.Database;
            restrictions[1] = schema;
            restrictions[2] = tableName;
            var dtCols = conn.GetSchema("Columns", restrictions);

            if (dtCols.Rows.Count == 0 && schema != null)
            {
                throw new InvalidOperationException("Table name '" + tableName + "\' with schema name \'" + schema + "\' not found. Check your setup and try again.");
            }

            if (dtCols.Rows.Count == 0)
            {
                throw new InvalidOperationException("Table name \'" + tableName + "\' not found. Check your setup and try again.");
            }

            return dtCols;
        }

        internal static string GetIndexManagementCmd(string action, string table, HashSet<string> disableIndexList)
        {
            //AND sys.objects.name = 'Books' AND sys.indexes.name = 'IX_Title'
            var sb = new StringBuilder();

            if (disableIndexList != null && disableIndexList.Any())
            {
                foreach (var index in disableIndexList)
                {
                    sb.Append(" AND sys.indexes.name = \'");
                    sb.Append(index);
                    sb.Append("\'");
                }
            }

            var cmd = "DECLARE @sql AS VARCHAR(MAX)=''; " +
                                "SELECT @sql = @sql + " +
                                "'ALTER INDEX [' + sys.indexes.name + '] ON ' + sys.objects.name + ' " + action + ";'" +
                                "FROM sys.indexes JOIN sys.objects ON sys.indexes.object_id = sys.objects.object_id " +
                                "WHERE sys.indexes.type_desc = 'NONCLUSTERED' " +
                                "AND sys.objects.type_desc = 'USER_TABLE'" +
                                " AND sys.objects.name = '" + table + "'" + (sb.Length > 0 ? sb.ToString() : "") + "; EXEC(@sql);";

            return cmd;
        }

        internal static void SetSqlBulkCopySettings(SqlBulkCopy bulkcopy, bool bulkCopyEnableStreaming, int? bulkCopyBatchSize, int? bulkCopyNotifyAfter, int bulkCopyTimeout)
        {
            bulkcopy.EnableStreaming = bulkCopyEnableStreaming;

            if (bulkCopyBatchSize.HasValue)
            {
                bulkcopy.BatchSize = bulkCopyBatchSize.Value;
            }

            if (bulkCopyNotifyAfter.HasValue)
            {
                bulkcopy.NotifyAfter = bulkCopyNotifyAfter.Value;
            }

            bulkcopy.BulkCopyTimeout = bulkCopyTimeout;
        }

        internal static string BuildCreateTempTable(HashSet<string> columns, DataTable schema, bool? outputIdentity = null)
        {
            var actualColumns = new Dictionary<string, string>();
            var actualColumnsMaxCharLength = new Dictionary<string, string>();
            var actualColumnsPrecision = new Dictionary<string, PrecisionType>();

            foreach (DataRow row in schema.Rows)
            {
                var columnType = row["DATA_TYPE"].ToString();
                var columnName = row["COLUMN_NAME"].ToString();

                actualColumns.Add(row["COLUMN_NAME"].ToString(), row["DATA_TYPE"].ToString());

                if (columnType == "varchar" || columnType == "nvarchar" ||
                    columnType == "char" || columnType == "binary" ||
                    columnType == "varbinary")
                {
                    actualColumnsMaxCharLength.Add(row["COLUMN_NAME"].ToString(),
                        row["CHARACTER_MAXIMUM_LENGTH"].ToString());
                }

                if (columnType == "numeric" || columnType == "decimal")
                {
                    var p = new PrecisionType
                    {
                        NumericPrecision = row["NUMERIC_PRECISION"].ToString(),
                        NumericScale = row["NUMERIC_SCALE"].ToString()
                    };
                    actualColumnsPrecision.Add(columnName, p);
                }
            }

            var command = new StringBuilder();

            command.Append("CREATE TABLE #TmpTable(");

            var paramList = new List<string>();

            foreach (var column in columns.ToList())
            {
                if (column == "InternalId")
                {
                    continue;
                }

                if (actualColumns.TryGetValue(column, out var columnType))
                {
                    columnType = GetVariableCharType(column, columnType, actualColumnsMaxCharLength);
                    columnType = GetDecimalPrecisionAndScaleType(column, columnType, actualColumnsPrecision);
                }

                paramList.Add("[" + column + "]" + " " + columnType);
            }

            var paramListConcatenated = string.Join(", ", paramList);

            command.Append(paramListConcatenated);

            if (outputIdentity.HasValue && outputIdentity.Value)
            {
                command.Append(", [InternalId] int");
            }
            command.Append(");");

            return command.ToString();
        }

        private static string GetVariableCharType(string column, string columnType, Dictionary<string, string> actualColumnsMaxCharLength)
        {
            if (columnType == "varchar" || columnType == "nvarchar")
            {
                if (actualColumnsMaxCharLength.TryGetValue(column, out var maxCharLength))
                {
                    if (maxCharLength == "-1")
                    {
                        maxCharLength = "max";
                    }

                    columnType = columnType + "(" + maxCharLength + ")";
                }
            }

            return columnType;
        }

        private static string GetDecimalPrecisionAndScaleType(string column, string columnType, Dictionary<string, PrecisionType> actualColumnsPrecision)
        {
            if (columnType == "decimal" || columnType == "numeric")
            {
                if (actualColumnsPrecision.TryGetValue(column, out var p))
                {
                    columnType = columnType + "(" + p.NumericPrecision + ", " + p.NumericScale + ")";
                }
            }

            return columnType;
        }

        internal static void InsertToTmpTable(SqlConnection conn, SqlTransaction transaction, DataTable dt, bool bulkCopyEnableStreaming, int? bulkCopyBatchSize, int? bulkCopyNotifyAfter, int bulkCopyTimeout, SqlBulkCopyOptions sqlBulkCopyOptions)
        {
            using (var bulkcopy = new SqlBulkCopy(conn, sqlBulkCopyOptions, transaction)
            {
                DestinationTableName = "#TmpTable"
            })
            {

                SetSqlBulkCopySettings(bulkcopy, bulkCopyEnableStreaming,
                    bulkCopyBatchSize,
                    bulkCopyNotifyAfter, bulkCopyTimeout);

                bulkcopy.WriteToServer(dt);
            }
        }

        internal static async Task InsertToTmpTableAsync(SqlConnection conn, SqlTransaction transaction, DataTable dt, bool bulkCopyEnableStreaming, int? bulkCopyBatchSize, int? bulkCopyNotifyAfter, int bulkCopyTimeout, SqlBulkCopyOptions sqlBulkCopyOptions)
        {
            using (var bulkcopy = new SqlBulkCopy(conn, sqlBulkCopyOptions, transaction)
            {
                DestinationTableName = "#TmpTable"
            })
            {

                SetSqlBulkCopySettings(bulkcopy, bulkCopyEnableStreaming,
                    bulkCopyBatchSize,
                    bulkCopyNotifyAfter, bulkCopyTimeout);

                await bulkcopy.WriteToServerAsync(dt);
            }
        }

        internal static string GetOutputCreateTableCmd(bool outputIdentity, string tmpTablename, OperationType operation)
        {
            if (operation == OperationType.Insert)
            {
                return outputIdentity ? "CREATE TABLE " + tmpTablename + "(InternalId int, Id int); " : "";
            }

            return string.Empty;
        }

        internal static string BuildJoinConditionsForUpdateOrInsert(string[] updateOn, string sourceAlias, string targetAlias)
        {
            var command = new StringBuilder();

            command.Append("ON " + "[" + targetAlias + "]" + "." + "[" + updateOn[0] + "]" + " = " + "[" + sourceAlias + "]" + "." + "[" + updateOn[0] + "]" + " ");

            if (updateOn.Length > 1)
            {
                // Start from index 1 to just append "AND" conditions
                for (var i = 1; i < updateOn.Length; i++)
                {
                    command.Append("AND " + "[" + targetAlias + "]" + "." + "[" + updateOn[i] + "]" + " = " + "[" + sourceAlias + "]" + "." + "[" + updateOn[i] + "]" + " ");
                }
            }

            return command.ToString();
        }

        internal static string BuildUpdateSet(HashSet<string> columns, string sourceAlias, string targetAlias, string identityColumn)
        {
            var command = new StringBuilder();
            var paramsSeparated = new List<string>();

            command.Append("UPDATE SET ");

            foreach (var column in columns.ToList())
            {
                if (identityColumn != null && column != identityColumn || identityColumn == null)
                {
                    if (column != "InternalId")
                    {
                        paramsSeparated.Add("[" + targetAlias + "]" + "." + "[" + column + "]" + " = " + "[" + sourceAlias + "]" + "." + "[" + column + "]");
                    }
                }
            }

            command.Append(string.Join(", ", paramsSeparated) + " ");

            return command.ToString();
        }

        internal static string BuildInsertSet(HashSet<string> columns, string sourceAlias, string identityColumn)
        {
            var command = new StringBuilder();
            var insertColumns = new List<string>();
            var values = new List<string>();

            command.Append("INSERT (");

            foreach (var column in columns.ToList())
            {
                if (identityColumn != null && column != identityColumn || identityColumn == null)
                {
                    if (column != "InternalId")
                    {
                        insertColumns.Add("[" + column + "]");
                        values.Add("[" + sourceAlias + "]" + "." + "[" + column + "]");
                    }
                }
            }

            command.Append(string.Join(", ", insertColumns));
            command.Append(") values (");
            command.Append(string.Join(", ", values));
            command.Append(")");

            return command.ToString();
        }

        internal static string GetOutputIdentityCmd(string identityColumn, bool outputIdentity, string tmpTableName)
        {
            var sb = new StringBuilder();
            if (identityColumn == null || !outputIdentity)
            {
                return "; ";
            }

            sb.Append("OUTPUT Source.InternalId, INSERTED." + identityColumn + " INTO " + tmpTableName + "(InternalId, " + identityColumn + "); ");
            return sb.ToString();
        }

        internal static (HashSet<string>, Dictionary<string, string> mappings) GetAllColumns(Type type)
        {
            var columns = new HashSet<string>();
            var mappings = new Dictionary<string, string>();

            //Get all the properties
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in props)
            {
                if (!property.CanRead || property.GetCustomAttribute<NotMappedAttribute>(false) != null)
                {
                    continue;
                }

                if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                {
                    columns.Add(property.Name);
                }

                var colAttr = property.GetCustomAttribute<ColumnAttribute>(true);
                if (colAttr != null && !colAttr.Name.IsNull())
                {
                    mappings[property.Name] = colAttr.Name;
                }
            }

            return (columns, mappings);
        }
    }
}