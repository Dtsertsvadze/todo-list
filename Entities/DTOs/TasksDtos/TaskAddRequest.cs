namespace Entities.DTOs.TasksDtos;
using System.ComponentModel.DataAnnotations;
using TagsDtos;
public class TaskAddRequest
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(30, ErrorMessage = "Title cannot be longer than 30 characters.")]
    public string? Title { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public string? Description { get; set; }

    public DateTime DueDateTime { get; set; }

    public bool IsComplete { get; set; }

    public Guid ToDoListId { get; set; }

    public List<TagResponse> Tags { get; set; } = new ();

    public TaskEntity ToTaskEntity()
    {
        return new TaskEntity()
        {
            Title = this.Title,
            Description = this.Description,
            DueDateTime = this.DueDateTime,
            IsComplete = this.IsComplete,
            CreatedAt = DateTime.UtcNow,
            ToDoListId = this.ToDoListId,
            TaskTags = this.Tags.Select(t => new TaskTag { TagId = t.Id }).ToList(),
        };
    }
}
