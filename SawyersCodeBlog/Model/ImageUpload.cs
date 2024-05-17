﻿using System.ComponentModel.DataAnnotations;

namespace SawyersCodeBlog.Model
{
    public class ImageUpload
    {
        public Guid Id { get; set; }

        [Required]
        public byte[]? Data { get; set; }

        [Required]
        public string? Type { get; set; }
    }
}
