namespace Services;
using Entities.DTOs.TodoListDtos;
using RepositoryContracts;
using ServiceContracts;
public class ToDoListService(IToDoListRepository toDoListRepository)
    : IToDoListService
{
    public async Task<List<ToDoListResponse?>> GetToDoListsAsync()
    {
        var toDoLists = await toDoListRepository.GetToDoListsAsync();

        return toDoLists.Select(temp => temp?.ToToDoListResponse()).ToList();
    }

    public async Task<ToDoListResponse?> GetToDoListAsync(Guid toDoListId)
    {
        ArgumentNullException.ThrowIfNull(toDoListId, nameof(toDoListId));

        var result = await toDoListRepository.GetToDoListAsync(toDoListId);

        ArgumentNullException.ThrowIfNull(result, nameof(result));

        return result.ToToDoListResponse();
    }

    public async Task<ToDoListResponse?> CreateToDoListAsync(ToDoListAddRequest toDoListAddRequest)
    {
        var toDoList = toDoListAddRequest.ToToDoList();

        toDoList.Id = Guid.NewGuid();
        toDoList.CreatedAt = DateTime.UtcNow;
        toDoList.IsComplete = false;

        var toDoListResponse = await toDoListRepository.CreateToDoListAsync(toDoList);

        ArgumentNullException.ThrowIfNull(toDoListResponse, nameof(toDoListResponse));

        return toDoListResponse.ToToDoListResponse();
    }

    public async Task<ToDoListResponse?> UpdateToDoListAsync(ToDoListUpdateRequest toDoListUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(toDoListUpdateRequest, nameof(toDoListUpdateRequest));

        var existingToDoList = await toDoListRepository.GetToDoListAsync(toDoListUpdateRequest.Id);

        ArgumentNullException.ThrowIfNull(existingToDoList, nameof(existingToDoList));

        existingToDoList.Title = toDoListUpdateRequest.Title;
        existingToDoList.Description = toDoListUpdateRequest.Description;

        var result = await toDoListRepository.UpdateToDoListAsync(existingToDoList);

        ArgumentNullException.ThrowIfNull(result, nameof(result));

        return result.ToToDoListResponse();
    }

    public async Task<bool> DeleteToDoListAsync(Guid toDoListId)
    {
        return await toDoListRepository.DeleteToDoListAsync(toDoListId);
    }
}
