using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.SessionSpecifications;

public sealed class CheckIfSessionIsOngoingOrUpcomingSpecification : Specification<Session>
{
    public CheckIfSessionIsOngoingOrUpcomingSpecification(Guid sessionId)
        :base(s => s.Id == sessionId && s.EndDate > DateTime.Now)
    {
        
    }
}
