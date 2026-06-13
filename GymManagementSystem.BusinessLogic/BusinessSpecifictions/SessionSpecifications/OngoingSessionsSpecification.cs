using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.SessionSpecifications;

public sealed class OngoingSessionsSpecification : Specification<Session>
{
    public OngoingSessionsSpecification()
        :base(s => s.StartDate <= DateTime.Now &&  s.EndDate >= DateTime.Now)
    {
            
    }
}
