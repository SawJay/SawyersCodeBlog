using System.ComponentModel.DataAnnotations;

namespace SawyersCodeBlog.Client.Models
{
    public class CommentDTO
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

        public string? AuthorName { get; set; }
        public string? AuthorImageURL { get; set; }
        public string? AuthorId { get; set; }

        public int BlogPostId { get; set; }


    }
}