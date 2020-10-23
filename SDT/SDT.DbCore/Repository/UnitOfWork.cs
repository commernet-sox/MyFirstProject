using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace SDT.DbCore
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext>
        where TDbContext : DbContext
    {
        #region Members
        public TDbContext DataContext { get; protected internal set; }

        public IDbContextTransaction CurrentTransaction => DataContext.Database.CurrentTransaction;
        #endregion

        #region Constructors
        public UnitOfWork(TDbContext dbcotext) => DataContext = dbcotext;
        #endregion

        #region Methods
        public IDbContextTransaction BeginTrans(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            if (CurrentTransaction != null)
            {
                throw new InvalidOperationException("current transaction opened");
            }
            return DataContext.Database.BeginTransaction(level);
        }

        public Task<IDbContextTransaction> BeginTransAsync(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            if (CurrentTransaction != null)
            {
                throw new InvalidOperationException("current transaction opened");
            }

            return DataContext.Database.BeginTransactionAsync(level);
        }

        public int SaveChange() => DataContext.SaveChanges();

        public Task<int> SaveChangeAsync(CancellationToken cancellationToken = default) => DataContext.SaveChangesAsync(cancellationToken);

        public int Commit()
        {
            if (CurrentTransaction == null)
            {
                throw new ArgumentNullException(nameof(CurrentTransaction));
            }

            try
            {
                var result = SaveChange();
                CurrentTransaction.Commit();
                return result;
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                }
            }
        }

        public async ValueTask<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            if (CurrentTransaction == null)
            {
                throw new ArgumentNullException(nameof(CurrentTransaction));
            }

            try
            {
                var result = await SaveChangeAsync(cancellationToken);
                await CurrentTransaction.CommitAsync(cancellationToken);
                return result;
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                }
            }
        }

        public void Rollback()
        {
            try
            {
                CurrentTransaction?.Rollback();
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                }
            }
        }

        public Task RollbackAsync()
        {
            try
            {
                return CurrentTransaction?.RollbackAsync();
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                }
            }
        }

        public void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            foreach (var entry in DataContext.ChangeTracker.Entries())
            {
                entry.State = EntityState.Unchanged;
            }
        }

        public void Dispose() => DataContext.Dispose();
        #endregion
    }
}
