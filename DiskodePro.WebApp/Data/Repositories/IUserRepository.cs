using DiskodePro.WebApp.Data.DTOs;
using DiskodePro.WebApp.Models;

namespace DiskodePro.WebApp.Data.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int userId);
    Task<User> CreateUserAsync(UserDTO user);
    Task<User> UpdateUserAsync(int userId, UserDTO user);
    Task DeleteUserAsync(int userId);
}