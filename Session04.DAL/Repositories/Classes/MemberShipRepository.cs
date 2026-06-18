using Microsoft.EntityFrameworkCore;
using Session04.DAL.DbContexts;
using Session04.DAL.Models;
using Session04.DAL.Repositories.Interfaces;

namespace Session04.DAL.Repositories.Classes
{
    public class MemberShipRepository : GenericRepository<MemberShip>, IMemberShipRepository
    {
        private readonly GymDbContext dbContext;

        public MemberShipRepository(GymDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<MemberShip>> GetAllMembershipsWithMemberAndPlansAsync(CancellationToken ct = default)
        {
            var query = dbContext.MemberShips.AsNoTracking().Include(m => m.Member)
                .Include(m => m.Plan);
            return await query.ToListAsync(ct);
        }
    }
}
