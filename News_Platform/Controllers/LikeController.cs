using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using News_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using News_Platform.Services.Implementations;

namespace News_Platform.Controllers
{
    [Route("api/likes")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }


        [Authorize]
        [HttpPost("article/{articleId}/like")]
        public async Task<IActionResult> LikeArticle(long articleId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid user ID.");
            }

            bool success = await _likeService.LikeArticleAsync(userId, articleId);
            return success ? Ok("Liked") : BadRequest("Already liked");
        }

        [Authorize]
        [HttpPost("comment/{commentId}/like")]
        public async Task<IActionResult> LikeComment(long commentId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid user ID.");
            }

            bool success = await _likeService.LikeCommentAsync(userId, commentId);
            return success ? Ok("Liked") : BadRequest("Already liked");
        }

        [Authorize]
        [HttpDelete("article/{articleId}/unlike")]
        public async Task<IActionResult> UnlikeArticle(long articleId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid user ID.");
            }

            await _likeService.UnlikeArticleAsync(userId, articleId);
            return Ok("Unliked");
        }

        [Authorize]
        [HttpDelete("comment/{commentId}/unlike")]
        public async Task<IActionResult> UnlikeComment(long commentId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid user ID.");
            }

            await _likeService.UnlikeCommentAsync(userId, commentId);
            return Ok("Unliked");
        }
    }
}
