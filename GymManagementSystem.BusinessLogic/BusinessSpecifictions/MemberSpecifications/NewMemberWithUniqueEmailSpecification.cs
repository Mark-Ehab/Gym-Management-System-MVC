using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.MemberSpecifications;

public sealed class NewMemberWithUniqueEmailSpecification : Specification<Member>
{
    public NewMemberWithUniqueEmailSpecification(string email)
        :base(m => m.Email == email)
    {
        
    }
}
