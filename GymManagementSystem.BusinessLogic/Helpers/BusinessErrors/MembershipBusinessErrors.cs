using GymManagementSystem.BusinessLogic.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;

public static class MembershipBusinessErrors
{
    public static Error NewMembershipActivePlansDropDownListNotFound
        => new("Membership.Create.ActivePlansDropDownListNotFound", "No available active plans to create a new membership !");
    public static Error NewMembershipAvailableMembersDropDownListNotFound
        => new("Membership.Create.AvailableMembersDropDownListNotFound", "No available memebers to create a new membership !");
    public static Error PlanNotActiveOrNotFound
        => new("Membership.Create.PlanNotActiveOrNotFound", "Selected plan is now either not found or not active !");
    public static Error MemberHasDuplicateMemberships
        => new("Membership.Create.MemberHasDuplicateMemberships", "Selected member has duplicate memberships !");
    public static Error NewMembershipNotCreated
        => new("Membership.Create.NotCreated", "Cannot create a new membership due to an internal error !");
    public static Error MembershipToBeCancelledNotFound
        => new("Membership.Delete.NotFound", "The cancelled membership is not currently found on the system !");
    public static Error MembershipNotCancelled
        => new("Membership.Delete.NotCancelled", "Membership is not cancelled due to an internal error !");
}
