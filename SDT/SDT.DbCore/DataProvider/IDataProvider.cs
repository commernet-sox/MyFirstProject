using System;
using System.Data;
using System.Data.Common;

namespace SDT.DbCore
{
    [Obsolete("不推荐使用，后续将删除")]
    public interface IDataProvider : IDisposable
    {
        /// <summary>
        /// database connection
        /// </summary>
        DbConnection DbConnection { get; }

        /// <summary>
        /// database trans
        /// </summary>
        DbTransaction Transaction { get; }

        /// <summary>
        /// begin transaction
        /// </summary>
        /// <param name="iso"></param>
        /// <returns></returns>
        DbTransaction BeginTran(IsolationLevel iso = IsolationLevel.ReadCommitted);

        DbDataAdapter DataAdapter(DbCommand command);

        /// <summary>
        /// commit transaction
        /// </summary>
        void Commit();

        /// <summary>
        /// trans rollback 
        /// </summary>
        void Rollback();

        /// <summary>
        /// create database command
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DbCommand CreateCommand(string cmd, params DbParameter[] parameters);
    }
}
