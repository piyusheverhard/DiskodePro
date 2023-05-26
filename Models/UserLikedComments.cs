using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diskode.Models
{
    public class UserLikedComments
    {
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public int CommentId { get; set; }

        [ForeignKey("CommentId")]
        public Comment Comment { get; set; } = null!;

    }
}