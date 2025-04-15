namespace News_Platform.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using News_Platform.DTOs;
    using News_Platform.Models;


    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetAllArticlesAsync();
        Task<Article> GetArticleByIdAsync(long id);
        Task<Article> GetArticleBySlugAsync(string slug);
        Task AddArticleAsync(Article article);
        Task UpdateArticleAsync(Article article);
        Task DeleteArticleAsync(long id);
        Task<List<Article>> GetTrendingArticlesAsync(int limit = 20);
        IQueryable<Article> GetQueryableArticles();
        Task<PaginatedResult<ArticleDto>> SearchArticlesAsync(string query, int page, int pageSize);

        Task<(IQueryable<Article> Query, int TotalCount)> GetTrendingArticlesByCategoryAsync(long categoryId);

        Task<(IQueryable<Article> Query, int TotalCount)> GetArticlesByStatusAndAuthorIdAsync(long authorId, long status);

    }


}
