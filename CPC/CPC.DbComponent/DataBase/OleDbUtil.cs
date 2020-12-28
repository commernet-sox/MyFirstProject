using CPC.DbComponent.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;

namespace CPC.DbComponent
{
    public class OleDbUtil : IDbUtil, IDisposable
    {
        private readonly OleDbConnection conn;
        private OleDbTransaction trans;
        private readonly bool disposed = false;

        public OleDbUtil(string strConn)
        {
            conn = new OleDbConnection(strConn);
            TransactionType = TransactionType.Normal;
        }

        public OleDbUtil(string strConn, TransactionUtil transactionUtil)
        {
            conn = new OleDbConnection(strConn);
            if (transactionUtil != null)
            {
                conn.Open();
                conn.EnlistTransaction(transactionUtil.Transaction);

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
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            trans = conn.BeginTransaction();
            return trans;
        }

        public void Close() => Dispose();

        public void Commit()
        {
            try
            {
                if (trans != null)
                {
                    trans.Commit();
                }
            }
            catch (OleDbException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public void RollBack()
        {
            try
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
            }
            catch (OleDbException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public IDbCommand CreateCommand(string strCmd)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            var command = conn.CreateCommand();
            command.CommandText = strCmd;
            if (trans != null)
            {
                command.Transaction = trans;
            }

            return command;
        }


        public DataSet ExecProcedure(string procName, DataTable dtParms)
        {
            DataSet dataSet = null;
            try
            {
                var selectCommand = new OleDbCommand();
                var parameters = GetParameters(procName);
                selectCommand.CommandType = CommandType.StoredProcedure;
                selectCommand.CommandText = procName;
                if (dtParms != null)
                {
                }
                foreach (var parameter in parameters)
                {
                    selectCommand.Parameters.Add(parameter);
                }
                selectCommand.Transaction = trans;
                selectCommand.Connection = (OleDbConnection)DbConnection;
                new OleDbDataAdapter(selectCommand).Fill(dataSet);
            }
            catch (OleDbException exception)
            {
                Console.Write(exception.Message);
            }
            return dataSet;
        }

        public int ExecuteNonQuery(string strSql)
        {
            var count = 0;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            try
            {
                var command = CreateCommand(strSql) as OleDbCommand;
                count = command.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                throw ex;
            }
            finally
            {
                if (TransactionType == TransactionType.Normal)
                {
                    if (trans == null)
                    {
                        conn.Close();
                    }
                }
            }
            return count;
        }

        public int ExecuteNonQuery(string strSql, IEnumerable<IDbDataParameter> parameters)
        {
            var count = 0;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            try
            {
                var command = CreateCommand(strSql) as OleDbCommand;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                count = command.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                throw ex;
            }
            finally
            {
                if (TransactionType == TransactionType.Normal)
                {
                    if (trans == null)
                    {
                        conn.Close();
                    }
                }
            }
            return count;
        }

        public object ExecuteScalar(string strSql)
        {
            object obj;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            try
            {
                var command = CreateCommand(strSql) as OleDbCommand;
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
                "select sysobjects.name as OBJECT_NAME,colid as POSITION,syscolumns.name as ARGUMENT_NAME,systypes.name as DATA_TYPE,case status2 when 1 then 'IN' when 2 then 'OUT' end  as IN_OUT,syscolumns.length as DATA_LENGTH,syscolumns.prec as DATA_PRECISION,syscolumns.scale as DATA_SCALE  from sysobjects,syscolumns,systypes 　where sysobjects.id = syscolumns.id and  syscolumns.usertype = systypes.usertype and  sysobjects.name='{0}' and sysobjects.type='P' order by colid";
            format = string.Format(format, procName, procName);
            return GetDataTable(format);
        }

        public DataTable GetTableColumnsName(string tableName) => throw new NotImplementedException();

        public DataSet GetDataSet(string strSql)
        {
            var dataSet = new DataSet();
            try
            {
                var adapter = new OleDbDataAdapter(strSql, conn);
                if (trans != null)
                {
                    adapter.SelectCommand.Transaction = trans;
                }

                adapter.Fill(dataSet);
                adapter.Dispose();
            }
            catch (OleDbException ex)
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
                var adapter = new OleDbDataAdapter(strSql, conn);
                if (trans != null)
                {
                    adapter.SelectCommand.Transaction = trans;
                }

                adapter.Fill(dataSet, tableName);
                adapter.Dispose();
            }
            catch (OleDbException ex)
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
                var adapter = new OleDbDataAdapter(strSql, conn);
                if (trans != null)
                {
                    adapter.SelectCommand.Transaction = trans;
                }

                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
                }
                adapter.Fill(dataSet);
                adapter.Dispose();
            }
            catch (OleDbException ex)
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
                var adapter = new OleDbDataAdapter(strSql, conn);
                if (trans != null)
                {
                    adapter.SelectCommand.Transaction = trans;
                }

                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
                }
                adapter.Fill(dataSet, tableName);
                adapter.Dispose();
            }
            catch (OleDbException ex)
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
                var adapter = new OleDbDataAdapter(strSql, conn);
                if (trans != null)
                {
                    adapter.SelectCommand.Transaction = trans;
                }

