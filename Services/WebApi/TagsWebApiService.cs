namespace Services.WebApi;
using System.Net.Http.Json;
using Entities.DTOs.TagsDtos;
public class TagsWebApiService(HttpClient httpClient)
{
    public async Task<TagResponse?> AddTagToTask(Guid taskId, TagAddRequest tagAddRequest)
    {
        var response = await httpClient.PostAsJsonAsync($"/api/tags/{taskId}", tagAddRequest);

        response.EnsureSuccessStatusCode();

        if (response.Content.Headers.ContentType?.MediaType != "application/json" ||
            !(response.Content.Headers.ContentLength > 0))
        {
            return null;
        }

        var addedTag = await response.Content.ReadFromJsonAsync<TagResponse>();
        return addedTag;
    }

    public async Task<bool> DeleteTag(Guid tagId)
    {
        var response = await httpClient.DeleteAsync($"/api/tags/{tagId}");

        return response.IsSuccessStatusCode;
    }
}
