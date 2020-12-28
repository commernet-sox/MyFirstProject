using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace CPC.DBCore
{
    [Obsolete("不推荐使用，后续将删除")]
    public class SqlServerDataProvider : DbDataProvider
    {
        public SqlServerDataProvider(string connectionString) : this(new SqlConnection(connectionString))
        {

        }

        public SqlServerDataProvider(SqlConnection connection) => DbConnection = connection;

        public override DbDataAdapter DataAdapter(DbCommand command)
        {
            var sda = new SqlDataAdapter(command as SqlCommand);
            return sda;
        }
    }
}
