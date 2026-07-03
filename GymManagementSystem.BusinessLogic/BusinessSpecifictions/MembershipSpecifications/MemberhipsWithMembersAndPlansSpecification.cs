using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.MembershipSpecifications;

public sealed class MemberhipsWithMembersAndPlansSpecification : Specification<Membership>
{
    public MemberhipsWithMembersAndPlansSpecification()
        :base()
    {
        AddInclude(query => 
            query.Include(ms => ms.Member)
            .Include(ms => ms.Plan));
    }
}
