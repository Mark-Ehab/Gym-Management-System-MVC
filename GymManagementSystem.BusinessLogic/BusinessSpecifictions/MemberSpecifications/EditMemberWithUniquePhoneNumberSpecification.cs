using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.MemberSpecifications;

public sealed class EditMemberWithUniquePhoneNumberSpecification : Specification<Member>
{
    public EditMemberWithUniquePhoneNumberSpecification(Guid id, string phone)
        :base(m => m.Phone == phone && m.Id != id)
    {
        
    }
}
