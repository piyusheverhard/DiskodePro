using DiskodePro.WebApi.Data.DTOs;
using DiskodePro.WebApi.Models;

namespace DiskodePro.WebApi.Data.Repositories;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAllPostsAsync();
    Task<Post> GetPostByIdAsync(int postId);
    Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId);
    Task<Post> CreatePostAsync(PostDTO post);
    Task<Post> UpdatePostAsync(int postId, PostDTO updatedPost);
    Task DeletePostAsync(int postId);
}