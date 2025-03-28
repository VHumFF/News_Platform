using Microsoft.EntityFrameworkCore;
using News_Platform.Data;
using News_Platform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace News_Platform.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetCommentsByArticleIdAsync(long articleId)
        {
            return await _context.Comments
                .Where(c => c.ArticleID == articleId)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }



        public async Task<Comment?> GetCommentByIdAsync(long commentId)
        {
            return await _context.Comments.FindAsync(commentId);
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(long commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
