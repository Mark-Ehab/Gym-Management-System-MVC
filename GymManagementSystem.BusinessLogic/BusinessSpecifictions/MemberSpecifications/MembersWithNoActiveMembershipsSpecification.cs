using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.MemberSpecifications;

public sealed class MembersWithNoActiveMembershipsSpecification : Specification<Member>
{
    public MembersWithNoActiveMembershipsSpecification()
 		:base(m => m.Memberships.Count == 0)
	{
		AddInclude(query => query.Include(m => m.Memberships.Where(ms => ms.StartDate.AddDays(ms.Plan.DurationDays) > DateOnly.FromDateTime(DateTime.Now))));
	}
}
