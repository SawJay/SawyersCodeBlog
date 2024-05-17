using Humanizer;
using Microsoft.Extensions.Hosting;
using SawyersCodeBlog.Client.Models;
using System.ComponentModel.DataAnnotations;

namespace SawyersCodeBlog.Model
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [Length(2, 50)]
        public string? Name { get; set; }
        public virtual ICollection<BlogPost> Posts { get; set; } = new HashSet<BlogPost>();

    }

    public static class TagExtensions
    {
        public static TagDTO ToDTO(this Tag tag)
        {
            TagDTO dto = new TagDTO()
            {
                Id = tag.Id,
                Name = tag.Name,
            };

            foreach (BlogPost post in tag.Posts)
            {
                BlogPostDTO postDTO = post.ToDTO();
                dto.Posts.Add(postDTO);
            }

            return dto;
        }
    }
}