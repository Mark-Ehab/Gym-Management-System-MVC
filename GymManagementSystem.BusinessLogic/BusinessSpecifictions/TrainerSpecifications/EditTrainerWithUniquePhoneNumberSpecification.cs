using GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.BusinessSpecifictions.TrainerSpecifications;

public sealed class EditTrainerWithUniquePhoneNumberSpecification : Specification<Trainer>
{
    public EditTrainerWithUniquePhoneNumberSpecification(Guid id, string phone):
        base(t => t.Phone == phone && t.Id != id)
    {
        
    }
}
