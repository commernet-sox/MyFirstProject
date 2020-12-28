using System.Collections.Generic;

namespace CPC.DBCore.Bulk
{
    public class Setup<T>
    {
        #region Members
        private readonly string _sourceAlias;
        private readonly string _targetAlias;
        private readonly BulkOperations _ext;
        #endregion

        #region Constructors
        internal Setup(string sourceAlias, string targetAlias, BulkOperations ext)
        {
            _sourceAlias = sourceAlias;
            _targetAlias = targetAlias;
            _ext = ext;
        }
        #endregion

        #region Methods
        public CollectionSelect<T> ForCollection(IEnumerable<T> list) => new CollectionSelect<T>(list, _sourceAlias, _targetAlias, _ext);
        #endregion
    }
}
