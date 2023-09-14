using DiskodePro.WebApp.Data.DTOs;
using DiskodePro.WebApp.Data.Repositories;
using DiskodePro.WebApp.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DiskodePro.WebApp.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRootComment([FromBody] CommentDTO comment)
    {
        try
        {
            var createdComment = await _commentRepository.CreateRootCommentAsync(comment);
            return CreatedAtAction(nameof(GetCommentById), new { commentId = createdComment.CommentId },
                createdComment);
        }
        catch (PostNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UserNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("{parentCommentId}/replies")]
    public async Task<IActionResult> CreateReply(int parentCommentId, [FromBody] CommentDTO reply)
    {
        reply.ParentCommentId = parentCommentId;

        try
        {
            var createdReply = await _commentRepository.CreateReplyAsync(reply);
            return CreatedAtAction(nameof(GetCommentById), new { commentId = createdReply.CommentId }, createdReply);
        }
        catch (PostNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (CommentNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UserNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{commentId}")]
    public async Task<IActionResult> GetCommentById(int commentId)
    {
        try
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            return Ok(comment);
        }
        catch (CommentNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetCommentsByUserId(int userId)
    {
        try
        {
            var comments = await _commentRepository.GetCommentsByUserIdAsync(userId);
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
    }

    [HttpGet("{parentCommentId}/replies")]
    public async Task<IActionResult> GetRepliesByParentCommentId(int parentCommentId)
    {
        try
        {
            var replies = await _commentRepository.GetRepliesByParentCommentIdAsync(parentCommentId);
            return Ok(replies);
        }
        catch (CommentNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("posts/{postId}/root")]
    public async Task<IActionResult> GetRootCommentsByPostId(int postId)
    {
        try
        {
            var rootComments = await _commentRepository.GetRootCommentsByPostIdAsync(postId);
            return Ok(rootComments);
        }
        catch (PostNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{commentId}")]
    public async Task<IActionResult> UpdateComment(int commentId, [FromBody] CommentDTO updatedCommentDto)
    {
        try
        {
            var comment = await _commentRepository.UpdateCommentAsync(commentId, updatedCommentDto);
            return Ok(comment);
        }
        catch (CommentNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{commentId}")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        try
        {
            await _commentRepository.DeleteCommentAsync(commentId);
            return NoContent();
        }
        catch (CommentNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}