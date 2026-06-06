using GymManagementSystem.BusinessLogic.Contracts.Services;
using GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;
using GymManagementSystem.BusinessLogic.Results;
using GymManagementSystem.Presentation.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Presentation.Controllers;

public sealed class TrainersController : Controller
{
    /* Fields */
    private readonly ITrainerService _trainerService;

    /* Constructor */
    public TrainersController(ITrainerService service)
    {
        _trainerService = service;
    }

    /* Actions (Endpoints) */
    // GET BaseURL/Trainers/Index
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var allTrainersDTOs = await _trainerService.GetAllTrainersAsync(ct);

        if (allTrainersDTOs is null)
            return View();

        var allTrainersViewModels = allTrainersDTOs.Select(atd => new AllTrainersViewModel()
        {
            Id = atd.Id,
            Name = atd.Name,
            Email = atd.Email,
            Phone = atd.Phone,
            Specialization = atd.Speciality.ToString()
        });

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

        var trainerDetailsViewModel = new TrainerDetailsViewModel()
        {
            Name = trainerDetailsDTOResult.Value.Name,
            Email = trainerDetailsDTOResult.Value.Email,
            Phone = trainerDetailsDTOResult.Value.Phone,
            DateOfBirth = trainerDetailsDTOResult.Value.DateOBirth.ToString(),
            Specialization = trainerDetailsDTOResult.Value.Speciality.ToString(),
            Address = string.Join(" - ", trainerDetailsDTOResult.Value.BuildingNumber, trainerDetailsDTOResult.Value.Street, trainerDetailsDTOResult.Value.City)
        };

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

        var trainerCreateDTO = new TrainerCreateDTO()
        {
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone,
            Gender = model.Gender,
            DateOfBirth = model.DateOfBirth,
            Speciality = model.Specialties,
            BuildingNumber = model.BuildingNumber,
            City = model.City,
            Street = model.Street
        };

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

        var trainerToBeEditedViewModel = new TrainerToBeEditedViewModel()
        {
            Name = trainerToBeEditedDTOResult.Value.Name,
            Email = trainerToBeEditedDTOResult.Value.Email,
            Phone = trainerToBeEditedDTOResult.Value.Phone,
            Specialties = trainerToBeEditedDTOResult.Value.Speciality,
            BuildingNumber = trainerToBeEditedDTOResult.Value.BuildingNumber,
            Street = trainerToBeEditedDTOResult.Value.Street,
            City = trainerToBeEditedDTOResult.Value.City
        };

        return View(trainerToBeEditedViewModel);
    }

    // POST BaseURL/Trainers/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] Guid id, TrainerToBeEditedViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        var trainerToBeEditedDTO = new TrainerToBeEditedDTO()
        {
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone,
            Speciality = model.Specialties,
            BuildingNumber = model.BuildingNumber,
            Street = model.Street,
            City = model.City,
        };

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
