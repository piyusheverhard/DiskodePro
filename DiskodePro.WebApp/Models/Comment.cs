using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiskodePro.WebApp.Models;

public class Comment
{
    [Key] public int CommentId { get; set; }

    public required int CreatorId { get; set; }

    public required int PostId { get; set; }

    public required string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? ModifiedAt { get; set; }

    public bool IsDeleted { get; set; } = false;

    public int? ParentCommentId { get; set; }

    [ForeignKey("CreatorId")] public User Creator { get; set; } = null!;

    [ForeignKey("PostId")] public Post Post { get; set; } = null!;

    [ForeignKey("ParentCommentId")] public Comment? ParentComment { get; set; }

    [InverseProperty("ParentComment")] public ICollection<Comment> Replies { get; set; } = new HashSet<Comment>();

    public ICollection<UserLikedComments> LikedByUsers { get; set; } = new HashSet<UserLikedComments>();
}