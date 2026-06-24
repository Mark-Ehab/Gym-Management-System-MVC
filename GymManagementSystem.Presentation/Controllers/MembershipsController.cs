using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Presentation.Controllers;

[Authorize]
public class MembershipsController : Controller
{
    /* Fields */
    private readonly IMembershipService _membershipService;

    /* Constructor */
    public MembershipsController(IMembershipService membershipService)
    {
        _membershipService = membershipService;
    }

    /* Actions (Endpoints) */

    // GET BaseUrl/Memberships/Index
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
