using AutoMapper;
using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using GymManagementSystem.BusinessLogic.DTOs.AnalyticsDTOs;
using GymManagementSystem.Presentation.Models;
using GymManagementSystem.Presentation.ViewModels.AnalyticsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymManagementSystem.Presentation.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IAnalyticsService _analyticsService;
    private readonly IMapper _mapper;

    public HomeController(IAnalyticsService analyticsService, IMapper mapper)
    {
        _analyticsService = analyticsService;
        _mapper = mapper;
    }
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var analyticsDTOResult = await _analyticsService.GetHomeAnalyticsAsync(ct);

        return View(_mapper.Map<HomeAnalyticsViewModel>(analyticsDTOResult.Value));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
