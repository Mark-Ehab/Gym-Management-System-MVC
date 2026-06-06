using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.PlanSpecifications;

public sealed class PlanHasActiveMembershipsSpecification : Specification<Membership>
{
    public PlanHasActiveMembershipsSpecification(Guid planId)
        :base(ms => ms.PlanId == planId && ms.StartDate.AddDays(ms.Plan.DurationDays) > DateOnly.FromDateTime(DateTime.UtcNow))
    {
        
    }
}
