using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace News_Platform.Models
{
    public class Comment
    {
        [Key]
        public long CommentID { get; set; }

        [ForeignKey("Article")]
        public long ArticleID { get; set; }
        public Article Article { get; set; }

        [ForeignKey("User")]
        public long UserID { get; set; }
        public User User { get; set; }

        [ForeignKey("ParentComment")]
        public long? ParentCommentID { get; set; }
        public Comment? ParentComment { get; set; }

        [Required]
        public string Content { get; set; }

        public int Status { get; set; } = 0; // 0 = pending, 1 = approved, 2 = rejected

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(8);
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(8);

        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
