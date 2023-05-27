using Diskode.Models;
using Diskode.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Diskode.Data.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly DiskodeDbContext _context;

    public CommentRepository(DiskodeDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Comment> CreateReplyAsync(Comment reply)
    {
        try
        {
            var post = await _context.Posts.FindAsync(reply.PostId);
            var parentComment = await _context.Comments.FindAsync(reply.ParentCommentId);
            var user = await _context.Users.FindAsync(reply.CreatorId);

            if (post == null)
            {
                throw new PostNotFoundException($"Post with ID {reply.PostId} not found.");
            }

            if (parentComment == null)
            {
                throw new CommentNotFoundException($"Parent comment with ID {reply.ParentCommentId} not found.");
            }

            if (user == null)
            {
                throw new UserNotFoundException($"User with ID {reply.CreatorId} not found.");
            }

            reply.CreatedAt = DateTime.UtcNow;

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

    public async Task<Comment> CreateRootCommentAsync(Comment comment)
    {
        try
        {
            var post = await _context.Posts.FindAsync(comment.PostId);
            var user = await _context.Users.FindAsync(comment.CreatorId);

            if (post == null)
            {
                throw new PostNotFoundException($"Post with ID {comment.PostId} not found.");
            }

            if (user == null)
            {
                throw new UserNotFoundException($"User with ID {comment.CreatorId} not found.");
            }

            comment.CreatedAt = DateTime.UtcNow;

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

            if (comment == null)
            {
                throw new CommentNotFoundException($"Comment with ID {commentId} not found.");
            }

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

            if (comment == null)
            {
                throw new CommentNotFoundException($"Comment with ID {commentId} not found.");
            }

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

            if (user == null)
            {
                throw new UserNotFoundException($"User with ID {userId} not found.");
            }

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
            var parentComment = await _context.Comments.FindAsync(parentCommentId);
            if (parentComment == null)
            {
                throw new CommentNotFoundException($"Parent Comment with ID {parentCommentId} not found.");
            }

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
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
            {
                throw new PostNotFoundException($"Post with ID {postId} not found.");
            }

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

    public async Task<Comment> UpdateCommentAsync(Comment updatedComment)
    {
        try
        {
            var comment = await _context.Comments.FindAsync(updatedComment.CommentId);

            if (comment == null || comment.IsDeleted)
            {
                throw new CommentNotFoundException($"Comment with ID {updatedComment.CommentId} not found.");
            }

            // Update the comment properties
            comment.Content = updatedComment.Content;
            comment.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return updatedComment;
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