using Entities;

namespace RepositoryContracts;

public interface ITagsRepository
{
    public Task<IEnumerable<TagEntity>> GetTagsAsync(Guid taskId);

    public Task<TagEntity> AddTagAsync(TagEntity tagEntity);

    public Task<TagEntity> UpdateTagAsync(TagEntity tagEntity);

    public Task<bool> DeleteTagAsync(Guid tagId);

}
