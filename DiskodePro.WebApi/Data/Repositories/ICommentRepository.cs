using DiskodePro.WebApi.Data.DTOs;
using DiskodePro.WebApi.Models;

namespace DiskodePro.WebApi.Data.Repositories;

public interface ICommentRepository
{
    Task<Comment> CreateRootCommentAsync(CommentDTO commentDto);
    Task<Comment> CreateReplyAsync(CommentDTO commentDto);
    Task<Comment> GetCommentByIdAsync(int commentId);
    Task<IEnumerable<Comment>> GetRootCommentsByPostIdAsync(int postId);
    Task<IEnumerable<Comment>> GetRepliesByParentCommentIdAsync(int parentCommentId);
    Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId);
    Task<Comment> UpdateCommentAsync(int commentId, CommentDTO commentDto);
    Task DeleteCommentAsync(int commentId);
}