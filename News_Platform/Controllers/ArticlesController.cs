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
    }
}
