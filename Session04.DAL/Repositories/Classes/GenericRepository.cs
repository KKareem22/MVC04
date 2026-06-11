using Microsoft.EntityFrameworkCore;
using Session04.DAL.DbContexts;
using Session04.DAL.Models;
using Session04.DAL.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Session04.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }
        public async Task<int> AddAsync(TEntity entity)
        {

            _dbSet.Add(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default) => _dbSet.AsNoTracking().AnyAsync(predicate, ct);


        public async Task<int> DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<TEntity?> FirstOrDeafultAsync(Expression<Func<TEntity, bool>> predicate, bool Tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity?> query = Tracking ? _dbSet : _dbSet.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<IEnumerable<TEntity>> GetALLAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _dbSet : _dbSet.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await _dbSet.FindAsync([id], ct);


        public async Task<int> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
