using System.ComponentModel.DataAnnotations;

namespace News_Platform.DTOs
{
    public class JournalistActivationDto
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string TemporaryPassword { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "New password must be at least 8 characters long.")]
        public string NewPassword { get; set; }
    }

}
