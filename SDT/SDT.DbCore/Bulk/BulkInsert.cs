using SDT.BaseTool;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SDT.DbCore
{
    public class BulkInsert<T> : IBulkTrans
    {
        #region Members
        internal BulkOption<T> Option { get; private set; }

        private readonly BulkOperations _ext;
        #endregion

        #region Constructors
        internal BulkInsert(BulkOption<T> option, BulkOperations ext)
        {
            Option = option;
            _ext = ext;
            _ext.SetBulkExt(this);
        }
        #endregion

        #region Methods
        void IBulkTrans.CommitTrans(SqlConnection conn, SqlCredential credentials)
        {
            if (!Option.Data.Any())
            {
                return;
            }

            if (Option.IsDisableIndex && !Option.DisableIndexes.IsNull())
            {
                throw new InvalidOperationException("Invalid setup. If \'TmpDisableAllNonClusteredIndexes\' is invoked, you can not use the \'AddTmpDisableNonClusteredIndex\' method.");
            }

            var dt = BulkUtil.ToDataTable(Option.Data, Option.Columns, Option.CustomColumnMappings);

            // Must be after ToDataTable is called. 
            BulkUtil.DoColumnMappings(Option.CustomColumnMappings, Option.Columns, Option.UpdateOnList);

            using (conn)
            {
                conn.Open();

                using (var transaction = conn.BeginTransaction())
                {
                    //Bulk insert into temp table
                    using (var bulkcopy = new SqlBulkCopy(conn, Option.SqlBulkCopyOptions, transaction))
                    {
                        try
                        {
                            bulkcopy.DestinationTableName = BulkUtil.GetFullQualifyingTableName(conn.Database, Option.Schema, Option.TableName);
                            BulkUtil.MapColumns(bulkcopy, Option.Columns, Option.CustomColumnMappings);

                            BulkUtil.SetSqlBulkCopySettings(bulkcopy, Option.BulkCopyEnableStreaming, Option.BulkCopyBatchSize,
                                Option.BulkCopyNotifyAfter, Option.BulkTimeout);

                            var command = conn.CreateCommand();

                            command.Connection = conn;
                            command.Transaction = transaction;

                            if (Option.IsDisableIndex || !Option.DisableIndexes.IsNull())
                            {
                                command.CommandText = BulkUtil.GetIndexManagementCmd(IndexOperation.Disable, Option.TableName, Option.DisableIndexes);
                                command.ExecuteNonQuery();
                            }

                            bulkcopy.WriteToServer(dt);

                            if (Option.IsDisableIndex || !Option.DisableIndexes.IsNull())
                            {
                                command.CommandText = BulkUtil.GetIndexManagementCmd(IndexOperation.Rebuild, Option.TableName, Option.DisableIndexes);
                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            bulkcopy.Close();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }

        async Task IBulkTrans.CommitTransAsync(SqlConnection conn, SqlCredential credentials)
        {
            if (!Option.Data.Any())
            {
                return;
            }

            if (Option.IsDisableIndex && !Option.DisableIndexes.IsNull())
            {
                throw new InvalidOperationException("Invalid setup. If \'TmpDisableAllNonClusteredIndexes\' is invoked, you can not use the \'AddTmpDisableNonClusteredIndex\' method.");
            }

            var dt = BulkUtil.ToDataTable(Option.Data, Option.Columns, Option.CustomColumnMappings);

            // Must be after ToDataTable is called. 
            BulkUtil.DoColumnMappings(Option.CustomColumnMappings, Option.Columns, Option.UpdateOnList);

            using (conn)
            {
                conn.Open();

                using (var transaction = conn.BeginTransaction())
                {
                    //Bulk insert into temp table
                    using (var bulkcopy = new SqlBulkCopy(conn, Option.SqlBulkCopyOptions, transaction))
                    {
                        try
                        {
                            bulkcopy.DestinationTableName = BulkUtil.GetFullQualifyingTableName(conn.Database, Option.Schema, Option.TableName);
                            BulkUtil.MapColumns(bulkcopy, Option.Columns, Option.CustomColumnMappings);

                            BulkUtil.SetSqlBulkCopySettings(bulkcopy, Option.BulkCopyEnableStreaming, Option.BulkCopyBatchSize,
                                Option.BulkCopyNotifyAfter, Option.BulkTimeout);

                            var command = conn.CreateCommand();

                            command.Connection = conn;
                            command.Transaction = transaction;

                            if (Option.IsDisableIndex || !Option.DisableIndexes.IsNull())
                            {
                                command.CommandText = BulkUtil.GetIndexManagementCmd(IndexOperation.Disable, Option.TableName, Option.DisableIndexes);
                                command.ExecuteNonQuery();
                            }

                            await bulkcopy.WriteToServerAsync(dt);

                            if (Option.IsDisableIndex || !Option.DisableIndexes.IsNull())
                            {
                                command.CommandText = BulkUtil.GetIndexManagementCmd(IndexOperation.Rebuild, Option.TableName, Option.DisableIndexes);
                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();

                            bulkcopy.Close();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }
        #endregion
    }
}
