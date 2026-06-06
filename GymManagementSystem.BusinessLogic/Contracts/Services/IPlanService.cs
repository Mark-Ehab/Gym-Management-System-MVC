using GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;
using GymManagementSystem.BusinessLogic.Results;
using GymManagementSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Contracts.Services;

public interface IPlanService
{
    Task<IEnumerable<AllPlansDTO>> GetAllPlansAsync(CancellationToken ct = default);
    Task<Result<PlanDetailsDTO>> GetPlanDetailsAsync(Guid id, CancellationToken ct = default);
    Task<Result<PlanToBeEditedDTO>> GetPlanToBeEditedAsync(Guid id, CancellationToken ct = default);
    Task<Result> UpdatePlanToBeEditedAsync(Guid id, PlanToBeEditedDTO planDTO, CancellationToken ct = default);
    Task<Result> ChangePlanStatusAsync(Guid id, CancellationToken ct = default);
}
