namespace Entities.DTOs.TodoListDtos;
using TasksDtos;

public class ToDoListResponse
{
    public Guid? Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool? IsComplete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public List<TaskResponse>? Tasks { get; set; }

    public Guid? UserId { get; set; }
}

public static class ToDoListExtensions
{
    public static ToDoListResponse? ToToDoListResponse(this ToDoListEntity toDoList)
    {
        return new ToDoListResponse
        {
            Id = toDoList.Id,
            Title = toDoList.Title,
            Description = toDoList.Description,
            IsComplete = toDoList.IsComplete,
            CreatedAt = toDoList.CreatedAt,
            UserId = toDoList.UserId,
            Tasks = toDoList.Tasks?.Select(t => t.ToTaskResponse()).ToList(),
        };
    }
}
