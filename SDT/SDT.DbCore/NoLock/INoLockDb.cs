using Microsoft.EntityFrameworkCore;
using System;

namespace SDT.DbCore
{
    public interface INoLockDb<TDbContext> : IDisposable where TDbContext : DbContext
    {
        TDbContext Context { get; }
    }
}
