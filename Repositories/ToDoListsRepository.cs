namespace Repositories;
using Database;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

public class ToDoListsRepository(ToDoListDbContext db)
    : IToDoListRepository
{
    public async Task<IEnumerable<ToDoListEntity?>> GetToDoListsAsync()
    {
        return await db.ToDoLists
            .Include(t => t.Tasks) !
            .ThenInclude(t => t.Comments)
            .Include(t => t.Tasks) !
            .ThenInclude(t => t.TaskTags) !
            .ThenInclude(tt => tt.Tag)
            .ToListAsync();
    }

    public async Task<ToDoListEntity?> GetToDoListAsync(Guid toDoListId)
    {
        return await db.ToDoLists
            .Include(t => t.Tasks) !
            .ThenInclude(t => t.Comments)
            .Include(t => t.Tasks) !
            .ThenInclude(t => t.TaskTags) !
            .ThenInclude(tt => tt.Tag)
            .FirstOrDefaultAsync(temp => temp.Id == toDoListId);
    }

    public async Task<ToDoListEntity> CreateToDoListAsync(ToDoListEntity toDoListEntity)
    {
        db.ToDoLists.Add(toDoListEntity);
        await db.SaveChangesAsync();

        return toDoListEntity;
    }

    public async Task<ToDoListEntity> UpdateToDoListAsync(ToDoListEntity toDoListEntity)
    {
        var matchingToDoList = db.ToDoLists
            .Include(t => t.Tasks) !
            .ThenInclude(t => t.Comments)
            .FirstOrDefault(temp => temp.Id == toDoListEntity.Id);

        if (matchingToDoList is null)
        {
            return toDoListEntity;
        }

        matchingToDoList.Title = toDoListEntity.Title;
        matchingToDoList.Description = toDoListEntity.Description;

        await db.SaveChangesAsync();

        return matchingToDoList;
    }

    public async Task<bool> DeleteToDoListAsync(Guid toDoListId)
    {
        db.RemoveRange(db.ToDoLists.Where(temp => temp.Id == toDoListId));
        var rowsDeleted = await db.SaveChangesAsync();

        return rowsDeleted > 0;
    }
}
