using System.ComponentModel.DataAnnotations;

namespace News_Platform.DTOs
{
    public class AdminUserCreationDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
