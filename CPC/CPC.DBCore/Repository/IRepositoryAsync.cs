using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CPC.DBCore
{
    public interface IRepositoryAsync<TEntity>
        where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity item, CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(TEntity item, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(params object[] ids);

        Task<int> DeleteAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(TEntity item, CancellationToken cancellationToken = default);

        Task<int> UpdateAsync(IEnumerable<TEntity> item, CancellationToken cancellationToken = default);

        Task<bool> MergeAsync(TEntity persisted, TEntity current, CancellationToken cancellationToken = default);

        ValueTask<TEntity> GetAsync(params object[] ids);
    }
}
