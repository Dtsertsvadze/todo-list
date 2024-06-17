using Database;
using Entities;
using Entities.DTOs.TagsDtos;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;

namespace Services;

public class TagsService : ITagsService
{
    private readonly ToDoListDbContext _dbContext;

    public TagsService(ToDoListDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<IEnumerable<TagResponse>> GetTagsAsync(Guid taskId)
    {
        var tags = await this._dbContext.TaskTags
            .Where(ttm => ttm.TaskId == taskId)
            .Select(ttm => new TagResponse
            {
                Id = ttm.Tag.Id,
                Name = ttm.Tag.Name
            })
            .ToListAsync();

        return tags;
    }

    public async Task<TagResponse> AddTagToTaskAsync(Guid taskId, TagAddRequest tagAddRequest)
    {
        var task = await this._dbContext.Tasks.FindAsync(taskId);
        if (task == null)
        {
            return null;
        }

        var tag = new TagEntity
        {
            Name = tagAddRequest.Name
        };

        var result = await this._dbContext.Tags.AddAsync(tag);
        await this._dbContext.SaveChangesAsync();

        var taskTagMapping = new TaskTag
        {
            TaskId = taskId,
            TagId = result.Entity.Id
        };

        this._dbContext.TaskTags.Add(taskTagMapping);
        await this._dbContext.SaveChangesAsync();

        return new TagResponse
        {
            Id = result.Entity.Id,
            Name = result.Entity.Name
        };
    }

    public async Task<TagResponse> UpdateTagAsync(Guid tagId, TagUpdateRequest tagUpdateRequest)
    {
        var existingTag = await this._dbContext.Tags.FindAsync(tagId);
        if (existingTag == null)
        {
            return null;
        }

        existingTag.Name = tagUpdateRequest.Name;
        this._dbContext.Tags.Update(existingTag);
        await this._dbContext.SaveChangesAsync();

        return new TagResponse
        {
            Id = existingTag.Id,
            Name = existingTag.Name
        };
    }

    public async Task<bool> DeleteTagAsync(Guid tagId)
    {
        var tag = await this._dbContext.Tags.FindAsync(tagId);
        if (tag == null)
        {
            return false;
        }

        this._dbContext.Tags.Remove(tag);
        await this._dbContext.SaveChangesAsync();
        return true;
    }
}
