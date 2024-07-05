#pragma warning disable SA1200

using Entities.DTOs.CommentDtos;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        this._commentService = commentService;
    }

    [HttpPost("{taskId}/[action]")]
    public async Task<IActionResult> AddCommentToTask(Guid taskId, [FromBody] CommentAddRequest commentAddRequest)
    {
        try
        {
            var comment = await this._commentService.AddCommentAsync(taskId, commentAddRequest);
            return Ok(comment);
        }
        catch (NullReferenceException ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{taskId}/[action]")]
    public async Task<IActionResult> GetCommentsForTask(Guid taskId)
    {
        try
        {
            var comments = await this._commentService.GetCommentsByTaskIdAsync(taskId);
            return Ok(comments);
        }
        catch (NullReferenceException ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{commentId}/[action]")]
    public async Task<IActionResult> UpdateComment([FromBody] CommentUpdateRequest commentUpdateRequest, Guid commentId)
    {
        var comment = await this._commentService.GetCommentByIdAsync(commentId);

        if (comment == null)
        {
            return this.NotFound();
        }

        try
        {
            commentUpdateRequest.Id = commentId;
            var updatedComment = await this._commentService.UpdateCommentAsync(commentUpdateRequest);
            return this.Ok(updatedComment);
        }
        catch (NullReferenceException ex)
        {
            return this.StatusCode(500, $"Internal server error: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            return this.BadRequest(ex.Message);
        }
    }

    [HttpDelete("{commentId}/[action]")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        try
        {
            await this._commentService.DeleteCommentAsync(commentId);
            return this.Ok();
        }
        catch (NullReferenceException ex)
        {
            return this.StatusCode(500, $"Internal server error: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            return this.BadRequest(ex.Message);
        }
    }
}
