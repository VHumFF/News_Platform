using System.ComponentModel.DataAnnotations;

namespace News_Platform.DTOs
{
    public class UpdateArticleRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(50000, ErrorMessage = "Content cannot be longer than 50000 characters.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "Invalid Category")]
        public long CategoryID { get; set; }
    }
}
