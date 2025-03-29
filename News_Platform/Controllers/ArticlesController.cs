using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News_Platform.DTOs;
using News_Platform.Services.Interfaces;

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

        [AllowAnonymous]
        [HttpGet("trending")]
        public async Task<ActionResult<List<TrendingArticleDto>>> GetTrendingArticles([FromQuery] int limit = 20)
        {
            var trendingArticles = await _articleService.GetTrendingArticlesAsync(limit);
            return Ok(trendingArticles);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(long id)
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

            var article = await _articleService.GetArticle(id, userId);

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
            try
            {
                if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out long userId) ||
                    !long.TryParse(User.FindFirst("role")?.Value, out long role))
                {
                    return Unauthorized(new { error = "Invalid authentication token." });
                }

                if (role != 1)
                {
                    return Forbid("You do not have permission to add an article.");
                }

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
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred while adding the article." });
            }
        }


        [HttpPut("{articleId}")]
        [Authorize]
        public async Task<IActionResult> UpdateArticle(long articleId, [FromBody] UpdateArticleRequest request)
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var role = long.Parse(User.FindFirst("role")?.Value);

            if (role != 1)
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

            if (role != 1)
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

            if (role != 1)
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

        [AllowAnonymous]
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestNews([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and pageSize must be greater than zero.");
            }

            var result = await _articleService.GetLatestNewsAsync(page, pageSize);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetArticlesByCategory(long categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _articleService.GetArticlesByCategoryAsync(categoryId, page, pageSize);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> SearchArticles([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var result = await _articleService.SearchArticlesAsync(query, page, pageSize);
            return Ok(result);
        }



    }
}
