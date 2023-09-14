using System.ComponentModel.DataAnnotations.Schema;

namespace DiskodePro.WebApp.Models;

public class UsersFollowUsers
{
    public int FollowerId { get; set; }

    [ForeignKey("FollowerId")] public User Follower { get; set; } = null!;

    public int FolloweeId { get; set; }

    [ForeignKey("FolloweeId")] public User Followee { get; set; } = null!;
}