namespace Entities.DTOs.TagsDtos;

public class TagResponse
{
    public Guid Id { get; init; }

    public string? Name { get; init; }
}

public static class TagResponseExtensions
{
    public static TagResponse ToTagResponse(this TagEntity tagEntity)
    {
        return new TagResponse
        {
            Id = tagEntity.Id,
            Name = tagEntity.Name,
        };
    }
}
