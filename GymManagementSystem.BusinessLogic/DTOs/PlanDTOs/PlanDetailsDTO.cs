using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;

public sealed class PlanDetailsDTO
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public string Description { get; set; } = default!;
    public bool Status { get; set; } = default!;
}
