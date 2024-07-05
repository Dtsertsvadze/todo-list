namespace WebApp.Controllers;
using Entities.DTOs.TagsDtos;
using Entities.DTOs.TasksDtos;
using Microsoft.AspNetCore.Mvc;
using Services.WebApi;

public class TasksController : Controller
{
    private readonly TagsWebApiService _tagsWebApiService;
    private readonly TasksWebApiService _tasksWebApiService;
    private readonly ILogger<TasksController> _logger;

    public TasksController(TasksWebApiService tasksWebApiService, ILogger<TasksController> logger, TagsWebApiService tagsWebApiService)
    {
        this._logger = logger;
        this._tagsWebApiService = tagsWebApiService;
        this._tasksWebApiService = tasksWebApiService;
    }

    [HttpGet]
    public IActionResult Create(Guid toDoListId)
    {
        var model = new TaskAddRequest
        {
            ToDoListId = toDoListId,
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskAddRequest model, string? tag)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var createdTask = await this._tasksWebApiService.CreateTaskAsync(model, model.ToDoListId);

        if (tag == null)
        {
            return RedirectToAction("Get", "TodoList", new { id = model.ToDoListId });
        }

        var tagAddRequest = new TagAddRequest { Name = tag };
        await this._tagsWebApiService.AddTagToTask(createdTask.Id, tagAddRequest);

        return RedirectToAction("Get", "TodoList", new { id = model.ToDoListId });
    }

    [HttpGet("todolist/{toDoListId}/tasks/{taskId}")]
    public async Task<IActionResult> Get(Guid toDoListId, Guid taskId)
    {
        var task = await this._tasksWebApiService.GetTaskAsync(toDoListId, taskId);

        return View(task);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid toDoListId, Guid taskId)
    {
        var task = await this._tasksWebApiService.GetTaskAsync(toDoListId, taskId);

        return View(task);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid toDoListId, Guid id)
    {
        await this._tasksWebApiService.DeleteTaskAsync(toDoListId, id);

        return RedirectToAction("Get", "ToDoList", new { id = toDoListId });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid toDoListId, Guid taskId)
    {
        var task = await this._tasksWebApiService.GetTaskAsync(toDoListId, taskId);

        return View(task);
    }

    [HttpPost, ActionName("Edit")]
    public async Task<IActionResult> EditConfirmed(Guid todoListId, Guid taskId, TaskUpdateRequest task)
    {
        if (!ModelState.IsValid)
        {
            return View(task.ToTaskResponse());
        }

        await this._tasksWebApiService.UpdateTaskAsync(task.ToDoListId, task.Id, task);

        return RedirectToAction("Get", new { toDoListId = task.ToDoListId, taskId = task.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Complete(Guid toDoListId, Guid taskId)
    {
        var task = await this._tasksWebApiService.GetTaskAsync(toDoListId, taskId);

        return View(task);
    }

    [HttpPost, ActionName("Complete")]
    public async Task<IActionResult> CompleteConfirmed(Guid toDoListId, Guid taskId)
    {
        if (taskId == Guid.Empty || toDoListId == Guid.Empty)
        {
            return BadRequest("Invalid taskId or toDoListId");
        }

        try
        {
            await this._tasksWebApiService.CompleteTaskAsync(toDoListId, taskId);
            return RedirectToAction("Get", "ToDoList", new { id = toDoListId });
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "Error completing task: {TaskId} in list: {ToDoListId}", taskId, toDoListId);

            ModelState.AddModelError(string.Empty, "An error occurred while completing the task. Please try again.");

            // Retrieve the task again to redisplay the form
            var task = await this._tasksWebApiService.GetTaskAsync(toDoListId, taskId);
            return View(task);
        }
    }
}
