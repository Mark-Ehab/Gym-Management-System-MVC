using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.SessionSpecifications;

public sealed class SessionWithCategorySpecification : Specification<Session>
{
    public SessionWithCategorySpecification(Guid sessionId)
    : base(s => s.Id == sessionId)
    {
        AddInclude(query => query
          .Include(s => s.Category));
    }
}
