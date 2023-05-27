using Diskode.Models;

namespace Diskode.Data.Repositories;

public interface ICommentRepository
{
    Task<Comment> CreateRootCommentAsync(Comment comment);
    Task<Comment> CreateReplyAsync(Comment comment);
    Task<Comment> GetCommentByIdAsync(int commentId);
    Task<IEnumerable<Comment>> GetRootCommentsByPostIdAsync(int postId);
    Task<IEnumerable<Comment>> GetRepliesByParentCommentIdAsync(int parentCommentId);
    Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId);
    Task<Comment> UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(int commentId);
}