using DiskodePro.WebApi.Data.Repositories;
using DiskodePro.WebApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DiskodePro.WebApi.Controllers;

[ApiController]
[Route("api/savedposts")]
public class SavePostController : ControllerBase
{
    private readonly IUserSavedPostsRepository _userSavedPostsRepository;

    public SavePostController(IUserSavedPostsRepository userSavedPostsRepository)
    {
        _userSavedPostsRepository = userSavedPostsRepository;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetSavedPostsByUser(int userId)
    {
        try
        {
            var savedPosts = await _userSavedPostsRepository.GetSavedPostsByUserAsync(userId);
            return Ok(savedPosts);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching saved posts.");
        }
    }

    [HttpPost("{userId}/{postId}")]
    public async Task<IActionResult> SavePost(int userId, int postId)
    {
        try
        {
            await _userSavedPostsRepository.SavePostAsync(userId, postId);
            return Ok();
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (PostNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while saving the post.");
        }
    }

    [HttpPost("{userId}/{postId}/unsave")]
    public async Task<IActionResult> UnsavePost(int userId, int postId)
    {
        try
        {
            await _userSavedPostsRepository.UnsavePostAsync(userId, postId);
            return Ok();
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while removing the saved post.");
        }
    }
}