using Entities;
using Entities.DTOs.TodoListDtos;

namespace ServiceContracts;

public interface IToDoListService
{
    public Task<List<ToDoListResponse?>> GetToDoListsAsync();

    public Task<ToDoListResponse?> GetToDoListAsync(Guid toDoListId);

    public Task<ToDoListResponse?> CreateToDoListAsync(ToDoListAddRequest toDoListAddRequest);

    public Task<ToDoListResponse?> UpdateToDoListAsync(ToDoListUpdateRequest toDoListUpdateRequest);

    public Task<bool> DeleteToDoListAsync(Guid toDoListId);
}
