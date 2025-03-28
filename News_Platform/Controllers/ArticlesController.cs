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
    }
}
