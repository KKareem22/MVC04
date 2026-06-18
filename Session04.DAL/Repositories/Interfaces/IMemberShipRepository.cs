using Session04.DAL.Models;

namespace Session04.DAL.Repositories.Interfaces
{
    public interface IMemberShipRepository : IGenericRepository<MemberShip>
    {
        Task<List<MemberShip>> GetAllMembershipsWithMemberAndPlansAsync(CancellationToken ct = default);
    }
}
