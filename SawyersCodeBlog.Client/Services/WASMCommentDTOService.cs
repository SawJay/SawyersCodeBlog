using SawyersCodeBlog.Client.Models;
using SawyersCodeBlog.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace SawyersCodeBlog.Client.Services
{
    public class WASMCommentDTOService : ICommentDTOService
    {
        private readonly HttpClient _httpClient;

        public WASMCommentDTOService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CommentDTO> CreateCommentAsync(CommentDTO commentDTO)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/comments", commentDTO);
            response.EnsureSuccessStatusCode();

            CommentDTO? comment = await response.Content.ReadFromJsonAsync<CommentDTO>();
            return comment!;
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/comments/{commentId}");
            response.EnsureSuccessStatusCode();
        }

        public Task<CommentDTO?> GetCommentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsAsync(int blogPostId)
        {
            IEnumerable<CommentDTO> comments = await _httpClient.GetFromJsonAsync<IEnumerable<CommentDTO>>($"api/comments/{blogPostId}") ?? [];
            return comments;
        }

        public async Task UpdateCommentAsync(CommentDTO comment)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<CommentDTO>($"api/comments/{comment.Id}", comment);
            response.EnsureSuccessStatusCode();
        }
    }
}
