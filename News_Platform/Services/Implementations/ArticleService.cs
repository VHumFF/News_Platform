using Microsoft.EntityFrameworkCore;
using News_Platform.DTOs;
using News_Platform.Models;
using News_Platform.Repositories.Interfaces;
using News_Platform.Services.Interfaces;

namespace News_Platform.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILikeRepository _likeRepository;

        public ArticleService(
         IArticleRepository articleRepository,
         IUserRepository userRepository,
         ICategoryRepository categoryRepository,
         ILikeRepository likeRepository)
        {
            _articleRepository = articleRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _likeRepository = likeRepository;
        }

        public async Task<List<TrendingArticleDto>> GetTrendingArticlesAsync(int limit = 10)
        {
            List<Article> trendingArticles = await _articleRepository.GetTrendingArticlesAsync(limit);

            List<TrendingArticleDto> trendingDtos = trendingArticles.Select(article => new TrendingArticleDto
            {
                ArticleID = article.ArticleID,
                Title = article.Title,
                Slug = article.Slug,
                Status = article.Status,
                ImageURL = article.ImageURL,
                PublishedAt = article.PublishedAt,
                TotalViews = article.TotalViews,
                Last24HoursViews = article.Last24HoursViews,
                Last7DaysViews = article.Last7DaysViews
            }).ToList();

            return trendingDtos;
        }


        public async Task<ArticleDto> GetArticle(long id, long? userId)
        {
            var article = await _articleRepository.GetArticleByIdAsync(id);
            if (article == null)
            {
                return null;
            }

            int likeCount = await _likeRepository.GetLikeCountByArticleIdAsync(id);
            bool hasLiked = false;

            if (userId.HasValue)
            {
                hasLiked = await _likeRepository.UserHasLikedArticleAsync(userId.Value, id);
            }

            return new ArticleDto
            {
                ArticleID = article.ArticleID,
                Title = article.Title ?? "Untitled",
                Slug = article.Slug ?? "no-slug",
                Content = article.Content ?? "No content available",
                AuthorID = article.AuthorID,
                AuthorFirstName = article.Author?.FirstName ?? "Unknown",
                AuthorLastName = article.Author?.LastName ?? "Author",
                CategoryID = article.CategoryID,
                CategoryName = article.Category?.Name ?? "No category",
                ImageURL = article.ImageURL ?? "No image",
                Status = article.Status,
                TotalViews = article.TotalViews,
                Last24HoursViews = article.Last24HoursViews,
                Last7DaysViews = article.Last7DaysViews,
                LikeCount = likeCount,
                IsLiked = hasLiked
            };
        }



        public async Task<ArticleDto> AddArticleAsync(string title, string content, long categoryId, string imageUrl, long userId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                throw new ArgumentException("Category not found.");
            }

            var author = await _userRepository.GetUserByIdAsync(userId);
            if (author == null)
            {
                throw new ArgumentException("Author not found.");
            }

            var article = new Article
            {
                Title = title,
                Content = content,
                Slug = GenerateSlug(title),
                AuthorID = userId,
                CategoryID = categoryId,
                ImageURL = imageUrl,
                Status = 0,
                TotalViews = 0,
                Last24HoursViews = 0,
                Last7DaysViews = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };


            await _articleRepository.AddArticleAsync(article);


            return new ArticleDto
            {
                ArticleID = article.ArticleID,
                Title = article.Title,
                Slug = article.Slug,
                Content = article.Content,
                AuthorID = article.AuthorID,
                AuthorFirstName = author.FirstName,
                AuthorLastName = author.LastName,
                CategoryID = article.CategoryID,
                ImageURL = article.ImageURL ?? "No image",
                Status = article.Status,
                TotalViews = article.TotalViews,
                Last24HoursViews = article.Last24HoursViews,
                Last7DaysViews = article.Last7DaysViews
            };
        }


        public async Task<bool> UpdateArticleAsync(long articleId, string title, string content, long categoryId, long authorId)
        {
            var article = await _articleRepository.GetArticleByIdAsync(articleId);
            if (article == null)
            {
                return false;
            }

            if (article.AuthorID != authorId)
            {
                throw new UnauthorizedAccessException("You are not the author of this article.");
            }

            article.Title = title;
            article.Content = content;
            article.CategoryID = categoryId;

            await _articleRepository.UpdateArticleAsync(article);

            return true;
        }


        private string GenerateSlug(string title)
        {
            return title.ToLower().Replace(" ", "-");
        }

        public async Task<bool> DeleteArticleAsync(long articleId, long userId)
        {
            var article = await _articleRepository.GetArticleByIdAsync(articleId);
            if (article == null)
            {
                return false;
            }

            if (article.AuthorID != userId)
            {
                throw new UnauthorizedAccessException("You are not the author of this article.");
            }

            await _articleRepository.DeleteArticleAsync(articleId);

            return true;
        }


        public async Task<bool> PublishArticleAsync(long articleId, long authorId)
        {
            var article = await _articleRepository.GetArticleByIdAsync(articleId);

            if (article == null)
            {
                throw new KeyNotFoundException("Article not found.");
            }

            if (article.AuthorID != authorId)
            {
                throw new UnauthorizedAccessException("You are not the author of this article.");
            }

            if (article.Status == 1)
            {
                throw new InvalidOperationException("Article is already published.");
            }

            article.Status = 1;
            article.PublishedAt = DateTime.UtcNow;

            await _articleRepository.UpdateArticleAsync(article);

            return true;
        }



        public async Task<PaginatedResult<ArticleDto>> GetLatestNewsAsync(int page, int pageSize)
        {
            var query = _articleRepository.GetQueryableArticles()
                .Where(a => a.Status == 1)
                .OrderByDescending(a => a.PublishedAt);

            int totalArticles = await query.CountAsync();

            var articles = await query
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
                    CategoryName = a.Category.Name
                })
                .ToListAsync();

            return new PaginatedResult<ArticleDto>
            {
                Items = articles,
                TotalCount = totalArticles,
                Page = page,
                PageSize = pageSize
            };
        }


        public async Task<PaginatedResult<ArticleDto>> GetArticlesByCategoryAsync(long categoryId, int page, int pageSize)
        {
            var query = _articleRepository.GetQueryableArticles()
                .Where(a => a.Status == 1 && a.CategoryID == categoryId)
                .OrderByDescending(a => a.Last24HoursViews)
                .ThenByDescending(a => a.Last7DaysViews)
                .ThenByDescending(a => a.TotalViews);

            int totalArticles = await query.CountAsync();

            var articles = await query
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
                    CategoryID = a.Category.CategoryID,
                    CategoryName = a.Category.Name,
                    Last24HoursViews = a.Last24HoursViews,
                    Last7DaysViews = a.Last7DaysViews,
                    TotalViews = a.TotalViews
                })
                .ToListAsync();

            return new PaginatedResult<ArticleDto>
            {
                Items = articles,
                TotalCount = totalArticles,
                Page = page,
                PageSize = pageSize
            };
        }


        public async Task<PaginatedResult<ArticleDto>> SearchArticlesAsync(string query, int page, int pageSize)
        {
            return await _articleRepository.SearchArticlesAsync(query, page, pageSize);
        }











        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            return await _articleRepository.GetAllArticlesAsync();
        }

        public async Task<Article?> GetArticleByIdAsync(long id)
        {
            return await _articleRepository.GetArticleByIdAsync(id);
        }

        public async Task<Article?> GetArticleBySlugAsync(string slug)
        {
            return await _articleRepository.GetArticleBySlugAsync(slug);
        }

        public async Task AddArticleAsync(Article article)
        {
            await _articleRepository.AddArticleAsync(article);
        }

        public async Task DeleteArticleAsync(long id)
        {
            await _articleRepository.DeleteArticleAsync(id);
        }
    }
}
