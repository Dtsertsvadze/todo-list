namespace Entities.Identity;

public class AuthenticationResponse
{
    public string? PersonName { get; set; } = string.Empty;

    public string? Token { get; init; } = string.Empty;

    public string? Email { get; set; } = string.Empty;

    public DateTime Expiration { get; set; }

    public string? RefreshToken { get; init; } = string.Empty;

    public DateTime RefreshTokenExpiration { get; init; }
}
