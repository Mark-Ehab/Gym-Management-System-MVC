using AutoMapper;
using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using GymManagementSystem.BusinessLogic.DTOs.PlanDTOs;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.Presentation.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Presentation.Controllers;

[Authorize]
public sealed class PlansController : Controller
{
    /* Fields */
    private readonly IPlanService _planService;
    private readonly IMapper _mapper;

    /* Constructor */
    public PlansController(IPlanService service, IMapper mapper)
    {
        _planService = service;
        _mapper = mapper;
    }

    /* Actions (Endpoints) */
    // GET BaseURL/Plans/Index
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var allPlansDTOs = await _planService.GetAllPlansAsync(ct);

        if (!allPlansDTOs.Any())
            return View();

        var allPlanViewModels = _mapper.Map<IEnumerable<AllPlansViewModel>>(allPlansDTOs);

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

        var planDetailsViewModel = _mapper.Map<PlanDetailsViewModel>(planDetailsDTOResult.Value);

        return View(planDetailsViewModel);
    }

    // GET BaseURL/Plans/Edit/{id}
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var planTobeEditedDTOResult = await _planService.GetPlanToBeEditedAsync(id,ct);

        if (planTobeEditedDTOResult.IsFailure)
        {
            TempData["FailureAlert"] = planTobeEditedDTOResult.Error!.Description;
            return RedirectToAction(nameof(Index));
        }

        var planToBeEditedViewModel = _mapper.Map<PlanToBeEditedViewModel>(planTobeEditedDTOResult.Value);

        return View(planToBeEditedViewModel);
    }

    // POST BaseURL/Plans/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] Guid id, PlanToBeEditedViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        var modelDTO = _mapper.Map<PlanToBeEditedDTO>(model); 

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
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        var result = await _planService.ChangePlanStatusAsync(id,ct);

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