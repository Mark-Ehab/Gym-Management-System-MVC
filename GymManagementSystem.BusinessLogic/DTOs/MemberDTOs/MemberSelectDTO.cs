using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;

public sealed class MemberSelectDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
