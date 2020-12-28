using Microsoft.EntityFrameworkCore;

namespace CPC.DBCore
{
    public class NoLockDb<TDbContext> : INoLockDb<TDbContext> where TDbContext : DbContext
    {
        public TDbContext Context { get; private set; }

        public NoLockDb(TDbContext context) => Context = context;

        public void Dispose() => Context?.Dispose();
    }

}
