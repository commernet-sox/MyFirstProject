using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CPC.DBCore.Bulk
{
    public class BulkOperations
    {
        #region Members
        private IBulkTrans _trans;
        private const string SourceAlias = "Source";
        private const string TargetAlias = "Target";
        private readonly SqlConnection _connection;
        #endregion

        #region Constructors
        internal BulkOperations(SqlConnection connection) => _connection = connection;

        /// <summary>
        /// 仅支持SQL Server数据库
        /// </summary>
        /// <param name="connectionString"></param>
        public BulkOperations(string connectionString) : this(new SqlConnection(connectionString))
        {
        }
        #endregion

        #region Methods
        internal void SetBulkExt(IBulkTrans trans) => _trans = trans;

        public void CommitTrans(SqlCredential credentials = null) => _trans.CommitTrans(_connection, credentials);

        public async Task CommitTransAsync(SqlCredential credentials = null) => await _trans.CommitTransAsync(_connection, credentials);

        public CollectionSelect<T> Setup<T>(Func<Setup<T>, CollectionSelect<T>> list)
        {
            var tableSelect = list(new Setup<T>(SourceAlias, TargetAlias, this));
            return tableSelect;
        }
        #endregion

    }
}