using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Diskode.Models
{
    public class UsersFollowUsers
    {
        public int FollowerId { get; set; }

        [ForeignKey("FollowerId")]
        public User Follower { get; set; } = null!;

        public int FolloweeId { get; set; }

        [ForeignKey("FolloweeId")]
        public User Followee { get; set; } = null!;


    }
}