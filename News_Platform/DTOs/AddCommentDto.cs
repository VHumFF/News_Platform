namespace News_Platform.DTOs
{
    public class AddCommentDto
    {
        public long ArticleID { get; set; }
        public long? ParentCommentID { get; set; }
        public string Content { get; set; }
    }
}
