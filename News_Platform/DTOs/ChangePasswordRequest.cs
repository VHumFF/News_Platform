using System.ComponentModel.DataAnnotations;

namespace News_Platform.DTOs
{
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; }

        [Required, MinLength(6)]
        public string NewPassword { get; set; }
    }

}
