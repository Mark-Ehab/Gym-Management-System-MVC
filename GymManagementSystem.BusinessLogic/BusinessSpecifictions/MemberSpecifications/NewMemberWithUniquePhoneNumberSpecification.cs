using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.MemberSpecifications;

public sealed class NewMemberWithUniquePhoneNumberSpecification : Specification<Member>
{
    public NewMemberWithUniquePhoneNumberSpecification(string phone)
        :base(m => m.Phone == phone)
    {
        
    }
}
