using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.TrainerSpecifications;

public sealed class NewTrainerWithUniqueEmailSpecification : Specification<Trainer>
{
    public NewTrainerWithUniqueEmailSpecification(string email):
        base(t => t.Email == email)
    {
        
    }
}
