using AutoMapper;
using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;
using GymManagementSystem.Presentation.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Presentation.Controllers;

[Authorize]
public sealed class TrainersController : Controller
{
    /* Fields */
    private readonly ITrainerService _trainerService;
    private readonly IMapper _mapper;

    /* Constructor */
    public TrainersController(ITrainerService service,IMapper mapper)
    {
        _trainerService = service;
        _mapper = mapper;
    }

    /* Actions (Endpoints) */
    // GET BaseURL/Trainers/Index
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var allTrainersDTOs = await _trainerService.GetAllTrainersAsync(ct);

        var allTrainersViewModels = _mapper.Map<IEnumerable<AllTrainersViewModel>>(allTrainersDTOs);

        return View(allTrainersViewModels);
    }

    // GET BaseURL/Trainers/Details
    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken ct)
    {
        var trainerDetailsDTOResult = await _trainerService.GetTrainerDetailsAsync(id, ct);

        if (trainerDetailsDTOResult.IsFailure)
        {
            TempData["FailureAlert"] = trainerDetailsDTOResult.Error!.Description;
            return RedirectToAction(nameof(Index));
        }

        var trainerDetailsViewModel = _mapper.Map<TrainerDetailsViewModel>(trainerDetailsDTOResult.Value);

        return View(trainerDetailsViewModel);
    }

    // GET BaseURL/Trainers/Create
    [HttpGet]
    public IActionResult Create()
        => View();

    // POST BaseURL/Trainers/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrainerCreateViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        var trainerCreateDTO = _mapper.Map<TrainerCreateDTO>(model);

        var result = await _trainerService.AddTrainerAsync(trainerCreateDTO, ct);

        if (result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "Trainer is created successfully !";
        }
        else
        {
            TempData["FailureAlert"] = result.Error!.Description;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET BaseURL/Trainers/Edit/{id}
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var trainerToBeEditedDTOResult = await _trainerService.GetTrainerToBeEditedAsync(id, ct);

        if (trainerToBeEditedDTOResult.IsFailure)
        {
            TempData["FailureAlert"] = trainerToBeEditedDTOResult.Error!.Description;
            return RedirectToAction(nameof(Index));
        }

        var trainerToBeEditedViewModel = _mapper.Map<TrainerToBeEditedViewModel>(trainerToBeEditedDTOResult.Value);

        return View(trainerToBeEditedViewModel);
    }

    // POST BaseURL/Trainers/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] Guid id, TrainerToBeEditedViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        var trainerToBeEditedDTO = _mapper.Map<TrainerToBeEditedDTO>(model);

        var result = await _trainerService.EditTrainerAsync(id, trainerToBeEditedDTO, ct);

        if (result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "Trainer is edited successfully !";
        }
        else
        {
            TempData["FailureAlert"] = result.Error!.Description;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET BaseURL/Trainers/Delete/{id}
    [HttpGet]
    public IActionResult Delete(Guid id)
        => View();

    // POST BaseURL/Trainers/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _trainerService.DeleteTrainerAsync(id, ct);

        if (result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "Trainer is deleted successfully !";
        }
        else
        {
            TempData["FailureAlert"] = result.Error!.Description;
        }

        return RedirectToAction(nameof(Index));
    }
}
