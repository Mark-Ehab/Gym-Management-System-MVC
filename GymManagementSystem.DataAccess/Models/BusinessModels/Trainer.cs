using GymManagementSystem.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Models.BusinessModels;

public sealed class Trainer : GymUser
{
    public Speciality Speciality { get; set; }
    public DateOnly HireDate { get; set; }
    public ICollection<Session>? Sessions { get; set; } = [];
}
