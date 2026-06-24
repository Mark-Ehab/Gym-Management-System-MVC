using AutoMapper;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.MembershipSpecifications;
using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.BusinessLogic.DTOs.MemberShipDTOs;
using GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;
using GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;
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

    public Task<Result<IEnumerable<MemberSelectDTO>>> GetMembersForDropDownListAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<PlanSelectDTO>>> GetPlansForDropDownListAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
