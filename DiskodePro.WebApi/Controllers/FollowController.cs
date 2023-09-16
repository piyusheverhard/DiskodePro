using DiskodePro.WebApi.Data.Repositories;
using DiskodePro.WebApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DiskodePro.WebApi.Controllers;

[ApiController]
[Route("api/follows")]
public class FollowController : ControllerBase
{
    private readonly IUserFollowsUserRepository _userFollowsUserRepository;

    public FollowController(IUserFollowsUserRepository userFollowsUserRepository)
    {
        _userFollowsUserRepository = userFollowsUserRepository;
    }

    [HttpPost("{followerId}/{followeeId}/follow")]
    public async Task<IActionResult> FollowUser(int followerId, int followeeId)
    {
        try
        {
            await _userFollowsUserRepository.FollowUserAsync(followerId, followeeId);
            return Ok();
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while following the user.");
        }
    }

    [HttpPost("{followerId}/{followeeId}/unfollow")]
    public async Task<IActionResult> UnfollowUser(int followerId, int followeeId)
    {
        try
        {
            await _userFollowsUserRepository.UnfollowUserAsync(followerId, followeeId);
            return Ok();
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while unfollowing the user.");
        }
    }

    [HttpGet("{userId}/followers")]
    public async Task<IActionResult> GetFollowers(int userId)
    {
        try
        {
            var followers = await _userFollowsUserRepository.GetFollowersAsync(userId);
            return Ok(followers);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching followers.");
        }
    }

    [HttpGet("{userId}/following")]
    public async Task<IActionResult> GetFollowing(int userId)
    {
        try
        {
            var following = await _userFollowsUserRepository.GetFollowingAsync(userId);
            return Ok(following);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            return StatusCode(500, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching following users.");
        }
    }
}