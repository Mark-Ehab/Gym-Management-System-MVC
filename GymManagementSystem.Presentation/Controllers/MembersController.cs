using AutoMapper;
using GymManagementSystem.BusinessLogic.Contracts.AttachmentService;
using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using GymManagementSystem.BusinessLogic.DTOs.HealthRecordDTOs;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.DataAccess.Enums;
using GymManagementSystem.Presentation.ViewModels.HealthRecordViewModels;
using GymManagementSystem.Presentation.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Presentation.Controllers;

[Authorize(Roles = "SuperAdmin")]
public sealed class MembersController : Controller
{
    /* Fields */
    private readonly IMemberService _memberService;
    private readonly IMapper _mapper;
    private readonly IAttachmentService _attachmentService;

    /* Constructor */
    public MembersController(IMemberService service,
                             IMapper mapper,
                             IAttachmentService attachmentService)
    {
        _memberService = service;
        _mapper = mapper;
        _attachmentService = attachmentService;
    }

    /* Actions (Endpoints) */

    // GET BaseURL/Members/Index
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var allMembersDTOs = await _memberService.GetAllMembersAsync(ct);

        var allMembersViewModels = _mapper.Map<IEnumerable<AllMembersViewModel>>(allMembersDTOs);

        return View(allMembersViewModels);
    }

    // GET BaseURL/Members/Details/{id}
    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken ct)
    {
        var memberDetailsDTOResult = await _memberService.GetMemberDetailsAsync(id,ct);

        if (memberDetailsDTOResult.IsFailure)
        {
            TempData["FailureAlert"] = memberDetailsDTOResult.Error!.Description;
            return RedirectToAction(nameof(Index));
        }

        var memberDetailsViewModel = _mapper.Map<MemberDetailsViewModel>(memberDetailsDTOResult.Value);

        return View(memberDetailsViewModel);
    }

    // GET BaseURL/Members/ProfilePicture/{id}
    [HttpGet]
    public async Task<IActionResult> ProfilePicture(Guid id, CancellationToken ct)
    {
        var memberDetailsDTOResult = await _memberService.GetMemberDetailsAsync(id, ct);

        if (memberDetailsDTOResult.IsFailure)
        {
            TempData["FailureAlert"] = memberDetailsDTOResult.Error!.Description;
            return RedirectToAction(nameof(Index));
        }

        if(string.IsNullOrWhiteSpace(memberDetailsDTOResult.Value.Photo))
            return RedirectToAction(nameof(Index));

        var result = _attachmentService.GetFile(memberDetailsDTOResult.Value.Photo, "MemberImages");

        return File(result.Value.Item1, result.Value.Item2);
    }

    // GET BaseURL/Members/HealthRecordDetails/{id}
    [HttpGet]
    public async Task<IActionResult> HealthRecordDetails(Guid id, CancellationToken ct)
    {
        var memberHealthRecordDetailsDTOResult = await _memberService.GetMemberHealthRecordDetailsAsync(id,ct);

        if (memberHealthRecordDetailsDTOResult.IsFailure)
        {
            TempData["FailureAlert"] = memberHealthRecordDetailsDTOResult.Error!.Description;
            return RedirectToAction(nameof(Index));
        }

        var memberHealthRecordDetailsViewModel = _mapper.Map<HealthRecordViewModel>(memberHealthRecordDetailsDTOResult.Value);

        return View(memberHealthRecordDetailsViewModel);
    }

    // GET BaseURL/Members/Create
    [HttpGet]
    public IActionResult Create()
        => View();

    // POST BaseURL/Members/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MemberCreateViewModel model , CancellationToken ct)
    {
        if(!ModelState.IsValid)
            return View(model);

        var memberCreateDTO = _mapper.Map<MemberCreateDTO>(model);

        var result = await _memberService.AddMemberAsync(memberCreateDTO,ct);

        if (result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "Member is created successfully !";
        }
        else
        {
            TempData["FailureAlert"] = result.Error!.Description;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET BaseURL/Members/Edit/{id}
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var memberToBeEditedDTOResult = await _memberService.GetMemberToBeEditedAsync(id,ct);

        if (memberToBeEditedDTOResult.IsFailure)
        {
            TempData["FailureAlert"] = memberToBeEditedDTOResult.Error!.Description;

            return RedirectToAction(nameof(Index));
        }

        var memberToBeEditedViewModel = _mapper.Map<MemberToBeEditedViewModel>(memberToBeEditedDTOResult.Value);

        return View(memberToBeEditedViewModel);
    }

    // POST BaseURL/Members/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] Guid id, MemberToBeEditedViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        var memberToBeEditedDTO = _mapper.Map<MemberToBeEditedDTO>(model);

        var result = await _memberService.EditMemberAsync(id, memberToBeEditedDTO, ct);

        if (result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "Member is edited successfully !";
        }
        else
        {
            TempData["FailureAlert"] = result.Error!.Description;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET BaseURL/Members/Delete/{id}
    [HttpGet]
    public IActionResult Delete(Guid id)
        => View();

    // POST BaseURL/Members/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _memberService.DeleteMemberAsync(id, ct);

        if (result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "Member is deleted successfully !";
        }
        else
        {
            TempData["FailureAlert"] = result.Error!.Description;
        }

        return RedirectToAction(nameof(Index));
    }
}
