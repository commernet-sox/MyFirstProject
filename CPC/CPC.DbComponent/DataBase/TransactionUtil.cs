using System;
using System.Collections.Generic;
using System.Transactions;

namespace CPC.DbComponent.DataBase
{
    public class TransactionUtil : IDisposable
    {
        private readonly bool disposed = false;
        public IList<IDbUtil> DbUtils { get; set; }
        public CommittableTransaction Transaction { get; set; }

        public TransactionUtil() => DbUtils = new List<IDbUtil>();

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (DbUtils != null)
                    {
                        foreach (var dbUtil in DbUtils)
                        {
                            dbUtil.Dispose();
                        }
                        DbUtils.Clear();
                    }
                }
            }
        }

        ~TransactionUtil()
        {
            Dispose(false);
        }
        #endregion
    }
}
