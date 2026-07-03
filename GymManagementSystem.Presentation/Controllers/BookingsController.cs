using AutoMapper;
using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;
using GymManagementSystem.BusinessLogic.Services.BusinessServices;
using GymManagementSystem.Presentation.ViewModels.BookingViewModels;
using GymManagementSystem.Presentation.ViewModels.SessionViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.Presentation.Controllers;

[Authorize]
public sealed class BookingsController : Controller
{
    /* Fields */
    private readonly IBookingService _bookingService;
    private readonly IMapper _mapper;

    /* Constructor */
    public BookingsController(IBookingService bookingService,IMapper mapper)
    {
        _bookingService = bookingService;
        _mapper = mapper;
    }

    /* Actions (Endpoints) */

    // GET BaseURL/Bookings/Index
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        var allSessionDTOs = await _bookingService.GetAllUpcomingAndOngoingSessionsAsync(ct);

        var allSessionViewModel = _mapper.Map<IEnumerable<AllSessionsViewModel>>(allSessionDTOs);

        return View(allSessionViewModel);
    }

    // GET BaseURL/Bookings/UpcomingSessions/Members/{Id}
    [HttpGet("Bookings/UpcomingSessions/Members/{Id}", Name = "upcomingSessionMembers")]
    public async Task<IActionResult> UpcomingSessionMembers(Guid id, CancellationToken ct = default)
    {
        var sessionMemberDTOs = await _bookingService.GetSessionMembersBookingsAsync(id,ct);

        var sessionMemberViewModels = _mapper.Map<IEnumerable<SessionMemberBookingViewModel>>(sessionMemberDTOs);
        
        return View(sessionMemberViewModels);
    }

    // GET BaseURL/Bookings/OngoingSessions/Members/{Id}
    [HttpGet("Bookings/OngoingSessions/Members/{Id}", Name = "ongoingSessionMembers")]
    public async Task<IActionResult> OngoingSessionMembers(Guid id, CancellationToken ct = default)
    {
        var sessionMemberDTOs = await _bookingService.GetSessionMembersBookingsAsync(id, ct);

        var sessionMemberViewModels = _mapper.Map<IEnumerable<SessionMemberBookingViewModel>>(sessionMemberDTOs);

        return View(sessionMemberViewModels);
    }

    // GET BaseURL/Bookings/UpcomingSessions/Create/{Id}
    [HttpGet("Bookings/UpcomingSessions/Create/{Id}")]
    public async Task<IActionResult> Create(Guid id, CancellationToken ct = default)
    {
        var result = await ProvideAvailableMembersToBookSessionDropDownListsValuesAsync(id, ct);

        if(result.IsFailure)
        {
            TempData["FailureAlert"] = result.Error!.Description;
            return RedirectToAction(nameof(UpcomingSessionMembers), new { Id = id });
        }

        return View();
    }

    // POST BaseURL/Bookings/UpcomingSessions/Create/{Id}
    [HttpPost("Bookings/UpcomingSessions/Create/{Id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromRoute] Guid id, CreateBookingViewModel createBookingViewModel, CancellationToken ct = default)
    {
        Result result;
        if(!ModelState.IsValid)
        {
            result = await ProvideAvailableMembersToBookSessionDropDownListsValuesAsync(id, ct);

            if (result.IsFailure)
            {
                TempData["FailureAlert"] = result.Error!.Description;
                return RedirectToAction(nameof(UpcomingSessionMembers), new { Id = id });
            }
            return View(createBookingViewModel);
        }

        result = await _bookingService.CreateBookingOnUpcomingSessionAsync(id,createBookingViewModel.MemberId, ct);

        if(result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "Booking is created successfully !";
        }
        else if(result.IsFailure && result.Error!.Code == "Booking.Create.SessionNotFound") 
        {
            TempData["FailureAlert"] = result.Error!.Description;
            return RedirectToAction(nameof(Index));
        }
        else 
        {
            TempData["FailureAlert"] = result.Error!.Description;
        }

        return RedirectToAction(nameof(UpcomingSessionMembers),new {Id = id});
    }

    // POST BaseURL/Bookings/UpcomingSessions/Cancel/{Id}
    [HttpPost("Bookings/UpcomingSessions/Cancel/{Id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel([FromRoute] Guid id, CreateBookingViewModel createBookingViewModel, CancellationToken ct = default)
    {
        var result = await _bookingService.CancelBookingOnUpcomingSessionAsync(id);

        if (result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "Booking is cancelled successfully !";
        }
        else
        {
            TempData["FailureAlert"] = result.Error!.Description;
        }

        return RedirectToAction(nameof(UpcomingSessionMembers),new {Id = result.Value});
    }

    // POST BaseURL/Bookings/UpcomingSessions/MarkAttendance/{Id}
    [HttpPost("Bookings/UpcomingSessions/MarkAttendance/{Id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAttendance([FromRoute] Guid id, CreateBookingViewModel createBookingViewModel, CancellationToken ct = default)
    {
        var result = await _bookingService.MarkMemberAsAttendedAsync(id);

        if (result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "Attendance is updated successfully !";
        }
        else
        {
            TempData["FailureAlert"] = result.Error!.Description;
        }

        return RedirectToAction(nameof(OngoingSessionMembers),new {Id = result.Value});
    }

    private async Task<Result> ProvideAvailableMembersToBookSessionDropDownListsValuesAsync(Guid sessionId,CancellationToken ct)
    {
        var availableMembersToBookSessionResult = await _bookingService.GetAvailableMembersForBookingDropDownListAsync(sessionId, ct: ct);

        if (availableMembersToBookSessionResult.IsFailure)
            return Result.Failure(availableMembersToBookSessionResult.Error!);

        ViewBag.Members = new SelectList(availableMembersToBookSessionResult.Value, nameof(MemberSelectDTO.Id), nameof(MemberSelectDTO.Name));
        return Result.Success();
    }
}
