using SawyersCodeBlog.Client.Components.Pages.Posts;
using SawyersCodeBlog.Client.Models;

namespace SawyersCodeBlog.Client.Services.Interfaces
{
    public interface IBlogPostDTOService
    {
        Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPost);
        Task<IEnumerable<BlogPostDTO>> GetBlogPostsAsync();
        Task<PagedList<BlogPostDTO>> GetPublishedBlogPostsAsync(int page, int pageSize);
        Task<BlogPostDTO?> GetBlogPostByIdAsync(int id);
        Task UpdateBlogPostAsync(BlogPostDTO blogPostDTO);
        Task DeleteBlogPostAsync(int blogPostId);
        Task IsDeleteBlogPostAsync(int blogPostId);
        Task<BlogPostDTO?> GetBlogPostBySlugAsync(string slug);
        Task<IEnumerable<BlogPostDTO>> GetPopularBlogPostsAsync();
        Task<PagedList<BlogPostDTO>> GetPostsByCategoryId(int categoryId, int page, int pageSize);
        Task<PagedList<BlogPostDTO>> SearchBlogPostsAsync(string query, int page, int pageSize);
    }
}
