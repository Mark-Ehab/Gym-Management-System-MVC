using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.ValueObjects;

public sealed class Address
{
    public string BuildingNumber { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
}
