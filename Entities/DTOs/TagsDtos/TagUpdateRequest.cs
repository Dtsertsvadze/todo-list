namespace Entities.DTOs.TagsDtos;
using System.ComponentModel.DataAnnotations;
public class TagUpdateRequest
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(30, ErrorMessage = "Name cannot be longer than 30 characters.")]
    public string? Name { get; set; }
}
