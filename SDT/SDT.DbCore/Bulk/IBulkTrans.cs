using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SDT.DbCore
{
    internal interface IBulkTrans
    {
        void CommitTrans(SqlConnection connection, SqlCredential credentials = null);

        Task CommitTransAsync(SqlConnection connection, SqlCredential credentials = null);
    }
}
