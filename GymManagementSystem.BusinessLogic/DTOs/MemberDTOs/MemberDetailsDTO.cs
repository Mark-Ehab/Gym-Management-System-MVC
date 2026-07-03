using GymManagementSystem.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;

public sealed class MemberDetailsDTO
{
    public string? Photo { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public Gender Gender { get; set; } 
    public DateOnly DateOfBirth { get; set; } 
    public string Address { get; set; } = default!;
    public string? PlanName { get; set; } 
    public DateOnly? MembershipStartDate { get; set; } 
    public DateOnly? MembershipEndDate { get; set; } 
}
