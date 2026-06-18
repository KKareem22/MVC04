using Session04.BLL.Services.Interfaces;
using Session04.BLL.ViewModels.SessionViewModels;
using Session04.DAL.Models;
using Session04.DAL.Repositories.Interfaces;

namespace Session04.BLL.Services.Classes
{
    public class AnalyticsServices : IAnalyticsService
    {
        private readonly IUnitOfWork unitOfWork;

        public AnalyticsServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default)
        {
            var Sessions = await unitOfWork.GetRepository<Session>().GetAllAsync(ct: ct);
            var totalMembers = await unitOfWork.GetRepository<Member>().CountAsync(ct: ct);
            var totalTrainers = await unitOfWork.GetRepository<Trainer>().CountAsync(ct: ct);
            var activeMembers = await unitOfWork.GetRepository<MemberShip>().CountAsync(m => m.EndDate > DateTime.Now, ct: ct);
            return new AnalyticsViewModel
            {
                TotalMembers = totalMembers,
                TotalTrainers = totalTrainers,
                ActiveMembers = activeMembers,
                UpcomingSessions = Sessions.Count(s => s.StartDate > DateTime.Now),
                OngoingSessions = Sessions.Count(s => s.StartDate <= DateTime.Now || s.EndDate >= DateTime.Now),
                CompletedSessions = Sessions.Count(s => s.EndDate < DateTime.Now)

            };
        }
    }
}
