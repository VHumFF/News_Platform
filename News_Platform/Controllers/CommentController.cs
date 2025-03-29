using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using News_Platform.DTOs;
using System.Security.Claims;
using News_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

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

        [AllowAnonymous]
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] AddCommentDto dto)
        {
            try
            {
                if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out long userId))
                {
                    return Unauthorized(new { error = "Invalid authentication token." });
                }

                await _commentService.AddCommentAsync(dto.ArticleID, userId, dto.Content, dto.ParentCommentID);

                return Ok(new { message = "Comment added successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred while adding the comment." });
            }
        }


        [Authorize]
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(long commentId)
        {
            try
            {
                if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out long userId))
                {
                    return Unauthorized(new { error = "Invalid authentication token." });
                }

                var result = await _commentService.DeleteCommentAsync(commentId, userId);

                return result
                    ? Ok(new { message = "Comment deleted successfully." })
                    : NotFound(new { error = "Comment not found." });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid("You can only delete your own comments.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred while deleting the comment." });
            }
        }


    }
}
