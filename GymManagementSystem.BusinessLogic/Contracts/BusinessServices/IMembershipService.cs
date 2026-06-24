using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.DTOs.CategoryDTOs;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.BusinessLogic.DTOs.MemberShipDTOs;
using GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Contracts.BusinessServices;

public interface IMembershipService
{
    Task<IEnumerable<AllMembershipsDTO>> GetAllMembershipsAsync(CancellationToken ct = default);
    Task<Result<IEnumerable<MemberSelectDTO>>> GetMembersForDropDownListAsync(CancellationToken ct = default);
    Task<Result<IEnumerable<PlanSelectDTO>>> GetPlansForDropDownListAsync(CancellationToken ct = default);
}
