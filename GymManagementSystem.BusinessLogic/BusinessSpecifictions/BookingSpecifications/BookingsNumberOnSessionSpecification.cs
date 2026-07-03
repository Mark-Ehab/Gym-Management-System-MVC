using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.BookingSpecifications;

public sealed class BookingsNumberOnSessionSpecification : Specification<Booking>
{
    public BookingsNumberOnSessionSpecification(Guid sessionId)
        :base(b => b.SessionId == sessionId)
    {
        
    }
}
