using CPC.DbComponent.DataBase;
using System;
using System.Transactions;

namespace CPC.DbComponent
{
    public class DBManager
    {
        public static IDbUtil Build(DataBaseType dbType, string connStr)
        {
            IDbUtil idbUtil;
            switch (dbType)
            {
                case DataBaseType.OleDbDBType:
                    idbUtil = new OleDbUtil(connStr);
                    break;
                case DataBaseType.SQLDBType:
                    idbUtil = new SQLUtil(connStr);
                    break;
                //case DataBaseType.SybaseDBType:
                //    idbbase = new SybaseDB(connStr);
                //    break;
                //case DataBaseType.OracleDBType:
                //idbUtil = new OracleUtil(connStr);
                //break;
                //case DataBaseType.SQLiteDBType:
                //    idbUtil = new SQLiteUtil(connStr);
                //    break;
                default:
                    throw new Exception("选择的数据库类型与数据库连接字符串不相符!");
            }
            return idbUtil;
        }

        public static IDbUtil Build(DataBaseType dbType, string connStr, TransactionUtil transactionUtil)
        {
            IDbUtil idbUtil;
            switch (dbType)
            {
                case DataBaseType.OleDbDBType:
                    idbUtil = new OleDbUtil(connStr, transactionUtil);
                    break;
                case DataBaseType.SQLDBType:
                    idbUtil = new SQLUtil(connStr, transactionUtil);
                    break;
                //case DataBaseType.SybaseDBType:
                //    idbbase = new SybaseDB(connStr, transaction);
                //    break;
                //case DataBaseType.OracleDBType:
                //idbUtil = new OracleUtil(connStr, transactionUtil);
                //break;
                default:
                    throw new Exception("选择的数据库类型与数据库连接字符串不相符!");
            }
            return idbUtil;
        }

        public static void UpdateDbUtilList(Action<TransactionUtil> action)
        {
            var options = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = new TimeSpan(0, 10, 0)
            };
            using (var transactionUtil = new TransactionUtil())
            {
                using (var transaction = new CommittableTransaction(options))
                {
                    try
                    {
                        transactionUtil.Transaction = transaction;
                        action?.Invoke(transactionUtil);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public static void UpdateDbUtilList(Func<TransactionUtil, bool> action)
        {
            var options = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = new TimeSpan(0, 10, 0)
            };
            using (var transactionUtil = new TransactionUtil())
            {
                using (var transaction = new CommittableTransaction(options))
                {
                    try
                    {
                        transactionUtil.Transaction = transaction;
                        var flag = true;
                        if (action != null)
                        {
                            flag = action.Invoke(transactionUtil);
                        }
                        if (flag)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}