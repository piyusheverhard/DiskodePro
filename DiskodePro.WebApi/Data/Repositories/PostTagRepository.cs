using DiskodePro.WebApi.Exceptions;
using DiskodePro.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DiskodePro.WebApi.Data.Repositories;

public class PostTagRepository : IPostTagRepository
{
    private readonly DiskodeDbContext _context;

    public PostTagRepository(DiskodeDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Post>> GetPostsByTagNameAsync(string tagName)
    {
        try
        {
            var posts = await _context.Set<Post>()
                .Where(p => p.Tags.Any(pt => pt.TagName == tagName))
                .ToListAsync();

            return posts;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while fetching posts by tag name.", ex);
        }
    }

    public async Task AddTagToPostAsync(string tagName, int postId)
    {
        try
        {
            var postTag = new PostTag
            {
                TagName = tagName,
                PostId = postId
            };

            await _context.Set<PostTag>().AddAsync(postTag);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while adding tag to post.", ex);
        }
    }

    public async Task<IEnumerable<string>> GetTagsAsync()
    {
        try
        {
            var tags = await _context.PostTags.Select(pt => pt.TagName).ToListAsync();
            return tags;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while fetching posts by tag name.", ex);
        }
    }
}