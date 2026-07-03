using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.MembershipSpecifications;

public sealed class MemberHasDuplicateMembershipsSpecification : Specification<Membership>
{
    public MemberHasDuplicateMembershipsSpecification(Guid memberId)
            : base(ms => ms.MemberId == memberId &&
            ms.StartDate.AddDays(ms.Plan.DurationDays) > DateOnly.FromDateTime(DateTime.Now))
    {

    }
}
