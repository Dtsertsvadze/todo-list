namespace Services;
using Entities.DTOs.TasksDtos;
using RepositoryContracts;
using ServiceContracts;
public class TasksService(ITasksRepository tasksRepository)
    : ITasksService
{
    public async Task<List<TaskResponse>> GetTasksAsync(Guid toDoListId)
    {
        var result = await tasksRepository.GetTasks(toDoListId);

        return result.Select(temp => temp.ToTaskResponse()).ToList();
    }

    public async Task<TaskResponse?> GetTaskAsync(Guid taskId)
    {
        var result = await tasksRepository.GetTaskById(taskId);

        ArgumentNullException.ThrowIfNull(result, "Task not found.");

        return result.ToTaskResponse();
    }

    public async Task<TaskResponse?> GetTaskById(Guid taskId)
    {
        var taskEntity = await tasksRepository.GetTaskById(taskId);
        if (taskEntity == null)
        {
            return null;
        }

        var taskResponse = new TaskResponse
        {
            Id = taskEntity.Id,
            Title = taskEntity.Title,
            Description = taskEntity.Description,
            DueDateTime = taskEntity.DueDateTime,
            ToDoListId = taskEntity.ToDoListId,
            IsComplete = taskEntity.IsComplete,
            CreatedAt = taskEntity.CreatedAt,
        };

        return taskResponse;
    }

    public async Task<TaskResponse?> CreateTaskAsync(TaskAddRequest taskAddRequest, Guid toDoListId)
    {
        var task = taskAddRequest.ToTaskEntity();

        task.Id = Guid.NewGuid();
        task.CreatedAt = DateTime.UtcNow;
        task.IsComplete = false;
        task.ToDoListId = toDoListId;

        var createdTask = await tasksRepository.AddTask(toDoListId, task);

        return createdTask.ToTaskResponse();
    }

    public async Task<TaskResponse?> UpdateTaskAsync(Guid taskId, TaskUpdateRequest taskUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(taskUpdateRequest, nameof(taskUpdateRequest));

        var existingTask = await tasksRepository.GetTaskById(taskId);

        ArgumentNullException.ThrowIfNull(existingTask, nameof(existingTask));

        existingTask.Title = taskUpdateRequest.Title;
        existingTask.Description = taskUpdateRequest.Description;
        existingTask.DueDateTime = taskUpdateRequest.DueDateTime;
        existingTask.IsComplete = taskUpdateRequest.IsComplete;

        var updatedTask = await tasksRepository.UpdateTask(existingTask);

        ArgumentNullException.ThrowIfNull(updatedTask, nameof(updatedTask));

        return updatedTask.ToTaskResponse();
    }

    public async Task<bool> DeleteTaskAsync(Guid taskId)
    {
        return await tasksRepository.DeleteTask(taskId);
    }

    public async Task<bool> CompleteTaskAsync(Guid taskId)
    {
        ArgumentNullException.ThrowIfNull(taskId, nameof(taskId));

        var existingTask = await tasksRepository.GetTaskById(taskId);

        ArgumentNullException.ThrowIfNull(existingTask, nameof(existingTask));

        existingTask.IsComplete = true;

        var updatedTask = await tasksRepository.UpdateTask(existingTask);

        return updatedTask.IsComplete;
    }
}
