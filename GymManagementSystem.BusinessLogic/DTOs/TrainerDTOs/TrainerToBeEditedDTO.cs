using GymManagementSystem.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;

public sealed class TrainerToBeEditedDTO
{
    public string? Name { get; set; } 
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public Speciality Speciality { get; set; }
    public string BuildingNumber { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
}
