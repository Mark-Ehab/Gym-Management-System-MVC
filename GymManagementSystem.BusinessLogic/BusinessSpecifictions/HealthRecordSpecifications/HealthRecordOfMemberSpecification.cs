using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.HealthRecordSpecifications;

public sealed class HealthRecordOfMemberSpecification : Specification <HealthRecord>
{
    public HealthRecordOfMemberSpecification(Guid memberId)
        :base(hr => hr.MemberId == memberId)
    {
        
    }
}
