using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;

public sealed class PlanSelectDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
