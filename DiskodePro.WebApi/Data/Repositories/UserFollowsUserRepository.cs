using DiskodePro.WebApi.Exceptions;
using DiskodePro.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DiskodePro.WebApi.Data.Repositories;

public class UserFollowsUserRepository : IUserFollowsUserRepository
{
    private readonly DiskodeDbContext _context;

    public UserFollowsUserRepository(DiskodeDbContext context)
    {
        _context = context;
    }

    public async Task FollowUserAsync(int followerId, int followeeId)
    {
        try
        {
            var follower = await _context.Users.FindAsync(followerId);
            if (follower == null) throw new UserNotFoundException($"User with ID {followerId} not found.");

            var followee = await _context.Users.FindAsync(followeeId);
            if (followee == null) throw new UserNotFoundException($"User with ID {followeeId} not found.");

            var userFollowsUser = new UsersFollowUsers { FollowerId = followerId, FolloweeId = followeeId };
            _context.UsersFollowUsers.Add(userFollowsUser);
            await _context.SaveChangesAsync();
        }
        catch (UserNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while following the user.", ex);
        }
    }

    public async Task UnfollowUserAsync(int followerId, int followeeId)
    {
        try
        {
            var follower = await _context.Users.FindAsync(followerId);
            if (follower == null) throw new UserNotFoundException($"User with ID {followerId} not found.");

            var followee = await _context.Users.FindAsync(followeeId);
            if (followee == null) throw new UserNotFoundException($"User with ID {followeeId} not found.");

            var userFollowsUser = await _context.UsersFollowUsers
                .FirstOrDefaultAsync(u => u.FollowerId == followerId && u.FolloweeId == followeeId);

            if (userFollowsUser != null)
            {
                _context.UsersFollowUsers.Remove(userFollowsUser);
                await _context.SaveChangesAsync();
            }
        }
        catch (UserNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while unfollowing the user.", ex);
        }
    }


    public async Task<IEnumerable<User>> GetFollowersAsync(int userId)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.Followers)
                .ThenInclude(ufu => ufu.Follower)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) throw new UserNotFoundException($"User with ID {userId} not found.");

            return user.Followers.Select(ufu => ufu.Follower);
        }
        catch (UserNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while fetching followers.", ex);
        }
    }

    public async Task<IEnumerable<User>> GetFollowingAsync(int userId)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.Following)
                .ThenInclude(ufu => ufu.Followee)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) throw new UserNotFoundException($"User with ID {userId} not found.");

            return user.Following.Select(ufu => ufu.Followee);
        }
        catch (UserNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while fetching following users.", ex);
        }
    }
}