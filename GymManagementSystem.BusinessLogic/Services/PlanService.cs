using GymManagementSystem.BusinessLogic.Contracts.Services;
using GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;
using GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;
using GymManagementSystem.BusinessLogic.Results;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using GymManagementSystem.DataAccess.UoW.Contract;
using System.Numerics;

namespace GymManagementSystem.BusinessLogic.Services;

public sealed class PlanService : IPlanService
{
    /* Fields */
    private readonly IUnitOfWork _unitOfWork;

    /* Constructor */
    public PlanService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /* Methods */
    public async Task<IEnumerable<AllPlansDTO>> GetAllPlansAsync(CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();

        var allPlans = await planRepo.GetAllAsync(ct);

        if (allPlans is null)
            return [];

        var allPlansDTOs = allPlans.Select(p => new AllPlansDTO
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Status = p.IsActive,
            DurationDays = p.DurationDays,
            Description = p.Description
        });

        return allPlansDTOs;
    }
    public async Task<Result<PlanDetailsDTO>> GetPlanDetailsAsync(Guid id, CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();

        var plan = await planRepo.GetByIdAsync(id, ct);

        if (plan is null)
            return Result<PlanDetailsDTO>.Failure(PlanBusinessErrors.PlanDetailsNotFound);

        return Result<PlanDetailsDTO>.Success(new() {
            Name = plan.Name,
            Status = plan.IsActive,
            Price = plan.Price,
            DurationDays = plan.DurationDays,
            Description = plan.Description
        });
    }

    public async Task<Result<PlanToBeEditedDTO>> GetPlanToBeEditedAsync(Guid id, CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();

        var planToBeEdited = await planRepo.GetByIdAsync(id, ct);

        if (planToBeEdited is null)
            return Result<PlanToBeEditedDTO>.Failure(PlanBusinessErrors.PlanToBeEditedNotFound);

        var membershipRepo = _unitOfWork.GetGenericRepository<Membership>();

        var planHasActiveMemberships = await membershipRepo.AnyAsync(ms => ms.PlanId == id, ct);

        if (planHasActiveMemberships)
            return Result<PlanToBeEditedDTO>.Failure(PlanBusinessErrors.PlanToBeEditedHasActiveMemberships);

        return Result<PlanToBeEditedDTO>.Success(new()
        {
            Name = planToBeEdited.Name,
            Price = planToBeEdited.Price,
            DurationDays = planToBeEdited.DurationDays,
            Description = planToBeEdited.Description
        });
    }

    public async Task<Result> UpdatePlanToBeEditedAsync(Guid id, PlanToBeEditedDTO planDTO, CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();

        var planToBeUpdated = await planRepo.GetByIdAsync(id,ct);

        if (planToBeUpdated is null)
            return Result.Failure(PlanBusinessErrors.PlanToBeEditedNotFound);

        var membershipRepo = _unitOfWork.GetGenericRepository<Membership>();

        var planHasActiveMemberships = await membershipRepo.AnyAsync(ms => ms.PlanId == id,ct);

        if (planHasActiveMemberships)
            return Result.Failure(PlanBusinessErrors.PlanToBeEditedHasActiveMemberships);

        planToBeUpdated!.Price = planDTO.Price;
        planToBeUpdated!.DurationDays = planDTO.DurationDays;
        planToBeUpdated!.Description = planDTO.Description;

        planRepo.Update(planToBeUpdated);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
            Result.Failure(PlanBusinessErrors.PlanNotEdited);

        return Result.Success();
    }

    public async Task<Result> ChangePlanStatusAsync(Guid id, CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();

        var planWithStatusToBeChanged = await planRepo.GetByIdAsync(id, ct);

        if (planWithStatusToBeChanged is null)
            return Result.Failure(PlanBusinessErrors.PlanWithStatusToBeChangedNotFound);

        var membershipRepo = _unitOfWork.GetGenericRepository<Membership>();

        var planHasActiveMemberships = await membershipRepo.AnyAsync(ms => ms.PlanId == id, ct);

        if (planHasActiveMemberships && planWithStatusToBeChanged.IsActive == true)
            return Result.Failure(PlanBusinessErrors.PlanWithActiveMembershipsCannotBeDeactivated);

        planWithStatusToBeChanged!.IsActive = !planWithStatusToBeChanged.IsActive;

        planRepo.Update(planWithStatusToBeChanged);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
            Result.Failure(PlanBusinessErrors.PlanStatusNotChanged);

        return Result.Success();
    }
}
