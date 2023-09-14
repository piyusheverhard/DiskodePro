using DiskodePro.WebApp.Exceptions;
using DiskodePro.WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DiskodePro.WebApp.Data.Repositories;

public class UserSavedPostsRepository : IUserSavedPostsRepository
{
    private readonly DiskodeDbContext _context;

    public UserSavedPostsRepository(DiskodeDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Post>> GetSavedPostsByUserAsync(int userId)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.SavedPosts)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) throw new UserNotFoundException($"User with ID {userId} not found.");

            return user.SavedPosts.Select(sp => sp.Post);
        }
        catch (UserNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while fetching saved posts.", ex);
        }
    }

    public async Task SavePostAsync(int userId, int postId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new UserNotFoundException($"User with ID {userId} not found.");

            var post = await _context.Posts.FindAsync(postId);
            if (post == null) throw new PostNotFoundException($"Post with ID {postId} not found.");

            var savedPost = new UserSavedPosts { UserId = userId, PostId = postId };
            _context.UserSavedPosts.Add(savedPost);
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
            throw new RepositoryException("An error occurred while saving the post.", ex);
        }
    }

    public async Task UnsavePostAsync(int userId, int postId)
    {
        try
        {
            var savedPost = await _context.UserSavedPosts
                .FirstOrDefaultAsync(sp => sp.UserId == userId && sp.PostId == postId);

            if (savedPost != null)
            {
                _context.UserSavedPosts.Remove(savedPost);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while removing the saved post.", ex);
        }
    }
}