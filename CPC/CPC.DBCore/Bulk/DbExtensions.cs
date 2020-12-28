using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;

namespace CPC.DBCore.Bulk
{
    /// <summary>
    /// doc to https://github.com/olegil/SqlBulkTools
    /// </summary>
    public static class DbExtensions
    {
        public static BulkOperations Bulk(this SqlConnection connection) => new BulkOperations(connection);

        public static BulkOperations Bulk(this DbContext context)
        {
            var conn = context.Database.GetDbConnection();
            if (conn.GetType().Name == "SqlConnection")
            {
                var connection = new SqlConnection(conn.ConnectionString);
                return connection.Bulk();
            }

            throw new NotSupportedException("this operation only supports Sql Server ");
        }
    }
}
