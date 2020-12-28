using Microsoft.EntityFrameworkCore;
using System;

namespace CPC.DBCore
{
    public interface INoLockDb<TDbContext> : IDisposable where TDbContext : DbContext
    {
        TDbContext Context { get; }
    }
}
