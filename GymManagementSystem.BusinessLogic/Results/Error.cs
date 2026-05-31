using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Results;

public sealed record Error(string Code, string Description);
