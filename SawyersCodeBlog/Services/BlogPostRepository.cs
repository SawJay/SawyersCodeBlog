﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SawyersCodeBlog.Data;
using SawyersCodeBlog.Model;
using SawyersCodeBlog.Services.Interfaces;
using System.Globalization;
using System.Runtime.InteropServices.Marshalling;
using System.Text.RegularExpressions;

namespace SawyersCodeBlog.Services
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public BlogPostRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            blogPost.Created = DateTimeOffset.Now;
            blogPost.Slug = await GenerateSlugAsync(blogPost.Title!, 0);

            context.Add(blogPost);
            await context.SaveChangesAsync();

            return blogPost;
        }

        private async Task<string> GenerateSlugAsync(string title, int id)
        {
            if (await ValidateSlugAsync(title, id))
            {
                return Slugify(title);
            }
            else
            {
                int i = 2;
                string newTitle = $"{title} {i}";
                bool isValid = await ValidateSlugAsync(newTitle, id);

                while (isValid == false)
                {
                    i++;
                    newTitle = $"{title} {i}";
                    isValid = await ValidateSlugAsync(newTitle, id);
                }

                return Slugify(newTitle);
            }
        }

        private async Task<bool> ValidateSlugAsync(string title, int blogId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            string newSlug = Slugify(title);
            bool isValid = false;

            if (blogId == 0)
            {
                // new post, check to see if slug is used
                isValid = !await context.BlogPosts.AnyAsync(bp => bp.Slug == newSlug);
            }
            else
            {
                // existing post, see if other posts have this
                isValid = !await context.BlogPosts.AnyAsync(bp => bp.Slug == newSlug && bp.Id != blogId);
            }

            return isValid;
        }

        private static string Slugify(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return title;
            }

            title = title.Normalize(System.Text.NormalizationForm.FormD);
            char[] chars = title.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();

            string normalizedTitle = new string(chars).Normalize(System.Text.NormalizationForm.FormC).ToLower().Trim();

            string titleWithoutSymbols = Regex.Replace(normalizedTitle, @"[^A-Za-z0-9\s-]", "");
            string slug = Regex.Replace(titleWithoutSymbols, @"\s+", "-");

            return slug;

        }

        public async Task<IEnumerable<BlogPost>> GetBlogPostsAsync()
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            IEnumerable<BlogPost> blogPosts = await context.BlogPosts.Include(bp => bp.Category).Include(bp => bp.Comments).ToListAsync();

            return blogPosts;
        }

        public async Task<BlogPost?> GetBlogPostByIdAsync(int id)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            BlogPost? blogPost = await context.BlogPosts.FirstOrDefaultAsync(b => b.Id == id);

            return blogPost;
        }

        public async Task UpdateBlogPostAsync(BlogPost blogPost)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            bool shouldUpdate = await context.BlogPosts.AnyAsync(b => b.Id == blogPost.Id);

            if (shouldUpdate)
            {
                ImageUpload? oldImage = null;

                if (blogPost.Image is not null)
                {
                    // save the new image
                    context.Images.Add(blogPost.Image);

                    // check for an old image
                    oldImage = await context.Images.FirstOrDefaultAsync(i => i.Id == blogPost.ImageId);

                    // fix the foreign key
                    blogPost.ImageId = blogPost.Image.Id;
                }

                blogPost.Slug = await GenerateSlugAsync(blogPost.Title!, blogPost.Id);

                context.BlogPosts.Update(blogPost);
                await context.SaveChangesAsync();

                if (oldImage is not null)
                {
                    context.Images.Remove(oldImage);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteBlogPostAsync(int blogPostId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            BlogPost? blogPost = await context.BlogPosts.FirstOrDefaultAsync(b => b.Id == blogPostId);

            if (blogPost is not null)
            {
                context.BlogPosts.Remove(blogPost);
                await context.SaveChangesAsync();
            }
        }

    }
}