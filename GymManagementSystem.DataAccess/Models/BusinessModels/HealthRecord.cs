using GymManagementSystem.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Models.BusinessModels;

public sealed class HealthRecord : BaseEntity
{
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public BloodType BloodType { get; set; }
    public string? Note { get; set; } 
    public DateOnly LastUpdate { get; set; }
    public Guid MemberId { get; set; }
    public Member Member { get; set; } = default!;
}
