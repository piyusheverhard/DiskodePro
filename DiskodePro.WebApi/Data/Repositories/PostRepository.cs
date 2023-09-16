using DiskodePro.WebApi.Data.DTOs;
using DiskodePro.WebApi.Exceptions;
using DiskodePro.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DiskodePro.WebApi.Data.Repositories;

public class PostRepository : IPostRepository
{
    private readonly DiskodeDbContext _context;

    public PostRepository(DiskodeDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Post> CreatePostAsync(PostDTO postDto)
    {
        try
        {
            var post = new Post
            {
                CreatorId = postDto.CreatorId,
                Title = postDto.Title,
                Content = postDto.Content
            };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryException("An error occurred while creating the post.", ex);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while creating the post.", ex);
        }
    }


    public async Task DeletePostAsync(int postId)
    {
        try
        {
            var post = await _context.Posts.FindAsync(postId);

            if (post == null) throw new PostNotFoundException($"Post with ID {postId} not found.");

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
        catch (PostNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // Handle database update exception
            throw new RepositoryException("Error occurred while deleting the post.", ex);
        }
    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync()
    {
        try
        {
            return await _context.Posts.ToListAsync();
        }
        catch (Exception ex)
        {
            // Handle any other exceptions
            throw new RepositoryException("Error occurred while retrieving posts.", ex);
        }
    }

    public async Task<Post> GetPostByIdAsync(int postId)
    {
        try
        {
            var post = await _context.Posts.FindAsync(postId);

            if (post == null) throw new PostNotFoundException($"Post with ID {postId} not found.");

            return post;
        }
        catch (PostNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // Handle any other exceptions
            throw new RepositoryException($"Error occurred while retrieving post with ID {postId}.", ex);
        }
    }

    public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId)
    {
        try
        {
            var user = await _context.Users
                .Include(user => user.CreatedPosts)
                .FirstOrDefaultAsync(user => user.UserId == userId);

            if (user == null) throw new UserNotFoundException($"User with ID {userId} does not exist.");

            return user.CreatedPosts.ToList();
        }
        catch (UserNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while retrieving posts by user ID.", ex);
        }
    }

    public async Task<Post> UpdatePostAsync(int postId, PostDTO updatedPost)
    {
        try
        {
            var existingPost = await _context.Posts.FindAsync(postId);

            if (existingPost == null)
                throw new PostNotFoundException($"Post with ID {postId} does not exist.");

            existingPost.Title = updatedPost.Title;
            existingPost.Content = updatedPost.Content;
            existingPost.ModifiedAt = DateTime.UtcNow; // Update the ModifiedAt property

            await _context.SaveChangesAsync();

            return existingPost;
        }
        catch (PostNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while updating the post.", ex);
        }
    }
}