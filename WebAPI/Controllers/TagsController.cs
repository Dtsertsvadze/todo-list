namespace WebAPI.Controllers;
using Entities.DTOs.TagsDtos;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

[Route("api/[controller]")]
[ApiController]
public class TagsController : ControllerBase
{
    private readonly ITagsService _tagsService;

    public TagsController(ITagsService tagsService)
    {
        this._tagsService = tagsService;
    }

    [HttpGet("{taskId}/tags")]
    public async Task<IActionResult> GetTags(Guid taskId)
    {
        try
        {
            var tags = await this._tagsService.GetTagsAsync(taskId);
            return Ok(tags);
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

    [HttpPost("{taskId}")]
    public async Task<IActionResult> AddTagToTask(Guid taskId, [FromBody] TagAddRequest tagAddRequest)
    {
        try
        {
            var tag = await this._tagsService.AddTagToTaskAsync(taskId, tagAddRequest);
            return Ok(tag);
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

    [HttpPut("{tagId}")]
    public async Task<IActionResult> UpdateTag(Guid tagId, [FromBody] TagUpdateRequest tagUpdateRequest)
    {
        try
        {
            var tag = await this._tagsService.UpdateTagAsync(tagId, tagUpdateRequest);
            return Ok(tag);
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

    [HttpDelete("{tagId}")]
    public async Task<IActionResult> DeleteTag(Guid tagId)
    {
        try
        {
            var result = await this._tagsService.DeleteTagAsync(tagId);
            return this.Ok(result);
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
