using SawyersCodeBlog.Client.Models;

namespace SawyersCodeBlog.Client.Services.Interfaces
{
    public interface IBlogPostDTOService
    {
        Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPost);
    }
}
