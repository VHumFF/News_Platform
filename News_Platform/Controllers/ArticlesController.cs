using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News_Platform.DTOs;
using News_Platform.Services;

namespace News_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet("trending")]
        public async Task<ActionResult<List<TrendingArticleDto>>> GetTrendingArticles([FromQuery] int limit = 20)
        {
            var trendingArticles = await _articleService.GetTrendingArticlesAsync(limit);
            return Ok(trendingArticles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(long id)
        {
 
            var article = await _articleService.GetArticle(id);

 
            if (article == null)
            {
                return NotFound(new { message = "Article not found" });
            }

            return Ok(article);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddArticle([FromBody] CreateArticleRequest request)
        {

            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var role = long.Parse(User.FindFirst("role")?.Value);

            if (role != 2)
            {
                return Forbid();
            }

            try
            {
                var article = await _articleService.AddArticleAsync(
                    request.Title,
                    request.Content,
                    request.CategoryID,
                    request.ImageURL,
                    userId
                );

                return CreatedAtAction(nameof(GetArticleById), new { id = article.ArticleID }, article);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{articleId}")]
        [Authorize]
        public async Task<IActionResult> UpdateArticle(long articleId, [FromBody] UpdateArticleRequest request)
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var role = long.Parse(User.FindFirst("role")?.Value);

            if (role != 2)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _articleService.UpdateArticleAsync(articleId, request.Title, request.Content, request.CategoryID, userId);

                if (!result)
                {
                    return NotFound("Article not found or update failed.");
                }

                return Ok("Article updated successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{articleId}")]
        [Authorize]
        public async Task<IActionResult> DeleteArticle(long articleId)
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var role = long.Parse(User.FindFirst("role")?.Value);

            if (role != 2)
            {
                return Forbid();
            }

            try
            {
                var result = await _articleService.DeleteArticleAsync(articleId, userId);

                if (!result)
                {
                    return NotFound("Article not found or deletion failed.");
                }

                return Ok("Article deleted successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut("{articleId}/publish")]
        [Authorize]
        public async Task<IActionResult> PublishArticle(long articleId)
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var role = long.Parse(User.FindFirst("role")?.Value);

            if (role != 2)
            {
                return Forbid();
            }

            try
            {
                var isPublished = await _articleService.PublishArticleAsync(articleId, userId);

                if (isPublished)
                {
                    return Ok("Article successfully published.");
                }

                return BadRequest("Article could not be published.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }



    }
}
