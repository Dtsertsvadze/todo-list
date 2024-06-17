using System.Net.Http.Json;
using Entities.DTOs.TodoListDtos;
using Task = System.Threading.Tasks.Task;

namespace Services.WebApi;

public class ToDoListWebApiService
{
    private readonly HttpClient _httpClient;

    public ToDoListWebApiService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    public async Task<ICollection<ToDoListResponse>> GetToDoListsAsync()
    {
        var response = await this._httpClient.GetAsync($"/api/ToDoList");

        response.EnsureSuccessStatusCode();

        var toDoLists = await response.Content.ReadFromJsonAsync<ICollection<ToDoListResponse>>();

        ArgumentNullException.ThrowIfNull(toDoLists, nameof(toDoLists));

        return toDoLists;
    }

    public async Task<ToDoListResponse> CreateTodoListAsync(ToDoListAddRequest toDoListAddRequest)
    {
        try
        {
            var response = await this._httpClient.PostAsJsonAsync("/api/todolist", toDoListAddRequest);

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
        var response = await this._httpClient.DeleteAsync($"/api/todolist/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<ToDoListResponse> UpdateToDoListAsync(Guid id, ToDoListUpdateRequest toDoListUpdateRequest)
    {
        try
        {
            var response = await this._httpClient.PutAsJsonAsync($"/api/todolist/{id}", toDoListUpdateRequest);

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
        var response = await this._httpClient.GetAsync($"/api/todolist/{id}");

        response.EnsureSuccessStatusCode();

        var toDoList = await response.Content.ReadFromJsonAsync<ToDoListResponse>();

        ArgumentNullException.ThrowIfNull(toDoList, nameof(toDoList));

        return toDoList;
    }
}
