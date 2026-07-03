using GymManagementSystem.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.HealthRecordDTOs;

public sealed class HealthRecordDTO
{
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public BloodType BloodType { get; set; }
    public string? Note { get; set; } 
}
