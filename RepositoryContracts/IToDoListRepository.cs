namespace RepositoryContracts;
using Entities;
public interface IToDoListRepository
{
    Task<IEnumerable<ToDoListEntity?>> GetToDoListsAsync(Guid userId);

    Task<ToDoListEntity?> GetToDoListAsync(Guid toDoListId);

    Task<ToDoListEntity> CreateToDoListAsync(ToDoListEntity toDoListEntity);

    Task<ToDoListEntity> UpdateToDoListAsync(ToDoListEntity toDoListEntity);

    Task<bool> DeleteToDoListAsync(Guid toDoListId);
}
