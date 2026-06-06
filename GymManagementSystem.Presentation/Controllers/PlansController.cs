using GymManagementSystem.BusinessLogic.Contracts.Services;
using GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.Presentation.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Presentation.Controllers;

public sealed class PlansController : Controller
{
    /* Fields */
    private readonly IPlanService _planService;

    /* Constructor */
    public PlansController(IPlanService service)
    {
        _planService = service;
    }

    /* Actions (Endpoints) */
    // GET BaseURL/Plans/Index
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var allPlansDTOs = await _planService.GetAllPlansAsync(ct);

        if (!allPlansDTOs.Any())
            return View();

        var allPlanViewModels = allPlansDTOs.Select(apd => new AllPlansViewModel
        {
            Id = apd.Id,
            Name = apd.Name,
            Price = apd.Price,
            Description = apd.Description,
            DurationDays = apd.DurationDays,
            Active = apd.Status
        });

        return View(allPlanViewModels);
    }

    // GET BaseURL/Plans/Details/{id}
    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken ct)
    {
        var planDetailsDTOResult = await _planService.GetPlanDetailsAsync(id, ct);

        if (planDetailsDTOResult.IsFailure)
        {
            TempData["FailureAlert"] = planDetailsDTOResult.Error!.Description;
            return RedirectToAction(nameof(Index));
        }

        var planDetailsViewModel = new PlanDetailsViewModel
        {
            Name = planDetailsDTOResult.Value.Name,
            Price = planDetailsDTOResult.Value.Price,
            Description = planDetailsDTOResult.Value.Description,
            DurationDays = planDetailsDTOResult.Value.DurationDays,
            Status = planDetailsDTOResult.Value.Status ? "Active" : "Inactive"
        };

        return View(planDetailsViewModel);
    }

    // GET BaseURL/Plans/Edit/{id}
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var planTobeEditedDTOResult = await _planService.GetPlanToBeEditedAsync(id);

        if (planTobeEditedDTOResult.IsFailure)
        {
            TempData["FailureAlert"] = planTobeEditedDTOResult.Error!.Description;
            return RedirectToAction(nameof(Index));
        }

        var planToBeEditedViewModel = new PlanToBeEditedViewModel()
        {
            Name = planTobeEditedDTOResult.Value.Name,
            Price = planTobeEditedDTOResult.Value.Price,
            Description = planTobeEditedDTOResult.Value.Description,
            DurationDays = planTobeEditedDTOResult.Value.DurationDays
        };

        return View(planToBeEditedViewModel);
    }

    // POST BaseURL/Plans/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] Guid id, PlanToBeEditedViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        var modelDTO = new PlanToBeEditedDTO()
        {
            Name = model.Name,
            Price = model.Price,
            Description = model.Description,
            DurationDays = model.DurationDays
        };

        var result = await _planService.UpdatePlanToBeEditedAsync(id, modelDTO, ct);

        if (result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "Plan is updated successfully !";
        }
        else
        {
            TempData["FailureAlert"] = result.Error!.Description;
        }

        return RedirectToAction(nameof(Index));
    }

    // POST BaseURL/Plans/Activate/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(Guid id)
    {
        var result = await _planService.ChangePlanStatusAsync(id);

        if (result.IsSuccessful)
        {
            TempData["SuccessAlert"] = "Plan status is changed successfully !";
        }
        else
        {
            TempData["FailureAlert"] = result.Error!.Description;
        }

        return RedirectToAction(nameof(Index));
    }
}