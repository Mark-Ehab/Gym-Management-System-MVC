using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.TrainerSpecifications;

public sealed class NewTrainerWithUniquePhoneNumberSpecification : Specification<Trainer>
{
    public NewTrainerWithUniquePhoneNumberSpecification(string phone)
        : base(t => t.Phone == phone)
    {
        
    }
}
