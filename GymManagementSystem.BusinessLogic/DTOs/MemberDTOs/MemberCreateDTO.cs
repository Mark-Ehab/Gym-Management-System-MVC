using GymManagementSystem.BusinessLogic.DTOs.HealthRecordDTOs;
using GymManagementSystem.DataAccess.Enums;
using GymManagementSystem.DataAccess.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;

public sealed class MemberCreateDTO
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; } = default!;
    public Gender Gender { get; set; } = default!;
    public string BuildingNumber { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public HealthRecordDTO HealthRecord { get; set; } = default!;
}
