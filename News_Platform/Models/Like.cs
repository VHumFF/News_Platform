using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace News_Platform.Models
{
    public class Like
    {
        [Key]
        public long LikeID { get; set; }

        [Required]
        public long UserID { get; set; }  // The user who liked the article or comment

        public long? ArticleID { get; set; }  // Can be null if it's a comment like
        public long? CommentID { get; set; }  // Can be null if it's an article like

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("ArticleID")]
        public virtual Article Article { get; set; }

        [ForeignKey("CommentID")]
        public virtual Comment Comment { get; set; }
    }
}
