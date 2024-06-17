namespace Entities.DTOs.TagsDtos;

public class TagResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}

public static class TagResponseExtensions
{
    public static TagResponse ToTagResponse(this TagEntity tagEntity)
    {
        return new TagResponse
        {
            Id = tagEntity.Id,
            Name = tagEntity.Name
        };
    }
}
