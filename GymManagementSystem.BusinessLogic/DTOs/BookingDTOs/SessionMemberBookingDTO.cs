using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.BookingDTOs;

public class SessionMemberBookingDTO
{
    public Guid BookingId { get; set; }
    public string MemberName { get; set; } = default!;
    public DateTime BookingDate { get; set; }
    public bool IsAttended { get; set; }
}
