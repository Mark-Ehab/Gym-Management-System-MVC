using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.SessionSpecifications;

public sealed class EditSessionTrainerIsNotAvailableSpecification : Specification<Session>
{
    public EditSessionTrainerIsNotAvailableSpecification(Guid sessionId, Guid trainerId, DateTime sessionStartDate, DateTime sessionEndDate)
        : base(s => s.TrainerId == trainerId &&
        s.Id != sessionId &&
        (s.StartDate == sessionStartDate ||
        (s.StartDate < sessionStartDate && s.EndDate > sessionStartDate) ||
        (s.StartDate > sessionStartDate && s.StartDate < sessionEndDate)))
    {
        
    }
}
