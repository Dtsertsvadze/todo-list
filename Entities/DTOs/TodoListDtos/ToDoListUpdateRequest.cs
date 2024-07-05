namespace Entities.DTOs.TodoListDtos;
using System.ComponentModel.DataAnnotations;
public class ToDoListUpdateRequest
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(30, ErrorMessage = "Title cannot be longer than 30 characters.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public string? Description { get; set; }

    public bool? IsComplete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public ToDoListEntity ToToDoList()
    {
        return new ToDoListEntity()
        {
            Title = this.Title,
            Description = this.Description,
            IsComplete = this.IsComplete,
            CreatedAt = this.CreatedAt,
        };
    }
}
