namespace Services.WebApi;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Entities.DTOs.TodoListDtos;
using Microsoft.Extensions.Logging;
using Task = Task;
public class ToDoListWebApiService(HttpClient httpClient, ILogger<ToDoListWebApiService> logger)
{
    public async Task<IEnumerable<ToDoListResponse>?> GetToDoListsAsync(string token)
    {
        try
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("/api/todolist");

            response.EnsureSuccessStatusCode();

            var toDoLists = await response.Content.ReadFromJsonAsync<IEnumerable<ToDoListResponse>>();

            ArgumentNullException.ThrowIfNull(toDoLists, nameof(toDoLists));

            return toDoLists;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP request failed: {ErrorMessage}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error in GetToDoListsAsync: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<ToDoListResponse> CreateTodoListAsync(ToDoListAddRequest toDoListAddRequest)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("/api/todolist", toDoListAddRequest);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadFromJsonAsync<ToDoListResponse>();

            ArgumentNullException.ThrowIfNull(responseContent, nameof(responseContent));

            return responseContent;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteToDoListAsync(Guid id)
    {
        var response = await httpClient.DeleteAsync($"/api/todolist/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<ToDoListResponse> UpdateToDoListAsync(Guid id, ToDoListUpdateRequest toDoListUpdateRequest)
    {
        try
        {
            var response = await httpClient.PutAsJsonAsync($"/api/todolist/{id}", toDoListUpdateRequest);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadFromJsonAsync<ToDoListResponse>();

            ArgumentNullException.ThrowIfNull(responseContent, nameof(responseContent));

            return responseContent;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }

    public async Task<ToDoListResponse> GetToDoListByIdAsync(Guid id)
    {
        var response = await httpClient.GetAsync($"/api/todolist/Get/{id}");

        response.EnsureSuccessStatusCode();

        var toDoList = await response.Content.ReadFromJsonAsync<ToDoListResponse>();

        ArgumentNullException.ThrowIfNull(toDoList, nameof(toDoList));

        return toDoList;
    }
}
