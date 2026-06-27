using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;

public sealed class TrainerSelectDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
