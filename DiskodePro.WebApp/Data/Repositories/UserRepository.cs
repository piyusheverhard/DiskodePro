using DiskodePro.WebApp.Exceptions;
using DiskodePro.WebApp.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace DiskodePro.WebApp.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DiskodeDbContext _context;

    public UserRepository(DiskodeDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (DbUpdateException ex)
        {
            // Handle unique constraint violation exception
            if (IsDuplicateEmailViolation(ex)) throw new DuplicateEmailException("Email address is already taken.", ex);

            // Handle other database-related exceptions
            throw new RepositoryException("An error occurred while creating the user.", ex);
        }
        catch (DuplicateEmailException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // Handle any other exceptions
            throw new RepositoryException("Error occurred while creating the user.", ex);
        }
    }


    public async Task DeleteUserAsync(int userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null) throw new UserNotFoundException($"User with ID {userId} not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        catch (UserNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // Handle database update exception
            throw new RepositoryException("Error occurred while deleting the user.", ex);
        }
    }


    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        try
        {
            return await _context.Users.ToListAsync();
        }
        catch (Exception ex)
        {
            // Handle any other exceptions
            throw new RepositoryException("Error occurred while retrieving users.", ex);
        }
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null) throw new UserNotFoundException($"User with ID {userId} not found.");

            return user;
        }
        catch (UserNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // Handle any other exceptions
            throw new RepositoryException($"Error occurred while retrieving user with ID {userId}.", ex);
        }
    }

    public async Task<User> UpdateUserAsync(User updatedUser)
    {
        try
        {
            var userId = updatedUser.UserId;
            var existingUser = await _context.Users.FindAsync(userId);

            if (existingUser == null) throw new UserNotFoundException($"User with ID {userId} not found.");

            // Update the existing user's properties with the new values
            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
            // Update other properties as needed

            await _context.SaveChangesAsync();

            return existingUser;
        }
        catch (UserNotFoundException)
        {
            throw;
        }
        catch (DbUpdateException ex)
        {
            // Handle database update exceptions
            if (IsDuplicateEmailViolation(ex)) throw new DuplicateEmailException("Email already exists.", ex);

            throw new RepositoryException("Error occurred while updating the user.", ex);
        }
        catch (Exception ex)
        {
            // Handle any other exceptions
            throw new RepositoryException("Error occurred while updating the user.", ex);
        }
    }

    private bool IsDuplicateEmailViolation(DbUpdateException ex)
    {
        // Check if the exception is due to a duplicate email violation
        return ex.InnerException is MySqlException mysqlEx && mysqlEx.Number == (int)MySqlErrorCode.DuplicateKeyEntry;
    }
}