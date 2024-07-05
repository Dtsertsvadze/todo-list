namespace Entities.DTOs.CommentDtos;
using System.ComponentModel.DataAnnotations;
public class CommentUpdateRequest
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Content is required.")]
    [StringLength(500, ErrorMessage = "Content cannot be longer than 500 characters.")]
    public string? Content { get; set; }

    public CommentEntity ToCommentEntity()
    {
        return new CommentEntity()
        {
            Id = this.Id,
            Content = this.Content,
        };
    }
}
