using System.Net.Http.Json;
using Entities.DTOs.TagsDtos;

namespace Services.WebApi;

public class TagsWebApiService
{
    private readonly HttpClient _httpClient;

    public TagsWebApiService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    public async Task<List<TagResponse>?> GetTags(Guid taskId)
    {
        var response = await this._httpClient.GetAsync($"/api/tags/{taskId}/tags");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<TagResponse>>();
    }

    public async Task<TagResponse?> AddTagToTask(Guid taskId, TagAddRequest tagAddRequest)
    {
        var response = await this._httpClient.PostAsJsonAsync($"/api/tags/{taskId}", tagAddRequest);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TagResponse>();
    }

    public async Task<TagResponse?> UpdateTag(Guid tagId, TagUpdateRequest tagUpdateRequest)
    {
        var response = await this._httpClient.PutAsJsonAsync($"/api/tags/{tagId}", tagUpdateRequest);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TagResponse>();
    }

    public async Task DeleteTag(Guid tagId)
    {
        var response = await this._httpClient.DeleteAsync($"/api/tags/{tagId}");
        response.EnsureSuccessStatusCode();
    }
}
