using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Models;

public sealed class Member : GymUser
{
    public DateOnly JoinDate { get; set; }
    public string? Photo { get; set; }
    public HealthRecord HealthRecord { get; set; } = default!;
    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Membership> Memberships { get; set; } = [];
}
