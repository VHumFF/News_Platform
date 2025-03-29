using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using News_Platform.DTOs;
using System.Security.Claims;
using News_Platform.Services.Interfaces;

namespace News_Platform.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{articleId}")]
        public async Task<IActionResult> GetCommentsByArticleId(long articleId)
        {
            long? userId = null;

            if (User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userIdClaim) && long.TryParse(userIdClaim, out long parsedUserId))
                {
                    userId = parsedUserId;
                }
            }

            var comments = await _commentService.GetCommentsByArticleIdAsync(articleId, userId);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] AddCommentDto dto)
        {
            var result = await _commentService.AddCommentAsync(dto.ArticleID, dto.UserID, dto.Content, dto.ParentCommentID);
            return result ? Ok("Comment added successfully.") : BadRequest("Failed to add comment.");
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(long commentId, [FromQuery] long userId)
        {
            var result = await _commentService.DeleteCommentAsync(commentId, userId);
            return result ? Ok("Comment deleted.") : NotFound("Comment not found.");
        }
    }
}
