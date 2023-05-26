using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Diskode.Models
{
    public class PostTags
    {
        public int TagId { get; set; }

        [ForeignKey("TagId")]
        public Tag Tag { get; set; } = null!;

        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; } = null!;


    }
}