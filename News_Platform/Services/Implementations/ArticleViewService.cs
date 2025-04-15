using News_Platform.Models;
using News_Platform.Repositories.Interfaces;
using News_Platform.Services.Interfaces;

namespace News_Platform.Services.Implementations
{
    public class ArticleViewService : IArticleViewService
    {
        private readonly IArticleViewRepository _articleViewRepository;

        public ArticleViewService(IArticleViewRepository articleViewRepository)
        {
            _articleViewRepository = articleViewRepository;
        }

        public async Task TrackArticleViewAsync(long articleId)
        {
            var articleView = new ArticleView
            {
                ArticleId = articleId,
                ViewedAt = DateTime.UtcNow.AddHours(8)
            };
            await _articleViewRepository.AddArticleViewAsync(articleView);
        }

        public async Task<int> GetArticleViewsInLast24HoursAsync(long articleId)
        {
            return await _articleViewRepository.GetArticleViewsCountAsync(articleId, DateTime.UtcNow.AddHours(8).AddHours(-24));
        }

        public async Task<int> GetArticleViewsInLast7DaysAsync(long articleId)
        {
            return await _articleViewRepository.GetArticleViewsCountAsync(articleId, DateTime.UtcNow.AddHours(8).AddDays(-7));
        }
    }

}
