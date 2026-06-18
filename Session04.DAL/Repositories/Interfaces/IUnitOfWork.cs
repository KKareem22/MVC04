using Session04.DAL.Models;

namespace Session04.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        public ISessionRepository _sessionRepository { get; }
        public IMemberShipRepository _memberShipRepository { get; }

    }
}
