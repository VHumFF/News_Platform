using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using News_Platform.Services.Interfaces;

namespace News_Platform.Controllers
{
    [Route("api/likes")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly LikeService _likeService;

        public LikeController(LikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("article/{articleId}/like")]
        public async Task<IActionResult> LikeArticle(long articleId, [FromQuery] long userId)
        {
            bool success = await _likeService.LikeArticleAsync(userId, articleId);
            return success ? Ok("Liked") : BadRequest("Already liked");
        }

        [HttpPost("comment/{commentId}/like")]
        public async Task<IActionResult> LikeComment(long commentId, [FromQuery] long userId)
        {
            bool success = await _likeService.LikeCommentAsync(userId, commentId);
            return success ? Ok("Liked") : BadRequest("Already liked");
        }

        [HttpDelete("article/{articleId}/unlike")]
        public async Task<IActionResult> UnlikeArticle(long articleId, [FromQuery] long userId)
        {
            await _likeService.UnlikeArticleAsync(userId, articleId);
            return Ok("Unliked");
        }

        [HttpDelete("comment/{commentId}/unlike")]
        public async Task<IActionResult> UnlikeComment(long commentId, [FromQuery] long userId)
        {
            await _likeService.UnlikeCommentAsync(userId, commentId);
            return Ok("Unliked");
        }
    }
}
