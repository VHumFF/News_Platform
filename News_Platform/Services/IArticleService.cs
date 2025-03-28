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
        Task DeleteArticleAsync(long id);
        Task<ArticleDto> GetArticle(long id, long? userId);
        Task<ArticleDto> AddArticleAsync(string title, string content, long categoryId, string imageUrl, long userId);
        Task<bool> UpdateArticleAsync(long articleId, string title, string content, long categoryId, long authorId);
        Task<bool> DeleteArticleAsync(long articleId, long userId);
        Task<bool> PublishArticleAsync(long articleId, long authorId);
        Task<PaginatedResult<ArticleDto>> GetLatestNewsAsync(int page, int pageSize);
        Task<PaginatedResult<ArticleDto>> GetArticlesByCategoryAsync(long categoryId, int page, int pageSize);
        Task<PaginatedResult<ArticleDto>> SearchArticlesAsync(string query, int page, int pageSize);


    }
}
