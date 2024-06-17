using Entities.DTOs.TagsDtos;

namespace ServiceContracts;

public interface ITagsService
{
    public Task<IEnumerable<TagResponse>> GetTagsAsync(Guid taskId);

    public Task<TagResponse> AddTagToTaskAsync(Guid taskId, TagAddRequest tagAddRequest);

    public Task<TagResponse> UpdateTagAsync(Guid tagId, TagUpdateRequest tagUpdateRequest);

    public Task<bool> DeleteTagAsync(Guid tagId);
}
