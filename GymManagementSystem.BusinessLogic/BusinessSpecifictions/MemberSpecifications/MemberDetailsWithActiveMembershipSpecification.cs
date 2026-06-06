using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.MemberSpecifications;

public sealed class MemberDetailsWithActiveMembershipSpecification : Specification<Member>
{
    public MemberDetailsWithActiveMembershipSpecification(Guid id) : 
        base(m => m.Id == id)
    {
        AddInclude(q => q.Include(m => m.Memberships
        .OrderByDescending(ms => ms.StartDate)
        .Take(1)
        )
        .ThenInclude(ms => ms.Plan));
    }
}
