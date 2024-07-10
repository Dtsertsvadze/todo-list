namespace WebApp.Controllers;
using Entities.DTOs.Password;
using Entities.DTOs.LogInDto;
using Entities.DTOs.RegisterDtos;
using Microsoft.AspNetCore.Mvc;
using Services.WebApi;
public class AccountController : Controller
{
    private readonly AccountWebApiService _accountWebApiService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(AccountWebApiService accountWebApiService, ILogger<AccountController> logger)
    {
        this._logger = logger;
        this._accountWebApiService = accountWebApiService;
    }

    public async Task<IActionResult> Index()
    {
        var token = Request.Cookies["jwtToken"];
        var profile = await this._accountWebApiService.GetUserProfileAsync(token !);
        return View(profile);
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(PasswordChangeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            var token = Request.Cookies["jwtToken"];

            await this._accountWebApiService.ChangePasswordAsync(request, token !);
            TempData["SuccessMessage"] = "Password changed successfully";
            return RedirectToAction(nameof(this.Index));
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "Failed to change password. Please try again.");
            return View(request);
        }
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterRequest model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await this._accountWebApiService.RegisterAsync(model);
            return RedirectToAction(nameof(TodoListController.Index), "TodoList");
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "Error during registration.");
            ModelState.AddModelError(string.Empty, "An error occurred during registration.");
            return View(model);
        }
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return View(loginRequest);
        }

        try
        {
            var authResponse = await this._accountWebApiService.LoginAsync(loginRequest);
            if (authResponse != null && !string.IsNullOrEmpty(authResponse.Token))
            {
                HttpContext.Response.Cookies.Append("jwtToken", authResponse.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                });

                return RedirectToAction(nameof(TodoListController.Index), "TodoList");
            }
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "Error during login.");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(loginRequest);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var response = await this._accountWebApiService.LogoutAsync();
            HttpContext.Response.Cookies.Delete("jwtToken");
            return response ? RedirectToAction(nameof(Login)) : RedirectToAction(nameof(TodoListController.Index), "TodoList");
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "Error during logout.");
            return RedirectToAction(nameof(Login));
        }
    }

    [HttpGet("is-email-in-use")]
    public async Task<IActionResult> IsEmailInUse(string email)
    {
        try
        {
            var isEmailInUse = await this._accountWebApiService.IsEmailAlreadyRegisteredAsync(email);
            return Json(isEmailInUse);
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "Error checking if email is in use.");
            return Json(false);
        }
    }
}
