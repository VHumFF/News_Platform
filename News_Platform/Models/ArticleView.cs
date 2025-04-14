namespace News_Platform.Models
{
    public class ArticleView
    {
        public long Id { get; set; }
        public long ArticleId { get; set; }
        public DateTime ViewedAt { get; set; }
    }

}
