using DiskodePro.WebApi.Data.Repositories;
using DiskodePro.WebApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DiskodePro.WebApi.Controllers;

[ApiController]
[Route("api/likes")]
public class LikeController : ControllerBase
{
    private readonly ILikeRepository _likeRepository;

    public LikeController(ILikeRepository likeRepository)
    {
        _likeRepository = likeRepository;
    }

    [HttpGet("comments/{userId}")]
    public async Task<IActionResult> GetLikedCommentsByUser(int userId)
    {
        try
        {
            var comments = await _likeRepository.GetLikedCommentsByUserAsync(userId);
            return Ok(comments);
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
            return StatusCode(500, "An error occurred while fetching liked comments.");
        }
    }

    [HttpGet("posts/{userId}")]
    public async Task<IActionResult> GetLikedPostsByUser(int userId)
    {
        try
        {
            var posts = await _likeRepository.GetLikedPostsByUserAsync(userId);
            return Ok(posts);
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
            return StatusCode(500, "An error occurred while fetching liked posts.");
        }
    }

    [HttpGet("comments/{commentId}")]
    public async Task<IActionResult> GetUsersWhoLikedComment(int commentId)
    {
        try
        {
            var users = await _likeRepository.GetUsersWhoLikedCommentAsync(commentId);
            return Ok(users);
        }
        catch (CommentNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching users who liked the comment.");
        }
    }

    [HttpGet("posts/{postId}")]
    public async Task<IActionResult> GetUsersWhoLikedPost(int postId)
    {
        try
        {
            var users = await _likeRepository.GetUsersWhoLikedPostAsync(postId);
            return Ok(users);
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
            return StatusCode(500, "An error occurred while fetching users who liked the post.");
        }
    }

    [HttpPost("comments/{userId}/{commentId}/toggle")]
    public async Task<IActionResult> ToggleCommentLike(int userId, int commentId)
    {
        try
        {
            await _likeRepository.ToggleCommentLikeAsync(userId, commentId);
            return Ok();
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (CommentNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while toggling comment like.");
        }
    }

    [HttpPost("posts/{userId}/{postId}/toggle")]
    public async Task<IActionResult> TogglePostLike(int userId, int postId)
    {
        try
        {
            await _likeRepository.TogglePostLikeAsync(userId, postId);
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
            return StatusCode(500, "An error occurred while toggling post like.");
        }
    }
}