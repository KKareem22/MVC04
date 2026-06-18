using AutoMapper;
using Session04.BLL.Common;
using Session04.BLL.Services.Interfaces;
using Session04.BLL.ViewModels.MemberShipViewModels;
using Session04.BLL.ViewModels.SessionViewModels;
using Session04.DAL.Models;
using Session04.DAL.Repositories.Classes;
using Session04.DAL.Repositories.Interfaces;

namespace Session04.BLL.Services.Classes
{
    public class MemberShipService : IMemberShipsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MemberShipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> CreateMembershipAsync(CreateMemberShipViewModel model, CancellationToken ct)
        {
            var PlanRepo = unitOfWork.GetRepository<Plan>();
            var plan = await PlanRepo.GetByIdAsync(model.PlanId, ct);
            var MemberRepo = unitOfWork.GetRepository<Member>();
            var member = await MemberRepo.GetByIdAsync(model.PlanId, ct);
            if (plan is null || member is null)
                return false;
            var membership = mapper.Map<CreateMemberShipViewModel, MemberShip>(model);
            var MemberShipRepo =  unitOfWork._memberShipRepository;
            MemberShipRepo.Add(membership);
            var Result = await unitOfWork.SaveChangesAsync(ct);
            return Result > 0;


        }

        public async Task<IEnumerable<MemberShipViewModel>?> GetAllMemberShipsAsync(CancellationToken ct = default)
        {
            var memberships = await unitOfWork._memberShipRepository.GetAllMembershipsWithMemberAndPlansAsync(ct: ct);
            if (memberships==null)
                return null;
            var mapped = mapper.Map<IEnumerable<MemberShipViewModel>>(memberships);
            return mapped;
        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            return mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);
        }

        public async Task<IEnumerable<PlanSelectListViewModel>> GetPlansForDropDownAsync(CancellationToken ct = default)
        {
            var Plans = await unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            return mapper.Map<IEnumerable<PlanSelectListViewModel>>(Plans);
        }
    }
}
