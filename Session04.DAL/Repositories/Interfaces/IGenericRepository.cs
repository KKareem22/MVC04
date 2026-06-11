using Session04.DAL.Models;
using System.Linq.Expressions;

namespace Session04.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        Task<IEnumerable<TEntity>> GetALLAsync(bool tracking = false, CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        public Task<int> AddAsync(TEntity entity);
        public Task<int> UpdateAsync(TEntity entity);
        public Task<int> DeleteAsync(TEntity entity);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
        Task<TEntity?> FirstOrDeafultAsync(Expression<Func<TEntity, bool>> predicate, bool Tracking = false, CancellationToken ct = default);


    }
}
