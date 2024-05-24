using SawyersCodeBlog.Model;

namespace SawyersCodeBlog.Services.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> CreateCommentAsync(Comment comment);
        Task<IEnumerable<Comment>> GetCommentsAsync();
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int commentId);
        Task<Comment?> GetCommentByIdAsync(int id);
    }
}
