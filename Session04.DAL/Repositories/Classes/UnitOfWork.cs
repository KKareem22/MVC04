using Session04.DAL.DbContexts;
using Session04.DAL.Models;
using Session04.DAL.Repositories.Interfaces;

namespace Session04.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        public ISessionRepository _sessionRepository { get; }

        public IMemberShipRepository _memberShipRepository { get; }

        private readonly Dictionary<string, object> _repositories = [];
        private readonly GymDbContext _dbContext;

        public UnitOfWork(GymDbContext dbContext, ISessionRepository sessionRepository
            ,IMemberShipRepository memberShipRepository)
        {
            _dbContext = dbContext;
            _sessionRepository = sessionRepository;
            _memberShipRepository= memberShipRepository;
        }



        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var TypeName = typeof(TEntity).Name;
            if (_repositories.TryGetValue(TypeName, out object? value))
            {
                return (IGenericRepository<TEntity>)value;
            }
            var Repo = new GenericRepository<TEntity>(_dbContext);
            _repositories[TypeName] = Repo;
            return Repo;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
       => _dbContext.SaveChangesAsync(ct);
    }
}
