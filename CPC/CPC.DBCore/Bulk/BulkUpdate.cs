using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CPC.DBCore.Bulk
{
    public class BulkUpdate<T> : IBulkTrans
    {
        #region Members
        internal BulkOption<T> Option { get; private set; }

        private readonly BulkOperations _ext;
        private readonly List<string> _matchTargetOn = new List<string>();
        private string _identityColumn;
        #endregion

        #region Constructors
        internal BulkUpdate(BulkOption<T> option, BulkOperations ext)
        {
            Option = option;
            _ext = ext;
            _ext.SetBulkExt(this);
        }
        #endregion

        #region Methods
        public BulkUpdate<T> MatchTargetOn(Expression<Func<T, object>> columnName)
        {
            var propertyName = columnName.GetPropertyName();

            if (propertyName == null)
            {
                throw new InvalidOperationException("MatchTargetOn column name can't be null.");
            }

            _matchTargetOn.Add(propertyName);

            return this;
        }

        public BulkUpdate<T> SetIdentityColumn(Expression<Func<T, object>> columnName)
        {
            var propertyName = columnName.GetPropertyName();

            if (propertyName == null)
            {
                throw new InvalidOperationException("SetIdentityColumn column name can't be null");
            }

            if (_identityColumn == null)
            {
                _identityColumn = propertyName;
            }
            else
            {
                throw new InvalidOperationException("Can't have more than one identity column");
            }
            return this;
        }

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

            if (_matchTargetOn.Count == 0)
            {
                throw new InvalidOperationException("MatchTargetOn list is empty when it's required for this operation. " +
                                                    "This is usually the primary key of your table but can also be more than one column depending on your business rules.");
            }

            var dt = BulkUtil.ToDataTable(Option.Data, Option.Columns, Option.CustomColumnMappings, _matchTargetOn);

            // Must be after ToDataTable is called. 
            BulkUtil.DoColumnMappings(Option.CustomColumnMappings, Option.Columns, _matchTargetOn);

            using (conn)
            {
                conn.Open();
                var dtCols = BulkUtil.GetDatabaseSchema(conn, Option.Schema, Option.TableName);

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var command = conn.CreateCommand();
                        command.Connection = conn;
                        command.Transaction = transaction;
                        command.CommandTimeout = Option.SqlTimeout;

                        //Creating temp table on database
                        command.CommandText = BulkUtil.BuildCreateTempTable(Option.Columns, dtCols);
                        command.ExecuteNonQuery();

                        //Bulk insert into temp table
                        BulkUtil.InsertToTmpTable(conn, transaction, dt, Option.BulkCopyEnableStreaming, Option.BulkCopyBatchSize,
                            Option.BulkCopyNotifyAfter, Option.BulkTimeout, Option.SqlBulkCopyOptions);

                        // Updating destination table, and dropping temp table
                        if (!Option.DisableIndexes.IsNull())
                        {
                            command.CommandText = BulkUtil.GetIndexManagementCmd(IndexOperation.Disable, Option.TableName, Option.DisableIndexes);
                            command.ExecuteNonQuery();
                        }

                        var comm = "MERGE INTO " + BulkUtil.GetFullQualifyingTableName(conn.Database, Option.Schema, Option.TableName) + " WITH (HOLDLOCK) AS Target " +
                                "USING #TmpTable AS Source " +
                                BulkUtil.BuildJoinConditionsForUpdateOrInsert(_matchTargetOn.ToArray(),
                                    Option.SourceAlias, Option.TargetAlias) +
                                "WHEN MATCHED THEN " +
                                BulkUtil.BuildUpdateSet(Option.Columns, Option.SourceAlias, Option.TargetAlias, _identityColumn) +
                                "; DROP TABLE #TmpTable;";
                        command.CommandText = comm;
                        command.ExecuteNonQuery();

                        if (!Option.DisableIndexes.IsNull())
                        {
                            command.CommandText = BulkUtil.GetIndexManagementCmd(IndexOperation.Rebuild, Option.TableName, Option.DisableIndexes);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (SqlException e)
                    {
                        for (var i = 0; i < e.Errors.Count; i++)
                        {
                            // Error 8102 is identity error. 
                            if (e.Errors[i].Number == 8102)
                            {
                                // Expensive call but neccessary to inform user of an important configuration setup. 
                                throw new Exception(e.Errors[i].Message + " SQLBulkTools requires the SetIdentityColumn method to be configured if an identity column is being used. Please reconfigure your setup and try again.");
                            }
                        }

                        transaction.Rollback();
                        throw;
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

            if (_matchTargetOn.Count == 0)
            {
                throw new InvalidOperationException("MatchTargetOn list is empty when it's required for this operation. " +
                                                    "This is usually the primary key of your table but can also be more than one column depending on your business rules.");
            }

            var dt = BulkUtil.ToDataTable(Option.Data, Option.Columns, Option.CustomColumnMappings, _matchTargetOn);

            // Must be after ToDataTable is called. 
            BulkUtil.DoColumnMappings(Option.CustomColumnMappings, Option.Columns, _matchTargetOn);

            using (conn)
            {
                conn.Open();
                var dtCols = BulkUtil.GetDatabaseSchema(conn, Option.Schema, Option.TableName);

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var command = conn.CreateCommand();
                        command.Connection = conn;
                        command.Transaction = transaction;
                        command.CommandTimeout = Option.SqlTimeout;

                        //Creating temp table on database
                        command.CommandText = BulkUtil.BuildCreateTempTable(Option.Columns, dtCols);
                        await command.ExecuteNonQueryAsync();

                        //Bulk insert into temp table
                        await BulkUtil.InsertToTmpTableAsync(conn, transaction, dt, Option.BulkCopyEnableStreaming, Option.BulkCopyBatchSize,
                             Option.BulkCopyNotifyAfter, Option.BulkTimeout, Option.SqlBulkCopyOptions);

                        // Updating destination table, and dropping temp table
                        if (!Option.DisableIndexes.IsNull())
                        {
                            command.CommandText = BulkUtil.GetIndexManagementCmd(IndexOperation.Disable, Option.TableName, Option.DisableIndexes);
                            command.ExecuteNonQuery();
                        }

                        var comm = "MERGE INTO " + BulkUtil.GetFullQualifyingTableName(conn.Database, Option.Schema, Option.TableName) + " WITH (HOLDLOCK) AS Target " +
                                "USING #TmpTable AS Source " +
                                BulkUtil.BuildJoinConditionsForUpdateOrInsert(_matchTargetOn.ToArray(),
                                    Option.SourceAlias, Option.TargetAlias) +
                                "WHEN MATCHED THEN " +
                                BulkUtil.BuildUpdateSet(Option.Columns, Option.SourceAlias, Option.TargetAlias, _identityColumn) +
                                "; DROP TABLE #TmpTable;";
                        command.CommandText = comm;
                        command.ExecuteNonQuery();

                        if (!Option.DisableIndexes.IsNull())
                        {
                            command.CommandText = BulkUtil.GetIndexManagementCmd(IndexOperation.Rebuild, Option.TableName, Option.DisableIndexes);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (SqlException e)
                    {
                        for (var i = 0; i < e.Errors.Count; i++)
                        {
                            // Error 8102 is identity error. 
                            if (e.Errors[i].Number == 8102)
                            {
                                // Expensive call but neccessary to inform user of an important configuration setup. 
                                throw new Exception(e.Errors[i].Message + " SQLBulkTools requires the SetIdentityColumn method to be configured if an identity column is being used. Please reconfigure your setup and try again.");
                            }
                        }

                        transaction.Rollback();
                        throw;
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
        #endregion
    }
}
