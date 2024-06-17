namespace Entities.DTOs.TasksDtos;

public class TaskUpdateRequest
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public Guid ToDoListId { get; set; }

    public DateTime DueDateTime { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsComplete { get; set; }



    public TaskEntity ToTaskEntity()
    {
        return new TaskEntity()
        {
            Id = this.Id,
            Title = this.Title,
            Description = this.Description,
        };
    }
}
