using System.Collections.Generic;
using System.Data.SqlClient;

namespace CPC.DBCore.Bulk
{
    internal class BulkOption<T>
    {
        internal IEnumerable<T> Data { get; set; } = new List<T>();

        internal string TableName { get; set; }

        internal string Schema { get; set; } = "dbo";

        internal HashSet<string> Columns { get; set; } = new HashSet<string>();

        internal List<string> UpdateOnList { get; set; } = new List<string>();

        internal HashSet<string> DisableIndexes { get; set; } = new HashSet<string>();

        internal bool IsDisableIndex { get; set; } = false;

        internal string SourceAlias { get; set; }

        internal string TargetAlias { get; set; }

        internal int SqlTimeout { get; set; } = 600;

        internal int BulkTimeout { get; set; } = 600;

        internal bool BulkCopyEnableStreaming { get; set; }

        internal int? BulkCopyNotifyAfter { get; set; }

        internal int? BulkCopyBatchSize { get; set; }

        internal SqlBulkCopyOptions SqlBulkCopyOptions { get; set; } = SqlBulkCopyOptions.Default;

        internal Dictionary<string, string> CustomColumnMappings { get; set; } = new Dictionary<string, string>();
    }
}
