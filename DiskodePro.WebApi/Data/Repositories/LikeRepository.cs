using DiskodePro.WebApi.Exceptions;
using DiskodePro.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DiskodePro.WebApi.Data.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly DiskodeDbContext _context;

    public LikeRepository(DiskodeDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>> GetLikedCommentsByUserAsync(int userId)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.LikedComments).ThenInclude(userLikedComments => userLikedComments.Comment)
                .FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) throw new UserNotFoundException($"User with ID {userId} not found.");

            var comments = user.LikedComments.Select(lc => lc.Comment);
            return comments;
        }
        catch (UserNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while fetching liked comments.", ex);
        }
    }

    public async Task<IEnumerable<Post>> GetLikedPostsByUserAsync(int userId)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.LikedPosts).ThenInclude(userLikedPosts => userLikedPosts.Post)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) throw new UserNotFoundException($"User with ID {userId} not found.");

            var likedPosts = user.LikedPosts.Select(lp => lp.Post);
            return likedPosts;
        }
        catch (UserNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while fetching liked posts.", ex);
        }
    }

    public async Task<IEnumerable<User>> GetUsersWhoLikedCommentAsync(int commentId)
    {
        try
        {
            var comment = await _context.Comments
                .Include(c => c.LikedByUsers).ThenInclude(userLikedComments => userLikedComments.User)
                .FirstOrDefaultAsync(c => c.CommentId == commentId);
            if (comment == null) throw new CommentNotFoundException($"Comment with ID {commentId} not found.");

            return comment.LikedByUsers.Select(lbu => lbu.User);
        }
        catch (CommentNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while fetching users who liked the comment.", ex);
        }
    }

    public async Task<IEnumerable<User>> GetUsersWhoLikedPostAsync(int postId)
    {
        try
        {
            var post = await _context.Posts
                .Include(p => p.LikedByUsers).ThenInclude(userLikedPosts => userLikedPosts.User)
                .FirstOrDefaultAsync(p => p.PostId == postId);

            if (post == null) throw new PostNotFoundException($"Post with ID {postId} not found.");

            return post.LikedByUsers.Select(lbu => lbu.User);
        }
        catch (PostNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while fetching users who liked the post.", ex);
        }
    }

    public async Task ToggleCommentLikeAsync(int userId, int commentId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new UserNotFoundException($"User with ID {userId} not found.");

            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) throw new CommentNotFoundException($"Comment with ID {commentId} not found.");

            var userLikedComment = await _context.UserLikedComments
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.CommentId == commentId);

            if (userLikedComment == null)
            {
                // Like does not exist, add a new like
                userLikedComment = new UserLikedComments { UserId = userId, CommentId = commentId };
                _context.UserLikedComments.Add(userLikedComment);
            }
            else
            {
                // Like exists, remove the existing like
                _context.UserLikedComments.Remove(userLikedComment);
            }

            await _context.SaveChangesAsync();
        }
        catch (UserNotFoundException)
        {
            throw;
        }
        catch (CommentNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while saving changes to the database.", ex);
        }
    }

    public async Task TogglePostLikeAsync(int userId, int postId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new UserNotFoundException($"User with ID {userId} not found.");

            var post = await _context.Posts.FindAsync(postId);
            if (post == null) throw new PostNotFoundException($"Post with ID {postId} not found.");

            var userLikedPost = await _context.UserLikedPosts
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.PostId == postId);

            if (userLikedPost == null)
            {
                // Like does not exist, add a new like
                userLikedPost = new UserLikedPosts { UserId = userId, PostId = postId };
                _context.UserLikedPosts.Add(userLikedPost);
            }
            else
            {
                // Like exists, remove the existing like
                _context.UserLikedPosts.Remove(userLikedPost);
            }

            await _context.SaveChangesAsync();
        }
        catch (UserNotFoundException)
        {
            throw;
        }
        catch (PostNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while saving changes to the database.", ex);
        }
    }
}