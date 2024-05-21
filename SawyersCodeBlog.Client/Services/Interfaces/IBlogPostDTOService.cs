using SawyersCodeBlog.Client.Models;

namespace SawyersCodeBlog.Client.Services.Interfaces
{
    public interface IBlogPostDTOService
    {
        Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPost);
        Task<IEnumerable<BlogPostDTO>> GetBlogPostsAsync();
        Task<BlogPostDTO?> GetBlogPostByIdAsync(int id);
        Task UpdateBlogPostAsync(BlogPostDTO blogPostDTO);
        Task DeleteBlogPostAsync(int blogPostId);
    }
}
