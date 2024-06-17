using Database;
using Entities;
using Entities.DTOs;
using Entities.DTOs.TasksDtos;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;

namespace Services;

public class TasksService : ITasksService
{
    private readonly ITasksRepository _tasksRepository;

    public TasksService(ITasksRepository tasksRepository, IToDoListRepository toDoListRepository)
    {
        this._tasksRepository = tasksRepository;
    }

    public async Task<List<TaskResponse>> GetTasksAsync(Guid toDoListId)
    {
        var result = await this._tasksRepository.GetTasks(toDoListId);

        return result.Select(temp => temp.ToTaskResponse()).ToList();
    }

    public async Task<TaskResponse?> GetTaskAsync(Guid taskId)
    {
        var result = await this._tasksRepository.GetTaskById(taskId);

        ArgumentNullException.ThrowIfNull(result, "Task not found.");

        return result.ToTaskResponse();
    }

    public async Task<TaskResponse?> CreateTaskAsync(TaskAddRequest taskAddRequest, Guid toDoListId)
    {
        var task = taskAddRequest.ToTaskEntity();

        task.Id = Guid.NewGuid();
        task.CreatedAt = DateTime.UtcNow;
        task.IsComplete = false;
        task.ToDoListId = toDoListId;

        var createdTask = await this._tasksRepository.AddTask(toDoListId, task);

        return createdTask.ToTaskResponse();
    }


    public async Task<TaskResponse?> UpdateTaskAsync(Guid taskId, TaskUpdateRequest taskUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(taskUpdateRequest, nameof(taskUpdateRequest));

        var existingTask = await this._tasksRepository.GetTaskById(taskId);

        ArgumentNullException.ThrowIfNull(existingTask, nameof(existingTask));

        existingTask.Title = taskUpdateRequest.Title;
        existingTask.Description = taskUpdateRequest.Description;
        existingTask.DueDateTime = taskUpdateRequest.DueDateTime;
        existingTask.IsComplete = taskUpdateRequest.IsComplete;

        var updatedTask = await this._tasksRepository.UpdateTask(existingTask);

        ArgumentNullException.ThrowIfNull(updatedTask, nameof(updatedTask));

        return updatedTask.ToTaskResponse();
    }

    public async Task<bool> DeleteTaskAsync(Guid taskId)
    {
        return await this._tasksRepository.DeleteTask(taskId);
    }
}
