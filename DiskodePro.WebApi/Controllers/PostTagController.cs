using DiskodePro.WebApi.Data.Repositories;
using DiskodePro.WebApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DiskodePro.WebApi.Controllers;

[ApiController]
[Route("api/tags")]
public class PostTagController : ControllerBase
{
    private readonly IPostTagRepository _postTagRepository;

    public PostTagController(IPostTagRepository postTagRepository)
    {
        _postTagRepository = postTagRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetTags()
    {
        try
        {
            var tags = await _postTagRepository.GetTagsAsync();
            return Ok(tags);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching tags.");
        }
    }

    [HttpGet("posts")]
    public async Task<IActionResult> GetPostsByTagName([FromQuery] string tagName)
    {
        try
        {
            var posts = await _postTagRepository.GetPostsByTagNameAsync(tagName);
            return Ok(posts);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching posts by tag name.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddTagToPost([FromQuery] string tagName, [FromQuery] int postId)
    {
        try
        {
            await _postTagRepository.AddTagToPostAsync(tagName, postId);
            return Ok();
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while adding tag to post.");
        }
    }
}