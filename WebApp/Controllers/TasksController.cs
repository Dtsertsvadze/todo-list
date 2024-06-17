using Microsoft.AspNetCore.Mvc;
using Services.WebApi;

namespace WebApp.Controllers;

public class TasksController : Controller
{
    private readonly TasksWebApiService _tasksWebApiService;
    private readonly ToDoListWebApiService _toDoListWebApiService;

    public TasksController(TasksWebApiService tasksWebApiService, ToDoListWebApiService toDoListWebApiService)
    {
        this._tasksWebApiService = tasksWebApiService;
        this._toDoListWebApiService = toDoListWebApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid id)
    {
        var toDoList = await this._toDoListWebApiService.GetToDoListByIdAsync(id);

        return View(toDoList);
    }

}
