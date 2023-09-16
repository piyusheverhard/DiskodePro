using DiskodePro.WebApi.Models;

namespace DiskodePro.WebApi.Data.Repositories;

public interface IPostTagRepository
{
    Task<IEnumerable<string>> GetTagsAsync();
    Task<IEnumerable<Post>> GetPostsByTagNameAsync(string tagName);
    Task AddTagToPostAsync(string tagName, int postId);
}