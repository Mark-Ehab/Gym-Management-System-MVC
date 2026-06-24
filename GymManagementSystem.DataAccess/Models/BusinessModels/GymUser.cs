using GymManagementSystem.DataAccess.Enums;
using GymManagementSystem.DataAccess.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Models.BusinessModels;

public abstract class GymUser : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; } 
    public Gender Gender { get; set; }
    public Address Address { get; set; } = default!;
}
