using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.PlanSpecifications;

public sealed class ActivePlansSpecification : Specification<Plan>
{
    public ActivePlansSpecification()
        :base(p => p.IsActive)
    {
        
    }
    public ActivePlansSpecification(Guid planId)
        :base(p => p.Id == planId && p.IsActive)
    {
        
    }
}
