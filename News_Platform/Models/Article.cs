using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace News_Platform.Models
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ArticleID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [MaxLength(255)]
        public string Slug { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public long AuthorID { get; set; }

        [ForeignKey("AuthorID")]
        public virtual User Author { get; set; }

        [Required]
        public long CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        public string ImageURL { get; set; }

        public long Status { get; set; } = 0; // Default to Draft

        public long TotalViews { get; set; } = 0;

        public long Last24HoursViews { get; set; } = 0;

        public long Last7DaysViews { get; set; } = 0;

        public DateTime? LastViewedAt { get; set; }

        public DateTime? PublishedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
