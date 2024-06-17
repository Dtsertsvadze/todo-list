using Entities;

namespace RepositoryContracts;

public interface ITasksRepository
{
    Task<IEnumerable<TaskEntity>> GetTasks(Guid toDoListId);

    Task<TaskEntity?> GetTaskById(Guid taskId);

    Task<TaskEntity> AddTask(Guid toDoListId, TaskEntity task);

    Task<TaskEntity> UpdateTask(TaskEntity task);

    Task<bool> DeleteTask(Guid taskId);
}
