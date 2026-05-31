using GymManagementSystem.BusinessLogic.Contracts.Services;
using GymManagementSystem.BusinessLogic.DTOs.HealthRecordDTOs;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.DataAccess.Enums;
using GymManagementSystem.Presentation.ViewModels.HealthRecordViewModels;
using GymManagementSystem.Presentation.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Presentation.Controllers;
public sealed class MembersController : Controller
{
    /* Fields */
    private readonly IMemberService _memberService;

    /* Constructor */
    public MembersController(IMemberService service)
    {
        _memberService = service;
    }

    /* Actions (Endpoints) */

    // GET BaseURL/Members/Index
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var allMembersDTOs = await _memberService.GetAllMembersAsync(ct);

        if (!allMembersDTOs.Any())
            return View();

        var allMembersViewModels = allMembersDTOs.Select(amd => new AllMembersViewModel
        {
            Id = amd.Id,
            Name = amd.Name,
            Email = amd.Email,
            Gender = amd.Gender.ToString(),
            Phone = amd.Phone,
            Photo = amd.Photo
        });

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

        var memberDetailsViewModel = new MemberDetailsViewModel
        {
            Photo = memberDetailsDTOResult.Value.Photo,
            Name = memberDetailsDTOResult.Value.Name,
            Email = memberDetailsDTOResult.Value.Email,
            Phone = memberDetailsDTOResult.Value.Phone,
            PlanName = memberDetailsDTOResult.Value.PlanName,
            Gender = memberDetailsDTOResult.Value.Gender.ToString(),
            Address = memberDetailsDTOResult.Value.Address,
            DateOfBirth = memberDetailsDTOResult.Value.DateOfBirth.ToString(),
            MembershipEndDate = memberDetailsDTOResult.Value.MembershipEndDate.ToString(),
            MembershipStartDate = memberDetailsDTOResult.Value.MembershipStartDate.ToString()
        };

        return View(memberDetailsViewModel);
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

        var memberHealthRecordDetailsViewModel = new HealthRecordViewModel
        {
            Height = memberHealthRecordDetailsDTOResult.Value.Height,
            Weight = memberHealthRecordDetailsDTOResult.Value.Weight,
            BloodType = memberHealthRecordDetailsDTOResult.Value.BloodType switch
            {
                BloodType.APositive => "A+",
                BloodType.BPositive => "B+",
                BloodType.OPositive => "O+",
                BloodType.ABPositive => "AB+",
                BloodType.ANegative => "A-",
                BloodType.BNegative => "B-",
                BloodType.ONegative => "O-",
                _ => "AB-",
            },
            Note = memberHealthRecordDetailsDTOResult.Value.Note
        };

        return View(memberHealthRecordDetailsViewModel);
    }

    // GET BaseURL/Members/Create
    [HttpGet]
    public IActionResult Create()
        => View();

    // POST BaseURL/Members/Create
    [HttpPost]
    public async Task<IActionResult> Create(MemberCreateViewModel model , CancellationToken ct)
    {
        if(!ModelState.IsValid)
            return View(model);

        var memberCreateDTO = new MemberCreateDTO()
        {
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone,
            DateOfBirth = model.DateOfBirth,
            Gender = model.Gender,
            BuildingNumber = model.BuildingNumber,
            Street = model.Street,
            City = model.City,
            HealthRecord = new HealthRecordDTO()
            {
                BloodType = model.HealthRecord.BloodType switch
                {
                    "A+" => BloodType.APositive,
                    "B+" => BloodType.BPositive,
                    "O+" => BloodType.OPositive,
                    "AB+" => BloodType.ABPositive,
                    "A-" => BloodType.ANegative,
                    "B-" => BloodType.BNegative,
                    "O-" => BloodType.ONegative,
                    _ => BloodType.ABNegative,
                },
                Height = model.HealthRecord.Height,
                Weight = model.HealthRecord.Weight,
                Note = model.HealthRecord?.Note ?? "No Notes."
            }
        };

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

        var memberToBeEditedViewModel = new MemberToBeEditedViewModel()
        {
            Photo = memberToBeEditedDTOResult.Value.Photo,
            Name = memberToBeEditedDTOResult.Value.Name,
            Email = memberToBeEditedDTOResult.Value.Email,
            Phone = memberToBeEditedDTOResult.Value.Phone,
            BuildingNumber = memberToBeEditedDTOResult.Value.BuildingNumber,
            Street = memberToBeEditedDTOResult.Value.Street,
            City = memberToBeEditedDTOResult.Value.City
        };

        return View(memberToBeEditedViewModel);
    }

    // POST BaseURL/Members/Edit/{id}
    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, MemberToBeEditedViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        var memberToBeEditedDTO = new MemberToBeEditedDTO()
        {
            Photo = model.Photo,
            Name= model.Name,
            Email= model.Email,
            BuildingNumber= model.BuildingNumber,
            City = model.City,
            Phone = model.Phone,
            Street = model.Street
        };

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
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
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
