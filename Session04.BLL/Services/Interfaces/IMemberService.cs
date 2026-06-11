using Session04.BLL.ViewModels.MemberViewModels;

namespace Session04.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);
        Task<HealthRecordViewModel?> GetHealthRecordAsync(int id, CancellationToken ct = default);
        Task<MemberViewModel?> GetMemberDetails(int id, CancellationToken ct = default);
        Task<bool?> UpdateMemberAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateViewModel(int id, CancellationToken ct = default);
        Task<bool> RemoveMemberAsync(int id, CancellationToken ct = default);
    }
}
