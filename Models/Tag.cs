using System.ComponentModel.DataAnnotations;

namespace Diskode.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        [StringLength(maximumLength: 50, MinimumLength = 1)]
        public required string Name { get; set; }

        public ICollection<PostTags> Posts { get; set; } = new HashSet<PostTags>();
    }
}