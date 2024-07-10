namespace WebApp.Controllers;
using Entities.DTOs.CommentDtos;
using Microsoft.AspNetCore.Mvc;
using Services.WebApi;

public class CommentsController(CommentsWebApiService commentService, TasksWebApiService taskService)
    : Controller
{
    [HttpGet]
    public IActionResult AddComment(Guid todoListId, Guid taskId)
    {
        ViewBag.TodoListId = todoListId;
        ViewBag.TaskId = taskId;
        return View(new CommentAddRequest());
    }

    [HttpPost]
    [Route("todolist/{todoListId}/tasks/{taskId}/comments/add")]
    public async Task<IActionResult> AddComment(Guid taskId, CommentAddRequest commentAddRequest)
    {
        var todoList = await taskService.GetTaskByIdAsync(taskId);
        if (!ModelState.IsValid)
        {
            ViewBag.TodoListId = todoList !.ToDoListId;
            ViewBag.TaskId = taskId;
            return View(commentAddRequest);
        }

        try
        {
            bool result = await commentService.AddCommentAsync(taskId, commentAddRequest);

            if (result)
            {
                return RedirectToAction("Get", "Tasks", new { toDoListId = todoList !.ToDoListId, taskId });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to add comment. Please try again.");
                ViewBag.TodoListId = todoList !.ToDoListId;
                ViewBag.TaskId = taskId;
                return View(commentAddRequest);
            }
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            ViewBag.TodoListId = todoList !.ToDoListId;
            ViewBag.TaskId = taskId;
            return View(commentAddRequest);
        }
    }

    [HttpGet]
    [HttpPost]
    public async Task<IActionResult> RemoveComment(Guid commentId, Guid taskId)
    {
        if (commentId == Guid.Empty || taskId == Guid.Empty)
        {
            return BadRequest("Invalid comment or task ID.");
        }

        try
        {
            var task = await taskService.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            await commentService.DeleteComment(commentId);

            return RedirectToAction("Get", "Tasks", new { toDoListId = task.ToDoListId, taskId });
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");

            return RedirectToAction("Get", "Tasks", new { taskId });
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditComment(Guid commentId, Guid taskId)
    {
        var comments = await commentService.GetCommentsForTask(taskId);
        var comment = comments?.FirstOrDefault(c => c.Id == commentId);

        ViewBag.TaskId = taskId;
        ViewBag.CommentId = commentId;
        return View(comment);
    }

    [HttpPost]
    public async Task<IActionResult> EditComment(Guid commentId, Guid taskId, CommentUpdateRequest model)
    {
        var success = await commentService.UpdateCommentAsync(model, commentId);
        var task = await taskService.GetTaskByIdAsync(taskId);
        if (success is not null)
        {
            return RedirectToAction("Get", "Tasks", new { task !.ToDoListId, taskId });
        }

        ModelState.AddModelError(string.Empty, "Could not update comment. Please try again.");
        ViewBag.TaskId = taskId;
        return View();
    }
}
