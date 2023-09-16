using System.ComponentModel.DataAnnotations;

namespace DiskodePro.WebApi.Data.DTOs;

public class UserDTO
{
    [StringLength(50)] public required string Name { get; set; }

    [EmailAddress] [StringLength(50)] public required string Email { get; set; }

    [StringLength(50, MinimumLength = 8)] public required string Password { get; set; }
}