using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long UserID { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; }

    public long Role { get; set; } = 0; // 0 for normal user, 1 for journalist, 2 for admin
    public long Status { get; set; } = 0; // 0 for inactive, 1 for active

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(8);
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(8);

}
