using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;

public sealed class MemberToBeEditedDTO
{
    public string? Photo { get; set; } 
    public string? Name { get; set; } 
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string BuildingNumber { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
}
