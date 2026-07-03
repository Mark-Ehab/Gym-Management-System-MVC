using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.MemberSpecifications;

public sealed class MemberHasActiveMembership : Specification<Member>
{
    public MemberHasActiveMembership(Guid memberId):
        base(m => m.Id == memberId && m.Memberships.Count == 1)
    {
        AddInclude(query => query.Include(m => m.Memberships.Where(ms => ms.StartDate.AddDays(ms.Plan.DurationDays) > DateOnly.FromDateTime(DateTime.Now))));
    }
}
