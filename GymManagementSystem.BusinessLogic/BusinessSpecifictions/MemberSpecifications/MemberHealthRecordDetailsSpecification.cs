using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.MemberSpecifications;

public sealed class MemberHealthRecordDetailsSpecification : Specification<HealthRecord>
{
    public MemberHealthRecordDetailsSpecification(Guid memberId):
        base(hr => hr.MemberId == memberId) 
    {
        
    }
}
