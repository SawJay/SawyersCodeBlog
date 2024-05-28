using Microsoft.EntityFrameworkCore;
using SawyersCodeBlog.Client.Components.Pages.Posts;
using SawyersCodeBlog.Data;
using SawyersCodeBlog.Model;
using SawyersCodeBlog.Services.Interfaces;

namespace SawyersCodeBlog.Services
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public CommentRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            comment.Created = DateTime.Now;

            context.Add(comment);
            await context.SaveChangesAsync();

            return comment;
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            Comment? comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment is not null)
            {
                context.Comments.Remove(comment);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            Comment? comment = await context.Comments.Include(c => c.BlogPost).FirstOrDefaultAsync(b => b.Id == id);

            return comment;
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync(int blogPostId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            IEnumerable<Comment> comments = await context.Comments.Where(c => c.BlogPostId == blogPostId).Include(c => c.Author).OrderByDescending(c => c.Created).ToListAsync();

            return comments;
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            bool shouldUpdate = await context.Comments.AnyAsync(c => c.Id == comment.Id);

            if (shouldUpdate)
            {
                context.Comments.Update(comment);
                await context.SaveChangesAsync();

            }
        }
    }
}
