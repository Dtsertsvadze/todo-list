namespace ServiceContracts;
using System.Security.Claims;
using Entities.Identity;
public interface IJwtService
{
    AuthenticationResponse CreateJwtToken(ApplicationUser user);

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
