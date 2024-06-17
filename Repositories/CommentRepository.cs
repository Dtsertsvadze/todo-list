using Database;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories;

public class CommentRepository(ToDoListDbContext db) : ICommentRepository
{

    public async Task<CommentEntity> AddCommentAsync(Guid taskId, CommentEntity comment)
    {
        await db.Comments.AddAsync(comment);

        await db.SaveChangesAsync();

        return comment;
    }

    public async Task<CommentEntity?> GetCommentByIdAsync(Guid id)
    {
        var comment = await db.Comments.FindAsync(id);
        return comment;
    }

    public async Task<List<CommentEntity>> GetCommentsByTaskIdAsync(Guid taskId)
    {
        var comments = await db.Comments
            .Where(c => c.TaskId == taskId).ToListAsync();

        return comments;
    }

    public async Task<CommentEntity> UpdateCommentAsync(CommentEntity comment)
    {
        var matchingComment = await db.Comments.FirstOrDefaultAsync(com => com.Id == comment.Id);

        if (matchingComment == null)
        {
            throw new Exception("Comment not found.");
        }

        matchingComment.Content = comment.Content;

        await db.SaveChangesAsync();

        return matchingComment;
    }

    public async Task<bool> DeleteCommentAsync(Guid id)
    {
        var comment = await db.Comments.FindAsync(id);

        if (comment == null)
        {
            throw new Exception("Comment not found.");
        }

        db.Comments.Remove(comment);

        int deletedComment = await db.SaveChangesAsync();

        return deletedComment > 0;
    }
}
