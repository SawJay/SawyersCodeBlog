using SawyersCodeBlog.Client.Models;
using System.Xml.Linq;

namespace SawyersCodeBlog.Client.Services.Interfaces
{
    public interface ICommentDTOService
    {
        Task<CommentDTO> CreateCommentAsync (CommentDTO commentDTO);
        Task<IEnumerable<CommentDTO>> GetCommentsAsync(int blogPostId);
        Task UpdateCommentAsync(CommentDTO commentDTO);
        Task DeleteCommentAsync(int commentId);
        Task<CommentDTO?> GetCommentByIdAsync(int id);
    }
}
