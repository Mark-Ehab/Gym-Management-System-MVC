using GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.TrainerSpecifications;

public sealed class EditTrainerWithUniqueEmailSpecification : Specification<Trainer>
{
    public EditTrainerWithUniqueEmailSpecification(Guid id, string email):
        base(t => t.Email == email && t.Id != id)
    {
        
    }
}
