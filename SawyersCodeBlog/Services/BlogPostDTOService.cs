﻿using SawyersCodeBlog.Client.Models;
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

            IEnumerable<string> tagNames = blogPostDto.Tags.Select(t => t.Name!);
            await _repository.AddTagsToBlogPostAsync(newPost.Id, tagNames);

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

        public async Task<BlogPostDTO?> GetBlogPostBySlugAsync(string slug)
        {
            BlogPost? blogPost = await _repository.GetBlogPostBySlugAsync(slug);
            return blogPost?.ToDTO();
        }

        public async Task<IEnumerable<BlogPostDTO>> GetBlogPostsAsync()
        {
            IEnumerable<BlogPost> blogPosts = await _repository.GetBlogPostsAsync();

            IEnumerable<BlogPostDTO> blogPostDTOs = blogPosts.Select(c => c.ToDTO());

            return blogPostDTOs;
        }

        public async Task<IEnumerable<BlogPostDTO>> GetPopularBlogPostsAsync()
        {
            IEnumerable<BlogPost> blogPosts = await _repository.GetPopularBlogPostsAsync();

            IEnumerable<BlogPostDTO> blogPostDTOs = blogPosts.Select(c => c.ToDTO());

            return blogPostDTOs;
        }

        public async Task<PagedList<BlogPostDTO>> GetPostsByCategoryId(int categoryId, int page, int pageSize)
        {

            PagedList<BlogPost> post = await _repository.GetPostsByCategoryId(categoryId, page, pageSize);

            PagedList<BlogPostDTO> postDTO = new PagedList<BlogPostDTO>()
            {
                Page = post.Page,
                TotalPages = post.TotalPages,
                TotalItems = post.TotalItems,
                Data = post.Data.Select(p => p.ToDTO())
            };

            return postDTO;

        }

        public async Task<PagedList<BlogPostDTO>> GetPostsByTagIdAsync(int tagId, int page, int pageSize)
        {
            PagedList<BlogPost> post = await _repository.GetPostsByTagIdAsync(tagId, page, pageSize);

            PagedList<BlogPostDTO> postDTO = new PagedList<BlogPostDTO>()
            {
                Page = post.Page,
                TotalPages = post.TotalPages,
                TotalItems = post.TotalItems,
                Data = post.Data.Select(p => p.ToDTO())
            };

            return postDTO;
        }

        public async Task<PagedList<BlogPostDTO>> GetPublishedBlogPostsAsync(int page, int pageSize)
        {
            PagedList<BlogPost> post = await _repository.GetPublishedBlogPostsAsync(page, pageSize);

            PagedList<BlogPostDTO> postDTO = new PagedList<BlogPostDTO>()
            {
                Page = post.Page,
                TotalPages = post.TotalPages,
                TotalItems = post.TotalItems,
                Data = post.Data.Select(p => p.ToDTO())
            };

            return postDTO;
        }

        public async Task<TagDTO?> GetTagByIdAsync(int tagId)
        {
            Tag? tag = await _repository.GetTagByIdAsync(tagId);

            return tag?.ToDTO();
        }

        public async Task IsDeleteBlogPostAsync(int blogPostId)
        {
            await _repository.IsDeleteBlogPostAsync(blogPostId);
        }

        public async Task<PagedList<BlogPostDTO>> SearchBlogPostsAsync(string query, int page, int pageSize)
        {
            PagedList<BlogPost> post = await _repository.SearchBlogPostsAsync(query, page, pageSize);

            PagedList<BlogPostDTO> postDTO = new PagedList<BlogPostDTO>()
            {
                Page = post.Page,
                TotalPages = post.TotalPages,
                TotalItems = post.TotalItems,
                Data = post.Data.Select(p => p.ToDTO())
            };

            return postDTO;
        }

        public async Task UpdateBlogPostAsync(BlogPostDTO blogPostDTO)
        {
            await _repository.RemoveTagsFromBlogPostAsync(blogPostDTO.Id);

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

                IEnumerable<string> selectedTags = blogPostDTO.Tags.Select(t => t.Name!);
                await _repository.AddTagsToBlogPostAsync(blogPostToUpdate.Id, selectedTags);
            }
        }
    }
}
