using DiskodePro.WebApi.Data.DTOs;
using DiskodePro.WebApi.Data.Repositories;
using DiskodePro.WebApi.Exceptions;
using DiskodePro.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiskodePro.WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }
        catch (RepositoryException ex)
        {
            // Log the exception or return an appropriate error response
            return StatusCode(500, $"An error occurred while retrieving users. {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            // Log the exception or return an appropriate error response
            return StatusCode(500, $"An error occurred while retrieving user with ID {id}. {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserDTO user)
    {
        try
        {
            var createdUser = await _userRepository.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserId }, createdUser);
        }
        catch (DuplicateEmailException ex)
        {
            return Conflict(ex.Message);
        }
        catch (RepositoryException ex)
        {
            // Log the exception or return an appropriate error response
            return StatusCode(500, $"An error occurred while creating the user. {ex.Message}");
        }
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserDTO updatedUser)
    {
        try
        {
            var user = await _userRepository.UpdateUserAsync(userId, updatedUser);
            return Ok(user);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (DuplicateEmailException ex)
        {
            return Conflict(ex.Message);
        }
        catch (RepositoryException ex)
        {
            // Log the exception or return an appropriate error response
            return StatusCode(500, $"An error occurred while updating the user. {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await _userRepository.DeleteUserAsync(id);
            return NoContent();
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (RepositoryException ex)
        {
            // Log the exception or return an appropriate error response
            return StatusCode(500, $"An error occurred while deleting the user. {ex.Message}");
        }
    }
}