using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using SawyersCodeBlog.Client.Models;
using SawyersCodeBlog.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SawyersCodeBlog.Model
{
    public class BlogPost
    {
        private DateTimeOffset _created;

        private DateTimeOffset? _updated;

        public int Id { get; set; }

        [Required]
        [Length(2, 200, ErrorMessage = "The {0} must be between {1} and {2} characters")]
        public string? Title { get; set; }

        [Required]
        public string? Slug { get; set; }

        [Required]
        [Length(2, 600, ErrorMessage = "The {0} must be between {1} and {2} characters")]
        public string? Abstract { get; set; }

        [Required]
        public string? Content { get; set; }
        
        [Required]
        public DateTimeOffset Created
        {
            get => _created;
            set => _created = value.ToUniversalTime();
        }

        public DateTimeOffset? Updated
        {
            get => _updated;
            set => _updated = value?.ToUniversalTime();
        }

        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }

        public Guid? ImageId { get; set; }

        public virtual ImageUpload? Image { get; set; }

        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

    }

    public static class BlogPostExtensions
    {
        public static BlogPostDTO ToDTO(this BlogPost post)
        {
            BlogPostDTO dTO = new BlogPostDTO()
            {
                Id = post.Id,
                Title = post.Title,
                Abstract = post.Abstract,
                Content = post.Content,
                Created = post.Created,
                Updated = post.Updated,
                IsPublished = post.IsPublished,
                IsDeleted = post.IsDeleted,
                ImageUrl = post.ImageId.HasValue ? $"api/uploads/{post.ImageId}" : UploadHelper.DefaultContactImage,
                CategoryId = post.CategoryId,
            };

            if(post.Category is not null)
            {
                post.Category.Posts.Clear();

                CategoryDTO categoryDTO = post.Category.ToDTO();
                dTO.Category = categoryDTO;
            }

            foreach (Comment comment in post.Comments)
            {
                post.Comments.Clear();

                CommentDTO commentDTO = comment.ToDTO();
                dTO.Comments.Add(commentDTO);
            }

            foreach (Tag tag in post.Tags)
            {
                post.Tags.Clear();

                TagDTO tagDTO = tag.ToDTO();
                dTO.Tags.Add(tagDTO);
            }

            return dTO;
        }
    }
}
