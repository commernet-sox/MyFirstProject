using SDT.BaseTool;
using System;
using System.Linq.Expressions;

namespace SDT.DbCore
{
    public class ColumnSelect<T>
    {
        #region Members
        internal BulkOption<T> Option { get; private set; }

        private readonly BulkOperations _ext;
        private readonly bool _isAllColumn;
        #endregion

        #region Constructors
        internal ColumnSelect(BulkOption<T> option, BulkOperations ext, bool allColumn = false)
        {
            Option = option;
            _ext = ext;
            _isAllColumn = allColumn;
        }
        #endregion

        #region Methods
        public ColumnSelect<T> AddColumn(Expression<Func<T, object>> columnName)
        {
            if (_isAllColumn)
            {
                throw new InvalidOperationException("column is full");
            }

            var propertyName = columnName.GetPropertyName();
            Option.Columns.Add(propertyName);
            BulkUtil.AddColumnMapping(this, propertyName);
            return this;
        }

        public ColumnSelect<T> RemoveColumn(Expression<Func<T, object>> columnName)
        {
            var propertyName = columnName.GetPropertyName();
            Option.Columns.Remove(propertyName);
            Option.CustomColumnMappings.Remove(propertyName);
            return this;
        }

        public ColumnSelect<T> CustomColumnMapping(Expression<Func<T, object>> columnName, string destination)
        {
            var propertyName = columnName.GetPropertyName();
            Option.CustomColumnMappings[propertyName] = destination;
            return this;
        }

        public ColumnSelect<T> AddDisableNonClusteredIndex(string indexName)
        {
            Option.DisableIndexes.Add(indexName);
            return this;
        }

        public ColumnSelect<T> DisableAllNonClusteredIndexes()
        {
            Option.IsDisableIndex = true;
            return this;
        }

        public BulkInsert<T> BulkInsert() => new BulkInsert<T>(Option, _ext);

        public BulkInsertOrUpdate<T> BulkInsertOrUpdate() => new BulkInsertOrUpdate<T>(Option, _ext);

        public BulkUpdate<T> BulkUpdate() => new BulkUpdate<T>(Option, _ext);

        public BulkDelete<T> BulkDelete() => new BulkDelete<T>(Option, _ext);
        #endregion
    }
}
