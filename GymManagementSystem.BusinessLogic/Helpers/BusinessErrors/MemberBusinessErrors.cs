using GymManagementSystem.BusinessLogic.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;

public static class MemberBusinessErrors
{
    public static Error MemberEmailAlreadyExists
        => new("Member.Email.AlreadyExists", "Email already exists on the system !");
    public static Error MemberPhoneNumberAlreadyExists
        => new("Member.PhoneNumber.AlreadyExists", "Phone number already exists on the system !");
    public static Error MemberDetailsNotFound
        => new("Member.Details.NotFound", "Member you are trying to get their details is not found on the system !");
    public static Error MemberHealthRecordDetailsNotFound
        => new("Member.HealthRecordDetails.NotFound", "Member health record details are not found on the system !");
    public static Error MemberToBeEditedNotFound
        => new("Member.Edit.NotFound", "Member you are trying to edit is not found on the system !");
    public static Error MemberToBeDeletedNotFound
        => new("Member.Delete.NotFound", "Member you are trying to delete is not found on the system !");
    public static Error MemberNotCreated
        => new("Member.Create.NotCreated", "Member is not created due to an internal error !");
    public static Error MemberNotEdited
        => new("Member.Edit.NotEdited", "Member is not edited due to an internal error !");
    public static Error MemberNotDeleted
        => new("Member.Delete.NotDeleted", "Member is not deleted due to an internal error !");
    public static Error MemberWithActiveCurrentOrFutureBookingsCannotBeDeleted
        => new("Member.Delete.ActiveBookings.CannotDelete", "This member has already active current or future bookings on the system and cannot be deleted !");
}
