namespace Entities.Identity;
using Microsoft.AspNetCore.Identity;
public class ApplicationUser : IdentityUser<Guid>
{
    public string? PersonName { get; init; }

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiration { get; set; }
}
