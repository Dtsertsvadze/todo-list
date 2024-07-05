namespace WebApp.Controllers;
using Entities.DTOs.TagsDtos;
using Microsoft.AspNetCore.Mvc;
using Services.WebApi;

public class TagsController(TagsWebApiService tagsWebApiService, TasksWebApiService taskService)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> AddTag(Guid taskId)
    {
        var toDoListId = await taskService.GetTaskByIdAsync(taskId);

        if (toDoListId != null)
        {
            ViewBag.TaskId = taskId;
            ViewBag.ToDoListId = toDoListId.ToDoListId;
        }

        var model = new TagAddRequest();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddTag(Guid taskId, TagAddRequest tagAddRequest)
    {
        var todoListId = await taskService.GetTaskByIdAsync(taskId);
        ArgumentNullException.ThrowIfNull(todoListId, nameof(todoListId));
        if (!ModelState.IsValid)
        {
            ViewBag.TaskId = taskId;
            return View(tagAddRequest);
        }

        try
        {
            var addedTag = await tagsWebApiService.AddTagToTask(taskId, tagAddRequest);
            if (addedTag != null)
            {
                TempData["SuccessMessage"] = "Tag added successfully.";
                return RedirectToAction("Get", "TodoList", new { id = todoListId.ToDoListId });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to add the tag.");
                ViewBag.TaskId = taskId;
                return View(tagAddRequest);
            }
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while adding the tag. Please try again.");
            ViewBag.TaskId = taskId;
            return View(tagAddRequest);
        }
    }

    [HttpPost]
    public async Task<IActionResult> RemoveTag(Guid toDoListId, Guid tagId)
    {
        try
        {
            await tagsWebApiService.DeleteTag(tagId);
            TempData["SuccessMessage"] = "Tag deleted successfully.";
        }
        catch (HttpRequestException)
        {
            TempData["ErrorMessage"] = "Failed to delete the tag. Please try again.";
        }

        // Redirect back to the TodoLists Get action
        return RedirectToAction("Get", "TodoList", new { id = toDoListId });
    }
}
