using System;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace CPC.DBCore.Bulk
{
    public class Table<T>
    {
        #region Members

        private readonly BulkOperations _ext;

        internal BulkOption<T> Option { get; set; }
        #endregion

        #region Constructors
        internal Table(BulkOption<T> option, BulkOperations ext)
        {
            Option = option;
            _ext = ext;
        }
        #endregion

        #region Methods
        public Table<T> WithSchema(string schema)
        {
            Option.Schema = schema;
            return this;
        }

        public Table<T> WithSqlCommandTimeout(int seconds)
        {
            Option.SqlTimeout = seconds;
            return this;
        }

        public Table<T> WithBulkCopyCommandTimeout(int seconds)
        {
            Option.BulkTimeout = seconds;
            return this;
        }

        public Table<T> WithBulkCopyEnableStreaming(bool status)
        {
            Option.BulkCopyEnableStreaming = status;
            return this;
        }

        public Table<T> WithBulkCopyNotifyAfter(int rows)
        {
            Option.BulkCopyNotifyAfter = rows;
            return this;
        }

        public Table<T> WithBulkCopyBatchSize(int rows)
        {
            Option.BulkCopyBatchSize = rows;
            return this;
        }

        public Table<T> WithSqlBulkCopyOptions(SqlBulkCopyOptions options)
        {
            Option.SqlBulkCopyOptions = options;
            return this;
        }

        public ColumnSelect<T> AddColumn(Expression<Func<T, object>> columnName)
        {
            var propertyName = columnName.GetPropertyName();
            Option.Columns.Add(propertyName);
            var column = BulkUtil.AddColumnMapping(new ColumnSelect<T>(Option, _ext), propertyName);
            return column;
        }

        public ColumnSelect<T> AddAllColumn()
        {
            var (columns,mappings) = BulkUtil.GetAllColumns(typeof(T));
            Option.Columns = columns;
            Option.CustomColumnMappings = mappings;
            return new ColumnSelect<T>(Option, _ext, true);
        }
        #endregion
    }
}
