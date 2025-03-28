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
        public long UserID { get; set; }

        public long? ArticleID { get; set; }
        public long? CommentID { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("ArticleID")]
        public virtual Article Article { get; set; }

        [ForeignKey("CommentID")]
        public virtual Comment Comment { get; set; }
    }
}
