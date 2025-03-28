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
    }
}
