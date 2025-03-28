using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using News_Platform.Data;
using News_Platform.Models;

namespace News_Platform.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext _context;

        public LikeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetLikeCountByArticleIdAsync(long articleId)
        {
            return await _context.Likes.CountAsync(l => l.ArticleID == articleId);
        }

        public async Task<int> GetLikeCountByCommentIdAsync(long commentId)
        {
            return await _context.Likes.CountAsync(l => l.CommentID == commentId);
        }

        public async Task<bool> UserHasLikedArticleAsync(long userId, long articleId)
        {
            return await _context.Likes.AnyAsync(l => l.UserID == userId && l.ArticleID == articleId);
        }

        public async Task<bool> UserHasLikedCommentAsync(long userId, long commentId)
        {
            return await _context.Likes.AnyAsync(l => l.UserID == userId && l.CommentID == commentId);
        }

        public async Task AddLikeAsync(Like like)
        {
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveLikeAsync(long userId, long? articleId, long? commentId)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserID == userId &&
                                          (articleId != null ? l.ArticleID == articleId : l.CommentID == commentId));

            if (like == null)
            {
                return false;
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
