using GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;
using GymManagementSystem.BusinessLogic.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Contracts.Services;

public interface ITrainerService
{
    Task<IEnumerable<AllTrainersDTO>> GetAllTrainersAsync(CancellationToken ct);
    Task<Result<TrainerDetailsDTO>> GetTrainerDetailsAsync(Guid id, CancellationToken ct);
    Task<Result> AddTrainerAsync(TrainerCreateDTO trainerDTO, CancellationToken ct);
    Task<Result<TrainerToBeEditedDTO>> GetTrainerToBeEditedAsync(Guid id, CancellationToken ct);
    Task<Result> EditTrainerAsync(Guid id, TrainerToBeEditedDTO trainerToBeEditedDTO, CancellationToken ct);
    Task<Result> DeleteTrainerAsync(Guid id, CancellationToken ct);
}
