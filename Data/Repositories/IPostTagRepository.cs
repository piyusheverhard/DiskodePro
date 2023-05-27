using Diskode.Models;

namespace Diskode.Data.Repositories;

public interface IPostTagRepository
{
    Task<IEnumerable<Post>> GetPostsByTagNameAsync(string tagName);
    Task AddTagToPostAsync(string tagName, int postId);
}
