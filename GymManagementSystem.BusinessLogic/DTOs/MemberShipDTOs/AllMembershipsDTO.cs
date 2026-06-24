using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.MemberShipDTOs;

public sealed class AllMembershipsDTO
{
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = default!;
    public string PlanName { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
