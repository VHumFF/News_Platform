using MySqlConnector;
using News_Platform.Data;
using News_Platform.Models;
using News_Platform.DTOs;
using Microsoft.EntityFrameworkCore;

namespace News_Platform.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly AppDbContext _context;

        public ArticleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Article>> GetTrendingArticlesAsync(int limit = 20)
        {
            var trendingArticles = await _context.Articles
                .OrderByDescending(a => a.Last24HoursViews)
                .ThenByDescending(a => a.Last7DaysViews)
                .ThenByDescending(a => a.TotalViews)
                .Take(limit)
                .ToListAsync();

            return trendingArticles;
        }





        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            return await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .ToListAsync();
        }

        public async Task<Article> GetArticleByIdAsync(long id)
        {
            return await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.ArticleID == id);
        }

        public async Task<Article> GetArticleBySlugAsync(string slug)
        {
            return await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Slug == slug);
        }

        public async Task AddArticleAsync(Article article)
        {
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateArticleAsync(Article article)
        {
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArticleAsync(long id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }
    }
}
