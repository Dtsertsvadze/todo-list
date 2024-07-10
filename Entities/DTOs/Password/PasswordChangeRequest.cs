namespace Entities.DTOs.Password;
using System.ComponentModel.DataAnnotations;

public class PasswordChangeRequest
{
    [Required]
    public string? CurrentPassword { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string? NewPassword { get; set; }

    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string? ConfirmNewPassword { get; set; }
}