                adapter.Fill(dataTable);
                adapter.Dispose();
            }
            catch (OleDbException ex)
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
                var adapter = new OleDbDataAdapter(strSql, conn);
                if (trans != null)
                {
                    adapter.SelectCommand.Transaction = trans;
                }

                adapter.Fill(dataSet, tableName);
                adapter.Dispose();
            }
            catch (OleDbException ex)
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
                var adapter = new OleDbDataAdapter(strSql, conn);
                if (trans != null)
                {
                    adapter.SelectCommand.Transaction = trans;
                }

                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
                }
                adapter.Fill(dataTable);
                adapter.Dispose();
            }
            catch (OleDbException ex)
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
                var adapter = new OleDbDataAdapter(strSql, conn);
                if (trans != null)
                {
                    adapter.SelectCommand.Transaction = trans;
                }

                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
                }
                adapter.Fill(dataSet, tableName);
                adapter.Dispose();
            }
            catch (OleDbException ex)
            {
                throw ex;
            }

            return dataSet.Tables[tableName];
        }

        public DataBaseType DatabaseType => DataBaseType.OleDbDBType;

        public DbConnection DbConnection => conn;

        public DataTable GetTableFrame(string tableName)
        {
            var oledbConnection = DbConnection as OleDbConnection;
            var schemaTable = oledbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });
            IList<string> primaryKeys = new List<string>();
            var columnTable = oledbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, null);
            foreach (var dr in columnTable.Select(string.Format("TABLE_NAME='{0}'", tableName)))
            {
                primaryKeys.Add(dr["COLUMN_NAME"].ToString());
            }

            var table = new DataTable();
            table.Columns.Add("COLUMNNAME", typeof(string));
            table.Columns.Add("TYPE", typeof(string));
            table.Columns.Add("LENGTH", typeof(int));
            table.Columns.Add("Prec", typeof(string));
            table.Columns.Add("SCALE", typeof(string));
            table.Columns.Add("ISNULLABLE", typeof(string));
            table.Columns.Add("Identity", typeof(bool));
            table.Columns.Add("Seed", typeof(string));
            table.Columns.Add("Increment", typeof(string));
            table.Columns.Add("PK", typeof(string));
            foreach (DataRow row in schemaTable.Rows)
            {
                var addRow = table.NewRow();
                addRow["COLUMNNAME"] = row["COLUMN_NAME"];
                addRow["TYPE"] = row["DATA_TYPE"];
                addRow["LENGTH"] = row["CHARACTER_MAXIMUM_LENGTH"];
                addRow["Prec"] = "";
                addRow["SCALE"] = row["NUMERIC_PRECISION"];
                addRow["ISNULLABLE"] = row["IS_NULLABLE"];
                addRow["Identity"] = false;
                addRow["Seed"] = "";
                addRow["Increment"] = "";
                if (primaryKeys.Contains(row["COLUMN_NAME"].ToString()))
                {
                    addRow["PK"] = string.Join(",", primaryKeys.ToArray());
                }
                table.Rows.Add(addRow);
            }
            return table;
        }

        private OleDbType DataType(string DATA_TYPE)
        {
            var empty = OleDbType.Empty;
            switch (DATA_TYPE.ToUpper())
            {
                case "BIGINT":
                    return OleDbType.BigInt;

                case "BINARY":
                    return OleDbType.Binary;

                case "BOOLEAN":
                    return OleDbType.Boolean;

                //case "BSTR":
                //    return OleDbType.BStr;

                case "CHAR":
                    return OleDbType.Char;

                case "CURRENCY":
                    return OleDbType.Currency;

                case "DATE":
                    return OleDbType.Date;

                case "DBDATE":
                    return OleDbType.DBDate;

                case "DBTIME":
                    return OleDbType.DBTime;

                case "DBTIMESTAMP":
                    return OleDbType.DBTimeStamp;

                case "DECIMAL":
                    return OleDbType.Decimal;

                case "DOUBLE":
                    return OleDbType.Double;

                case "EMPTY":
                    return OleDbType.Empty;

                case "ERROR":
                    return OleDbType.Error;

                case "FILETIME":
                    return OleDbType.Filetime;

                case "GUID":
                    return OleDbType.Guid;

                case "IDISPATCH":
                    return OleDbType.IDispatch;

                case "INTEGER":
                    return OleDbType.Integer;

                case "IUNKNOWN":
                    return OleDbType.IUnknown;

                case "LONGVARBINARY":
                    return OleDbType.LongVarBinary;

                case "LONGVARCHAR":
                    return OleDbType.LongVarChar;

                case "LONGVARWCHAR":
                    return OleDbType.LongVarWChar;

                case "NUMERIC":
                    return OleDbType.Numeric;

                case "PROPVARIANT":
                    return OleDbType.PropVariant;

                case "SINGLE":
                    return OleDbType.Single;

                case "SMALLINT":
                    return OleDbType.SmallInt;

                case "TINYINT":
                    return OleDbType.TinyInt;

                case "UNSIGNEDBIGINT":
                    return OleDbType.UnsignedBigInt;

                case "UNSIGNEDINT":
                    return OleDbType.UnsignedInt;

                case "UNSIGNEDSMALLINT":
                    return OleDbType.UnsignedSmallInt;

                case "UNSIGNEDTINYINT":
                    return OleDbType.UnsignedTinyInt;

                case "VARBINARY":
                    return OleDbType.VarBinary;

                case "VARCHAR":
                    return OleDbType.VarChar;

                case "VARIANT":
                    return OleDbType.Variant;

                case "VARNUMERIC":
                    return OleDbType.VarNumeric;

                case "VARWCHAR":
                    return OleDbType.VarWChar;

                case "WCHAR":
                    return OleDbType.WChar;
            }
            return empty;
        }

        private OleDbParameter[] GetParameters(string ProcName)
        {
            OleDbParameter[] parameterArray = null;
            var procedureParameter = GetProcedureParameter(ProcName);
            if (procedureParameter.Rows.Count != 0)
            {
                parameterArray = new OleDbParameter[procedureParameter.Rows.Count];
                for (var i = 0; i < procedureParameter.Rows.Count; i++)
                {
                    parameterArray[i] = new OleDbParameter
                    {
                        ParameterName = procedureParameter.Rows[i]["ARGUMENT_NAME"].ToString(),
                        OleDbType = DataType(procedureParameter.Rows[i]["DATA_TYPE"].ToString()),
                        Direction = (procedureParameter.Rows[i]["IN_OUT"].ToString() == "IN")
                                                                ? ParameterDirection.Input
                                                                : ParameterDirection.InputOutput
                    };
                }
            }
            return parameterArray;
        }

        public void BatchInsert(DataTable table) => throw new NotImplementedException();
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (trans != null)
                    {
                        trans.Dispose();
                    }
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }
        }



        ~OleDbUtil()
        {
            Dispose(false);
        }
        #endregion
    }
}