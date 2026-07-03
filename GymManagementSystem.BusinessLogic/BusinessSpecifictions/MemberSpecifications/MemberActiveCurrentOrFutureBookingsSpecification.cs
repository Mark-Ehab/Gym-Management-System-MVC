using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.MemberSpecifications;

public sealed class MemberActiveCurrentOrFutureBookingsSpecification : Specification<Booking>
{
    public MemberActiveCurrentOrFutureBookingsSpecification(Guid memberId)
        : base(b => b.MemberId == memberId && b.Session.EndDate > DateTime.UtcNow)
    {
        
    }
}
