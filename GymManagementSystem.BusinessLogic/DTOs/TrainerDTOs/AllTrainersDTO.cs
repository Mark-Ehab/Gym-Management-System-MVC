using GymManagementSystem.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;

public sealed class AllTrainersDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public Speciality Speciality { get; set; }
}
