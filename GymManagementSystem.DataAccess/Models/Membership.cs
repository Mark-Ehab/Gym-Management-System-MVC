using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Models;

public sealed class Membership : BaseEntity
{
    public Guid MemberId { get; set; } = default!;
    public Member Member { get; set; } = default!;
    public Guid PlanId { get; set; } = default!;
    public Plan Plan { get; set; } = default!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate => StartDate.AddDays(Plan.DurationDays);
    public string Status => EndDate < DateOnly.FromDateTime(DateTime.Now) ? "Inactive" : "Active";
    public bool IsActive => EndDate > DateOnly.FromDateTime(DateTime.Now);
}
