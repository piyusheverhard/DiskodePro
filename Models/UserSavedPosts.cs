using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Diskode.Models
{
    public class UserSavedPosts
    {
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; } = null!;


    }
}