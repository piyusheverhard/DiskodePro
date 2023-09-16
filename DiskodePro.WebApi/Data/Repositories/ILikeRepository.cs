using DiskodePro.WebApi.Models;

namespace DiskodePro.WebApi.Data.Repositories;

public interface ILikeRepository
{
    Task TogglePostLikeAsync(int userId, int postId);
    Task ToggleCommentLikeAsync(int userId, int commentId);
    Task<IEnumerable<Post>> GetLikedPostsByUserAsync(int userId);
    Task<IEnumerable<Comment>> GetLikedCommentsByUserAsync(int userId);
    Task<IEnumerable<User>> GetUsersWhoLikedPostAsync(int postId);
    Task<IEnumerable<User>> GetUsersWhoLikedCommentAsync(int commentId);
}