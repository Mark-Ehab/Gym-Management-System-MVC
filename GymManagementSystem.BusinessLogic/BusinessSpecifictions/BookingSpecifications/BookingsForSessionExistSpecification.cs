using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.BookingSpecifications;

public sealed class BookingsForSessionExistSpecification : Specification<Booking>
{
    public BookingsForSessionExistSpecification(Guid sessionId)
        :base(b => b.SessionId ==  sessionId)
    {
        
    }
}
