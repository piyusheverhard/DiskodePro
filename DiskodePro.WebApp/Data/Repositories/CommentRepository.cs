using DiskodePro.WebApp.Data.DTOs;
using DiskodePro.WebApp.Exceptions;
using DiskodePro.WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DiskodePro.WebApp.Data.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly DiskodeDbContext _context;

    public CommentRepository(DiskodeDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Comment> CreateReplyAsync(CommentDTO replyCommentDto)
    {
        try
        {
            var post = await _context.Posts.FindAsync(replyCommentDto.PostId);
            var parentComment = await _context.Comments.FindAsync(replyCommentDto.ParentCommentId);
            var user = await _context.Users.FindAsync(replyCommentDto.CreatorId);

            if (post == null) throw new PostNotFoundException($"Post with ID {replyCommentDto.PostId} not found.");

            if (parentComment == null)
                throw new CommentNotFoundException(
                    $"Parent comment with ID {replyCommentDto.ParentCommentId} not found.");

            if (user == null) throw new UserNotFoundException($"User with ID {replyCommentDto.CreatorId} not found.");

            var reply = new Comment
            {
                PostId = replyCommentDto.PostId,
                ParentCommentId = replyCommentDto.ParentCommentId,
                CreatorId = replyCommentDto.CreatorId,
                Content = replyCommentDto.Content
            };
            _context.Comments.Add(reply);
            await _context.SaveChangesAsync();

            return reply;
        }
        catch (PostNotFoundException)
        {
            throw; // Rethrow the exception to propagate it up the call stack
        }
        catch (CommentNotFoundException)
        {
            throw; // Rethrow the exception to propagate it up the call stack
        }
        catch (UserNotFoundException)
        {
            throw; // Rethrow the exception to propagate it up the call stack
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while creating the reply.", ex);
        }
    }

    public async Task<Comment> CreateRootCommentAsync(CommentDTO commentDto)
    {
        try
        {
            var post = await _context.Posts.FindAsync(commentDto.PostId);
            var user = await _context.Users.FindAsync(commentDto.CreatorId);

            if (post == null) throw new PostNotFoundException($"Post with ID {commentDto.PostId} not found.");

            if (user == null) throw new UserNotFoundException($"User with ID {commentDto.CreatorId} not found.");

            var comment = new Comment
            {
                CreatorId = commentDto.CreatorId,
                PostId = commentDto.PostId,
                Content = commentDto.Content
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return comment;
        }
        catch (PostNotFoundException)
        {
            throw; // Rethrow the exception to propagate it up the call stack
        }
        catch (UserNotFoundException)
        {
            throw; // Rethrow the exception to propagate it up the call stack
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while creating the root comment.", ex);
        }
    }

    public async Task DeleteCommentAsync(int commentId)
    {
        try
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null) throw new CommentNotFoundException($"Comment with ID {commentId} not found.");

            comment.IsDeleted = true;

            await _context.SaveChangesAsync();
        }
        catch (CommentNotFoundException)
        {
            throw; // Rethrow the exception to propagate it up the call stack
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while deleting the comment.", ex);
        }
    }

    public async Task<Comment> GetCommentByIdAsync(int commentId)
    {
        try
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null) throw new CommentNotFoundException($"Comment with ID {commentId} not found.");

            return comment;
        }
        catch (CommentNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // Handle any other exceptions
            throw new RepositoryException($"Error occurred while retrieving comment with ID {commentId}.", ex);
        }
    }

    public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId)
    {
        try
        {
            var user = await _context.Users
                .Include(user => user.CreatedComments)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) throw new UserNotFoundException($"User with ID {userId} not found.");

            var comments = user.CreatedComments.ToList();
            return comments;
        }
        catch (UserNotFoundException)
        {
            throw; // Re-throw the UserNotFoundException as it is
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while retrieving comments by user ID.", ex);
        }
    }

    public async Task<IEnumerable<Comment>> GetRepliesByParentCommentIdAsync(int parentCommentId)
    {
        try
        {
            var parentComment = await _context.Comments.Include(c => c.Replies)
                .FirstOrDefaultAsync(c => c.CommentId == parentCommentId);
            if (parentComment == null)
                throw new CommentNotFoundException($"Parent Comment with ID {parentCommentId} not found.");

            var replies = parentComment.Replies.ToList();

            return replies;
        }
        catch (CommentNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while retrieving comment replies.", ex);
        }
    }

    public async Task<IEnumerable<Comment>> GetRootCommentsByPostIdAsync(int postId)
    {
        try
        {
            var post = await _context.Posts.Include(p => p.Comments)
                .FirstOrDefaultAsync(c => c.PostId == postId);
            if (post == null) throw new PostNotFoundException($"Post with ID {postId} not found.");

            var rootComments = post.Comments.Where(c => c.ParentCommentId == null).ToList();

            return rootComments;
        }
        catch (PostNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while retrieving root comments.", ex);
        }
    }

    public async Task<Comment> UpdateCommentAsync(int commentId, CommentDTO updatedCommentDto)
    {
        try
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null || comment.IsDeleted)
                throw new CommentNotFoundException($"Comment with ID {commentId} not found.");

            // Update the comment properties
            comment.Content = updatedCommentDto.Content;
            comment.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return comment;
        }
        catch (CommentNotFoundException)
        {
            throw; // Rethrow the exception to propagate it up the call stack
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while updating the comment.", ex);
        }
    }
}