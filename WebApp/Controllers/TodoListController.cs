using Entities.DTOs.TodoListDtos;
using Microsoft.AspNetCore.Mvc;
using Services.WebApi;

namespace WebApp.Controllers;

public class TodoListController : Controller
{
    private readonly ToDoListWebApiService _toDoListWebApiService;

    public TodoListController(ToDoListWebApiService toDoListWebApiService)
    {
        this._toDoListWebApiService = toDoListWebApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var toDoLists = await this._toDoListWebApiService.GetToDoListsAsync();

        return View(toDoLists);
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
        var toDoLists = await this._toDoListWebApiService.GetToDoListsAsync();

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
        var toDoLists = await this._toDoListWebApiService.GetToDoListsAsync();
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
