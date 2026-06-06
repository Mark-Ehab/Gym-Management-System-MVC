using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.TrainerSpecifications;

public sealed class TrainerHasScheduledSessionsSpecification : Specification<Session>
{
    public TrainerHasScheduledSessionsSpecification(Guid id):
        base(s => s.TrainerId == id && s.EndDate > DateTime.UtcNow)
    {
        
    }
}
