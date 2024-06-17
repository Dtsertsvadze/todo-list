using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs.TagsDtos;

public class TagUpdateRequest
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(30, ErrorMessage = "Name cannot be longer than 30 characters.")]
    public string Name { get; set; }
}
