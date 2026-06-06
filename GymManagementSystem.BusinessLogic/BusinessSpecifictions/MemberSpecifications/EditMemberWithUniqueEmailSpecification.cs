using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.MemberSpecifications;

public sealed class EditMemberWithUniqueEmailSpecification : Specification<Member>
{
    public EditMemberWithUniqueEmailSpecification(Guid id, string email)
        :base(m => m.Email == email && m.Id != id)
    {
        
    }
}
