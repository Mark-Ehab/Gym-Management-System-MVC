using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Models.BusinessModels;

public sealed class Booking : BaseEntity
{
    public Guid MemberId { get; set; } 
    public Member Member { get; set; } = default!;
    public Guid SessionId { get; set; } 
    public Session Session { get; set; } = default!;
    public DateOnly BookingDate { get; set; }
    public bool IsAttended { get; set; }
}
