using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace News_Platform.Models
{
    [Table("Categories")]  // Explicitly maps to the Categories table
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CategoryID { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("CategoryName")]  // Maps to the CategoryName column
        public string Name { get; set; }

        public string Description { get; set; }

        [Column("CreatedAt")]  // Maps to the CreatedAt column
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("UpdatedAt")]  // Maps to the UpdatedAt column
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public ICollection<Article> Articles { get; set; }
    }
}
