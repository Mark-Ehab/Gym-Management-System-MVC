using AutoMapper;
using AutoMapper.Execution;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.BookingSpecifications;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.MemberSpecifications;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.SessionSpecifications;
using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using GymManagementSystem.BusinessLogic.DTOs.BookingDTOs;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.BusinessLogic.DTOs.SessionDTOs;
using GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;
using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.UoW.Contract;
using Microsoft.Extensions.Logging;
using Member = GymManagementSystem.DataAccess.Models.BusinessModels.Member;

namespace GymManagementSystem.BusinessLogic.Services.BusinessServices;

public sealed class BookingService : IBookingService
{
    /* Fields */
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<BookingService> _logger;

    /* Constructor */
    public BookingService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BookingService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    /* Methods */
    public async Task<IEnumerable<AllSessionsDTO>> GetAllUpcomingAndOngoingSessionsAsync(CancellationToken ct = default)
    {
        var sessionRepo = _unitOfWork.SessionRepository;
        var allOngoingAndUpcomingSessionsWithTrainerAndCategorySpecification = new AllOngoingAndUpcomingSessionsWithTrainerAndCategorySpecification();

        var allSessions = await sessionRepo.ListAsync(allOngoingAndUpcomingSessionsWithTrainerAndCategorySpecification, ct: ct);

        if (allSessions is null || allSessions.Count == 0)
        {
            _logger.LogInformation("No sessions are retrieved to either be booked or to mark attendance");
            return [];
        }

        var allSessionDTOs = _mapper.Map<IEnumerable<AllSessionsDTO>>(allSessions);

        var allSessionIds = new List<Guid>();
        allSessions.ToList().ForEach(s => allSessionIds.Add(s.Id));
        var numberOfBookingsPerSession = await sessionRepo.GetNumberOfBookingsPerSessions(allSessionIds, ct);

        if (numberOfBookingsPerSession is not null && numberOfBookingsPerSession.Count > 0)
            foreach (var session in numberOfBookingsPerSession)
                allSessionDTOs.FirstOrDefault(s => s.Id == session.Key)!.NumberOfBookingsPerSession = session.Value;

        _logger.LogInformation("Sessions: {@Sessions} are retrieved successfully to either be booked or mark attendance", allSessionDTOs.Select(s => new
        {
            s.Id,
            s.CategoryName
        }));
        return allSessionDTOs;
    }

    public async Task<IEnumerable<SessionMemberBookingDTO>> GetSessionMembersBookingsAsync(Guid sessionId, CancellationToken ct = default)
    {
        var bookingRepo = _unitOfWork.GetGenericRepository<Booking>();
        var bookingWithMemberSpecification = new BookingWithMemberSpecification(sessionId,true);

        var bookingsPerSession = await bookingRepo.ListAsync(bookingWithMemberSpecification, ct: ct);
        if(bookingsPerSession is null || bookingsPerSession.Count == 0)
        {
            _logger.LogWarning("No bookings for session {@SessionId} to view",sessionId);
            return [];
        }

        var sessionMembers = _mapper.Map<IEnumerable<SessionMemberBookingDTO>>(bookingsPerSession);
        _logger.LogInformation("Session members of session {@SessionId} are retrieved successfully: {@SessionMembers}", sessionId, sessionMembers);
        return sessionMembers;
    }

    public async Task<Result<IEnumerable<MemberSelectDTO>>> GetAvailableMembersForBookingDropDownListAsync(Guid sessionId, CancellationToken ct = default)
    {
        var bookingRepo = _unitOfWork.GetGenericRepository<Booking>();
        var bookingWithMemberSpecification = new BookingWithMemberSpecification(sessionId);
        var bookingsPerSession = await bookingRepo.ListAsync(bookingWithMemberSpecification, ct: ct);
        
        IEnumerable<Member>? availbleMembersToBookSession;
        var memberRepo = _unitOfWork.GetGenericRepository<Member>();
        if (bookingsPerSession is null || bookingsPerSession.Count == 0)
        {
            var membersWithActiveMembershipsSpecification = new MembersWithActiveMembershipsSpecification();
            availbleMembersToBookSession = await memberRepo.ListAsync(membersWithActiveMembershipsSpecification,ct: ct);
        }
        else
        {
            var nonAvailableMembersToBookSessionIds = bookingsPerSession.Select(b => b.MemberId);
            var membersAvailableToBookSessionSpecificaion = new MembersAvailableToBookSessionSpecificaion(nonAvailableMembersToBookSessionIds);
            availbleMembersToBookSession = await memberRepo.ListAsync(membersAvailableToBookSessionSpecificaion,ct: ct);
        }

        if(availbleMembersToBookSession is null || !availbleMembersToBookSession.Any())
        {
            _logger.LogWarning("No available members to book session {@SessionId}", sessionId);
            return Result<IEnumerable<MemberSelectDTO>>.Failure(BookingBusinessErrors.NoAvailableMembersToBookSession);
        }

        var memberSelectDTOs = _mapper.Map<IEnumerable<MemberSelectDTO>>(availbleMembersToBookSession);
        _logger.LogInformation("Available members to book session {@SessionId}: {@Members} ", sessionId, memberSelectDTOs);
        return Result<IEnumerable<MemberSelectDTO>>.Success(memberSelectDTOs);
    }

