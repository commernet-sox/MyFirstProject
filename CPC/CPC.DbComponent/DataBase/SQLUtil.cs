using CPC.DbComponent.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace CPC.DbComponent
{
    public class SQLUtil : IDbUtil
    {
        private readonly SqlConnection _conn;
        private SqlTransaction _trans;
        private readonly bool disposed = false;

        public SQLUtil(string strConn)
        {
            _conn = new SqlConnection(strConn);
            TransactionType = TransactionType.Normal;
        }

        public SQLUtil(string strConn, TransactionUtil transactionUtil)
        {
            _conn = new SqlConnection(strConn);
            if (transactionUtil != null)
            {
                _conn.Open();
                _conn.EnlistTransaction(transactionUtil.Transaction);

                TransactionType = TransactionType.Distributed;
                transactionUtil.DbUtils.Add(this);
            }
        }

        #region IDataBase 成员

        public TransactionType TransactionType
        {
            get;
            set;
        }

        public IDbTransaction BeginTrans()
        {
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }

            _trans = _conn.BeginTransaction();
            return _trans;
        }

        public void Close() => Dispose();

        public void Commit()
        {
            try
            {
                if (_trans != null)
                {
                    _trans.Commit();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                _conn.Close();
            }
        }

        public void RollBack()
        {
            try
            {
                if (_trans != null)
                {
                    _trans.Rollback();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                _conn.Close();
            }
        }

        public IDbCommand CreateCommand(string strCmd)
        {
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }
            var command = _conn.CreateCommand();
            command.CommandText = strCmd;
            if (_trans != null)
            {
                command.Transaction = _trans;
            }
            return command;
        }

        public DataSet ExecProcedure(string procName, DataTable dtParms)
        {
            var dataSet = new DataSet();
            var selectCommand = new SqlCommand();
            try
            {
                DataRow[] rowArray;
                var parameters = GetParameters(procName);
                selectCommand.CommandType = CommandType.StoredProcedure;
                selectCommand.CommandText = procName;
                if (dtParms != null)
                {
                    for (var i = 0; i < parameters.Length; i++)
                    {
                        rowArray = dtParms.Select("POSITION=" + ((i + 1)).ToString());
                        if (rowArray.Length > 0)
                        {
                            var dbType = parameters[i].DbType;
                            parameters[i].Value = rowArray[0]["PARMVALUE"];
                        }
                    }
                }
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        selectCommand.Parameters.Add(parameter);
                    }
                }
                if (_trans == null)
                {
                    _trans = (SqlTransaction)BeginTrans();
                }
                selectCommand.Transaction = _trans;
                selectCommand.Connection = (SqlConnection)DbConnection;
                new SqlDataAdapter(selectCommand).Fill(dataSet);
                foreach (SqlParameter parameter in selectCommand.Parameters)
                {
                    if (parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.InputOutput)
                    {
                        rowArray = dtParms.Select("ARGUMENT_NAME='" + parameter.ParameterName + "'");
                        if (rowArray.Length > 0)
                        {
                            rowArray[0]["PARMVALUE"] = parameter.Value;
                        }
                    }
                }
                var resultRow = dtParms.Select("ARGUMENT_NAME='@Return_MSG'");
                if (resultRow.Length > 0 && !string.IsNullOrEmpty(resultRow[0]["PARMVALUE"].ConvertString()))
                {
                    _trans.Rollback();
                }
                else
                {
                    _trans.Commit();
                }
            }
            catch (SqlException exception)
            {
                _trans.Rollback();
                Console.Write(exception.Message);
                throw exception;
            }
            finally
            {
                _trans.Dispose();
                selectCommand.Dispose();
                if (_trans == null)
                {
                    _conn.Close();
                }
            }
            return dataSet;
        }

        public int ExecuteNonQuery(string strSql)
        {
            var count = 0;
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }

            try
            {
                var command = CreateCommand(strSql) as SqlCommand;
                count = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                if (TransactionType == TransactionType.Normal)
                {
                    if (_trans == null)
                    {
                        _conn.Close();
                    }
                }
            }
            return count;
        }

        public int ExecuteNonQuery(string strSql, IEnumerable<IDbDataParameter> parameters)
        {
            var count = 0;
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }

            try
            {
                var command = CreateCommand(strSql) as SqlCommand;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                count = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                if (TransactionType == TransactionType.Normal)
                {
                    if (_trans == null)
                    {
                        _conn.Close();
                    }
                }
            }
            return count;
        }

        public object ExecuteScalar(string strSql)
        {
            object obj;
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }

            try
            {
                var command = CreateCommand(strSql) as SqlCommand;
                obj = command.ExecuteScalar();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return obj;
        }

        public DataTable GetProcedureParameter(string procName)
        {
            var format =
                "select Specific_Name as OBJECT_NAME,ORDINAL_POSITION as POSITION,PARAMETER_NAME as ARGUMENT_NAME ,DATA_TYPE ,PARAMETER_MODE as IN_OUT,CHARACTER_MAXIMUM_LENGTH as DATA_LENGTH,NUMERIC_PRECISION as DATA_PRECISION,NUMERIC_SCALE as DATA_SCALE  from INFORMATION_SCHEMA.PARAMETERS where Specific_Name='{0}' ORDER BY POSITION ";
            format = string.Format(format, procName, procName);
            var table = GetDataTable(format);
            table.Columns.Add("PARMVALUE");
            return table;
        }

        public DataTable GetTableColumnsName(string tableName) => throw new NotImplementedException();

        public DataSet GetDataSet(string strSql)
        {
            var dataSet = new DataSet();
            try
            {
                var adapter = new SqlDataAdapter(strSql, _conn);
                if (_trans != null)
                {
                    adapter.SelectCommand.Transaction = _trans;
                }

                adapter.Fill(dataSet);
                adapter.Dispose();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return dataSet;
        }

        public DataSet GetDataSet(string strSql, string tableName)
        {
            var dataSet = new DataSet();
            try
            {
                var adapter = new SqlDataAdapter(strSql, _conn);
                if (_trans != null)
                {
                    adapter.SelectCommand.Transaction = _trans;
                }

                adapter.Fill(dataSet, tableName);
                adapter.Dispose();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return dataSet;
        }

        public DataSet GetDataSet(string strSql, IEnumerable<IDbDataParameter> parameters)
        {
            var dataSet = new DataSet();
            try
            {
                var adapter = new SqlDataAdapter(strSql, _conn);
                if (_trans != null)
                {
                    adapter.SelectCommand.Transaction = _trans;
                }

                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
                }
                adapter.Fill(dataSet);
                adapter.Dispose();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return dataSet;
        }

        public DataSet GetDataSet(string strSql, string tableName, IEnumerable<IDbDataParameter> parameters)
        {
            var dataSet = new DataSet();
            try
            {
                var adapter = new SqlDataAdapter(strSql, _conn);
                if (_trans != null)
                {
                    adapter.SelectCommand.Transaction = _trans;
                }

                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
                }
                adapter.Fill(dataSet, tableName);
                adapter.Dispose();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return dataSet;
        }

        public DataTable GetDataTable(string strSql)
        {
            var dataTable = new DataTable();
            try
            {
                var adapter = new SqlDataAdapter(strSql, _conn);
                if (_trans != null)
                {
                    adapter.SelectCommand.Transaction = _trans;
                }

                adapter.Fill(dataTable);
                adapter.Dispose();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return dataTable;
        }

        public DataTable GetDataTable(string strSql, string tableName)
        {
            var dataSet = new DataSet();
            try
            {
                var adapter = new SqlDataAdapter(strSql, _conn);
                if (_trans != null)
                {
                    adapter.SelectCommand.Transaction = _trans;
                }

                adapter.Fill(dataSet, tableName);
                adapter.Dispose();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return dataSet.Tables[tableName];
        }

        public DataTable GetDataTable(string strSql, IEnumerable<IDbDataParameter> parameters)
        {
            var dataTable = new DataTable();
            try
            {
                var adapter = new SqlDataAdapter(strSql, _conn);
                if (_trans != null)
                {
                    adapter.SelectCommand.Transaction = _trans;
                }

                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
                }
                adapter.Fill(dataTable);
                adapter.Dispose();
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return dataTable;
        }

        public DataTable GetDataTable(string strSql, string tableName, IEnumerable<IDbDataParameter> parameters)
        {
            var dataSet = new DataSet();
            try
            {
                var adapter = new SqlDataAdapter(strSql, _conn);
                if (_trans != null)
                {
                    adapter.SelectCommand.Transaction = _trans;
                }

                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
                }
                adapter.Fill(dataSet, tableName);
                adapter.Dispose();
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return dataSet.Tables[tableName];
        }

        public DataBaseType DatabaseType => DataBaseType.SQLDBType;

        public DbConnection DbConnection => _conn;

        public DataTable GetTableFrame(string tableName)
        {
            DataTable table4;
            try
            {
                var format =
                    "SELECT COLUMNNAME=C.NAME, TYPE=B.NAME, LENGTH=C.PREC, SCALE=C.SCALE, ISNULLABLE=C.ISNULLABLE FROM SYSOBJECTS O, SYSCOLUMNS C, SYSTYPES B WHERE O.XTYPE = 'U' AND C.ID = O.ID AND C.XTYPE = B.XUSERTYPE AND O.NAME = '{0}'";
                format = string.Format(format, tableName);
                var table = GetDataTable(format);
                var str2 =
                    "SELECT DISTINCT PKNAME=A.NAME, COLUMNNAME=C.NAME FROM   SYSINDEXES   A,SYSINDEXKEYS   B,SYSCOLUMNS   C,SYSOBJECTS   D ,   SYSOBJECTS   E WHERE   A.INDID=B.INDID   AND   A.ID=B.ID  AND D.NAME = '{0}' AND   A.ID=C.ID   AND   C.ID=D.ID   AND   B.COLID=C.COLID AND A.NAME IN (SELECT AA.NAME FROM SYSOBJECTS AA, SYSOBJECTS BB WHERE AA.XTYPE = 'PK' AND BB.XTYPE = 'U' AND AA.PARENT_OBJ = BB.ID AND BB.NAME = '{0}')";
                str2 = string.Format(str2, tableName);
                var table2 = GetDataTable(str2);
                var table3 = new DataTable(tableName);
                table3.Columns.Add("ColumnName", typeof(string));
                table3.Columns.Add("Type", typeof(string));
                table3.Columns.Add("Length", typeof(string));
                table3.Columns.Add("Prec", typeof(string));
                table3.Columns.Add("Scale", typeof(string));
                table3.Columns.Add("Nullable", typeof(bool));
                table3.Columns.Add("Identity", typeof(bool));
                table3.Columns.Add("Seed", typeof(string));
                table3.Columns.Add("Increment", typeof(string));
                table3.Columns.Add("PK", typeof(string));
                for (var i = 0; i < table.Rows.Count; i++)
                {
                    var row = table3.NewRow();
                    var str3 = table.Rows[i][0].ToString().Trim();
                    row[0] = str3;
                    row[1] = table.Rows[i][1].ToString().Trim();
                    row[2] = table.Rows[i][2].ToString().Trim();
                    row[3] = "";
                    row[4] = table.Rows[i][3].ToString().Trim();
                    if (table.Rows[i][4].ToString().Trim() == "1")
                    {
                        row[5] = true;
                    }
                    else
                    {
                        row[5] = false;
                    }
                    row[6] = false;
                    row[7] = "";
                    row[8] = "";
                    row[9] = "";
                    if (table2.Rows.Count > 0)
                    {
                        for (var j = 0; j < table2.Rows.Count; j++)
                        {
                            if (table2.Rows[j][1].ToString().Trim() == str3)
                            {
                                row[9] = table2.Rows[j][0].ToString().Trim();
                                break;
                            }
                        }
                    }
                    table3.Rows.Add(row);
                }
                table4 = table3;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
            return table4;
        }

        private SqlDbType DataType(string DATA_TYPE)
        {
            var bigInt = SqlDbType.BigInt;
            switch (DATA_TYPE.ToUpper())
            {
                case "BIGINT":
                    return SqlDbType.BigInt;

                case "BINARY":
                    return SqlDbType.Binary;

                case "BIT":
                    return SqlDbType.Bit;

                case "CHAR":
                    return SqlDbType.Char;

                case "DATETIME":
                    return SqlDbType.DateTime;

                case "DECIMAL":
                    return SqlDbType.Decimal;

                case "FLOAT":
                    return SqlDbType.Float;

                case "IMAGE":
                    return SqlDbType.Image;

                case "INT":
                    return SqlDbType.Int;

                case "MONEY":
                    return SqlDbType.Money;

                case "NCHAR":
                    return SqlDbType.NChar;

                case "NTEXT":
                    return SqlDbType.NText;

                case "NVARCHAR":
                    return SqlDbType.NVarChar;

                case "REAL":
                    return SqlDbType.Real;

                case "SMALLDATETIME":
                    return SqlDbType.SmallDateTime;

                case "SMALLINT":
                    return SqlDbType.SmallInt;

                case "SMALLMONEY":
                    return SqlDbType.SmallMoney;

                case "TEXT":
                    return SqlDbType.Text;

                case "TIMESTAMP":
                    return SqlDbType.Timestamp;

                case "TINYINT":
                    return SqlDbType.TinyInt;

                case "UDT":
                    return SqlDbType.Udt;

                case "UNIQUEIDENTIFIER":
                    return SqlDbType.UniqueIdentifier;

                case "VARBINARY":
                    return SqlDbType.VarBinary;

                case "VARCHAR":
                    return SqlDbType.VarChar;

                case "VARIANT":
                    return SqlDbType.Variant;

                case "XML":
                    return SqlDbType.Xml;
                case "DATE":
                    return SqlDbType.Date;
            }
            return bigInt;
        }

        private SqlParameter[] GetParameters(string ProcName)
        {
            SqlParameter[] parameterArray = null;
            var procedureParameter = GetProcedureParameter(ProcName);
            if (procedureParameter.Rows.Count != 0)
            {
                parameterArray = new SqlParameter[procedureParameter.Rows.Count];
                for (var i = 0; i < procedureParameter.Rows.Count; i++)
                {
                    parameterArray[i] = new SqlParameter
                    {
                        ParameterName = procedureParameter.Rows[i]["ARGUMENT_NAME"].ToString(),
                        SqlDbType = DataType(procedureParameter.Rows[i]["DATA_TYPE"].ToString()),
                        Size = procedureParameter.Rows[i]["DATA_LENGTH"].ToString().ConvertInt32(),
                        Precision = procedureParameter.Rows[i]["DATA_PRECISION"].ToString().ConvertByte(),
                        Direction = (procedureParameter.Rows[i]["IN_OUT"].ToString() == "IN")
                                                                ? ParameterDirection.Input
                                                                : ParameterDirection.InputOutput
                    };
                }
            }
            return parameterArray;
        }

        public void BatchInsert(DataTable table)
        {
            if (table == null || table.Rows.Count < 1)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(table.TableName))
            {
                throw new Exception("批量插入表名不能为空！");
            }
            using (var bulkCopy = new SqlBulkCopy(_conn))
            {
                bulkCopy.DestinationTableName = table.TableName;
                bulkCopy.BatchSize = table.Rows.Count;
                bulkCopy.WriteToServer(table);
            }
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            TableInfo.Clear(this);
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (_trans != null)
                    {
                        _trans.Dispose();
                    }
                    if (_conn != null)
                    {
                        _conn.Close();
                    }
                }
            }
        }

        ~SQLUtil()
        {
            Dispose(false);
        }
        #endregion
    }
}