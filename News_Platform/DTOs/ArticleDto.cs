using News_Platform.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace News_Platform.DTOs
{
    public class ArticleDto
    {
        public long ArticleID { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public long AuthorID { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public long CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ImageURL { get; set; }
        public long Status { get; set; } = 0;
        public int LikeCount { get; set; }
        public long TotalViews { get; set; } = 0;
        public long Last24HoursViews { get; set; } = 0;
        public long Last7DaysViews { get; set; } = 0;
        public DateTime? PublishedAt { get; set; }

    }
}
