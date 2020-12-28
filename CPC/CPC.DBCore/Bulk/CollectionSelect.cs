using System.Collections.Generic;

namespace CPC.DBCore.Bulk
{
    public class CollectionSelect<T>
    {
        #region Members
        private readonly IEnumerable<T> _list;
        private readonly string _sourceAlias;
        private readonly string _targetAlias;
        private readonly BulkOperations _ext;
        #endregion

        #region Constructors
        internal CollectionSelect(IEnumerable<T> list, string sourceAlias, string targetAlias, BulkOperations ext)
        {
            _list = list;
            _sourceAlias = sourceAlias;
            _targetAlias = targetAlias;
            _ext = ext;
        }
        #endregion

        #region Methods
        public Table<T> WithTable(string tableName) => new Table<T>(new BulkOption<T>
        {
            TableName = tableName,
            SourceAlias = _sourceAlias,
            TargetAlias = _targetAlias,
            Data = _list
        },
            _ext);
        #endregion
    }
}
