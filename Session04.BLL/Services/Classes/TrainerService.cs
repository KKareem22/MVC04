using AutoMapper;
using Session04.BLL.Services.Interfaces;
using Session04.BLL.ViewModels.TrainerViewModels;
using Session04.DAL.Models;
using Session04.DAL.Repositories.Interfaces;


namespace Session04.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
        }
        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            return trainer is null ? null : _mapper.Map<TrainerViewModel>(trainer);
        }
        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();

            if (await repo.AnyAsync(t => t.Email == model.Email, ct))
                return false;
            if (await repo.AnyAsync(t => t.Phone == model.Phone, ct))
                return false;

            var entity = _mapper.Map<Trainer>(model);
            repo.Add(entity);

            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            return trainer is null ? null : _mapper.Map<TrainerToUpdateViewModel>(trainer);
        }
        public async Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainer = await repo.GetByIdAsync(trainerId, ct);
            if (trainer is null) return false;

            var hasFutureSessions = await _unitOfWork.GetRepository<Session>()
                .AnyAsync(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now, ct);
            if (hasFutureSessions)
                return false;

            repo.Delete(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainer = await repo.GetByIdAsync(trainerId, ct);
            if (trainer is null) return false;

            // BUG FIX from original: original code checked against MemberEntity (wrong table).
            if (await repo.AnyAsync(t => t.Email == model.Email && t.Id != trainerId, ct))
                return false;
            if (await repo.AnyAsync(t => t.Phone == model.Phone && t.Id != trainerId, ct))
                return false;

            _mapper.Map(model, trainer);
            trainer.UpdatedAt = DateTime.Now;
            repo.Update(trainer);

            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

    }
}
