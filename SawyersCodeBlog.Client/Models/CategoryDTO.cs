using SawyersCodeBlog.Helpers;
using System.ComponentModel.DataAnnotations;

namespace SawyersCodeBlog.Client.Models
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required]
        [Length(2, 50, ErrorMessage = "The {0} must be between {1} and {2} characters")]
        public string? Name { get; set; }

        [Length(1, 200, ErrorMessage = "The {0} must be between {1} and {2} characters")]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; } = ImageHelper.DefaultContactImage;

        public ICollection<BlogPostDTO> Posts { get; set; } = new HashSet<BlogPostDTO>();
    }
}