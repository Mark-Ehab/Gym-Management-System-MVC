using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.SessionSpecifications;

public sealed class CompletedSessionsSpecification : Specification<Session>
{
    public CompletedSessionsSpecification()
        :base(s => s.EndDate < DateTime.Now)
    {
        
    }
}
