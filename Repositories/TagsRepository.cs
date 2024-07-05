namespace Repositories;
using Database;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

public class TagsRepository(ToDoListDbContext context)
    : ITagsRepository
{
    public async Task<IEnumerable<TagEntity>> GetTagsAsync(Guid taskId)
    {
        return (await context.TaskTags
            .Where(t => t.TaskId == taskId)
            .Select(ttm => ttm.Tag)
            .ToListAsync()) !;
    }

    public async Task<TagEntity> AddTagAsync(TagEntity tagEntity)
    {
        await context.Tags.AddAsync(tagEntity);

        await context.SaveChangesAsync();

        return tagEntity;
    }

    public async Task<TagEntity> UpdateTagAsync(TagEntity tagEntity)
    {
        var existingTag = await context.Tags.FindAsync(tagEntity.Id);

        if (existingTag == null)
        {
            return tagEntity;
        }

        existingTag.Name = tagEntity.Name;
        context.Tags.Update(existingTag);
        await context.SaveChangesAsync();
        return existingTag;
    }

    public async Task<bool> DeleteTagAsync(Guid tagId)
    {
        var matchingTag = await context.Tags.FindAsync(tagId);

        ArgumentNullException.ThrowIfNull(matchingTag, nameof(matchingTag));

        context.Tags.Remove(matchingTag);

        var result = await context.SaveChangesAsync();

        return result > 0;
    }
}
