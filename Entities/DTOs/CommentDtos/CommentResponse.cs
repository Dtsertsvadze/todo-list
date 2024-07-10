namespace Entities.DTOs.CommentDtos;

public class CommentResponse
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public Guid TaskId { get; set; }

    public DateTime CreatedAt { get; set; }
}

public static class CommentResponseExtensions
{
    public static CommentResponse ToCommentResponse(this CommentEntity comment)
    {
        return new CommentResponse()
        {
            Id = comment.Id,
            Content = comment.Content,
            TaskId = comment.TaskId,
            CreatedAt = comment.CreatedAt,
        };
    }
}
