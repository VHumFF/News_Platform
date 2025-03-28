using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using News_Platform.Services;
using News_Platform.DTOs;

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
            var comments = await _commentService.GetCommentsByArticleIdAsync(articleId);
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
