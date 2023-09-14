using DiskodePro.WebApp.Data.DTOs;
using DiskodePro.WebApp.Models;

namespace DiskodePro.WebApp.Data.Repositories;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAllPostsAsync();
    Task<Post> GetPostByIdAsync(int postId);
    Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId);
    Task<Post> CreatePostAsync(PostDTO post);
    Task<Post> UpdatePostAsync(int postId, PostDTO updatedPost);
    Task DeletePostAsync(int postId);
}