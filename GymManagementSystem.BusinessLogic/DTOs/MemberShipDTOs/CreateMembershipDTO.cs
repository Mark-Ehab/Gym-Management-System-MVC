using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.MemberShipDTOs;

public sealed class CreateMembershipDTO
{
    public Guid MemberId { get; set; }
    public Guid PlanId { get; set; }
}
