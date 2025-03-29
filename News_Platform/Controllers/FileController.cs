using Microsoft.AspNetCore.Mvc;
using News_Platform.Services.Interfaces;

namespace News_Platform.Controllers
{
    public class FileController : Controller
    {
        private readonly IS3Service _s3Service;

        public FileController(IS3Service s3Service)
        {
            _s3Service = s3Service;
        }

        [HttpGet("presigned-url")]
        public IActionResult GetPresignedUrl(string extension = "jpg")
        {
            var allowedExtensions = new HashSet<string> { "jpg", "jpeg", "png", "gif" };
            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                return BadRequest("Invalid image type. Only JPG, JPEG, PNG, and GIF are allowed.");
            }

            string folder = "uploads/images";
            string uniqueId = Guid.NewGuid().ToString();
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            string fileName = $"{uniqueId}_{timestamp}.{extension}";

            string objectKey = $"{folder}/{fileName}";
            TimeSpan expiration = TimeSpan.FromMinutes(15);

            string presignedUrl = _s3Service.GeneratePresignedUrl(objectKey, expiration, $"image/{extension}");

            return Ok(new { url = presignedUrl, fileName });
        }


    }
}
