using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.SessionSpecifications;

public sealed class SessionWithCategoryAndTrainerSpecification : Specification<Session>
{
    public SessionWithCategoryAndTrainerSpecification(Guid sessionId) :
        base(s => s.Id == sessionId)
    {
        AddInclude(query => query
        .Include(s => s.Category)
        .Include(s => s.Trainer));
    }
}
