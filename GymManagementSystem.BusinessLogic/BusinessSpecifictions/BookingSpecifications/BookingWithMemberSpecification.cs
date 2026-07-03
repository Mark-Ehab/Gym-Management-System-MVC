using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.BookingSpecifications;

public sealed class BookingWithMemberSpecification : Specification<Booking>
{
    public BookingWithMemberSpecification(Guid sessionId,bool includeMemberEnabled = false)
        :base(b => b.SessionId == sessionId)
    {
        if(includeMemberEnabled)
            AddInclude(query => query.Include(b => b.Member));
    }
}
