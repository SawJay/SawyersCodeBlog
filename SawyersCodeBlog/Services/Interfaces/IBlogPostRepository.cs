using SawyersCodeBlog.Client.Models;
using SawyersCodeBlog.Model;

namespace SawyersCodeBlog.Services.Interfaces
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateBlogPostAsync (BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetBlogPostsAsync();
        Task<PagedList<BlogPost>> GetPublishedBlogPostsAsync(int page, int pageSize);
        Task<BlogPost?> GetBlogPostByIdAsync(int id);
        Task UpdateBlogPostAsync(BlogPost blogPost);
        Task DeleteBlogPostAsync(int blogPostId);
        Task IsDeleteBlogPostAsync(int blogPostId);
        Task AddTagsToBlogPostAsync (int blogPostId, IEnumerable<string> tagNames);
        Task RemoveTagsFromBlogPostAsync(int blogPostId);
        Task<BlogPost?> GetBlogPostBySlugAsync(string slug);
        Task<IEnumerable<BlogPost>> GetPopularBlogPostsAsync();
        Task<PagedList<BlogPost>> GetPostsByCategoryId(int categoryId, int page, int pageSize);
        Task<PagedList<BlogPost>> SearchBlogPostsAsync(string query, int page, int pageSize);
        Task<Tag?> GetTagByIdAsync(int tagId);
        Task<PagedList<BlogPost>> GetPostsByTagIdAsync(int tagId, int page, int pageSize);

    }
}
