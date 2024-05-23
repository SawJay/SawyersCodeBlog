using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SawyersCodeBlog.Client.Models;
using SawyersCodeBlog.Data;
using SawyersCodeBlog.Helpers.Extensions;
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

            IEnumerable<BlogPost> blogPosts = await context.BlogPosts.Include(bp => bp.Category).Include(bp => bp.Comments).Include(bp => bp.Tags).ToListAsync();

            return blogPosts;
        }

        public async Task<BlogPost?> GetBlogPostByIdAsync(int id)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            BlogPost? blogPost = await context.BlogPosts.Include(b =>b.Tags).FirstOrDefaultAsync(b => b.Id == id);

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

        public async Task AddTagsToBlogPostAsync(int blogPostId, IEnumerable<string> tagNames)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();
            TextInfo textInfo = new CultureInfo("en-US").TextInfo;

            BlogPost? blogPost = await context.BlogPosts.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == blogPostId);

            if (blogPost is not null)
            {
                foreach(string tagName in  tagNames)
                {
                    Tag? existingTag = await context.Tags.FirstOrDefaultAsync(t => t.Name!.ToLower().Trim() == tagName.ToLower().Trim());

                    if (existingTag is not null)
                    {
                        blogPost.Tags.Add(existingTag);
                    }
                    else
                    {
                        string titleCaseTagName = textInfo.ToTitleCase(tagName.Trim());
                        Tag newTag = new Tag() { Name = titleCaseTagName };

                        context.Tags.Add(newTag);
                        blogPost.Tags.Add(newTag);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveTagsFromBlogPostAsync(int blogPostId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            BlogPost? blogPost = await context.BlogPosts.Include(b  => b.Tags).FirstOrDefaultAsync(b => b.Id == blogPostId);

            if (blogPost is not null)
            {
                blogPost.Tags.Clear();
                await context.SaveChangesAsync();

                // to do, delete tags that have no posts
            }
        }

        public async Task IsDeleteBlogPostAsync(int blogPostId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            BlogPost? blogPost = await context.BlogPosts.FirstOrDefaultAsync(b => b.Id == blogPostId);

            if (blogPost is not null)
            {
                blogPost.IsPublished = false;
                blogPost.IsDeleted = true;

                context.BlogPosts.Update(blogPost);
                await context.SaveChangesAsync();
            }
        }

        public async Task<BlogPost?> GetBlogPostBySlugAsync(string slug)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            BlogPost? blogPost = await context.BlogPosts.Where(b => b.IsPublished == true && b.IsDeleted == false)
                                                        .Include(bp => bp.Category).Include(bp => bp.Comments).ThenInclude(c => c.Author).Include(bp => bp.Tags).FirstOrDefaultAsync(b => b.Slug == slug);

            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetPopularBlogPostsAsync()
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            IEnumerable<BlogPost> posts = await context.BlogPosts.Where(c => c.IsPublished == true && c.IsDeleted == false)
                                                                 .Include(c => c.Category).Include(c => c.Tags)
                                                                 .Include(c => c.Comments).OrderByDescending(c => c.Comments.Count()).Take(4).ToListAsync();

            return posts;
        }

        public async Task<PagedList<BlogPost>> GetPublishedBlogPostsAsync(int page, int pageSize)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            PagedList<BlogPost> blogPosts = await context.BlogPosts.Where(c => c.IsPublished == true).Include(bp => bp.Category).Include(bp => bp.Comments).Include(bp => bp.Tags).OrderByDescending(b => b.Created).ToPagedListAsync(page, pageSize);

            return blogPosts;
        }
    }
}
