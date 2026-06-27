using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.SessionDTOs;

public sealed class SessionDetailsDTO
{
    public string CategoryName { get; set; } = default!;
    public string TrainerName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public byte Capacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = default!;
    public int NumberOfBookingsPerSession { get; set; } = default;
}
