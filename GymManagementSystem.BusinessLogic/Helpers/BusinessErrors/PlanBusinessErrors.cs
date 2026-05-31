using GymManagementSystem.BusinessLogic.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;

public static class PlanBusinessErrors
{
    public static Error PlanDetailsNotFound
        => new("Plan.Details.NotFound", "Plan details are not found on the system !");
    public static Error PlanToBeEditedNotFound
        => new("Plan.Edit.NotFound", "Plan you are trying to edit is not found on the system !");
    public static Error PlanWithStatusToBeChangedNotFound
        => new("Plan.Activate.NotFound", "Plan you are trying to change its status is not found on the system !");
    public static Error PlanNotEdited
        => new("Plan.Edit.NotEdited", "Plan is not edited due to an internal error !");
    public static Error PlanStatusNotChanged
        => new("Plan.Edit.NotChanged", "Plan status is not changed due to an internal error !");
    public static Error PlanToBeEditedHasActiveMemberships
        => new("Plan.Edit.ActiveMemberships", "Plan you are trying to edit has active memberships on the system !");
    public static Error PlanWithActiveMembershipsCannotBeDeactivated
        => new("Plan.Activate.ActiveMemberships.CannotDeactivate", "This plan has active memberships on the system and cannot be deactivated !");
}
