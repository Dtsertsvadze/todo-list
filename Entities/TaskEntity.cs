namespace Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TaskEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(30, ErrorMessage = "Title cannot be longer than 30 characters.")]
    public string? Title { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public string? Description { get; set; }

    public DateTime DueDateTime { get; set; }

    public bool IsComplete { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<CommentEntity>? Comments { get; set; }

    public ICollection<TaskTag>? TaskTags { get; set; }

    public Guid ToDoListId { get; set; }

    [ForeignKey("ToDoListId")]
    public ToDoListEntity? ToDoList { get; set; }
}
