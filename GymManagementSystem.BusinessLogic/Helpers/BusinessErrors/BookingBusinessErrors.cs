using GymManagementSystem.BusinessLogic.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;

public static class BookingBusinessErrors
{
    public static Error NoAvailableMembersToBookSession
        => new("Booking.Create.NoAvailableMembers", "No available members to book the session !");
    public static Error MemberToBookSessionNotFound
        => new("Booking.Create.MemberNotFound", "Member is not found to book the session !");
    public static Error MemberAlreadyBookedSession
        => new("Booking.Create.MemberAlreadyBookedSession", "This member already booked the session !");
    public static Error MemberToBookSessionWithNoActiveMembership
        => new("Booking.Create.MemberWithNoActiveMembership", "This member has no active membership currently !");
    public static Error BookingNotCreated
        => new("Booking.Create.NotCreated", "Booking on the session is not created due to an internal server error !");
    public static Error SessionToBeBookedNotFound
        => new("Booking.Create.SessionNotFound", "Session is currently not found on the system !");
    public static Error SessionFullyBooked
        => new("Booking.Create.SessionFullyBooked", "Session is fully booked !");
    public static Error BookingToBeCancelledNotFound
        => new("Booking.Cancel.NotFound", "Booking is not found on the system to be cancelled !");
    public static Error BookingNotCancelled
    => new("Booking.Cancel.NotCancelled", "Booking on the session is not cancelled due to an internal server error !");
    public static Error BookingMemberAttendanceNotCancelled
    => new("Booking.Update.Member.Attedance.NotUpdated", "Member attendance on the session is not updated to attended due to an internal server error !");
}
