using Humanizer;
using Microsoft.Extensions.Hosting;
using SawyersCodeBlog.Client.Models;
using SawyersCodeBlog.Helpers;
using System.ComponentModel.DataAnnotations;

namespace SawyersCodeBlog.Model
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Length(2, 50, ErrorMessage = "The {0} must be between {1} and {2} characters")]
        public string? Name { get; set; }

        [Length(1, 200, ErrorMessage = "The {0} must be between {1} and {2} characters")]
        public string? Description { get; set; }

        public Guid? ImageId { get; set; }

        public virtual ImageUpload? Image { get; set; }

        public virtual ICollection<BlogPost> Posts { get; set; } = new HashSet<BlogPost>();
    }

    public static class CategoryExtensions
    {
        public static CategoryDTO ToDTO(this Category category)
        {
            CategoryDTO dto = new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageId.HasValue ? $"api/uploads/{category.ImageId}" : UploadHelper.DefaultContactImage,
            };

            foreach (BlogPost post in category.Posts)
            {
                BlogPostDTO postDTO = post.ToDTO();
                dto.Posts.Add(postDTO);
            }

            return dto;
        }
    }
}