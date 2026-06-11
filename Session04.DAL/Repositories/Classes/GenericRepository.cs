using Microsoft.EntityFrameworkCore;
using Session04.DAL.DbContexts;
using Session04.DAL.Models;
using Session04.DAL.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Session04.DAL.Repositories.Classes
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        private readonly GymDbContext dbContext;
        public GenericRepository(GymDbContext c)
        {
            dbContext = c;

        }
        public void Add(T entity)
        {
            dbContext.Add(entity);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct)
        {
            return dbContext.Set<T>().AsNoTracking().AnyAsync(predicate, ct);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
        {
           return predicate is null? dbContext.Set<T>().AsNoTracking().CountAsync(ct)
                : dbContext.Set<T>().AsNoTracking().CountAsync(predicate, ct);
        }

        public void Delete(T entity)
        {
            dbContext.Remove(entity);
        }

        public Task<T?> FirstorDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        {
            return dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, ct);
        }

        public void Update(T entity)
        {
            dbContext.Update(entity);
        }

        async Task<IEnumerable<T>> IGenericRepository<T>.GetAllAsync(bool track, CancellationToken ct)
        {
            var query = track ? dbContext.Set<T>() : dbContext.Set<T>().AsNoTracking();
            return await query.ToListAsync(ct);
        }

        async Task<T?> IGenericRepository<T>.GetByIdAsync(int id, CancellationToken ct)
        {
            return await dbContext.Set<T>().FindAsync(id, ct);
        }

    }
}
