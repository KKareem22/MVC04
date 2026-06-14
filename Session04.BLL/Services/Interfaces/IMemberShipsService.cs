using Session04.BLL.Common;
using Session04.BLL.ViewModels.MemberShipViewModels;
using Session04.BLL.ViewModels.SessionViewModels;

namespace Session04.BLL.Services.Interfaces
{
    public interface IMemberShipsService
    {
        Task<IEnumerable<MemberShipViewModel>?> GetAllMemberShipsAsync(CancellationToken ct = default);
        Task<bool> CreateMembershipAsync(CreateMemberShipViewModel model, CancellationToken ct);
        Task<IEnumerable<PlanSelectListViewModel>> GetPlansForDropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(CancellationToken ct = default);
    }
}
