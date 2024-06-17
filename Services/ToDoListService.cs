using Database;
using Entities;
using Entities.DTOs.TodoListDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;

namespace Services;

public class ToDoListService : IToDoListService
{
    private readonly IToDoListRepository _toDoListRepository;
    private readonly ILogger<ToDoListService> _logger;

    public ToDoListService(IToDoListRepository toDoListRepository, ILogger<ToDoListService> logger)
    {
        this._toDoListRepository = toDoListRepository;
        this._logger = logger;
    }

    public async Task<List<ToDoListResponse?>> GetToDoListsAsync()
    {
        var toDoLists = await this._toDoListRepository.GetToDoListsAsync();

        return toDoLists.Select(temp => temp?.ToToDoListResponse()).ToList();
    }

    public async Task<ToDoListResponse?> GetToDoListAsync(Guid toDoListId)
    {
        ArgumentNullException.ThrowIfNull(toDoListId, nameof(toDoListId));

        var result = await this._toDoListRepository.GetToDoListAsync(toDoListId);

        ArgumentNullException.ThrowIfNull(result, nameof(result));

        return result.ToToDoListResponse();
    }

    public async Task<ToDoListResponse?> CreateToDoListAsync(ToDoListAddRequest toDoListAddRequest)
    {
        var toDoList = toDoListAddRequest.ToToDoList();

        toDoList.Id = Guid.NewGuid();
        toDoList.CreatedAt = DateTime.UtcNow;
        toDoList.IsComplete = false;

        var toDoListResponse = await this._toDoListRepository.CreateToDoListAsync(toDoList);

        ArgumentNullException.ThrowIfNull(toDoListResponse, nameof(toDoListResponse));

        return toDoListResponse.ToToDoListResponse();
    }


    public async Task<ToDoListResponse?> UpdateToDoListAsync(ToDoListUpdateRequest toDoListUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(toDoListUpdateRequest, nameof(toDoListUpdateRequest));

        var existingToDoList = await this._toDoListRepository.GetToDoListAsync(toDoListUpdateRequest.Id);

        ArgumentNullException.ThrowIfNull(existingToDoList, nameof(existingToDoList));

        existingToDoList.Title = toDoListUpdateRequest.Title;
        existingToDoList.Description = toDoListUpdateRequest.Description;

        var result = await this._toDoListRepository.UpdateToDoListAsync(existingToDoList);

        ArgumentNullException.ThrowIfNull(result, nameof(result));

        return result.ToToDoListResponse();
    }

    public async Task<bool> DeleteToDoListAsync(Guid toDoListId)
    {
        return await this._toDoListRepository.DeleteToDoListAsync(toDoListId);
    }
}
