namespace ServiceContracts;
using Entities.DTOs.TasksDtos;
public interface ITasksService
{
    Task<List<TaskResponse>> GetTasksAsync(Guid toDoListId);

    Task<TaskResponse?> GetTaskAsync(Guid taskId);

    Task<TaskResponse?> GetTaskById(Guid taskId);

    Task<TaskResponse?> CreateTaskAsync(TaskAddRequest taskAddRequest, Guid toDoListId);

    Task<TaskResponse?> UpdateTaskAsync(Guid taskId, TaskUpdateRequest taskUpdateRequest);

    Task<bool> DeleteTaskAsync(Guid taskId);

    Task<bool> CompleteTaskAsync(Guid taskId);
}
