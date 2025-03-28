using System;

namespace News_Platform.DTOs
{
    public class CommentDto
    {
        public long CommentID { get; set; }
        public long ArticleID { get; set; }
        public long UserID { get; set; }
        public long? ParentCommentID { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LikeCount { get; set; }
    }
}
