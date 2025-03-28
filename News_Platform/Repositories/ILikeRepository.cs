namespace News_Platform.Repositories
{
    public interface ILikeRepository
    {
        Task<int> GetLikeCountByArticleIdAsync(long articleId);
        Task<int> GetLikeCountByCommentIdAsync(long commentId);
    }
}
