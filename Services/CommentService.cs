namespace Services;
using Entities.DTOs.CommentDtos;
using RepositoryContracts;
using ServiceContracts;

public class CommentService(ICommentRepository commentRepository)
    : ICommentService
{
    public async Task<CommentResponse> AddCommentAsync(Guid taskId, CommentAddRequest commentAddRequest)
    {
        var commentEntity = commentAddRequest.ToCommentEntity();

        commentEntity.Id = Guid.NewGuid();
        commentEntity.TaskId = taskId;

        var addedComment = await commentRepository.AddCommentAsync(taskId, commentEntity);

        return addedComment.ToCommentResponse();
    }

    public async Task<CommentResponse?> GetCommentByIdAsync(Guid id)
    {
        var comment = await commentRepository.GetCommentByIdAsync(id);

        return comment?.ToCommentResponse();
    }

    public async Task<List<CommentResponse>> GetCommentsByTaskIdAsync(Guid taskId)
    {
        var comments = await commentRepository.GetCommentsByTaskIdAsync(taskId);

        return comments.Select(c => c.ToCommentResponse()).ToList();
    }

    public async Task<CommentResponse> UpdateCommentAsync(CommentUpdateRequest commentUpdateRequest)
    {
        var existingComment = await this.GetCommentByIdAsync(commentUpdateRequest.Id);

        ArgumentNullException.ThrowIfNull(existingComment, "Comment not found.");

        var commentEntity = commentUpdateRequest.ToCommentEntity();

        var updatedComment = await commentRepository.UpdateCommentAsync(commentEntity);

        return updatedComment.ToCommentResponse();
    }

    public async Task<bool> DeleteCommentAsync(Guid id)
    {
        var deletedComment = await commentRepository.DeleteCommentAsync(id);

        return deletedComment;
    }
}
