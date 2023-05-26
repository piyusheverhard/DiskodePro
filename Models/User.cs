using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diskode.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [StringLength(maximumLength: 50)]
        public required string Name { get; set; }


        [EmailAddress]
        [StringLength(maximumLength: 50)]
        public required string Email { get; set; }

        [StringLength(maximumLength: 50, MinimumLength = 8)]
        public required string Password { get; set; }

        public ICollection<Post> CreatedPosts { get; set; } = new HashSet<Post>();

        public ICollection<UserLikedPosts> LikedPosts { get; set; } = new HashSet<UserLikedPosts>();

        public ICollection<Comment> CreatedComments { get; set; } = new HashSet<Comment>();

        public ICollection<UserLikedComments> LikedComments { get; set; } = new HashSet<UserLikedComments>();

        [InverseProperty("Followee")]
        public ICollection<UsersFollowUsers> Followers { get; set; } = new HashSet<UsersFollowUsers>();

        [InverseProperty("Follower")]
        public ICollection<UsersFollowUsers> Following { get; set; } = new HashSet<UsersFollowUsers>();

        public ICollection<UserSavedPosts> SavedPosts { get; set; } = new HashSet<UserSavedPosts>();
    }
}