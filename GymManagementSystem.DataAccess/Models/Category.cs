using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Models;

public sealed class Category : BaseEntity
{
    public string Name { get; set; } = default!;
    public ICollection<Session> Sessions { get; set; } = [];
}
