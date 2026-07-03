using GymManagementSystem.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;

public sealed class AllMembersDTO
{
    public Guid Id { get; set; } 
    public string? Photo { get; set; } 
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public Gender Gender { get; set; } = default!;
}
