using System.ComponentModel.DataAnnotations;

namespace News_Platform.DTOs
{
    public class CreateArticleRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Range(0, 1, ErrorMessage = "Status must be either 0 (draft) or 1 (published).")]
        public long status { get; set; } = 0;


        [Required(ErrorMessage = "Content is required.")]
        [StringLength(50000, ErrorMessage = "Content cannot be longer than 50000 characters.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "Invalid Category")]
        public long CategoryID { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        public string ImageURL { get; set; }
    }
}
