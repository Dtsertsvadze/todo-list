namespace Entities.DTOs.RegisterDtos;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
public class RegisterRequest
{
    [Required(ErrorMessage = "Person name is required.")]
    public string? PersonName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [Remote(action: "IsEmailInUse", controller: "Account", ErrorMessage = "Email is already in use.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must be numeric.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password and confirm password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
