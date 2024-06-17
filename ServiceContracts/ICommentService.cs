using Entities.DTOs.CommentDtos;

namespace ServiceContracts;

public interface ICommentService
{
    public Task<CommentResponse> AddCommentAsync(Guid taskId, CommentAddRequest commentAddRequest);

    public Task<CommentResponse?> GetCommentByIdAsync(Guid id);

    public Task<List<CommentResponse>> GetCommentsByTaskIdAsync(Guid taskId);

    public Task<CommentResponse> UpdateCommentAsync(CommentUpdateRequest commentUpdateRequest);

    public Task<bool> DeleteCommentAsync(Guid id);
}
