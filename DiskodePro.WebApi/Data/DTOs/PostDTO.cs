namespace DiskodePro.WebApi.Data.DTOs;

public class PostDTO
{
    public required int CreatorId { get; set; }

    public required string Title { get; set; }

    public required string Content { get; set; }
}