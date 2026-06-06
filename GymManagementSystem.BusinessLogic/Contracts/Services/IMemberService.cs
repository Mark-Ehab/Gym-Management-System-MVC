using GymManagementSystem.BusinessLogic.DTOs.HealthRecordDTOs;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.BusinessLogic.Results;
using GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Contracts.Services;

public interface IMemberService
{
    Task<IEnumerable<AllMembersDTO>> GetAllMembersAsync(CancellationToken ct = default);
    Task<Result<MemberDetailsDTO>> GetMemberDetailsAsync(Guid id, CancellationToken ct = default);
    Task<Result<HealthRecordDTO>> GetMemberHealthRecordDetailsAsync(Guid id, CancellationToken ct = default);
    Task<Result> AddMemberAsync(MemberCreateDTO memberToBeCreatedDTO, CancellationToken ct = default);
    Task<Result<MemberToBeEditedDTO>> GetMemberToBeEditedAsync(Guid id, CancellationToken ct = default);
    Task<Result> EditMemberAsync(Guid id, MemberToBeEditedDTO memberToBeEditedDTO, CancellationToken ct = default);
    Task<Result> DeleteMemberAsync(Guid id, CancellationToken ct = default);
}
