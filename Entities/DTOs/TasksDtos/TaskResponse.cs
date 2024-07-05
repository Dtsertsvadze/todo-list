namespace Entities.DTOs.TasksDtos;
using CommentDtos;
using TagsDtos;

public class TaskResponse
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime DueDateTime { get; set; }

    public bool IsComplete { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid ToDoListId { get; set; }

    public List<CommentResponse>? Comments { get; set; }

    public List<TagResponse>? Tags { get; set; }
}

public static class TaskExtensions
{
    public static TaskResponse ToTaskResponse(this TaskEntity task)
    {
        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDateTime = task.DueDateTime,
            IsComplete = task.IsComplete,
            CreatedAt = task.CreatedAt,
            ToDoListId = task.ToDoListId,
            Comments = task.Comments?.Select(t => t.ToCommentResponse()).ToList(),
            Tags = task.TaskTags?.Select(t => t.Tag !.ToTagResponse()).ToList(),
        };
    }
}
