﻿using SawyersCodeBlog.Model;

namespace SawyersCodeBlog.Services.Interfaces
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateBlogPostAsync (BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetBlogPostsAsync();
        Task<BlogPost?> GetBlogPostByIdAsync(int id);
        Task UpdateBlogPostAsync(BlogPost blogPost);
        Task DeleteBlogPostAsync(int blogPostId);
        Task IsDeleteBlogPostAsync(int blogPostId);
        Task AddTagsToBlogPostAsync (int blogPostId, IEnumerable<string> tagNames);
        Task RemoveTagsFromBlogPostAsync(int blogPostId);
        Task<BlogPost?> GetBlogPostBySlugAsync(string slug);
        Task<IEnumerable<BlogPost>> GetPopularBlogPostsAsync();

    }
}
