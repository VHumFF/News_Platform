namespace News_Platform.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
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
    }
    

}
