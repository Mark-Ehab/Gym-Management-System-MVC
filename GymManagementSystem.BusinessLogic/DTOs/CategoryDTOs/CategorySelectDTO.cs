using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.CategoryDTOs;

public sealed class CategorySelectDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
