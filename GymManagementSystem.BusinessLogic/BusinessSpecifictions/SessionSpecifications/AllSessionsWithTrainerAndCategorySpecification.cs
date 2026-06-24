using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.SessionSpecifications;

public sealed class AllSessionsWithTrainerAndCategorySpecification : Specification<Session>
{
    public AllSessionsWithTrainerAndCategorySpecification():
        base()
    {
        AddInclude(query => query
            .Include(s => s.Trainer)
            .Include(s => s.Category));
    }
}
