namespace News_Platform.DTOs
{
    public class TrendingArticleDto
    {
        public long ArticleID { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public long Status { get; set; }
        public string ImageURL { get; set; }
        public DateTime? PublishedAt { get; set; }
        public long TotalViews { get; set; }
        public long Last24HoursViews { get; set; }
        public long Last7DaysViews { get; set; }
    }
}
