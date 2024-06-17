using Database;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories;

public class TagsRepository : ITagsRepository
{
    private readonly ToDoListDbContext _context;

    public TagsRepository(ToDoListDbContext context)
    {
        this._context = context;
    }

    public async Task<IEnumerable<TagEntity>> GetTagsAsync(Guid taskId)
    {
        return await this._context.TaskTags
            .Where(t => t.TaskId == taskId)
            .Select(ttm => ttm.Tag)
            .ToListAsync();
    }

    public async Task<TagEntity> AddTagAsync(TagEntity tagEntity)
    {
        await this._context.Tags.AddAsync(tagEntity);

        await this._context.SaveChangesAsync();

        return tagEntity;
    }

    public async Task<TagEntity> UpdateTagAsync(TagEntity tagEntity)
    {
        var existingTag = await this._context.Tags.FindAsync(tagEntity.Id);

        if (existingTag == null)
        {
            return tagEntity;
        }

        existingTag.Name = tagEntity.Name;
        this._context.Tags.Update(existingTag);
        await this._context.SaveChangesAsync();
        return existingTag;
    }

    public async Task<bool> DeleteTagAsync(Guid tagId)
    {
        var matchingTag = await this._context.Tags.FindAsync(tagId);

        ArgumentNullException.ThrowIfNull(matchingTag, nameof(matchingTag));

        this._context.Tags.Remove(matchingTag);

        var result = await this._context.SaveChangesAsync();

        return result > 0;
    }
}
