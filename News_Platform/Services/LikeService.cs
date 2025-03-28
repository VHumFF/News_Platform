using System.Threading.Tasks;

namespace News_Platform.Services
{
    public interface ILikeService
    {
        Task<bool> LikeArticleAsync(long userId, long articleId);
        Task<bool> UnlikeArticleAsync(long userId, long articleId);
        Task<bool> LikeCommentAsync(long userId, long commentId);
        Task<bool> UnlikeCommentAsync(long userId, long commentId);
    }
}
