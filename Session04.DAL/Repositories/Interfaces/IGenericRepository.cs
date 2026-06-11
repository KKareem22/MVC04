using Session04.DAL.Models;
using System.Linq.Expressions;

namespace Session04.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity, new()
    {
        Task<IEnumerable<T>> GetAllAsync(bool track = false, CancellationToken ct = default);
        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);

        Task<T?> FirstorDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);


    }
}
