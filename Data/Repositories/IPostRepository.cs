using Diskode.Models;

namespace Diskode.Data.Repositories;
public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAllPostsAsync();
    Task<Post> GetPostByIdAsync(int postId);
    Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId);
    Task<Post> CreatePostAsync(Post post);
    Task<Post> UpdatePostAsync(Post updatedPost);
    Task DeletePostAsync(int postId);
}