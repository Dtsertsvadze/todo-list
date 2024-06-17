using System.Net.Http.Json;
using Entities.DTOs;
using Entities.DTOs.TasksDtos;

namespace Services.WebApi;

public class TasksWebApiService
{
    private readonly HttpClient _httpClient;

        public TasksWebApiService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<ICollection<TaskResponse>> GetTasksAsync(Guid toDoListId)
        {
            var response = await this._httpClient.GetAsync($"/api/todolist/{toDoListId}/tasks");

            response.EnsureSuccessStatusCode();

            var tasks = await response.Content.ReadFromJsonAsync<ICollection<TaskResponse>>();

            ArgumentNullException.ThrowIfNull(tasks, nameof(tasks));

            return tasks;
        }

        public async Task<TaskResponse> GetTaskAsync(Guid toDoListId, Guid taskId)
        {
            var response = await this._httpClient.GetAsync($"/api/todolist/{toDoListId}/tasks/{taskId}");

            response.EnsureSuccessStatusCode();

            var task = await response.Content.ReadFromJsonAsync<TaskResponse>();

            ArgumentNullException.ThrowIfNull(task, nameof(task));

            return task;
        }

        public async Task<TaskResponse> CreateTaskAsync(TaskAddRequest taskAddRequest, Guid toDoListId)
        {
            var response = await this._httpClient.PostAsJsonAsync($"/api/todolist/{toDoListId}/tasks", taskAddRequest);

            response.EnsureSuccessStatusCode();

            var createdTask = await response.Content.ReadFromJsonAsync<TaskResponse>();

            ArgumentNullException.ThrowIfNull(createdTask, nameof(createdTask));

            return createdTask;
        }

        public async Task<TaskResponse> UpdateTaskAsync(Guid toDoListId, Guid taskId, TaskUpdateRequest taskUpdateRequest)
        {
            var response = await this._httpClient.PutAsJsonAsync($"/api/todolist/{toDoListId}/tasks/{taskId}", taskUpdateRequest);

            response.EnsureSuccessStatusCode();

            var updatedTask = await response.Content.ReadFromJsonAsync<TaskResponse>();

            ArgumentNullException.ThrowIfNull(updatedTask, nameof(updatedTask));

            return updatedTask;
        }

        public async Task DeleteTaskAsync(Guid toDoListId, Guid taskId)
        {
            var response = await this._httpClient.DeleteAsync($"/api/todolist/{toDoListId}/tasks/{taskId}");

            response.EnsureSuccessStatusCode();
        }
}
