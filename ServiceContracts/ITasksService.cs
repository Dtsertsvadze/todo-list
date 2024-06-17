using Entities;
using Entities.DTOs;
using Entities.DTOs.TasksDtos;

namespace ServiceContracts;

public interface ITasksService
{
    Task<List<TaskResponse>> GetTasksAsync(Guid toDoListId);

    Task<TaskResponse?> GetTaskAsync(Guid taskId);

    Task<TaskResponse?> CreateTaskAsync(TaskAddRequest taskAddRequest, Guid toDoListId);

    Task<TaskResponse?> UpdateTaskAsync(Guid taskId, TaskUpdateRequest taskUpdateRequest);

    Task<bool> DeleteTaskAsync(Guid taskId);
}
