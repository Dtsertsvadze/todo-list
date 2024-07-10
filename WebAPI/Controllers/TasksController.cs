namespace WebAPI.Controllers;
using Entities.DTOs.TasksDtos;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

[ApiController]
[Route("api/todolist/")]
public class TasksController : ControllerBase
{
    private readonly ITasksService _tasksService;

    public TasksController(ITasksService tasksService)
    {
        this._tasksService = tasksService;
    }

    [HttpGet("{toDoListId}/Tasks/{taskId}")]
    public async Task<IActionResult> GetTasks(Guid toDoListId, Guid taskId)
    {
        var task = await this._tasksService.GetTaskAsync(taskId);
        return Ok(task);
    }

    [HttpGet("{taskId}/[action]")]
    public async Task<IActionResult> GetTask(Guid taskId)
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

    [HttpPost("{toDoListId}/Tasks")]
    public async Task<IActionResult> CreateTaskAsync([FromBody] TaskAddRequest taskAddRequest, Guid toDoListId)
    {
        var createdTask = await this._tasksService.CreateTaskAsync(taskAddRequest, toDoListId);

        return CreatedAtAction(nameof(this.GetTask), new { toDoListId, taskId = createdTask?.Id }, createdTask);
    }

    [HttpPut("{toDoListId}/Tasks/{taskId}")]
    public async Task<IActionResult> UpdateTaskAsync(Guid toDoListId, Guid taskId, [FromBody] TaskUpdateRequest taskUpdateRequest)
    {
        var updatedTask = await this._tasksService.UpdateTaskAsync(taskId, taskUpdateRequest);

        return Ok(updatedTask);
    }

    [HttpDelete("{toDoListId}/Tasks/{taskId}")]
    public async Task<IActionResult> DeleteTaskAsync(Guid toDoListId, Guid taskId)
    {
        var success = await this._tasksService.DeleteTaskAsync(taskId);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPut("{toDoListId}/Tasks/{taskId}/complete")]
    public async Task<IActionResult> CompleteTaskAsync(Guid toDoListId, Guid taskId)
    {
        var success = await this._tasksService.CompleteTaskAsync(taskId);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
