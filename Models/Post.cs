using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diskode.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        public required int CreatorId { get; set; }

        public required string Title { get; set; }

        public required string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey("CreatorId")]
        public User Creator { get; set; } = null!;

        public ICollection<UserLikedPosts> LikedByUsers { get; set; } = new HashSet<UserLikedPosts>();

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public ICollection<PostTag> Tags { get; set; } = new HashSet<PostTag>();
    }
}