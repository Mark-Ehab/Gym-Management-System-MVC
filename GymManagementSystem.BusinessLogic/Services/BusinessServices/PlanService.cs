using AutoMapper;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.PlanSpecifications;
using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;
using GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;
using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using GymManagementSystem.DataAccess.UoW.Contract;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace GymManagementSystem.BusinessLogic.Services.BusinessServices;

public sealed class PlanService : IPlanService
{
    /* Fields */
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PlanService> _logger;

    /* Constructor */
    public PlanService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PlanService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    /* Methods */
    public async Task<IEnumerable<AllPlansDTO>> GetAllPlansAsync(CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();

        var allPlans = await planRepo.GetAllAsync(ct: ct);

        if (allPlans is null || allPlans.Count == 0)
        {
            _logger.LogWarning("No plans are retrieved when plan's index page is hitted");
            return [];
        }

        var allPlansDTOs = _mapper.Map<IEnumerable<AllPlansDTO>>(allPlans);

        _logger.LogInformation("Plans: {@Plans} are retrieved successfully",allPlansDTOs.Select(p => new
        {
            p.Id,
            p.Name
        }));
        return allPlansDTOs;
    }
    public async Task<Result<PlanDetailsDTO>> GetPlanDetailsAsync(Guid id, CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();

        var plan = await planRepo.GetByIdAsync(id, ct: ct);

        if (plan is null)
        {
            _logger.LogWarning("Plan of Id ({@Id}) is not found", id);
            return Result<PlanDetailsDTO>.Failure(PlanBusinessErrors.PlanDetailsNotFound);
        }

        var planDTO = _mapper.Map<PlanDetailsDTO>(plan);

        _logger.LogInformation("Plan {@Plan} is retrieved successfully", new {Id = id , planDTO.Name});
        return Result<PlanDetailsDTO>.Success(planDTO);
    }

    public async Task<Result<PlanToBeEditedDTO>> GetPlanToBeEditedAsync(Guid id, CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();

        var planToBeEdited = await planRepo.GetByIdAsync(id, ct: ct);

        if (planToBeEdited is null)
        {
            _logger.LogWarning("Plan to be edited of Id ({@Id}) is not found", id);
            return Result<PlanToBeEditedDTO>.Failure(PlanBusinessErrors.PlanToBeEditedNotFound);
        }

        var membershipRepo = _unitOfWork.GetGenericRepository<Membership>();
        var planHasActiveMembershipsSpecification = new PlanHasActiveMembershipsSpecification(id);
        var planHasActiveMemberships = await membershipRepo.AnyAsync(planHasActiveMembershipsSpecification, ct);

        if (planHasActiveMemberships)
        {
            _logger.LogWarning("Plan to be edited of Id ({@Id}) has active memberships", id);
            return Result<PlanToBeEditedDTO>.Failure(PlanBusinessErrors.PlanToBeEditedHasActiveMemberships);
        }

        var planToBeEditedDTO = _mapper.Map<PlanToBeEditedDTO>(planToBeEdited);

        _logger.LogInformation("Plan to be edited of Id ({@Id}) is retrieved successfully", id);
        return Result<PlanToBeEditedDTO>.Success(planToBeEditedDTO);
    }

    public async Task<Result> UpdatePlanToBeEditedAsync(Guid id, PlanToBeEditedDTO planDTO, CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();

        var planToBeUpdated = await planRepo.GetByIdAsync(id, ct: ct);

        if (planToBeUpdated is null)
        {
            _logger.LogWarning("Plan to be edited of Id ({@Id}) is not found", id);
            return Result.Failure(PlanBusinessErrors.PlanToBeEditedNotFound);
        }

        var membershipRepo = _unitOfWork.GetGenericRepository<Membership>();
        var planHasActiveMembershipsSpecification = new PlanHasActiveMembershipsSpecification(id);
        var planHasActiveMemberships = await membershipRepo.AnyAsync(planHasActiveMembershipsSpecification, ct);

        if (planHasActiveMemberships)
        {
            _logger.LogWarning("Plan to be edited of Id ({@Id}) has active memberships", id);
            return Result.Failure(PlanBusinessErrors.PlanToBeEditedHasActiveMemberships);
        }

        _mapper.Map(planDTO,planToBeUpdated);

        planRepo.Update(planToBeUpdated);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
        {
            _logger.LogError("Plan of Id ({@Id}) is not updated due to an internal DB error", id);
            Result.Failure(PlanBusinessErrors.PlanNotEdited);
        }

        _logger.LogInformation("Plan of Id ({@Id}) is updated successfully",id);
        return Result.Success();
    }

    public async Task<Result> ChangePlanStatusAsync(Guid id, CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();

        var planWithStatusToBeChanged = await planRepo.GetByIdAsync(id, ct: ct);

        if (planWithStatusToBeChanged is null)
        {
            _logger.LogWarning("Plan with status to be changed of Id ({@Id}) is not found", id);
            return Result.Failure(PlanBusinessErrors.PlanWithStatusToBeChangedNotFound);
        }

        var membershipRepo = _unitOfWork.GetGenericRepository<Membership>();
        var planHasActiveMembershipsSpecification = new PlanHasActiveMembershipsSpecification(id);
        var planHasActiveMemberships = await membershipRepo.AnyAsync(planHasActiveMembershipsSpecification, ct);

        if (planHasActiveMemberships && planWithStatusToBeChanged.IsActive == true)
        {
            _logger.LogWarning("Plan of Id ({@Id}) cannot be deactivated as it has active memberships", id);
            return Result.Failure(PlanBusinessErrors.PlanWithActiveMembershipsCannotBeDeactivated);
        }

        planWithStatusToBeChanged!.IsActive = !planWithStatusToBeChanged.IsActive;

        planRepo.Update(planWithStatusToBeChanged);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
        {
            _logger.LogError("Plan status of Id ({@Id}) is not changed due to internal DB error", id);
            Result.Failure(PlanBusinessErrors.PlanStatusNotChanged);
        }

        _logger.LogInformation("Plan status of Id ({@Id}) is changed successfully", id);
        return Result.Success();
    }
}
