using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SawyersCodeBlog.Client.Models
{
    public class BlogPostDTO
    {
        private DateTimeOffset? _updated;

        private DateTimeOffset _created;

        public string? ImageUrl { get; set; }

        public int Id { get; set; }

        [Required]
        [Length(2, 200, ErrorMessage = "The {0} must be between {1} and {2} characters")]
        public string? Title { get; set; }

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
            get => _updated?.ToLocalTime();
            set => _updated = value?.ToUniversalTime();
        }

        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }

        public int CategoryId { get; set; }

        public virtual CategoryDTO? Category { get; set; }
        public ICollection<CommentDTO> Comments { get; set; } = new HashSet<CommentDTO>();
        public ICollection<TagDTO> Tags { get; set; } = new HashSet<TagDTO>();
    }
}
