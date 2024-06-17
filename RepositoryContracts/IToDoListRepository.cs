using Entities;

namespace RepositoryContracts;

public interface IToDoListRepository
{
    Task<IEnumerable<ToDoListEntity?>> GetToDoListsAsync();

    Task<ToDoListEntity?> GetToDoListAsync(Guid toDoListId);

    Task<ToDoListEntity> CreateToDoListAsync(ToDoListEntity toDoListEntity);

    Task<ToDoListEntity> UpdateToDoListAsync(ToDoListEntity toDoListEntity);

    Task<bool> DeleteToDoListAsync(Guid toDoListId);
}
