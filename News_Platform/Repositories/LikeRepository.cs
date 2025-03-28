using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using News_Platform.Data;

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
    }
}
