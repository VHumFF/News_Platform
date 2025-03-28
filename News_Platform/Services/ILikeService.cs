using System.Threading.Tasks;
using News_Platform.Models;
using News_Platform.Repositories;

namespace News_Platform.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepository;

        public LikeService(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<bool> LikeArticleAsync(long userId, long articleId)
        {
            if (await _likeRepository.UserHasLikedArticleAsync(userId, articleId))
                return false;

            await _likeRepository.AddLikeAsync(new Like { UserID = userId, ArticleID = articleId });
            return true;
        }

        public async Task<bool> LikeCommentAsync(long userId, long commentId)
        {
            if (await _likeRepository.UserHasLikedCommentAsync(userId, commentId))
                return false;

            await _likeRepository.AddLikeAsync(new Like { UserID = userId, CommentID = commentId });
            return true;
        }

        public async Task<bool> UnlikeArticleAsync(long userId, long articleId)
        {
            var success = await _likeRepository.RemoveLikeAsync(userId, articleId, null);
            return success;
        }

        public async Task<bool> UnlikeCommentAsync(long userId, long commentId)
        {
            var success = await _likeRepository.RemoveLikeAsync(userId, null, commentId);
            return success;
        }
    }
}
