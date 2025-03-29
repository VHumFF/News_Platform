using System.Collections.Generic;
using System.Threading.Tasks;
using News_Platform.Models;

namespace News_Platform.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetCommentsByArticleIdAsync(long articleId);
        Task<Comment?> GetCommentByIdAsync(long commentId);
        Task AddCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(long commentId);
    }
}
