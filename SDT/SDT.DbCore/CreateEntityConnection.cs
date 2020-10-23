using System.Data;
using System.Data.Common;

namespace SDT.DbCore
{
    internal class CreateEntityConnection: DbConnection
    {
        public CreateEntityConnection(DbConnection originalConnection, DbDataReader originalDataReader)
        {
            OriginalConnection = originalConnection;
            OriginalDataReader = originalDataReader;
        }

        private DbConnection OriginalConnection { get; }

        internal DbDataReader OriginalDataReader { get; set; }

        public override string ConnectionString
        {
            get => OriginalConnection.ConnectionString;
            set => OriginalConnection.ConnectionString = value;
        }

        public override string Database => OriginalConnection.Database;

        public override string DataSource => OriginalConnection.DataSource;

        public override string ServerVersion => OriginalConnection.ServerVersion;

        public override ConnectionState State => OriginalConnection.State;

        public override void ChangeDatabase(string databaseName) => OriginalConnection.ChangeDatabase(databaseName);

        public override void Close() => OriginalConnection.Close();

        public override void Open() => OriginalConnection.Open();

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => OriginalConnection.BeginTransaction();

        protected override DbCommand CreateDbCommand() => new CreateEntityCommand(OriginalConnection.CreateCommand(), OriginalDataReader);
    }
}
