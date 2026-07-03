using AutoMapper;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.MembershipSpecifications;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.MemberSpecifications;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.PlanSpecifications;
using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.BusinessLogic.DTOs.MemberShipDTOs;
using GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;
using GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;
using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.UoW.Contract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Services.BusinessServices;

public class MembershipService : IMembershipService
{
    /* Fields */
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<MembershipService> _logger;

    /* Constructor */
    public MembershipService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MembershipService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    /* Methods */
    public async Task<IEnumerable<AllMembershipsDTO>> GetAllMembershipsAsync(CancellationToken ct = default)
    {
        var membershipRepo = _unitOfWork.GetGenericRepository<Membership>();
        var memberhipsWithMembersAndPlansSpecification = new MemberhipsWithMembersAndPlansSpecification();
        var allMemberships = await membershipRepo.ListAsync(memberhipsWithMembersAndPlansSpecification, ct:ct);

        if (allMemberships is null || allMemberships.Count == 0)
        {
            _logger.LogWarning("No memberships are retrieved when membership's index page is hitted");
            return [];
        }

        var allMembershipsDTOs = _mapper.Map<IEnumerable<AllMembershipsDTO>>(allMemberships);

        _logger.LogInformation("Memberships: {@Memberships} are retrieved successfully", allMembershipsDTOs);
        return allMembershipsDTOs;
    }

    public async Task<Result<IEnumerable<MemberSelectDTO>>> GetMembersForDropDownListAsync(CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetGenericRepository<Member>();
        var membersWitNoActiveMembershipsSpecification = new MembersWithNoActiveMembershipsSpecification();

        var allowedMembersToSubscribe = await memberRepo.ListAsync(membersWitNoActiveMembershipsSpecification, ct: ct);

        if (allowedMembersToSubscribe is null || allowedMembersToSubscribe.Count == 0)
        {
            _logger.LogWarning("No eligible members for subscription is retrieved to be displayed in drop down list when creating a new membership");
            return Result<IEnumerable<MemberSelectDTO>>.Failure(MembershipBusinessErrors.NewMembershipAvailableMembersDropDownListNotFound);
        }

        var allowedMembersToSubscribeSelectDTOs = _mapper.Map<IEnumerable<MemberSelectDTO>>(allowedMembersToSubscribe);

        _logger.LogInformation("Allowed Members To Subscribe: {@AllowedMembersToSubscribe} are retrieved for drop down list when creating a new memebership", allowedMembersToSubscribeSelectDTOs);
        return Result<IEnumerable<MemberSelectDTO>>.Success(allowedMembersToSubscribeSelectDTOs);
    }

    public async Task<Result<IEnumerable<PlanSelectDTO>>> GetPlansForDropDownListAsync(CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();
        var activePlansSpecification = new ActivePlansSpecification();

        var activePlans = await planRepo.ListAsync(activePlansSpecification, ct: ct);

        if(activePlans is null || activePlans.Count == 0)
        {
            _logger.LogWarning("No active plan is retrieved to be displayed in drop down list when creating a new membership");
            return Result<IEnumerable<PlanSelectDTO>>.Failure(MembershipBusinessErrors.NewMembershipActivePlansDropDownListNotFound);
        }

        var activePlanSelectDTOs = _mapper.Map<IEnumerable<PlanSelectDTO>>(activePlans);

        _logger.LogInformation("Active Plans: {@ActivePlans} are retrieved for drop down list when creating a new memebership", activePlanSelectDTOs);
        return Result<IEnumerable<PlanSelectDTO>>.Success(activePlanSelectDTOs);
    }

    public async Task<Result> AddMembership(CreateMembershipDTO membershipDTO, CancellationToken ct = default)
    {
        var planRepo = _unitOfWork.GetGenericRepository<Plan>();
        var activePlansSpecification = new ActivePlansSpecification(membershipDTO.PlanId);

        if(!await planRepo.AnyAsync(activePlansSpecification,ct))
        {
            _logger.LogWarning("{@Plan} is either not found or not active", membershipDTO.PlanId);
            return Result.Failure(MembershipBusinessErrors.PlanNotActiveOrNotFound);
        }

        var membershipRepo = _unitOfWork.GetGenericRepository<Membership>();
        var memberHasDuplicateMembershipsSpecification = new MemberHasDuplicateMembershipsSpecification(membershipDTO.MemberId);

        if (await membershipRepo.AnyAsync(memberHasDuplicateMembershipsSpecification, ct))
        {
            _logger.LogWarning("{@Member} has duplicate memberships", membershipDTO.MemberId);
            return Result.Failure(MembershipBusinessErrors.MemberHasDuplicateMemberships);
        }

        var membership = _mapper.Map<Membership>(membershipDTO);

        membershipRepo.Add(membership);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync();

        if (numOfRowsAffected == 0)
        {
            _logger.LogError("New memberships is not created due to an internal DB error");
            return Result.Failure(MembershipBusinessErrors.NewMembershipNotCreated);
        }

        _logger.LogInformation("New membership is created successfully with Id:{@Id}", membership.Id);
        return Result.Success();

    }

    public async Task<Result> DeleteMembership(Guid membershipId, CancellationToken ct = default)
    {
        var membershipRepo = _unitOfWork.GetGenericRepository<Membership>();
        var membershipsToBeDeletedForMemberSpecificaiton = new MembershipsToBeDeletedForMemberSpecificaiton(membershipId);

        var membershipToBeDeleted = await membershipRepo.FirstOrDefaultAsync(membershipsToBeDeletedForMemberSpecificaiton,ct:ct);

        if (membershipToBeDeleted is null)
        {
            _logger.LogWarning("Membership of Id ({@MembershipId}) is not found to be deleted", membershipId);
            return Result.Failure(MembershipBusinessErrors.MembershipToBeCancelledNotFound);
        }

        membershipRepo.SoftDelete(membershipToBeDeleted);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync();

        if (numOfRowsAffected == 0)
        {
            _logger.LogError("Membership of Id ({@MembershipId}) is not deleted due to an internal DB error",membershipId);
            return Result.Failure(MembershipBusinessErrors.MembershipNotCancelled);
        }

        _logger.LogInformation("Membership of Id ({@MembershipId}) is deleted successfully", membershipId);
        return Result.Success();
    }
}
