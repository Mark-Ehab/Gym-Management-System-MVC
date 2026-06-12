using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.SessionDTOs;

public sealed class CreateSessionDTO
{
    public Guid CategoryId { get; set; }
    public Guid TrainerId { get; set; }
    public string Description { get; set; } = default!;
    public byte Capacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
