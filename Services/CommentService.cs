using Entities.DTOs.CommentDtos;
using RepositoryContracts;
using ServiceContracts;

namespace Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;

    public CommentService(ICommentRepository commentRepository)
    {
        this._commentRepository = commentRepository;
    }

    public async Task<CommentResponse> AddCommentAsync(Guid taskId, CommentAddRequest commentAddRequest)
    {
        var commentEntity = commentAddRequest.ToCommentEntity();

        commentEntity.Id = Guid.NewGuid();
        commentEntity.TaskId = taskId;

        var addedComment = await this._commentRepository.AddCommentAsync(taskId, commentEntity);

        return addedComment.ToCommentResponse();
    }

    public async Task<CommentResponse?> GetCommentByIdAsync(Guid id)
    {
        var comment = await this._commentRepository.GetCommentByIdAsync(id);

        return comment?.ToCommentResponse();
    }

    public async Task<List<CommentResponse>> GetCommentsByTaskIdAsync(Guid taskId)
    {
        var comments = await this._commentRepository.GetCommentsByTaskIdAsync(taskId);

        return comments.Select(c => c.ToCommentResponse()).ToList();
    }

    public async Task<CommentResponse> UpdateCommentAsync(CommentUpdateRequest commentUpdateRequest)
    {
        var existingComment = await this.GetCommentByIdAsync(commentUpdateRequest.Id);

        ArgumentNullException.ThrowIfNull(existingComment, "Comment not found.");

        var commentEntity = commentUpdateRequest.ToCommentEntity();

        var updatedComment = await this._commentRepository.UpdateCommentAsync(commentEntity);

        return updatedComment.ToCommentResponse();
    }

    public async Task<bool> DeleteCommentAsync(Guid id)
    {
        var deletedComment = await this._commentRepository.DeleteCommentAsync(id);

        return deletedComment;
    }
}
