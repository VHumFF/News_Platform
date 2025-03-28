using News_Platform.DTOs;
using News_Platform.Models;
using News_Platform.Repositories;

namespace News_Platform.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<List<TrendingArticleDto>> GetTrendingArticlesAsync(int limit = 10)
        {
            List<Article> trendingArticles = await _articleRepository.GetTrendingArticlesAsync(limit);

            List<TrendingArticleDto> trendingDtos = trendingArticles.Select(article => new TrendingArticleDto
            {
                ArticleID = article.ArticleID,
                Title = article.Title,
                Slug = article.Slug,
                Status = article.Status,
                ImageURL = article.ImageURL,
                PublishedAt = article.PublishedAt,
                TotalViews = article.TotalViews,
                Last24HoursViews = article.Last24HoursViews,
                Last7DaysViews = article.Last7DaysViews
            }).ToList();

            return trendingDtos;
        }




        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            return await _articleRepository.GetAllArticlesAsync();
        }

        public async Task<Article?> GetArticleByIdAsync(long id)
        {
            return await _articleRepository.GetArticleByIdAsync(id);
        }

        public async Task<Article?> GetArticleBySlugAsync(string slug)
        {
            return await _articleRepository.GetArticleBySlugAsync(slug);
        }

        public async Task AddArticleAsync(Article article)
        {
            await _articleRepository.AddArticleAsync(article);
        }

        public async Task UpdateArticleAsync(Article article)
        {
            await _articleRepository.UpdateArticleAsync(article);
        }

        public async Task DeleteArticleAsync(long id)
        {
            await _articleRepository.DeleteArticleAsync(id);
        }
    }
}
