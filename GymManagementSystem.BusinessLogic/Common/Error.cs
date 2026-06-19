using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Common;

public sealed record Error(string Code, string Description);
