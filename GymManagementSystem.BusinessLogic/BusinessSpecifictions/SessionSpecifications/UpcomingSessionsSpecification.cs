using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.SessionSpecifications;

public sealed class UpcomingSessionsSpecification : Specification<Session>
{
    public UpcomingSessionsSpecification()
        :base(s => s.StartDate >  DateTime.Now)
    {
        
    }
}
