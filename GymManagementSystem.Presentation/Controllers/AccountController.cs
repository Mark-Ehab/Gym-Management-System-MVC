using GymManagementSystem.DataAccess.Models.IdentityModels;
using GymManagementSystem.Presentation.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Presentation.Controllers;

public class AccountController : Controller
{
    /* Fields */
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    /* Constructor */
    public AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    /* Actions (Endpoints) */

    // GET BaseUrl/Account/Login
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated ?? false)
            return RedirectToAction(nameof(HomeController.Index), "Home");

        return View(new LoginViewModel { ReturnUrl = returnUrl});
    }

    // POST BaseUrl/Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if(!ModelState.IsValid)
         return View(loginViewModel);

        var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

        if(user is null)
        {
            ModelState.AddModelError("InvalidLogin","User is not found !");
            return View(loginViewModel);
        }

        var signInResult = await _signInManager.PasswordSignInAsync(user,loginViewModel.Password,true,true);

        if(!signInResult.Succeeded)
        {
            ModelState.AddModelError("InvalidLogin", "Email or Password is invalid !");
            return View(loginViewModel);
        }

        if(signInResult.IsLockedOut)
        {
            ModelState.AddModelError("InvalidLogin", "Please, re-try login after 15 mins !");
            return View(loginViewModel);
        }

        if(!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl) && Url.IsLocalUrl(loginViewModel.ReturnUrl))
        {
            return Redirect(loginViewModel.ReturnUrl);
        }

        return RedirectToAction(nameof(HomeController.Index),"Home");
    }

    // POST BaseUrl/Account/Logout
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction(nameof(Login));
    }

    // GET BaseUrl/Account/AccessDenied
    [HttpGet]
    public async Task<IActionResult> AccessDenied()
        => View();
}
