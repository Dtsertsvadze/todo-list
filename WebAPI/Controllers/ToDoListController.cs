using System.Text.Json;
using System.Text.Json.Serialization;
using Entities;
using Entities.DTOs;
using Entities.DTOs.TodoListDtos;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoListController : ControllerBase
{
    private readonly IToDoListService _toDoListService;

    public ToDoListController(IToDoListService toDoListService)
    {
        this._toDoListService = toDoListService;
    }

    [HttpGet]
    public async Task<IActionResult> GetToDoLists()
    {
        var toDoLists = await this._toDoListService.GetToDoListsAsync();

        return Ok(toDoLists);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetToDoList(Guid id)
    {
        var toDoList = await this._toDoListService.GetToDoListAsync(id);
            if (toDoList == null)
            {
                return NotFound();
            }
            return Ok(toDoList);
    }

    [HttpPost]
    public async Task<IActionResult> CreateToDoList([FromBody] ToDoListAddRequest toDoListAddRequest)
    {
        var createdToDoList = await this._toDoListService.CreateToDoListAsync(toDoListAddRequest);

        ArgumentNullException.ThrowIfNull(createdToDoList, nameof(createdToDoList));

        return CreatedAtAction(nameof(this.GetToDoList), new { id = createdToDoList.Id }, createdToDoList);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateToDoList(Guid id, [FromBody] ToDoListUpdateRequest toDoListUpdateRequest)
    {
        toDoListUpdateRequest.Id = id;

        var updatedToDoList = await this._toDoListService.UpdateToDoListAsync(toDoListUpdateRequest);

        if (updatedToDoList == null)
        {
            return NotFound();
        }

        return Ok(updatedToDoList);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteToDoList(Guid id)
    {
        var deleted = await this._toDoListService.DeleteToDoListAsync(id);

        return deleted ? NoContent() : NotFound();
    }
}
