namespace RepositoryContracts;
using Entities;
public interface ICommentRepository
{
    public Task<CommentEntity> AddCommentAsync(Guid taskId, CommentEntity comment);

    public Task<CommentEntity?> GetCommentByIdAsync(Guid id);

    public Task<List<CommentEntity>> GetCommentsByTaskIdAsync(Guid taskId);

    public Task<CommentEntity> UpdateCommentAsync(CommentEntity comment);

    public Task<bool> DeleteCommentAsync(Guid id);
}
