using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiskodePro.WebApp.Models;

public class PostTag
{
    [StringLength(50, MinimumLength = 1)] public string TagName { get; set; } = null!;

    public int PostId { get; set; }

    [ForeignKey("PostId")] public Post Post { get; set; } = null!;
}