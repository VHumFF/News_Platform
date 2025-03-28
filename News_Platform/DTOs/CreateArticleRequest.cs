namespace News_Platform.DTOs
{
    public class CreateArticleRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public long CategoryID { get; set; }
        public string ImageURL { get; set; }
    }
}
