using System.ComponentModel.DataAnnotations.Schema;

namespace DiskodePro.WebApi.Models;

public class UserSavedPosts
{
    public int UserId { get; set; }

    [ForeignKey("UserId")] public User User { get; set; } = null!;

    public int PostId { get; set; }

    [ForeignKey("PostId")] public Post Post { get; set; } = null!;
}