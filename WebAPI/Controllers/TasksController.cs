using Entities.DTOs.TasksDtos;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/todolist/{toDoListId}/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITasksService _tasksService;

    public TasksController(ITasksService tasksService)
    {
        this._tasksService = tasksService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks(Guid toDoListId)
    {
        var tasks = await this._tasksService.GetTasksAsync(toDoListId);
        return Ok(tasks);
    }

    [HttpGet("{taskId}")]
    public async Task<IActionResult> GetTask(Guid toDoListId, Guid taskId)
    {
        try
        {
            var task = await this._tasksService.GetTaskAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTaskAsync([FromBody] TaskAddRequest taskAddRequest, Guid toDoListId)
    {
        var createdTask = await this._tasksService.CreateTaskAsync(taskAddRequest, toDoListId);

        return CreatedAtAction(nameof(this.GetTask), new { toDoListId, taskId = createdTask?.Id }, createdTask);
    }

    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTaskAsync(Guid toDoListId, Guid taskId, [FromBody] TaskUpdateRequest taskUpdateRequest)
    {
        var updatedTask = await this._tasksService.UpdateTaskAsync(taskId, taskUpdateRequest);

        return Ok(updatedTask);
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTaskAsync(Guid toDoListId, Guid taskId)
    {
        var success = await this._tasksService.DeleteTaskAsync(taskId);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
