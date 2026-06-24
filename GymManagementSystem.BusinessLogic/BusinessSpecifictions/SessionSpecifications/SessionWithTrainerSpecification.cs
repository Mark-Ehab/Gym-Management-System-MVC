using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.SessionSpecifications;

public sealed class SessionWithTrainerSpecification : Specification<Session>
{
    public SessionWithTrainerSpecification(Guid sessionId)
        :base(s => s.Id == sessionId)
    {
        AddInclude(query => query
          .Include(s => s.Trainer));
    }
}
