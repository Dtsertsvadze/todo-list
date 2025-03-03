namespace Entities;
using System.ComponentModel.DataAnnotations;
public class TagEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(30, ErrorMessage = "Name cannot be longer than 30 characters.")]
    public string? Name { get; set; }

    public ICollection<TaskTag>? TaskTags { get; set; }
}
