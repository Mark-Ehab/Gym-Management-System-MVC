using AutoMapper;
using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.BusinessLogic.DTOs.MemberShipDTOs;
using GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;
using GymManagementSystem.Presentation.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.Presentation.Controllers;

[Authorize]
public class MembershipsController : Controller
{
    /* Fields */
    private readonly IMembershipService _membershipService;
    private readonly IMapper _mapper;

    /* Constructor */
    public MembershipsController(IMembershipService membershipService,
        IMapper mapper)
    {
        _membershipService = membershipService;
        _mapper = mapper;
    }

    /* Actions (Endpoints) */

    // GET BaseUrl/Memberships/Index
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var allMembershipsDTOs = await _membershipService.GetAllMembershipsAsync(ct);

        var allMembershipsViewModels = _mapper.Map<IEnumerable<AllMembershipsViewModel>>(allMembershipsDTOs);

        return View(allMembershipsViewModels);
    }

    // GET BaseUrl/Memberships/Create
    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        var result = await ProvidePlansAndMembersDropDownListsValues(ct);

        if(result.IsFailure)
        {
            TempData["FailureAlert"] = result.Error!.Description;
            return RedirectToAction(nameof(Index));
        }

        return View();
    }

    // POST BaseUrl/Memberships/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMembershipViewModel createMembershipViewModel,CancellationToken ct)
    {
        Result plansAndMembersResult;

        if (!ModelState.IsValid)
        {
            plansAndMembersResult = await ProvidePlansAndMembersDropDownListsValues(ct);

            if (plansAndMembersResult.IsFailure)
            {
                TempData["FailureAlert"] = plansAndMembersResult.Error!.Description;
                return RedirectToAction(nameof(Index));
            }

            return View(createMembershipViewModel);
        }
        
        var createMembershipDTO = _mapper.Map<CreateMembershipDTO>(createMembershipViewModel);

        var result = await _membershipService.AddMembership(createMembershipDTO, ct);

        if (result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "Membership is created successfully !";
            return RedirectToAction(nameof(Index));
        }

        TempData["FailureAlert"] = result.Error!.Description;

        plansAndMembersResult = await ProvidePlansAndMembersDropDownListsValues(ct);

        if (plansAndMembersResult.IsFailure)
        {
            TempData["FailureAlert"] = plansAndMembersResult.Error!.Description;
            return RedirectToAction(nameof(Index));
        }

        return View(createMembershipViewModel);
    }

    // POST BaseUrl/Memberships/Cancel/{Id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel([FromForm] Guid Id,CancellationToken ct)
    {
        var result = await _membershipService.DeleteMembership(Id, ct);

        if (result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "Membership is cancelled successfully !";
            return RedirectToAction(nameof(Index));
        } 
        else
        {
            TempData["FailureAlert"] = result.Error!.Description;
            return RedirectToAction(nameof(Index));
        }
    }

    private async Task<Result> ProvidePlansAndMembersDropDownListsValues(CancellationToken ct)
    {
        var plansResult = await _membershipService.GetPlansForDropDownListAsync(ct);

        if(plansResult.IsFailure)
            return Result.Failure(plansResult.Error!);

        var membersResult = await _membershipService.GetMembersForDropDownListAsync(ct);

        if(membersResult.IsFailure)
            return Result.Failure(membersResult.Error!);
        
        ViewBag.Plans = new SelectList(plansResult.Value, nameof(PlanSelectDTO.Id), nameof(PlanSelectDTO.Name));
        ViewBag.Members = new SelectList(membersResult.Value, nameof(MemberSelectDTO.Id), nameof(MemberSelectDTO.Name));
        return Result.Success();
    }
}