using System.Net.Http.Json;
using Entities.DTOs.CommentDtos;

namespace Services.WebApi;

public class CommentsWebApiService
{
    private readonly HttpClient _httpClient;

    public CommentsWebApiService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    public async Task<bool> AddCommentAsync(Guid taskId, CommentAddRequest commentAddRequest)
    {
        var response = await this._httpClient.PostAsJsonAsync($"/api/tasks/{taskId}/comments", commentAddRequest);

        response.EnsureSuccessStatusCode();

        return response.IsSuccessStatusCode;
    }

    public async Task<List<CommentResponse>> GetCommentsForTask(Guid taskId)
    {
        var response = await this._httpClient.GetAsync($"/api/comments/{taskId}/GetCommentsForTask");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<CommentResponse>>();

        return result ?? new List<CommentResponse>();
    }

    public async Task<CommentResponse?> UpdateCommentAsync(CommentUpdateRequest commentUpdateRequest, Guid commentId)
    {
        var response = await this._httpClient.PutAsJsonAsync($"/api/comments/{commentId}/UpdateComment", commentUpdateRequest);

        response.EnsureSuccessStatusCode();

        var updatedComment = await response.Content.ReadFromJsonAsync<CommentResponse>();

        return updatedComment;
    }

    public async Task DeleteComment(Guid commentId)
    {
        var response = await this._httpClient.DeleteAsync($"/api/comments/{commentId}/DeleteComment");
        response.EnsureSuccessStatusCode();
    }
}
