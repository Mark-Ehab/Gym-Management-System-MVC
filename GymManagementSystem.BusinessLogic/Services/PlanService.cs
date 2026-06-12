using AutoMapper;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.PlanSpecifications;
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
    private readonly IMapper _mapper;

    /* Constructor */
    public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /* Methods */
    public async Task<IEnumerable<AllPlansDTO>> GetAllPlansAsync(CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();

        var allPlans = await planRepo.GetAllAsync(ct: ct);

        if (allPlans is null || allPlans.Count == 0)
            return [];

        return _mapper.Map<IEnumerable<AllPlansDTO>>(allPlans);
    }
    public async Task<Result<PlanDetailsDTO>> GetPlanDetailsAsync(Guid id, CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();

        var plan = await planRepo.GetByIdAsync(id, ct: ct);

        if (plan is null)
            return Result<PlanDetailsDTO>.Failure(PlanBusinessErrors.PlanDetailsNotFound);

        return Result<PlanDetailsDTO>.Success(_mapper.Map<PlanDetailsDTO>(plan));
    }

    public async Task<Result<PlanToBeEditedDTO>> GetPlanToBeEditedAsync(Guid id, CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();

        var planToBeEdited = await planRepo.GetByIdAsync(id, ct: ct);

        if (planToBeEdited is null)
            return Result<PlanToBeEditedDTO>.Failure(PlanBusinessErrors.PlanToBeEditedNotFound);

        var membershipRepo = _unitOfWork.GetGenericRepository<Membership>();
        var planHasActiveMembershipsSpecification = new PlanHasActiveMembershipsSpecification(id);

        var planHasActiveMemberships = await membershipRepo.AnyAsync(planHasActiveMembershipsSpecification, ct);

        if (planHasActiveMemberships)
            return Result<PlanToBeEditedDTO>.Failure(PlanBusinessErrors.PlanToBeEditedHasActiveMemberships);

        return Result<PlanToBeEditedDTO>.Success(_mapper.Map<PlanToBeEditedDTO>(planToBeEdited));
    }

    public async Task<Result> UpdatePlanToBeEditedAsync(Guid id, PlanToBeEditedDTO planDTO, CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();

        var planToBeUpdated = await planRepo.GetByIdAsync(id, ct: ct);

        if (planToBeUpdated is null)
            return Result.Failure(PlanBusinessErrors.PlanToBeEditedNotFound);

        var membershipRepo = _unitOfWork.GetGenericRepository<Membership>();
        var planHasActiveMembershipsSpecification = new PlanHasActiveMembershipsSpecification(id);

        var planHasActiveMemberships = await membershipRepo.AnyAsync(planHasActiveMembershipsSpecification, ct);

        if (planHasActiveMemberships)
            return Result.Failure(PlanBusinessErrors.PlanToBeEditedHasActiveMemberships);

        _mapper.Map(planDTO,planToBeUpdated);

        planRepo.Update(planToBeUpdated);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
            Result.Failure(PlanBusinessErrors.PlanNotEdited);

        return Result.Success();
    }

    public async Task<Result> ChangePlanStatusAsync(Guid id, CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();

        var planWithStatusToBeChanged = await planRepo.GetByIdAsync(id, ct: ct);

        if (planWithStatusToBeChanged is null)
            return Result.Failure(PlanBusinessErrors.PlanWithStatusToBeChangedNotFound);

        var membershipRepo = _unitOfWork.GetGenericRepository<Membership>();

        var planHasActiveMembershipsSpecification = new PlanHasActiveMembershipsSpecification(id);

        var planHasActiveMemberships = await membershipRepo.AnyAsync(planHasActiveMembershipsSpecification, ct);

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
