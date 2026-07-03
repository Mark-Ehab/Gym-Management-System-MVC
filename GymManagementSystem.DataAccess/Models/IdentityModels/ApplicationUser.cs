using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Models.IdentityModels;

public class ApplicationUser : IdentityUser<Guid> 
{
    public string FullName { get; set; } = default!;
}
