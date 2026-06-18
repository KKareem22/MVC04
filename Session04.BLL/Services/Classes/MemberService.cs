using AutoMapper;
using Session04.BLL.Services.Interfaces;
using Session04.BLL.ViewModels.MemberViewModels;
using Session04.DAL.Models;
using Session04.DAL.Repositories.Interfaces;

namespace Session04.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var EmailExists = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email, ct);
            var PhoneExists = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone, ct);
            if (EmailExists || PhoneExists)
                return false;

            var member = mapper.Map<Member>(model);
            unitOfWork.GetRepository<Member>().Add(member);
            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            if (!members.Any())
                return [];
            var memberViewModels = mapper.Map<IEnumerable<Member>, IEnumerable<MemberViewModel>>(members);
            return memberViewModels;
        }

        public async Task<HealthRecordViewModel?> GetHealthRecordAsync(int id, CancellationToken ct = default)
        {
            var record = await unitOfWork.GetRepository<HealthRecord>().GetByIdAsync(id, ct);
            if (record == null)
                return null;
            var healthRecordViewModel = mapper.Map<HealthRecord, HealthRecordViewModel>(record);
            return healthRecordViewModel;
        }

        public async Task<MemberViewModel?> GetMemberDetails(int id, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null)
                return null;
            var memberviewModel = mapper.Map<Member, MemberViewModel>(member);
            var Activemembership = await unitOfWork.GetRepository<MemberShip>().FirstorDefaultAsync(m => m.MemberId == id && m.EndDate > DateTime.Now, ct);
            if (Activemembership is not null)
            {
                var Plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(Activemembership.PlanId, ct);
                memberviewModel.PlanName = Plan?.Name;
                memberviewModel.MembershipStartDate = Activemembership.CreatedAt.ToShortDateString();
                memberviewModel.MembershipEndDate = Activemembership.EndDate.ToShortDateString();
            }
            return memberviewModel;
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateViewModel(int id, CancellationToken ct = default)
        {
            var Member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (Member is null)
                return null;
            else
            {
                return mapper.Map<Member, MemberToUpdateViewModel>(Member);
            }

        }

        public async Task<bool> RemoveMemberAsync(int id, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null)
                return false;
            var HasFutureBookings = await unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == id && b.Session.EndDate > DateTime.Now, ct: ct);
            if (HasFutureBookings)
                return false;
            unitOfWork.GetRepository<Member>().Delete(member);
            var Result = await unitOfWork.SaveChangesAsync(ct);
            return Result > 0;

        }

        public async Task<bool?> UpdateMemberAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null)
                return null;
            if (await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != id, ct))
                return false;
            if (await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != id, ct))
                return false;
            mapper.Map(model, member);
            member.UpdatedAt = DateTime.Now;

            unitOfWork.GetRepository<Member>().Update(member);
            var result = await unitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }


    }
}
