using AutoMapper;
using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using GymManagementSystem.BusinessLogic.DTOs.CategoryDTOs;
using GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;
using GymManagementSystem.BusinessLogic.DTOs.SessionDTOs;
using GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;
using GymManagementSystem.Presentation.ViewModels.SessionViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.Presentation.Controllers;

[Authorize]
public class SessionsController : Controller
{
    /* Fields */
    private readonly ISessionService _sessionService;
    private readonly IMapper _mapper;

    /* Constructor */
    public SessionsController(ISessionService sessionService, IMapper mapper)
    {
        _sessionService = sessionService;
        _mapper = mapper;
    }

    /* Actions (Endpoints) */
    // GET BaseURL/Sessions/Index
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var allSessionDTOs = await _sessionService.GetAllSessionsAsync(ct);

        var allSessionViewModels = _mapper.Map<IEnumerable<AllSessionsViewModel>>(allSessionDTOs);

        return View(allSessionViewModels);
    }

    // GET BaseURL/Sessions/Create
    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken ct)
    {

        var categoriesResult = await ProvideCategoryListValuesAsync(ct);

        if(categoriesResult.IsFailure)
        {
            TempData["FailureAlert"] = categoriesResult.Error!.Description;
            return RedirectToAction(nameof(Index));
        }

        var trainersResult = await ProvideTrainerDropDownListValuesAsync(ct);

        if (trainersResult.IsFailure)
        {
            TempData["FailureAlert"] = trainersResult.Error!.Description;
            return RedirectToAction(nameof(Index));
        }
       
        return View();
    }

    // POST BaseURL/Sessions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSessionViewModel createSessionViewModel, CancellationToken ct)
    {
        if(!ModelState.IsValid)
        {
            var categoriesResult = await ProvideCategoryListValuesAsync(ct);

            if (categoriesResult.IsFailure)
            {
                TempData["FailureAlert"] = categoriesResult.Error!.Description;
                return RedirectToAction(nameof(Index));
            }

            var trainersResult = await ProvideTrainerDropDownListValuesAsync(ct);

            if (trainersResult.IsFailure)
            {
                TempData["FailureAlert"] = trainersResult.Error!.Description;
                return RedirectToAction(nameof(Index));
            }

            return View(createSessionViewModel);
        }

        var createSessionDTO = _mapper.Map<CreateSessionDTO>(createSessionViewModel);

        var result = await _sessionService.AddSessionAsync(createSessionDTO, ct);

        if(result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "New session is created successfully on the system !";
        }
        else 
        {
            TempData["FailureAlert"] = result.Error!.Description;
            await ProvideCategoryListValuesAsync(ct);
            await ProvideTrainerDropDownListValuesAsync(ct);
            return View(createSessionViewModel);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET BaseURL/Sessions/Details/{id}
    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken ct)
    {
        var sessionDetailsDTOResult = await _sessionService.GetSessionDetailsAsync(id, ct);

        if(sessionDetailsDTOResult.IsFailure)
        {
            TempData["FailureAlert"] = sessionDetailsDTOResult.Error!.Description;
            return RedirectToAction(nameof(Index));
        }

        var sessionDetialsViewModel = _mapper.Map<SessionDetailsViewModel>(sessionDetailsDTOResult.Value);

        return View(sessionDetialsViewModel);
    }

    // GET BaseURL/Sessions/Edit/{id}
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var sessionToBeEditedDTOResult = await _sessionService.GetSessionToBeEditedAsync(id, ct);

        if(sessionToBeEditedDTOResult.IsFailure)
        {
            TempData["FailureAlert"] = sessionToBeEditedDTOResult.Error!.Description;
            return RedirectToAction(nameof(Index));
        }

        var trainersResult = await ProvideTrainerDropDownListValuesAsync(ct);

        if (trainersResult.IsFailure)
        {
            TempData["FailureAlert"] = trainersResult.Error!.Description;
            return RedirectToAction(nameof(Index));
        }

        var sessionToBeEditedViewModel = _mapper.Map<EditSessionViewModel>(sessionToBeEditedDTOResult.Value);

        return View(sessionToBeEditedViewModel);

    }

    // POST BaseURL/Sessions/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] Guid id, EditSessionViewModel editSessionViewModel, CancellationToken ct)
    {
        if(!ModelState.IsValid)
        {
            var trainersResult = await ProvideTrainerDropDownListValuesAsync(ct);

            if (trainersResult.IsFailure)
            {
                TempData["FailureAlert"] = trainersResult.Error!.Description;
                return RedirectToAction(nameof(Index));
            }

            return View(editSessionViewModel);
        }

        var editSessionDTO = _mapper.Map<EditSessionDTO>(editSessionViewModel);

        var result = await _sessionService.EditSessionAsync(id, editSessionDTO, ct);

        if(result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "Session is edited successfully !";
        }
        else if (result.Error!.Code == "Session.Edited.NotEdited" ||  result.Error!.Code == "Session.Edit.NotFound")
        {
            TempData["FailureAlert"] = result.Error!.Description;
        }
        else
        {
            TempData["FailureAlert"] = result.Error!.Description;
            await  ProvideTrainerDropDownListValuesAsync(ct);
            return View(editSessionViewModel);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET BaseURL/Sessions/Delete/{id}
    [HttpGet]
    public async Task<IActionResult> Delete([FromRoute]Guid id, CancellationToken ct)
    {
        var result = await _sessionService.CheckIfSessionToBeDeletedIsOngoingOrUpcomingAsync(id,ct);

        if (result.IsFailure)
        {
            TempData["FailureAlert"] = result.Error!.Description;
            return RedirectToAction(nameof(Index));
        }

        return View();
    }

    // POST BaseURL/Sessions/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed([FromRoute] Guid id, CancellationToken ct)
    {

        var result = await _sessionService.DeleteSessionAsync(id, ct);

        if (result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "Session is deleted successfully !";
        }
        else
        {
            TempData["FailureAlert"] = result.Error!.Description;
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<Result> ProvideCategoryListValuesAsync(CancellationToken ct = default)
    {
        var categoryDropDownListResult = await _sessionService.GetAllPossibleSessionCategoriesForDropDownListAsync(ct);

        if (categoryDropDownListResult.IsFailure)
            return Result.Failure(categoryDropDownListResult.Error!);

        ViewBag.Categories = new SelectList(categoryDropDownListResult.Value, nameof(CategorySelectDTO.Id), nameof(CategorySelectDTO.Name));
        return Result.Success();
    }
    private async Task<Result> ProvideTrainerDropDownListValuesAsync(CancellationToken ct = default)
    {
        var trainerDropDownListResult = await _sessionService.GetAllPossibleSessionTrainersForDropDownListAsync(ct);

        if (trainerDropDownListResult.IsFailure)
            return Result.Failure(trainerDropDownListResult.Error!);

        ViewBag.Trainers = new SelectList(trainerDropDownListResult.Value, nameof(TrainerSelectDTO.Id), nameof(TrainerSelectDTO.Name));
        return Result.Success();
    }
}
