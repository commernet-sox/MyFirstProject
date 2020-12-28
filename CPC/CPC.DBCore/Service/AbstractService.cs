using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CPC.DBCore
{
    public abstract class AbstractService<TDbContext, TEntity, TDto> : IDisposable
        where TDbContext : DbContext
        where TEntity : class
        where TDto : class
    {
        #region Constructors
        public AbstractService(IRepository<TDbContext, TEntity> repository) => Repository = repository;
        #endregion

        #region Members
        public IRepository<TDbContext, TEntity> Repository { get; protected internal set; }

        public virtual IQueryable<TDto> Query => Transfer(Repository.Query);

        public virtual TDbContext DataContext => Repository.UnitOfWork.DataContext;
        #endregion

        #region Transfer
        protected abstract TEntity Transfer(TDto dto);

        protected abstract TDto Transfer(TEntity entity);

        protected virtual List<TDto> Transfer(IEnumerable<TEntity> entities)
        {
            var list = new List<TDto>(entities.Count());
            foreach (var entity in entities)
            {
                list.Add(Transfer(entity));
            }
            return list;
        }

        protected virtual List<TEntity> Transfer(IEnumerable<TDto> dtos)
        {
            var list = new List<TEntity>(dtos.Count());
            foreach (var dto in dtos)
            {
                list.Add(Transfer(dto));
            }
            return list;
        }

        protected abstract IQueryable<TEntity> Transfer(IQueryable<TDto> dtos);

        protected abstract IQueryable<TDto> Transfer(IQueryable<TEntity> entities);
        #endregion

        #region Methods
        public virtual TEntity Add(TDto item)
        {
            var entity = Transfer(item);
            var result = Repository.Add(entity);
            return result;
        }

        public virtual IEnumerable<TEntity> Add(IEnumerable<TDto> items)
        {
            var entities = Transfer(items);
            var result = Repository.Add(entities);
            return result;
        }

        public virtual async Task<TEntity> AddAsync(TDto item, CancellationToken cancellationToken = default)
        {
            var entity = Transfer(item);
            var result = await Repository.AddAsync(entity, cancellationToken);
            return result;
        }

        public virtual async Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TDto> items, CancellationToken cancellationToken = default)
        {
            var entities = Transfer(items);
            var result = await Repository.AddAsync(entities, cancellationToken);
            return result;
        }

        public virtual bool Delete(TDto item)
        {
            var entity = Transfer(item);
            return Repository.Delete(entity);
        }

        public virtual int Delete(IEnumerable<TDto> items)
        {
            var entities = Transfer(items);
            return Repository.Delete(entities);
        }

        public virtual Task<bool> DeleteAsync(TDto item, CancellationToken cancellationToken = default)
        {
            var entity = Transfer(item);
            return Repository.DeleteAsync(entity, cancellationToken);
        }

        public virtual Task<int> DeleteAsync(IEnumerable<TDto> items, CancellationToken cancellationToken = default)
        {
            var entities = Transfer(items);
            return Repository.DeleteAsync(entities, cancellationToken);
        }

        public virtual bool Update(TDto item)
        {
            var entity = Transfer(item);
            return Repository.Update(entity);
        }

        public int Update(IEnumerable<TDto> items)
        {
            var entities = Transfer(items);
            return Repository.Update(entities);
        }

        public virtual Task<bool> UpdateAsync(TDto item, CancellationToken cancellationToken = default)
        {
            var entity = Transfer(item);
            return Repository.UpdateAsync(entity, cancellationToken);
        }

        public virtual Task<int> UpdateAsync(IEnumerable<TDto> items, CancellationToken cancellationToken = default)
        {
            var entities = Transfer(items);
            return Repository.UpdateAsync(entities, cancellationToken);
        }

        public virtual bool Merge(TDto persisted, TDto current)
        {
            var persistedItem = Transfer(persisted);
            var currentItem = Transfer(current);
            return Repository.Merge(persistedItem, currentItem);
        }

        public virtual Task<bool> MergeAsync(TDto persisted, TDto current, CancellationToken cancellationToken = default)
        {
            var persistedItem = Transfer(persisted);
            var currentItem = Transfer(current);
            return Repository.MergeAsync(persistedItem, currentItem, cancellationToken);
        }

        public virtual void Dispose() => Repository?.Dispose();
        #endregion
    }

}
