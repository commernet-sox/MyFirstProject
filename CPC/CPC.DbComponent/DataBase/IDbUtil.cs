using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace CPC.DbComponent
{
    public interface IDbUtil : IDisposable
    {
        DataBaseType DatabaseType { get; }

        TransactionType TransactionType { get; }

        DbConnection DbConnection { get; }

        IDbTransaction BeginTrans();

        void Close();

        void Commit();

        void RollBack();

        IDbCommand CreateCommand(string strCmd);

        DataSet ExecProcedure(string procName, DataTable dtParms);

        int ExecuteNonQuery(string strSql);

        int ExecuteNonQuery(string strSql, IEnumerable<IDbDataParameter> parameters);

        object ExecuteScalar(string strSql);

        DataTable GetProcedureParameter(string procName);

        DataTable GetTableColumnsName(string tableName);

        DataSet GetDataSet(string strSql);

        DataSet GetDataSet(string strSql, string tableName);

        DataSet GetDataSet(string strSql, IEnumerable<IDbDataParameter> parameters);

        DataSet GetDataSet(string strSql, string tableName, IEnumerable<IDbDataParameter> parameters);

        DataTable GetDataTable(string strSql);

        DataTable GetDataTable(string strSql, string tableName);

        DataTable GetDataTable(string strSql, IEnumerable<IDbDataParameter> parameters);

        DataTable GetDataTable(string strSql, string tableName, IEnumerable<IDbDataParameter> parameters);

        DataTable GetTableFrame(string tableName);

        void BatchInsert(DataTable table);
    }
}