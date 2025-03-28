using News_Platform.DTOs;
using News_Platform.Models;

namespace News_Platform.Services
{
    public interface IArticleService
    {
        Task<List<TrendingArticleDto>> GetTrendingArticlesAsync(int limit = 20);
        Task<IEnumerable<Article>> GetAllArticlesAsync();
        Task<Article?> GetArticleByIdAsync(long id);
        Task<Article?> GetArticleBySlugAsync(string slug);
        Task AddArticleAsync(Article article);
        Task UpdateArticleAsync(Article article);
        Task DeleteArticleAsync(long id);
        Task<ArticleDto> GetArticle(long id);
        Task<ArticleDto> AddArticleAsync(string title, string content, long categoryId, string imageUrl, long userId);
    }
}
