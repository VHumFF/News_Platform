using System.Collections.Generic;
using System.Threading.Tasks;
using News_Platform.DTOs;

namespace News_Platform.Services.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentDto>> GetCommentsByArticleIdAsync(long articleId, long? userId);
        Task<bool> AddCommentAsync(long articleId, long userId, string content, long? parentCommentId);
        Task<bool> DeleteCommentAsync(long commentId, long userId);
    }
}
