using System.Data;
using System.Data.Common;

namespace CPC.DBCore
{
    internal class CreateEntityCommand : DbCommand
    {
        public CreateEntityCommand(DbCommand originalCommand, DbDataReader originalDataReader)
        {
            OriginalCommand = originalCommand;
            OriginalDataReader = originalDataReader;
        }

        private DbCommand OriginalCommand { get; }

        private DbDataReader OriginalDataReader { get; }

        public override string CommandText
        {
            get => OriginalCommand.CommandText;
            set => OriginalCommand.CommandText = value;
        }

        public override int CommandTimeout
        {
            get => OriginalCommand.CommandTimeout;
            set => OriginalCommand.CommandTimeout = value;
        }

        public override CommandType CommandType
        {
            get => OriginalCommand.CommandType;
            set => OriginalCommand.CommandType = value;
        }

        public override bool DesignTimeVisible
        {
            get => OriginalCommand.DesignTimeVisible;
            set => OriginalCommand.DesignTimeVisible = value;
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get => OriginalCommand.UpdatedRowSource;
            set => OriginalCommand.UpdatedRowSource = value;
        }

        protected override DbConnection DbConnection
        {
            get => OriginalCommand.Connection;
            set => OriginalCommand.Connection = value;
        }

        protected override DbParameterCollection DbParameterCollection => OriginalCommand.Parameters;

        protected override DbTransaction DbTransaction
        {
            get => OriginalCommand.Transaction;
            set => OriginalCommand.Transaction = value;
        }

        public override void Cancel() => OriginalCommand.Cancel();

        public override int ExecuteNonQuery() => OriginalCommand.ExecuteNonQuery();

        public override object ExecuteScalar() => OriginalCommand.ExecuteScalar();

        public override void Prepare() => OriginalCommand.Prepare();

        protected override DbParameter CreateDbParameter() => OriginalCommand.CreateParameter();

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) => OriginalDataReader == null || OriginalDataReader.IsClosed ? OriginalCommand.ExecuteReader(behavior) : OriginalDataReader;
    }
}
