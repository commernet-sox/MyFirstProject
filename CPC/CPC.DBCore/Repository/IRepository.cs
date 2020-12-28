using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CPC.DBCore
{
    public interface IRepository<TDbContext, TEntity> : IRepositoryAsync<TEntity>, IDisposable
        where TEntity : class
        where TDbContext : DbContext
    {
        IUnitOfWork<TDbContext> UnitOfWork { get; }

        IQueryable<TEntity> Query { get; }

        bool? InstantSaveChange { get; set; }

        TEntity Add(TEntity item);

        IEnumerable<TEntity> Add(IEnumerable<TEntity> items);

        bool Delete(TEntity item);

        bool Delete(params object[] ids);

        int Delete(IEnumerable<TEntity> items);

        bool Update(TEntity item);

        int Update(IEnumerable<TEntity> item);

        bool Merge(TEntity persisted, TEntity current);

        TEntity Get(params object[] ids);

        IEnumerable<TEntity> SqlQuery(string sqlQuery, params object[] parameters);

        DbSet<TEntity> CreateSet();

        void Attach(TEntity item);

        void SetModified(TEntity item);

        void ApplyCurrentValues<T>(T original, T current)
            where T : class;
    }
}
