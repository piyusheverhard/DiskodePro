using DiskodePro.WebApp.Models;

namespace DiskodePro.WebApp.Data.Repositories;

public interface IPostTagRepository
{
    Task<IEnumerable<string>> GetTagsAsync();
    Task<IEnumerable<Post>> GetPostsByTagNameAsync(string tagName);
    Task AddTagToPostAsync(string tagName, int postId);
}