using System.ComponentModel.DataAnnotations;

namespace News_Platform.DTOs
{
    public class JournalistUserCreationDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
