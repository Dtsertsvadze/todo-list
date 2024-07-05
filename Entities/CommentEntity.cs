namespace Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CommentEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Content is required.")]
    [StringLength(500, ErrorMessage = "Content cannot be longer than 500 characters.")]
    public string? Content { get; set; }

    public Guid TaskId { get; set; }

    [ForeignKey("TaskId")]
    public TaskEntity? Task { get; set; }
}
