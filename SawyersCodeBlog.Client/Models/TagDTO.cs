using System.ComponentModel.DataAnnotations;

namespace SawyersCodeBlog.Client.Models
{
    public class TagDTO
    {
        public int Id { get; set; }

        [Required]
        [Length(2, 50)]
        public string? Name { get; set; }
        public ICollection<BlogPostDTO> Posts { get; set; } = new HashSet<BlogPostDTO>();
    }
}