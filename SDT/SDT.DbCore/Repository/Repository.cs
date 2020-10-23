using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SDT.DbCore
{
    public class Repository<TDbContext, TEntity> : IRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        #region Constructors
        public Repository(IUnitOfWork<TDbContext> unitOfWork)
        {
            UnitOfWork = unitOfWork;
            _lazySet = new Lazy<DbSet<TEntity>>(() => { return UnitOfWork.DataContext.Set<TEntity>(); });
        }
        #endregion

        #region Members
        public IUnitOfWork<TDbContext> UnitOfWork { get; private set; }

        private readonly Lazy<DbSet<TEntity>> _lazySet;

        public IQueryable<TEntity> Query => CreateSet().AsNoTracking();

        public bool? InstantSaveChange { get; set; }
        #endregion

        #region Private Methods
        protected virtual int Save()
        {
            if (InstantSaveChange == true || (!InstantSaveChange.HasValue && UnitOfWork.CurrentTransaction == null))
            {
                return UnitOfWork.SaveChange();
            }

            return -1;
        }

        protected virtual async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            if (InstantSaveChange == true || (!InstantSaveChange.HasValue && UnitOfWork.CurrentTransaction == null))
            {
                return await UnitOfWork.SaveChangeAsync();
            }

            return -1;
        }
        #endregion

        #region Public Methods
        public TEntity Add(TEntity item)
        {
            var entity = CreateSet().Add(item).Entity;
            Save();
            return entity;
        }

        public IEnumerable<TEntity> Add(IEnumerable<TEntity> items)
        {
            CreateSet().AddRange(items);
            Save();
            return items;
        }

        public async Task<TEntity> AddAsync(TEntity item, CancellationToken cancellationToken = default)
        {
            var set = await CreateSet().AddAsync(item, cancellationToken);
            var entity = set.Entity;
            Save();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default)
        {
            await CreateSet().AddRangeAsync(items, cancellationToken);
            Save();
            return items;
        }

        public bool Delete(TEntity item)
        {
            Attach(item);
            CreateSet().Remove(item);
            return Save() != 0;
        }

        public bool Delete(params object[] ids)
        {
            var entity = Get(ids);
            if (entity == null)
            {
                return false;
            }

            return Delete(entity);
        }

        public int Delete(IEnumerable<TEntity> items)
        {
            foreach (var item in items)
            {
                Attach(item);
            }

            CreateSet().RemoveRange(items);
            return Save();
        }

        public async Task<bool> DeleteAsync(TEntity item, CancellationToken cancellationToken = default)
        {
            Attach(item);
            CreateSet().Remove(item);
            return await SaveAsync(cancellationToken) != 0;
        }

        public async Task<bool> DeleteAsync(params object[] ids)
        {
            var entity = Get(ids);
            if (entity == null)
            {
                return false;
            }

            return await DeleteAsync(entity);
        }

        public Task<int> DeleteAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default)
        {
            foreach (var item in items)
            {
                Attach(item);
            }

            CreateSet().RemoveRange(items);
            return SaveAsync(cancellationToken);
        }

        public void Dispose() => UnitOfWork.Dispose();

        public TEntity Get(params object[] ids)
        {
            var entity = CreateSet().Find(ids);
            return entity;
        }

        public ValueTask<TEntity> GetAsync(params object[] ids)
        {
            var entity = CreateSet().FindAsync(ids);
            return entity;
        }

        public bool Merge(TEntity persisted, TEntity current)
        {
            ApplyCurrentValues(persisted, current);
            return Save() != 0;
        }

        public async Task<bool> MergeAsync(TEntity persisted, TEntity current, CancellationToken cancellationToken = default)
        {
            ApplyCurrentValues(persisted, current);
            return await SaveAsync(cancellationToken) != 0;
        }

        public bool Update(TEntity item)
        {
            SetModified(item);
            return Save() != 0;
        }

        public int Update(IEnumerable<TEntity> items)
        {
            foreach (var item in items)
            {
                SetModified(item);
            }

            return Save();
        }

        public async Task<bool> UpdateAsync(TEntity item, CancellationToken cancellationToken = default)
        {
            SetModified(item);
            return await SaveAsync(cancellationToken) != 0;
        }

        public Task<int> UpdateAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default)
        {
            foreach (var item in items)
            {
                SetModified(item);
            }
            return SaveAsync(cancellationToken);
        }

        public IEnumerable<TEntity> SqlQuery(string sqlQuery, params object[] parameters) => CreateSet().FromSqlRaw(sqlQuery, parameters).AsNoTracking().ToList();

        public DbSet<TEntity> CreateSet() => _lazySet.Value;

        public void Attach(TEntity item) => UnitOfWork.DataContext.Entry(item).State = EntityState.Unchanged;

        public void SetModified(TEntity item) => UnitOfWork.DataContext.Entry(item).State = EntityState.Modified;

        public void ApplyCurrentValues<T>(T original, T current)
            where T : class
        {
            UnitOfWork.DataContext.Entry(original).State = EntityState.Unchanged;
            UnitOfWork.DataContext.Entry(original).CurrentValues.SetValues(current);
        }
        #endregion
    }
}
