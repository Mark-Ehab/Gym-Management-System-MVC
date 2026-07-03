using GymManagementSystem.BusinessLogic.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;

public static class SessionBusinessErrors
{
    public static Error NewSessionCategoryDropDownListNotFound
        => new("Session.Create.CategoryDropDownListNotFound", "No available categories to create a new session !");
    public static Error NewSessionTrainerDropDownListNotFound
        => new("Session.Create.TrainerDropDownListNotFound", "No available trainers to be assigned to a new session !");
    public static Error NewSessionStartDateAfterEndDate
        => new("Session.Create.StartDateAfterEndDate", "Cannot create a new session as Start date is after End date !");
    public static Error NewSessionStartDateInThePast
        => new("Session.Create.StartDateInThePast", "Cannot create a new session as Start date is not a future date !");
    public static Error NewSessionCategoryNotFound
        => new("Session.Create.CategoryNotFound", "Cannot create a new session as selected category is not found !");
    public static Error NewSessionTrainerNotFound
        => new("Session.Create.TrainerNotFound", "Cannot create a new session as selected trainer is not found !");
    public static Error NewSessionInvalidCategory
        => new("Session.Create.InvalidCategory", "Cannot create a new session as selected category is Invalid !");
    public static Error NewSessionCategoryAndTrainerSpecialityNotMatched
        => new("Session.Create.CategoryAndTrainerSpecialityNotMatched", "Cannot create a new session as selected category is not matched with selected trainer speciality !");
    public static Error NewSessionNotCreated
        => new("Session.Create.NotCreated", "Cannot create a new session due to an internal error !");
    public static Error SessionWithDetailsNotFound
        => new("Session.Details.NotFound", "Session you trying to get its details is not found on the system !");
    public static Error SessionToBeEditedNotFound
        => new("Session.Edit.NotFound", "Session you trying to edit is not found on the system !");
    public static Error CannotEditOngoingOrCompletedSessions
        => new("Session.Edit.OngoingOrCompletedSessions", "Cannot edit ongoing or completed session !");
    public static Error SessionToBeEditedStartDateAfterEndDate
    => new("Session.Edit.StartDateAfterEndDate", "Cannot edit session Start date to be after End date !");
    public static Error SessionToBeEditedStartDateInThePast
        => new("Session.Edit.StartDateInThePast", "Cannot edit session Start date to be in a past date !");
    public static Error SessionToBeEditedCategoryAndTrainerSpecialityNotMatched
        => new("Session.Edit.CategoryAndTrainerSpecialityNotMatched", "Selected trainer speciality doesn't match the scategory of the session to be updated !");
    public static Error SessionToBeEditedHasActiveBookings
        => new("Session.Edit.ActiveBookings", "Cannot edit a session that has active bookings !");
    public static Error SessionTrainerUnavailable
        => new("Session.NewTrainerUnavailable", "New selected trainer already have another session in the same time !");
    public static Error SessionNotEdited
        => new("Session.Edited.NotEdited", "Cannot edit the session due to an internal error !");
    public static Error SessionToBeEditedTrainerNotFound
        => new("Session.Edit.TrainerNotFound", "Cannot edit the session as selected trainer is not found !");
    public static Error OngoingOrUpcomingSessionsCannotBeDeleted
        => new("Session.Delete.OngoingOrUpcoming", "Cannot delete an ongoing or upcoming sessions !");
    public static Error SessionToBeDeletedNotFound
        => new("Session.Delete.NotFound", "Session you trying to delete is not found on the system !");
    public static Error CannotDeleteSessionWithBookings
        => new("Session.Delete.HasBookings", "Cannot delete the session as it has bookings on it !");
    public static Error SessionNotDeleted
        => new("Session.Delete.NotDeleted", "Cannot delete the session due to an internal error !");
}