    public async Task<Result> CreateBookingOnUpcomingSessionAsync(Guid sessionId, Guid memberId, CancellationToken ct = default)
    {
        var memberRepo = _unitOfWork.GetGenericRepository<Member>();

        var memberToBookSession = await memberRepo.GetByIdAsync(memberId, ct:ct);

        if(memberToBookSession is null)
        {
            _logger.LogWarning("Member ({@MemberId}) is currently not found to book the session", memberId);
            return Result.Failure(BookingBusinessErrors.MemberToBookSessionNotFound);
        }

        var memberHasActiveMembership = new MemberHasActiveMembership(memberId);

        if (!await memberRepo.AnyAsync(memberHasActiveMembership,ct))
        {
            _logger.LogWarning("Member ({@MemberId}) has no active membership", memberId);
            return Result.Failure(BookingBusinessErrors.MemberToBookSessionWithNoActiveMembership);
        }

        var bookingRepo = _unitOfWork.GetGenericRepository<Booking>();
        var bookingWithMemberSpecification = new BookingWithMemberSpecification(sessionId);
        var bookingsPerSession = await bookingRepo.ListAsync(bookingWithMemberSpecification, ct: ct);
        var nonAvailableMembersToBookSessionIds = bookingsPerSession.Select(b => b.MemberId);

        if(nonAvailableMembersToBookSessionIds.Contains(memberId))
        {
            _logger.LogWarning("Member ({@MemberId}) has already booked the session", memberId);
            return Result.Failure(BookingBusinessErrors.MemberAlreadyBookedSession);
        }

        var sessionRepo = _unitOfWork.SessionRepository;

        var sessionToBeBooked = await sessionRepo.GetByIdAsync(sessionId);

        if(sessionToBeBooked is null)
        {
            _logger.LogWarning("Session ({@SessionId}) is not found", sessionId);
            return Result.Failure(BookingBusinessErrors.SessionToBeBookedNotFound);
        }

        var bookingsNumberOnSessionSpecification = new BookingsNumberOnSessionSpecification(sessionId);
        var numberOfBookingsOnSession = await bookingRepo.CountAsync(bookingsNumberOnSessionSpecification, ct);

        if(numberOfBookingsOnSession >= sessionToBeBooked.Capacity)
        {
            _logger.LogWarning("Session ({@SessionId}) is fully booked", sessionId);
            return Result.Failure(BookingBusinessErrors.SessionFullyBooked);
        }

        var newBooking = new Booking()
        {
            MemberId = memberId,
            SessionId = sessionId
        };

        bookingRepo.Add(newBooking);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);
        if (numOfRowsAffected == 0)
        {
            _logger.LogError("Booking for member {@MemberId} is not created on session {@SessionId} due to an internal DB error",memberId,sessionId);
            return Result.Failure(BookingBusinessErrors.BookingNotCreated);
        }

        _logger.LogInformation("Booking for member {@MemberId} is created on session {@SessionId} successfully with Id:{@Id}",memberId,sessionId,newBooking.Id);
        return Result.Success();
    }

    public async Task<Result<Guid>> CancelBookingOnUpcomingSessionAsync(Guid bookingId, CancellationToken ct = default)
    {
        var bookingRepo = _unitOfWork.GetGenericRepository<Booking>();
        var bookingToBeDeleted = await bookingRepo.GetByIdAsync(bookingId);

        if(bookingToBeDeleted is null)
        {
            _logger.LogWarning("Booking {@BookingId} to be deleted is not found",bookingId);
            return Result<Guid>.Failure(BookingBusinessErrors.BookingToBeCancelledNotFound);
        }

        var sessionId = bookingToBeDeleted.SessionId;

        bookingRepo.SoftDelete(bookingToBeDeleted);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);
        if (numOfRowsAffected == 0)
        {
            _logger.LogError("Booking {@BookingId} is canclled due to an internal DB error", bookingId);
            return Result<Guid>.Failure(BookingBusinessErrors.BookingNotCancelled);
        }

        _logger.LogInformation("Booking {@BookingId} is cancelled on session successfully", bookingId);
        return Result<Guid>.Success(sessionId);
    }

    public async Task<Result<Guid>> MarkMemberAsAttendedAsync(Guid bookingId, CancellationToken ct = default)
    {
        var bookingRepo = _unitOfWork.GetGenericRepository<Booking>();
        var booking = await bookingRepo.GetByIdAsync(bookingId);

        if (booking is null)
        {
            _logger.LogWarning("Booking {@BookingId} to update its member attendance is not found", bookingId);
            return Result<Guid>.Failure(BookingBusinessErrors.BookingToBeCancelledNotFound);
        }

        var sessionId = booking.SessionId;

        booking.IsAttended = true;
        bookingRepo.Update(booking);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);
        if (numOfRowsAffected == 0)
        {
            _logger.LogError("Booking {@BookingId} member attendance is not updated due to an internal DB error", bookingId);
            return Result<Guid>.Failure(BookingBusinessErrors.BookingMemberAttendanceNotCancelled);
        }

        _logger.LogInformation("Booking {@BookingId} member attendance is updated to attended on session successfully", bookingId);
        return Result<Guid>.Success(sessionId);
    }
}
