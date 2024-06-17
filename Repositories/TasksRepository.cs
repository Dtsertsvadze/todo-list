using Database;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;

namespace Repositories;

public class TasksRepository(ToDoListDbContext db, ILogger<TasksRepository> logger) : ITasksRepository
{
    private readonly ILogger<TasksRepository> _logger = logger;

    public async Task<IEnumerable<TaskEntity>> GetTasks(Guid toDoListId)
    {
        var result = await db.Tasks
            .Include(t => t.Comments)
            .Include(t => t.TaskTags)
            .ThenInclude(t => t.Tag)
            .Where(t => t.ToDoListId == toDoListId).ToListAsync();

        return result;
    }

    public async Task<TaskEntity?> GetTaskById(Guid taskId)
    {
        return await db.Tasks
            .Include(t => t.Comments)
            .Include(t => t.TaskTags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public async Task<TaskEntity> AddTask(Guid toDoListId,TaskEntity task)
    {
        var todoList = await db.ToDoLists
            .Include(l => l.Tasks)!
            .FirstOrDefaultAsync(temp => temp.Id == toDoListId);

        if (todoList is null)
        {
            throw new ArgumentException("ToDo List not found");
        }

        db.Tasks.Add(task);

        await db.SaveChangesAsync();

        return task;
    }

    public async Task<TaskEntity> UpdateTask(TaskEntity task)
    {
        var matchingTask = await db.Tasks.FirstOrDefaultAsync(temp => temp.Id == task.Id);

        if (matchingTask is null)
        {
            return task;
        }

        matchingTask.Title = task.Title;
        matchingTask.Description = task.Description;

        await db.SaveChangesAsync();

        return matchingTask;
    }

    public async Task<bool> DeleteTask(Guid taskId)
    {
        db.Tasks.RemoveRange(db.Tasks.Where(temp => temp.Id == taskId));
        var rowsDeleted = await db.SaveChangesAsync();

        return rowsDeleted > 0;
    }
}
