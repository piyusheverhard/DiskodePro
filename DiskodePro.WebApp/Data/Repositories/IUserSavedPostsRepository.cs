using DiskodePro.WebApp.Models;

namespace DiskodePro.WebApp.Data.Repositories;

public interface IUserSavedPostsRepository
{
    Task<IEnumerable<Post>> GetSavedPostsByUserAsync(int userId);
    Task SavePostAsync(int userId, int postId);
    Task UnsavePostAsync(int userId, int postId);
}