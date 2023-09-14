using DiskodePro.WebApp.Models;

namespace DiskodePro.WebApp.Data.Repositories;

public interface IUserFollowsUserRepository
{
    Task FollowUserAsync(int followerId, int followeeId);
    Task UnfollowUserAsync(int followerId, int followeeId);
    Task<IEnumerable<User>> GetFollowersAsync(int userId);
    Task<IEnumerable<User>> GetFollowingAsync(int userId);
}