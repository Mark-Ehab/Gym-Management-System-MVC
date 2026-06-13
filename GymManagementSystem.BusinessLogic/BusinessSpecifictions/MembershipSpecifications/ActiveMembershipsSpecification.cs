using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.MembershipSpecifications;

public sealed class ActiveMembershipsSpecification : Specification<Membership>
{
    public ActiveMembershipsSpecification()
        :base()
    {
        AddInclude(query => query
        .Include(ms => ms.Plan)
        .Where(ms => ms.StartDate.AddDays(ms.Plan.DurationDays) > DateOnly.FromDateTime(DateTime.Now)));
    }
}
