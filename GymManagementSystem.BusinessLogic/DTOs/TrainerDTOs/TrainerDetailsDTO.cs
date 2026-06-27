using GymManagementSystem.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;

public sealed class TrainerDetailsDTO
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public Speciality Speciality { get; set; }
    public string BuildingNumber {  get; set; } = default!;
    public string Street {  get; set; } = default!;
    public string City {  get; set; } = default!;
}
