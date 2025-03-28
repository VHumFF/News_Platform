using News_Platform.Models;

namespace News_Platform.Repositories
{
    public interface ILikeRepository
    {
        Task<int> GetLikeCountByArticleIdAsync(long articleId);
        Task<int> GetLikeCountByCommentIdAsync(long commentId);
        Task<bool> UserHasLikedArticleAsync(long userId, long articleId);
        Task<bool> UserHasLikedCommentAsync(long userId, long commentId);
        Task AddLikeAsync(Like like);
        Task<bool> RemoveLikeAsync(long userId, long? articleId, long? commentId);
    }
}
