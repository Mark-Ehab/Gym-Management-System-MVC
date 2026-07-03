using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.DTOs.BookingDTOs;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.BusinessLogic.DTOs.SessionDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Contracts.BusinessServices;

public interface IBookingService
{
    Task<IEnumerable<AllSessionsDTO>> GetAllUpcomingAndOngoingSessionsAsync(CancellationToken ct = default);
    Task<IEnumerable<SessionMemberBookingDTO>> GetSessionMembersBookingsAsync(Guid sessionId, CancellationToken ct = default);
    Task<Result<IEnumerable<MemberSelectDTO>>> GetAvailableMembersForBookingDropDownListAsync(Guid sessionId, CancellationToken ct = default);
    Task<Result> CreateBookingOnUpcomingSessionAsync(Guid sessionId ,Guid memberId, CancellationToken ct = default);
    Task<Result<Guid>> CancelBookingOnUpcomingSessionAsync(Guid bookingId, CancellationToken ct = default);
    Task<Result<Guid>> MarkMemberAsAttendedAsync(Guid bookingId, CancellationToken ct = default);
}
