using DiskodePro.WebApi.Data.DTOs;
using DiskodePro.WebApi.Data.Repositories;
using DiskodePro.WebApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DiskodePro.WebApi.Controllers;

[ApiController]
[Route("api/posts")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostDTO post)
    {
        try
        {
            var createdPost = await _postRepository.CreatePostAsync(post);
            return CreatedAtAction(nameof(GetPostById), new { postId = createdPost.PostId }, createdPost);
        }
        catch (UserNotFoundException ex)
        {
            return BadRequest(ex.Message);
        } 
        catch (RepositoryException ex)
        {
            return StatusCode(500, $"An error occurred while creating post. {ex.Message}");
        }
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePost(int postId)
    {
        try
        {
            await _postRepository.DeletePostAsync(postId);
            return NoContent();
        }
        catch (PostNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, $"An error occurred while deleting post. {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        try
        {
            var posts = await _postRepository.GetAllPostsAsync();
            return Ok(posts);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, $"An error occurred while retrieving posts. {ex.Message}");
        }
    }

    [HttpGet("{postId}")]
    public async Task<IActionResult> GetPostById(int postId)
    {
        try
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            return Ok(post);
        }
        catch (PostNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, $"An error occurred while retrieving post. {ex.Message}");
            ;
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetPostsByUserId(int userId)
    {
        try
        {
            var posts = await _postRepository.GetPostsByUserIdAsync(userId);
            return Ok(posts);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, $"An error occurred while retrieving posts. {ex.Message}");
        }
    }

    [HttpPut("{postId}")]
    public async Task<IActionResult> UpdatePost(int postId, [FromBody] PostDTO updatedPost)
    {
        try
        {
            var post = await _postRepository.UpdatePostAsync(postId, updatedPost);
            return Ok(post);
        }
        catch (PostNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, $"An error occurred while updating post. {ex.Message}");
        }
    }
}