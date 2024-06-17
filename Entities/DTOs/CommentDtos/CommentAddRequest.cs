using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs.CommentDtos;

public class CommentAddRequest
{
    [Required(ErrorMessage = "Content is required.")]
    [StringLength(500, ErrorMessage = "Content cannot be longer than 500 characters.")]
    public string? Content { get; set; }

    [Required(ErrorMessage = "TaskId is required.")]
    public Guid TaskId { get; set; }

    public CommentEntity ToCommentEntity()
    {
        return new CommentEntity()
        {
            Content = this.Content,
            TaskId = this.TaskId,
        };
    }
}
