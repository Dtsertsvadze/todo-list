namespace Entities.DTOs.LogInDto;

public class LoginResponse
{
    public string? Token { get; set; }

    public string? RefreshToken { get; set; }

    public string? ErrorMessage { get; set; }

    public bool Success
    {
        get { return string.IsNullOrEmpty(this.ErrorMessage); }
    }
}
