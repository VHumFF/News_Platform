namespace News_Platform.Services.Interfaces
{
    public interface IArticleViewService
    {
        Task TrackArticleViewAsync(long articleId);
        Task<int> GetArticleViewsInLast24HoursAsync(long articleId);
        Task<int> GetArticleViewsInLast7DaysAsync(long articleId);
    }

}
