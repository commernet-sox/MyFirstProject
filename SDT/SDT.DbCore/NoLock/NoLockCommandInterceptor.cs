using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SDT.DbCore
{
    internal class NoLockCommandInterceptor : DbCommandInterceptor
    {
        #region Members
        private static readonly Regex TableAliasRegex =
     new Regex(@"(?<tableAlias>AS \[[a-zA-Z]\w*\](?! WITH \(NOLOCK\)))",
         RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion

        #region Methods
        public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
        {
            command.CommandText = TableAliasRegex.Replace(
            command.CommandText,
            "${tableAlias} WITH (NOLOCK)"
            );
            return base.ScalarExecuting(command, eventData, result);
        }

        public override Task<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
        {
            command.CommandText = TableAliasRegex.Replace(
            command.CommandText,
            "${tableAlias} WITH (NOLOCK)"
            );
            return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            command.CommandText = TableAliasRegex.Replace(
            command.CommandText,
            "${tableAlias} WITH (NOLOCK)"
            );
            return result;
        }

        public override Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            command.CommandText = TableAliasRegex.Replace(
            command.CommandText,
            "${tableAlias} WITH (NOLOCK)"
            );
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }
        #endregion
    }
}
