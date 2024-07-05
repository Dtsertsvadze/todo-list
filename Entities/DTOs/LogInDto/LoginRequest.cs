namespace Entities.DTOs.LogInDto;
using System.ComponentModel.DataAnnotations;
public class LoginRequest
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; set; }
}
