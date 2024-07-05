namespace WebApp.Controllers;
using Entities.DTOs.TodoListDtos;
using Microsoft.AspNetCore.Mvc;
using Services.WebApi;

public class TodoListController : Controller
{
    private readonly ToDoListWebApiService _toDoListWebApiService;
    private readonly ILogger<TodoListController> _logger;

    public TodoListController(ToDoListWebApiService toDoListWebApiService, ILogger<TodoListController> logger)
    {
        this._toDoListWebApiService = toDoListWebApiService;
        this._logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var jwtToken = Request.Cookies["jwtToken"];
        if (string.IsNullOrEmpty(jwtToken))
        {
            return RedirectToAction("Login", "Account");
        }

        try
        {
            var todoLists = await this._toDoListWebApiService.GetToDoListsAsync(jwtToken);
            return View(todoLists);
        }
        catch (HttpRequestException)
        {
            return StatusCode(500, "An error occurred while fetching todo lists.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid id)
    {
        var toDoList = await this._toDoListWebApiService.GetToDoListByIdAsync(id);

        return View(toDoList);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ToDoListAddRequest toDoList)
    {
        if (!ModelState.IsValid)
        {
            return View(toDoList);
        }

        await this._toDoListWebApiService.CreateTodoListAsync(toDoList);
        return RedirectToAction(nameof(this.Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var toDoLists = await this._toDoListWebApiService.GetToDoListsAsync("dummy");

        ArgumentNullException.ThrowIfNull(toDoLists, nameof(toDoLists));

        var toDoList = toDoLists.FirstOrDefault(t => t.Id == id);

        if (toDoList == null)
        {
            return NotFound();
        }

        return View(toDoList);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await this._toDoListWebApiService.DeleteToDoListAsync(id);
        return RedirectToAction(nameof(this.Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var toDoLists = await this._toDoListWebApiService.GetToDoListsAsync("dummy");

        ArgumentNullException.ThrowIfNull(toDoLists, nameof(toDoLists));
        var toDoList = toDoLists.FirstOrDefault(t => t.Id == id);
        if (toDoList == null)
        {
            return NotFound();
        }

        return View(toDoList);
    }

    [HttpPost, ActionName("edit")]
    public async Task<IActionResult> EditConfirmed(Guid id, ToDoListUpdateRequest toDoList)
    {
        if (id != toDoList.Id)
        {
            return NotFound();
        }

        var updatedToDoList = await this._toDoListWebApiService.UpdateToDoListAsync(id, toDoList);

        if (!ModelState.IsValid)
        {
            return View(updatedToDoList);
        }

        return RedirectToAction(nameof(this.Index));
    }
}
