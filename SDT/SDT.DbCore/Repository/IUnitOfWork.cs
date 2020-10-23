using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace SDT.DbCore
{
    public interface IUnitOfWork<TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        TDbContext DataContext { get; }

        IDbContextTransaction CurrentTransaction { get; }

        Task<IDbContextTransaction> BeginTransAsync(IsolationLevel level = IsolationLevel.ReadCommitted);

        IDbContextTransaction BeginTrans(IsolationLevel level = IsolationLevel.ReadCommitted);

        int SaveChange();

        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);

        int Commit();

        ValueTask<int> CommitAsync(CancellationToken cancellationToken = default);

        void Rollback();

        Task RollbackAsync();

        void RollbackChanges();
    }
}
