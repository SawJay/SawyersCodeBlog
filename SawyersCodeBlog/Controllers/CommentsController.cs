using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SawyersCodeBlog.Client.Models;
using SawyersCodeBlog.Client.Services.Interfaces;
using SawyersCodeBlog.Data;

namespace SawyersCodeBlog.Controllers
{
    [Route("api/[controller]")] // api/comments
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentDTOService _commentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(ICommentDTOService commentService, UserManager<ApplicationUser> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        // get comments by blog post id - anyone
        [HttpGet("{blogPostId:int}")]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentsForBlogPost([FromRoute] int blogPostId)
        {
            try
            {
                IEnumerable<CommentDTO> comments = await _commentService.GetCommentsAsync(blogPostId);

                return Ok(comments);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        // get comment by id - anyone

        // create comment - logged in
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CommentDTO>> CreateComment([FromBody] CommentDTO comment)
        {
            string userId = _userManager.GetUserId(User)!;

            comment.AuthorId = userId;

            CommentDTO commentDTO = await _commentService.CreateCommentAsync(comment);

            if (comment.AuthorId == userId)
            {
                return Ok(commentDTO);
            }
            else
            {
                return BadRequest();
            }
        }

        // update comment - logged in, own comment, mod or admin
        [HttpPut("{commentId:int}")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentDTO comment, [FromRoute] int commentId)
        {
            if (comment.Id != commentId)
            {
                return BadRequest();
            }

            string userId = _userManager.GetUserId(User)!;
            bool inAuthorRole = User.IsInRole("Author");
            bool inModeratorRole = User.IsInRole("Moderator");

            CommentDTO? commentDTO = await _commentService.GetCommentByIdAsync(commentId);

            if (commentDTO is not null)
            {
                if (inAuthorRole || inModeratorRole || commentDTO.AuthorId == userId)
                {
                    await _commentService.UpdateCommentAsync(comment);
                    return Ok();
                }
            }

            return NotFound();

        }

        // delete comment - logged in, own comment, mod or admin
        [HttpDelete("{commentId:int}")] // DELETE: /api/comments/5
        [Authorize]
        public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
        {
            string userId = _userManager.GetUserId(User)!;
            bool inAuthorRole = User.IsInRole("Author");
            bool inModeratorRole = User.IsInRole("Moderator");

            CommentDTO? comment = await _commentService.GetCommentByIdAsync(commentId);

            if (inAuthorRole || inModeratorRole || comment?.AuthorId == userId)
            {
                await _commentService.DeleteCommentAsync(commentId);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }

}

