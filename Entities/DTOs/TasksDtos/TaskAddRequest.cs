using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DTOs.TasksDtos;

public class TaskAddRequest
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(30, ErrorMessage = "Title cannot be longer than 30 characters.")]
    public string? Title { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public string? Description { get; set; }

    public DateTime DueDateTime { get; set; }

    public bool IsComplete { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid ToDoListId { get; set; }

    public TaskEntity ToTaskEntity()
    {
        return new TaskEntity()
        {
            Title = this.Title,
            Description = this.Description,
            DueDateTime = this.DueDateTime,
            IsComplete = this.IsComplete,
            CreatedAt = this.CreatedAt,
            ToDoListId = this.ToDoListId,
        };
    }
}
