using SDT.BaseTool;
using System;
using System.Data;
using System.Data.Common;

namespace SDT.DbCore
{
    [Obsolete("不推荐使用，后续将删除")]
    public abstract class DbDataProvider : IDataProvider
    {
        public DbTransaction Transaction { get; protected set; }

        public DbConnection DbConnection { get; protected set; }

        public DbTransaction BeginTran(IsolationLevel iso = IsolationLevel.ReadCommitted)
        {
            if (DbConnection.State != ConnectionState.Open)
            {
                DbConnection.Open();
            }

            Transaction = DbConnection.BeginTransaction(iso);
            return Transaction;
        }

        public void Commit() => Transaction?.Commit();

        public DbCommand CreateCommand(string cmd, params DbParameter[] parameters)
        {
            if (DbConnection.State != ConnectionState.Open)
            {
                DbConnection.Open();
            }
            var command = DbConnection.CreateCommand();
            command.CommandText = cmd;
            if (parameters.IsNull())
            {
                command.Parameters.AddRange(parameters);
            }

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }
            return command;
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            if (DbConnection.State != ConnectionState.Closed)
            {
                DbConnection.Close();
            }
            DbConnection?.Dispose();
        }

        public void Rollback() => Transaction?.Rollback();

        public abstract DbDataAdapter DataAdapter(DbCommand command);
    }
}
