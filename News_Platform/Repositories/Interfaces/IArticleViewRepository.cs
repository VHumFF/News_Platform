using News_Platform.Models;

namespace News_Platform.Repositories.Interfaces
{
    public interface IArticleViewRepository
    {
        Task AddArticleViewAsync(ArticleView articleView);
        Task<int> GetArticleViewsCountAsync(long articleId, DateTime since);
    }

}
