using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.SessionSpecifications;

public sealed class CreateSessionTrainerIsNotAvailableSpecification : Specification<Session>
{
    public CreateSessionTrainerIsNotAvailableSpecification(Guid trainerId, DateTime sessionStartDate, DateTime sessionEndDate)
        :base(s => s.TrainerId == trainerId &&
        (s.StartDate == sessionStartDate ||
        (s.StartDate < sessionStartDate && s.EndDate > sessionStartDate) ||
        (s.StartDate > sessionStartDate && s.StartDate < sessionEndDate)))
    {
        
    }
}
