using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CPC.DBCore
{
    public class TableService<TDbContext, TEntity, TDto> : IDisposable
        where TDbContext : DbContext
        where TEntity : class
        where TDto : class
    {
        #region Constructors
        public TableService(IRepository<TDbContext, TEntity> repository) => Repository = repository;
        #endregion

        #region Members
        public IRepository<TDbContext, TEntity> Repository { get; protected internal set; }

        public virtual IQueryable<TDto> Query => Repository.Query.Map<TDto>();

        public virtual TDbContext DataContext => Repository.UnitOfWork.DataContext;
        #endregion

        #region Methods
        public virtual TEntity Add(TDto item)
        {
            var entity = item.Map<TEntity>();
            var result = Repository.Add(entity);
            return result;
        }

        public virtual IEnumerable<TEntity> Add(IEnumerable<TDto> items)
        {
            var entities = items.Map<IEnumerable<TEntity>>();
            var result = Repository.Add(entities);
            return result;
        }

        public virtual async Task<TEntity> AddAsync(TDto item, CancellationToken cancellationToken = default)
        {
            var entity = item.Map<TEntity>();
            var result = await Repository.AddAsync(entity, cancellationToken);
            return result;
        }

        public virtual async Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TDto> items, CancellationToken cancellationToken = default)
        {
            var entities = items.Map<IEnumerable<TEntity>>();
            var result = await Repository.AddAsync(entities, cancellationToken);
            return result;
        }

        public virtual bool Delete(TDto item)
        {
            var entity = item.Map<TEntity>();
            return Repository.Delete(entity);
        }

        public virtual int Delete(IEnumerable<TDto> items)
        {
            var entities = items.Map<IEnumerable<TEntity>>();
            return Repository.Delete(entities);
        }

        public virtual Task<bool> DeleteAsync(TDto item, CancellationToken cancellationToken = default)
        {
            var entity = item.Map<TEntity>();
            return Repository.DeleteAsync(entity, cancellationToken);
        }

        public virtual Task<int> DeleteAsync(IEnumerable<TDto> items, CancellationToken cancellationToken = default)
        {
            var entities = items.Map<IEnumerable<TEntity>>();
            return Repository.DeleteAsync(entities, cancellationToken);
        }

        public virtual bool Update(TDto item)
        {
            var entity = item.Map<TEntity>();
            return Repository.Update(entity);
        }

        public int Update(IEnumerable<TDto> items)
        {
            var entities = items.Map<IEnumerable<TEntity>>();
            return Repository.Update(entities);
        }

        public virtual Task<bool> UpdateAsync(TDto item, CancellationToken cancellationToken = default)
        {
            var entity = item.Map<TEntity>();
            return Repository.UpdateAsync(entity, cancellationToken);
        }

        public virtual Task<int> UpdateAsync(IEnumerable<TDto> items, CancellationToken cancellationToken = default)
        {
            var entities = items.Map<IEnumerable<TEntity>>();
            return Repository.UpdateAsync(entities, cancellationToken);
        }

        public virtual bool Merge(TDto persisted, TDto current)
        {
            var persistedItem = persisted.Map<TEntity>();
            var currentItem = current.Map<TEntity>();
            return Repository.Merge(persistedItem, currentItem);
        }

        public virtual Task<bool> MergeAsync(TDto persisted, TDto current, CancellationToken cancellationToken = default)
        {
            var persistedItem = persisted.Map<TEntity>();
            var currentItem = current.Map<TEntity>();
            return Repository.MergeAsync(persistedItem, currentItem, cancellationToken);
        }

        public virtual void Dispose() => Repository?.Dispose();
        #endregion
    }
}
