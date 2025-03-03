﻿namespace ServiceContracts;
using Entities.DTOs.TodoListDtos;
public interface IToDoListService
{
    public Task<List<ToDoListResponse?>> GetToDoListsAsync(Guid userId);

    public Task<ToDoListResponse?> GetToDoListAsync(Guid toDoListId);

    public Task<ToDoListResponse?> CreateToDoListAsync(ToDoListAddRequest toDoListAddRequest);

    public Task<ToDoListResponse?> UpdateToDoListAsync(ToDoListUpdateRequest toDoListUpdateRequest);

    public Task<bool> DeleteToDoListAsync(Guid toDoListId);
}
