namespace Services;
using Database;
using Entities;
using Entities.DTOs.TagsDtos;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;

public class TagsService(ToDoListDbContext dbContext)
    : ITagsService
{
    public async Task<IEnumerable<TagResponse>> GetTagsAsync(Guid taskId)
    {
        var tags = await dbContext.TaskTags
            .Where(ttm => ttm.TaskId == taskId)
            .Select(ttm => new TagResponse
            {
                Id = ttm.Tag !.Id,
                Name = ttm.Tag.Name,
            })
            .ToListAsync();

        return tags;
    }

    public async Task<TagResponse> AddTagToTaskAsync(Guid taskId, TagAddRequest tagAddRequest)
    {
        var task = await dbContext.Tasks.FindAsync(taskId);
        if (task == null)
        {
            return null !;
        }

        var tag = new TagEntity
        {
            Name = tagAddRequest.Name,
        };

        var result = await dbContext.Tags.AddAsync(tag);
        await dbContext.SaveChangesAsync();

        var taskTagMapping = new TaskTag
        {
            TaskId = taskId,
            TagId = result.Entity.Id,
        };

        dbContext.TaskTags.Add(taskTagMapping);
        await dbContext.SaveChangesAsync();

        return new TagResponse
        {
            Id = result.Entity.Id,
            Name = result.Entity.Name,
        };
    }

    public async Task<TagResponse> UpdateTagAsync(Guid tagId, TagUpdateRequest tagUpdateRequest)
    {
        var existingTag = await dbContext.Tags.FindAsync(tagId);
        if (existingTag == null)
        {
            return null!;
        }

        existingTag.Name = tagUpdateRequest.Name;
        dbContext.Tags.Update(existingTag);
        await dbContext.SaveChangesAsync();

        return new TagResponse
        {
            Id = existingTag.Id,
            Name = existingTag.Name,
        };
    }

    public async Task<bool> DeleteTagAsync(Guid tagId)
    {
        var tag = await dbContext.Tags.FindAsync(tagId);
        if (tag == null)
        {
            return false;
        }

        dbContext.Tags.Remove(tag);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
