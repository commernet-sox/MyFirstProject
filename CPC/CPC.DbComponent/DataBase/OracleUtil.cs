using CPC.DbComponent.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;

namespace CPC.DbComponent
{
    public class OracleUtil : IDbUtil, IDisposable
    {
        //private readonly OracleConnection conn;
        //private OracleTransaction trans;
        //private bool disposed = false;

        public OracleUtil(string strConn)
        {
            //conn = new OracleConnection(strConn);
            //TransactionType = TransactionType.Normal;
        }

        public OracleUtil(string strConn, TransactionUtil transactionUtil)
        {
            //conn = new OracleConnection(strConn);
            //if (transactionUtil != null)
            //{
            //    conn.Open();
            //    conn.EnlistTransaction(transactionUtil.Transaction);

            //    TransactionType = TransactionType.Distributed;
            //    transactionUtil.DbUtils.Add(this);
            //}
        }

        #region IDataBase 成员

        public TransactionType TransactionType
        {
            get;
            set;
        }

        public IDbTransaction BeginTrans() =>
            //if (conn.State != ConnectionState.Open)
            //    conn.Open();
            //trans = conn.BeginTransaction();
            //return trans;
            null;

        public void Close() => Dispose();

        public void Commit()
        {
            try
            {
                //if (trans != null)
                //    trans.Commit();
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            finally
            {
                //conn.Close();
            }
        }

        public void RollBack()
        {
            try
            {
                //if (trans != null)
                //    trans.Rollback();
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            finally
            {
                //conn.Close();
            }
        }

        public IDbCommand CreateCommand(string strCmd) =>
            //if (conn.State != ConnectionState.Open)
            //{
            //    conn.Open();
            //}
            //OracleCommand command = conn.CreateCommand();
            //command.CommandText = strCmd;
            //if (trans != null)
            //{
            //    command.Transaction = trans;
            //}
            //return command;
            null;


        public DataSet ExecProcedure(string procName, DataTable dtParms) =>
            //var dataSet = new DataSet();
            //var selectCommand = new OracleCommand();
            //try
            //{
            //    try
            //    {
            //        DataRow[] rowArray;
            //        OracleParameter[] parameters = GetParameters(procName);
            //        selectCommand.CommandType = CommandType.StoredProcedure;
            //        selectCommand.CommandText = procName;
            //        if (dtParms != null)
            //        {
            //            for (int i = 0; i < parameters.Length; i++)
            //            {
            //                rowArray = dtParms.Select("POSITION=" + ((i + 1)).ToString());
            //                if (rowArray.Length > 0)
            //                {
            //                    DbType dbType = parameters[i].DbType;
            //                    parameters[i].Value = rowArray[0]["PARMVALUE"];
            //                    //设置参数类型
            //                    //if(rowArray[0]["IN_OUT"]!=null && rowArray[0]["IN_OUT"].ToString()!="")
            //                    //{
            //                    //    switch(rowArray[0]["IN_OUT"].ToString())
            //                    //    {
            //                    //        case "IN":
            //                    //            parameters[i].Direction=ParameterDirection.Input;
            //                    //            break;
            //                    //        case "IN/OUT":
            //                    //            parameters[i].Direction = ParameterDirection.InputOutput;
            //                    //            break;
            //                    //        case "OUT":
            //                    //            parameters[i].Direction = ParameterDirection.Output;
            //                    //            break;
            //                    //        default:
            //                    //            parameters[i].Direction = ParameterDirection.ReturnValue;
            //                    //            break;
            //                    //    }
            //                    //}
            //                }
            //            }
            //        }
            //        if (parameters != null)
            //        {
            //            foreach (OracleParameter parameter in parameters)
            //            {
            //                selectCommand.Parameters.Add(parameter);
            //            }
            //        }
            //        if (trans == null)
            //        {
            //            trans = (OracleTransaction)BeginTrans();
            //        }
            //        selectCommand.Transaction = trans;
            //        selectCommand.Connection = (OracleConnection)DbConnection;
            //        new OracleDataAdapter(selectCommand).Fill(dataSet);
            //        foreach (OracleParameter parameter in selectCommand.Parameters)
            //        {
            //            if (parameter.Direction == ParameterDirection.Output)
            //            {
            //                rowArray = dtParms.Select("ARGUMENT_NAME='" + parameter.ParameterName + "'");
            //                if (rowArray.Length > 0)
            //                {
            //                    rowArray[0]["PARMVALUE"] = parameter.Value;
            //                }
            //            }
            //        }
            //        trans.Commit();
            //    }
            //    catch (OracleException exception)
            //    {
            //        trans.Rollback();
            //        Console.Write(exception.Message);
            //        throw exception;
            //    }
            //}
            //finally
            //{
            //    trans.Dispose();
            //    selectCommand.Dispose();
            //    if (trans == null)
            //    {
            //        conn.Close();
            //    }
            //}
            //return dataSet;
            null;

        public int ExecuteNonQuery(string strSql) =>
            //int count = 0;
            //if (conn.State != ConnectionState.Open)
            //    conn.Open();
            //try
            //{
            //    var command = CreateCommand(strSql) as OracleCommand;
            //    count = command.ExecuteNonQuery();
            //}
            //catch (OracleException ex)
            //{
            //    if (TransactionType == TransactionType.Normal)
            //    {
            //        if (trans != null)
            //            trans.Rollback();
            //    }
            //    throw ex;
            //}
            //finally
            //{
            //    if (TransactionType == TransactionType.Normal)
            //    {
            //        if (trans == null)
            //            conn.Close();
            //    }
            //}
            //return count;
            0;

        public object ExecuteScalar(string strSql) =>
            //object obj;
            //if (conn.State != ConnectionState.Open)
            //    conn.Open();
            //try
            //{
            //    var command = CreateCommand(strSql) as OracleCommand;
            //    obj = command.ExecuteScalar();
            //}
            //catch (Exception exception)
            //{
            //    throw exception;
            //}
            //return obj;
            null;

        public DataTable GetProcedureParameter(string procName)
        {
            var format =
                "SELECT OBJECT_NAME,POSITION,ARGUMENT_NAME,DATA_TYPE,IN_OUT,DATA_LENGTH,DATA_PRECISION,DATA_SCALE FROM ALL_ARGUMENTS  WHERE OBJECT_ID = (SELECT OBJECT_ID FROM USER_OBJECTS  WHERE OBJECT_TYPE='PROCEDURE' AND  OBJECT_NAME ='{0}') and  OBJECT_NAME ='{1}'";
            format = string.Format(format, procName, procName);
            var table = GetDataTable(format);
            table.Columns.Add("PARMVALUE");
            return table;
        }

        public DataTable GetTableColumnsName(string tableName) => throw new NotImplementedException();

        public DataSet GetDataSet(string strSql) =>
            //var dataSet = new DataSet();
            //try
            //{
            //    var adapter = new OracleDataAdapter(strSql, conn);
            //    if (trans != null)
            //        adapter.SelectCommand.Transaction = trans;
            //    adapter.Fill(dataSet);
            //    adapter.Dispose();
            //}
            //catch (OracleException ex)
            //{
            //    throw ex;
            //}

            //return dataSet;
            null;

        public DataSet GetDataSet(string strSql, string tableName) =>
            //var dataSet = new DataSet();
            //try
            //{
            //    var adapter = new OracleDataAdapter(strSql, conn);
            //    if (trans != null)
            //        adapter.SelectCommand.Transaction = trans;
            //    adapter.Fill(dataSet, tableName);
            //    adapter.Dispose();
            //}
            //catch (OracleException ex)
            //{
            //    throw ex;
            //}

            //return dataSet;
            null;

        public DataSet GetDataSet(string strSql, IEnumerable<IDbDataParameter> parameters) =>
            //var dataSet = new DataSet();
            //try
            //{
            //    var adapter = new OracleDataAdapter(strSql, conn);
            //    if (trans != null)
            //        adapter.SelectCommand.Transaction = trans;
            //    if (parameters!=null)
            //    {
            //        adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
            //    }
            //    adapter.Fill(dataSet);
            //    adapter.Dispose();
            //}
            //catch (OracleException ex)
            //{
            //    throw ex;
            //}

            //return dataSet;
            null;

        public DataSet GetDataSet(string strSql, string tableName, IEnumerable<IDbDataParameter> parameters) =>
            //var dataSet = new DataSet();
            //try
            //{
            //    var adapter = new OracleDataAdapter(strSql, conn);
            //    if (trans != null)
            //        adapter.SelectCommand.Transaction = trans;
            //    if (parameters != null)
            //    {
            //        adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
            //    }
            //    adapter.Fill(dataSet, tableName);
            //    adapter.Dispose();
            //}
            //catch (OracleException ex)
            //{
            //    throw ex;
            //}

            //return dataSet;
            null;

        public DataTable GetDataTable(string strSql) =>
            //var dataTable = new DataTable();
            //try
            //{
            //    var adapter = new OracleDataAdapter(strSql, conn);
            //    if (trans != null)
            //        adapter.SelectCommand.Transaction = trans;
            //    adapter.Fill(dataTable);
            //    adapter.Dispose();
            //}
            //catch (OracleException ex)
            //{
            //    throw ex;
            //}

            //return dataTable;
            null;

        public DataTable GetDataTable(string strSql, string tableName) =>
            //var dataSet = new DataSet();
            //try
            //{
            //    var adapter = new OracleDataAdapter(strSql, conn);
            //    if (trans != null)
            //        adapter.SelectCommand.Transaction = trans;
            //    adapter.Fill(dataSet, tableName);
            //    adapter.Dispose();
            //}
            //catch (OracleException ex)
            //{
            //    throw ex;
            //}

            //return dataSet.Tables[tableName];
            null;

        public DataTable GetDataTable(string strSql, IEnumerable<IDbDataParameter> parameters) =>
            //var dataTable = new DataTable();
            //try
            //{
            //    var adapter = new OracleDataAdapter(strSql, conn);
            //    if (trans != null)
            //        adapter.SelectCommand.Transaction = trans;
            //    if (parameters != null)
            //    {
            //        adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
            //    }
            //    adapter.Fill(dataTable);
            //    adapter.Dispose();
            //}
            //catch (OracleException ex)
            //{
            //    throw ex;
            //}

            //return dataTable;
            null;

        public DataTable GetDataTable(string strSql, string tableName, IEnumerable<IDbDataParameter> parameters) =>
            //var dataSet = new DataSet();
            //try
            //{
            //    var adapter = new OracleDataAdapter(strSql, conn);
            //    if (trans != null)
            //        adapter.SelectCommand.Transaction = trans;
            //    if (parameters != null)
            //    {
            //        adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
            //    }
            //    adapter.Fill(dataSet, tableName);
            //    adapter.Dispose();
            //}
            //catch (OracleException ex)
            //{
            //    throw ex;
            //}

            //return dataSet.Tables[tableName];
            null;

        public DataBaseType DatabaseType => DataBaseType.OracleDBType;

        public DbConnection DbConnection => null; //return conn;

        public DataTable GetTableFrame(string tableName)
        {
            DataTable table4;
            try
            {
                var format =
                    "select COLUMN_NAME, DATA_TYPE, DATA_LENGTH, DATA_PRECISION, DATA_SCALE, NULLABLE from all_tab_columns where table_name='{0}'";
                var table = GetDataTable(string.Format(format, tableName).ToUpper());
                var str2 =
                    "select COLUMN_NAME, CONSTRAINT_NAME from user_cons_columns where table_name = '{0}' and constraint_name = (select CONSTRAINT_NAME from user_constraints where table_name = '{0}' and constraint_type ='P')";
                var table2 = GetDataTable(string.Format(str2, tableName).ToUpper());
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
                    row[3] = table.Rows[i][3].ToString().Trim();
                    row[4] = table.Rows[i][4].ToString().Trim();
                    if (table.Rows[i][5].ToString().Trim() == "Y")
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
                    if (table2 != null)
                    {
                        for (var j = 0; j < table2.Rows.Count; j++)
                        {
                            if (str3 == table2.Rows[j][0].ToString().Trim())
                            {
                                row[9] = table2.Rows[j][1].ToString().Trim();
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

        private OracleType DataType(string DATA_TYPE)
        {
            OracleType type = 0;
            switch (DATA_TYPE.ToUpper())
            {
                case "BFILE":
                    return OracleType.BFile;

                case "BLOB":
                    return OracleType.Blob;

                case "BYTE":
                    return OracleType.Byte;

                case "CHAR":
                    return OracleType.Char;

                case "CLOB":
                    return OracleType.Clob;

                case "CURSOR":
                    return OracleType.Cursor;

                case "DATETIME":
                    return OracleType.DateTime;

                case "DOUBLE":
                    return OracleType.Double;

                case "FLOAT":
                    return OracleType.Float;

                case "INT16":
                    return OracleType.Int16;

                case "INT32":
                    return OracleType.Int32;

                case "INTERVALDAYTOSECOND":
                    return OracleType.IntervalDayToSecond;

                case "INTERVALYEARTOMONTH":
                    return OracleType.IntervalYearToMonth;

                case "LONGRAW":
                    return OracleType.LongRaw;

                case "LONGVARCHAR":
                    return OracleType.LongVarChar;

                case "NCHAR":
                    return OracleType.NChar;

                case "NCLOB":
                    return OracleType.NClob;

                case "NUMBER":
                    return OracleType.Number;

                case "NVARCHAR":
                    return OracleType.NVarChar;

                case "RAW":
                    return OracleType.Raw;

                case "ROWID":
                    return OracleType.RowId;

                case "SBYTE":
                    return OracleType.SByte;

                case "TIMESTAMP":
                    return OracleType.Timestamp;

                case "TIMESTAMPLOCAL":
                    return OracleType.TimestampLocal;

                case "TIMESTAMPWITHTZ":
                    return OracleType.TimestampWithTZ;

                case "UINT16":
                    return OracleType.UInt16;

                case "UINT32":
                    return OracleType.UInt32;

                case "VARCHAR":
                case "VARCHAR2":
                    return OracleType.VarChar;

                case "REF CURSOR":
                    return OracleType.Cursor;
            }
            return type;
        }

        private OracleParameter[] GetParameters(string ProcName)
        {
            OracleParameter[] parameterArray = null;
            OracleParameter parameter = null;
            var procedureParameter = GetProcedureParameter(ProcName);
            if (procedureParameter.Rows.Count != 0)
            {
                parameterArray = new OracleParameter[procedureParameter.Rows.Count];
                for (var i = 0; i < procedureParameter.Rows.Count; i++)
                {
                    parameter = new OracleParameter
                    {
                        ParameterName = procedureParameter.Rows[i]["ARGUMENT_NAME"].ToString(),
                        OracleType = DataType(procedureParameter.Rows[i]["DATA_TYPE"].ToString()),
                        Direction = (procedureParameter.Rows[i]["IN_OUT"].ToString() == "IN")
                                              ? ParameterDirection.Input
                                              : ParameterDirection.Output
                    };
                    parameterArray[i] = parameter;
                }
            }
            return parameterArray;
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            //if (!disposed)
            //{
            //    if (disposing)
            //    {
            //        if (trans != null)
            //        {
            //            trans.Dispose();
            //        }
            //        if (conn != null)
            //        {
            //            conn.Close();
            //        }
            //    }
            //}
        }

        ~OracleUtil()
        {
            Dispose(false);
        }
        #endregion


        public int ExecuteNonQuery(string strSql, IEnumerable<IDbDataParameter> parameters) => throw new NotImplementedException();

        public void BatchInsert(DataTable table) => throw new NotImplementedException();
    }
}