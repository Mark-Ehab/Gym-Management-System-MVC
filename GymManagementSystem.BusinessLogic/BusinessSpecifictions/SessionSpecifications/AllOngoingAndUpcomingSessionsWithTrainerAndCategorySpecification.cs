using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.SessionSpecifications;

public sealed class AllOngoingAndUpcomingSessionsWithTrainerAndCategorySpecification : Specification<Session>
{
    public AllOngoingAndUpcomingSessionsWithTrainerAndCategorySpecification() :
    base(s => s.EndDate > DateTime.Now)
    {
        AddInclude(query => query
            .Include(s => s.Trainer)
            .Include(s => s.Category));
    }
}
