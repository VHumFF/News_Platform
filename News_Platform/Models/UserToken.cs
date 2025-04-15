using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace News_Platform.Models
{
    public class UserToken
    {
        [Key]
        public long TokenID { get; set; }

        [Required]
        public long UserID { get; set; }

        [Required]
        [StringLength(255)]
        public string Token { get; set; }

        [Required]
        public long TokenType { get; set; } // 1 = Activation, 2 = journalist activation, 3 = Password Reset

        [Required]
        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(8);

        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
}
