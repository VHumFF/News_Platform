using News_Platform.Data;
using News_Platform.Models;
using News_Platform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace News_Platform.Repositories.Implementations
{
    public class ArticleViewRepository : IArticleViewRepository
    {
        private readonly AppDbContext _context;

        public ArticleViewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddArticleViewAsync(ArticleView articleView)
        {
            _context.ArticleViews.Add(articleView);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetArticleViewsCountAsync(long articleId, DateTime since)
        {
            return await _context.ArticleViews
                .Where(v => v.ArticleId == articleId && v.ViewedAt >= since)
                .CountAsync();
        }
    }

}
