using SawyersCodeBlog.Client.Components.Pages.Posts;
using SawyersCodeBlog.Client.Models;
using SawyersCodeBlog.Client.Services.Interfaces;
using SawyersCodeBlog.Helpers;
using SawyersCodeBlog.Model;
using SawyersCodeBlog.Services.Interfaces;

namespace SawyersCodeBlog.Services
{
    public class CommentDTOService : ICommentDTOService
    {
        private readonly ICommentRepository _repository;

        public CommentDTOService(ICommentRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommentDTO> CreateCommentAsync(CommentDTO commentDTO)
        {
            Comment newComment = new Comment()
            {
                Content = commentDTO.Content,
                AuthorId = commentDTO.AuthorId,
                Created = DateTimeOffset.Now,
                BlogPostId = commentDTO.BlogPostId,
            };

            newComment = await _repository.CreateCommentAsync(newComment);

            return newComment.ToDTO();
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            await _repository.DeleteCommentAsync(commentId);
        }

        public async Task<CommentDTO?> GetCommentByIdAsync(int id)
        {
            Comment? comment = await _repository.GetCommentByIdAsync(id);
            return comment?.ToDTO();
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsAsync()
        {
            {
                IEnumerable<Comment> comments = await _repository.GetCommentsAsync();

                IEnumerable<CommentDTO> commentDTOs = comments.Select(c => c.ToDTO());

                return commentDTOs;
            }
        }

        public async Task UpdateCommentAsync(CommentDTO commentDTO)
        {
            Comment? commentToUpdate = await _repository.GetCommentByIdAsync(commentDTO.Id);

            if (commentToUpdate is not null)
            {
                commentToUpdate.Content = commentDTO.Content;
                commentToUpdate.AuthorId = commentDTO.AuthorId;
                commentToUpdate.Updated = DateTimeOffset.Now;
                commentToUpdate.BlogPostId = commentDTO.BlogPostId;
            }
        }
    }
}
