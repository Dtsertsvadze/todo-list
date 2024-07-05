namespace WebAPI.Controllers;
using System.Security.Claims;
using Entities.DTOs.LogInDto;
using Entities.DTOs.RegisterDtos;
using Entities.DTOs.TokensDto;
using Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtService jwtService)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        this._jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterRequest registerRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            return BadRequest(new { message = errorMessage });
        }

        ApplicationUser user = new ApplicationUser()
        {
            Email = registerRequest.Email,
            PhoneNumber = registerRequest.PhoneNumber,
            UserName = registerRequest.Email,
            PersonName = registerRequest.PersonName,
        };

        IdentityResult result = await this._userManager.CreateAsync(user, registerRequest.Password);

        if (result.Succeeded)
        {
            await this._signInManager.SignInAsync(user, isPersistent: false);

            var authenticationResponse = this._jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpiration = authenticationResponse.RefreshTokenExpiration;
            await this._userManager.UpdateAsync(user);

            return Ok(authenticationResponse);
        }
        else
        {
            string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
            return Problem(errorMessage);
        }
    }

    [HttpGet]
    public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
    {
        ApplicationUser? user = await this._userManager.FindByEmailAsync(email);

        return Ok(user == null);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            return Problem(errorMessage);
        }

        var result = await this._signInManager.PasswordSignInAsync(loginRequest.Email!, loginRequest.Password!, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            ApplicationUser? user = await this._userManager.FindByEmailAsync(loginRequest.Email!);
            if (user == null)
            {
                return NoContent();
            }

            var authenticationResponse = this._jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpiration = authenticationResponse.RefreshTokenExpiration;
            await this._userManager.UpdateAsync(user);

            return Ok(authenticationResponse);
        }
        else
        {
            return Problem("Invalid email or password");
        }
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken(TokenModel? tokenModel)
    {
        if (tokenModel is null)
        {
            return BadRequest();
        }

        string? accessToken = tokenModel.Token;
        string? refreshToken = tokenModel.RefreshToken;

        ArgumentNullException.ThrowIfNull(accessToken, nameof(accessToken));

        ClaimsPrincipal? principal = this._jwtService.GetPrincipalFromExpiredToken(accessToken);

        if (principal is null)
        {
            return BadRequest();
        }

        string? email = principal.FindFirstValue(ClaimTypes.Email);

        ApplicationUser? user = await this._userManager.FindByEmailAsync(email!);

        if (user is null || user.RefreshToken != tokenModel.RefreshToken || user.RefreshTokenExpiration <= DateTime.UtcNow)
        {
            return BadRequest();
        }

        var authenticationResponse = this._jwtService.CreateJwtToken(user);

        user.RefreshToken = authenticationResponse.RefreshToken;
        user.RefreshTokenExpiration = authenticationResponse.RefreshTokenExpiration;
        await this._userManager.UpdateAsync(user);

        return Ok(authenticationResponse);
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await this._signInManager.SignOutAsync();
        return NoContent();
    }
}
