using SawyersCodeBlog.Client.Models;
using SawyersCodeBlog.Client.Services.Interfaces;
using SawyersCodeBlog.Helpers;
using SawyersCodeBlog.Model;
using SawyersCodeBlog.Services.Interfaces;
using System.Linq.Expressions;

namespace SawyersCodeBlog.Services
{
    public class BlogPostDTOService : IBlogPostDTOService
    {
        private readonly IBlogPostRepository _repository;

        public BlogPostDTOService(IBlogPostRepository repository)
        {
            _repository = repository;
        }

        public async Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPostDto)
        {
            BlogPost newPost = new BlogPost()
            {
                Title = blogPostDto.Title,
                Abstract = blogPostDto.Abstract,
                Content = blogPostDto.Content,
                IsPublished = blogPostDto.IsPublished,
                CategoryId = blogPostDto.CategoryId,
                Created = DateTimeOffset.Now,
            };

            if (blogPostDto.ImageUrl?.StartsWith("data:") == true)
            {
                try
                {
                    newPost.Image = UploadHelper.GetImageUpload(blogPostDto.ImageUrl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            newPost = await _repository.CreateBlogPostAsync(newPost);
            return newPost.ToDTO();
            
        }
    }
}
