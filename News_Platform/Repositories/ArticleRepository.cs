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
                .Where(a => a.Status == 1)
                .OrderByDescending(a => a.Last24HoursViews)
                .ThenByDescending(a => a.Last7DaysViews)
                .ThenByDescending(a => a.TotalViews)
                .Take(limit)
                .ToListAsync();

            return trendingArticles;
        }

        public async Task<PaginatedResult<ArticleDto>> SearchArticlesAsync(string query, int page, int pageSize)
        {
            IQueryable<Article> articlesQuery = _context.Articles
                .Where(a => a.Status == 1)
                .Where(a =>
                    EF.Functions.Like(a.Title, $"%{query}%") ||
                    EF.Functions.Like(a.Slug, $"%{query}%") ||
                    EF.Functions.Like(a.Content, $"%{query}%"))
                .OrderByDescending(a => a.Last24HoursViews)
                .ThenByDescending(a => a.Last7DaysViews)
                .ThenByDescending(a => a.TotalViews);

            int totalCount = await articlesQuery.CountAsync();

            var articles = await articlesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new ArticleDto
                {
                    ArticleID = a.ArticleID,
                    Title = a.Title,
                    Slug = a.Slug,
                    Content = a.Content,
                    ImageURL = a.ImageURL,
                    PublishedAt = a.PublishedAt,
                    AuthorFirstName = a.Author.FirstName,
                    AuthorLastName = a.Author.LastName,
                    CategoryName = a.Category.Name,
                    Last24HoursViews = a.Last24HoursViews,
                    Last7DaysViews = a.Last7DaysViews,
                    TotalViews = a.TotalViews
                })
                .ToListAsync();

            return new PaginatedResult<ArticleDto>
            {
                Items = articles,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }



        public IQueryable<Article> GetQueryableArticles()
        {
            return _context.Articles.AsQueryable();
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
