namespace DiskodePro.WebApp.Data.DTOs;

public class CommentDTO
{
    public required int CreatorId { get; set; }

    public required int PostId { get; set; }

    public required string Content { get; set; }

    public int? ParentCommentId { get; set; }
}