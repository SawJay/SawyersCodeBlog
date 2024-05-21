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

        public async Task DeleteBlogPostAsync(int blogPostId)
        {
            await _repository.DeleteBlogPostAsync(blogPostId);
        }

        public async Task<BlogPostDTO?> GetBlogPostByIdAsync(int id)
        {
            BlogPost? blogPost = await _repository.GetBlogPostByIdAsync(id);
            return blogPost?.ToDTO();
        }

        public async Task<IEnumerable<BlogPostDTO>> GetBlogPostsAsync()
        {
            IEnumerable<BlogPost> blogPosts = await _repository.GetBlogPostsAsync();

            IEnumerable<BlogPostDTO> blogPostDTOs = blogPosts.Select(c => c.ToDTO());

            return blogPostDTOs;
        }

        public async Task UpdateBlogPostAsync(BlogPostDTO blogPostDTO)
        {
            BlogPost? blogPostToUpdate = await _repository.GetBlogPostByIdAsync(blogPostDTO.Id);

            if (blogPostToUpdate is not null)
            {
                blogPostToUpdate.Title = blogPostDTO.Title;
                blogPostToUpdate.Abstract = blogPostDTO.Abstract;
                blogPostToUpdate.IsPublished = blogPostDTO.IsPublished;
                blogPostToUpdate.Content = blogPostDTO.Content;
                blogPostToUpdate.CategoryId = blogPostDTO.CategoryId;

                if (blogPostDTO.ImageUrl!.StartsWith("data:"))
                {
                    blogPostToUpdate.Image = UploadHelper.GetImageUpload(blogPostDTO.ImageUrl);
                }
                else
                {
                    blogPostToUpdate.Image = null;
                }

                await _repository.UpdateBlogPostAsync(blogPostToUpdate);
            }
        }
    }
}
