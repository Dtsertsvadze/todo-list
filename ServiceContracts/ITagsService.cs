namespace ServiceContracts;
using Entities.DTOs.TagsDtos;

public interface ITagsService
{
    public Task<IEnumerable<TagResponse>> GetTagsAsync(Guid taskId);

    public Task<TagResponse> AddTagToTaskAsync(Guid taskId, TagAddRequest tagAddRequest);

    public Task<TagResponse> UpdateTagAsync(Guid tagId, TagUpdateRequest tagUpdateRequest);

    public Task<bool> DeleteTagAsync(Guid tagId);
}
