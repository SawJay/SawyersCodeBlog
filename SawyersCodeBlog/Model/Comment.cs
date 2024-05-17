using SawyersCodeBlog.Client.Models;
using SawyersCodeBlog.Data;
using SawyersCodeBlog.Helpers;
using System.ComponentModel.DataAnnotations;

namespace SawyersCodeBlog.Model
{
    public class Comment
    {
        private DateTimeOffset? _updated;

        private DateTimeOffset _created;

        public int Id { get; set; }

        [Required]
        [Length(2, 5000, ErrorMessage = "The {0} must be between {1} and {2} characters")]
        public string? Content { get; set; }

        [Required]
        public DateTimeOffset Created
        {
            get => _created;
            set => _created = value.ToUniversalTime();
        }

        public DateTimeOffset? Updated
        {
            get => _updated?.ToLocalTime();
            set => _updated = value?.ToUniversalTime();
        }

        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters and at most {1} characters long.", MinimumLength = 2)]
        public string? UpdateReason { get; set; }

        [Required]
        public string? AuthorId { get; set; }

        public virtual ApplicationUser? Author { get; set; }

        public int BlogPostId { get; set; }

        public virtual BlogPost? BlogPost { get; set; }
    }

    public static class CommentExtensions
    {
        public static CommentDTO ToDTO(this Comment comment)
        {
            CommentDTO dto = new CommentDTO()
            {
                Id = comment.Id,
                Content = comment.Content,
                Created = comment.Created,
                Updated = comment.Updated,
                AuthorId = comment.AuthorId,
                BlogPostId = comment.BlogPostId,
                AuthorName = comment.Author?.FullName,
                AuthorImageURL = comment.Author?.ImageId.HasValue == true ? $"api/uploads/{comment.Author.ImageId}" : UploadHelper.DefaultContactImage,
            };

            return dto;
        }
    }
}