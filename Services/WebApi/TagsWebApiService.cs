namespace Services.WebApi;
using System.Net.Http.Json;
using Entities.DTOs.TagsDtos;
public class TagsWebApiService(HttpClient httpClient)
{
    public async Task<List<TagResponse>?> GetTags(Guid taskId)
    {
        var response = await httpClient.GetAsync($"/api/tags/{taskId}/tags");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<TagResponse>>();
    }

    public async Task<TagResponse?> AddTagToTask(Guid taskId, TagAddRequest tagAddRequest)
    {
        var response = await httpClient.PostAsJsonAsync($"/api/tags/{taskId}", tagAddRequest);

        response.EnsureSuccessStatusCode();

        if (response.Content.Headers.ContentType?.MediaType == "application/json" && response.Content.Headers.ContentLength > 0)
        {
            var addedTag = await response.Content.ReadFromJsonAsync<TagResponse>();
            return addedTag;
        }
        else
        {
            return null;
        }
    }

    public async Task<TagResponse?> UpdateTag(Guid tagId, TagUpdateRequest tagUpdateRequest)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/tags/{tagId}", tagUpdateRequest);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TagResponse>();
    }

    public async Task DeleteTag(Guid tagId)
    {
        var response = await httpClient.DeleteAsync($"/api/tags/{tagId}");
        response.EnsureSuccessStatusCode();
    }
}
